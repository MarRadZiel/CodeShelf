using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public static class MeshCombiner
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/MeshCombiner/Combine Meshes")]
    private static void GenerateCombinedMesh()
    {
        var gameObject = UnityEditor.Selection.activeGameObject;
        if (gameObject)
        {
            string path = UnityEditor.EditorUtility.SaveFilePanel("Save Mesh Asset", UnityEditor.SessionState.GetString("lastGeneratedMeshPath", "Assets/"), gameObject.name, "asset");
            if (!string.IsNullOrEmpty(path))
            {
                UnityEditor.SessionState.SetString("lastGeneratedMeshPath", System.IO.Path.GetDirectoryName(path));
                UnityEditor.Undo.RegisterFullObjectHierarchyUndo(gameObject, "Generated combined mesh");
                path = UnityEditor.FileUtil.GetProjectRelativePath(path);
                List<MeshFilter> meshFilters = new List<MeshFilter>(gameObject.GetComponentsInChildren<MeshFilter>());
                if (gameObject.TryGetComponent(out MeshFilter meshFilter) && meshFilters.Contains(meshFilter))
                {
                    meshFilters.Remove(meshFilter);
                }
                var mesh = CombineMeshes(gameObject.transform, meshFilters, out var materials);

                Mesh meshToSave = mesh;

                UnityEditor.MeshUtility.Optimize(meshToSave);

                UnityEditor.AssetDatabase.CreateAsset(meshToSave, path);
                UnityEditor.AssetDatabase.SaveAssets();

                if (UnityEditor.EditorUtility.DisplayDialog("Mesh generation finished", "Initialize object with generated mesh?", "Yes", "No"))
                {
                    if (!meshFilter)
                    {
                        meshFilter = gameObject.AddComponent<MeshFilter>();
                    }
                    if (!gameObject.TryGetComponent(out Renderer meshRenderer))
                    {
                        meshRenderer = gameObject.AddComponent<MeshRenderer>();
                    }

                    meshFilter.sharedMesh = mesh;
                    meshRenderer.sharedMaterials = materials;
                    if (gameObject.TryGetComponent(out MeshCollider collider))
                    {
                        collider.sharedMesh = mesh;
                    }
                }
            }
        }
    }
#endif

    /// <summary>Combines the meshes from the specified <see cref="MeshFilter"/> components into a single mesh,
    /// transforming all geometry into the local space of a given root transform and grouping submeshes by material to preserve correct rendering order.</summary>
    /// <param name="targetTransform">The root transform used to convert all mesh geometry into a shared local space.</param>
    /// <param name="meshFilters">The mesh filters whose meshes will be combined. Each mesh must be readable; non‑readable meshes will cause the method to return <c>null</c></param>
    /// <param name="combinedMaterials">Outputs the array of materials corresponding to the submeshes in the combined mesh, in the same order they appear in the combined geometry.</param>
    /// <param name="generateLightmapUVs">If <c>true</c>, generates a secondary UV set for lightmapping on the combined mesh.<br/>
    /// <b>Note:</b> This option is only available in the Unity Editor.</param>
    /// <param name="optimizeMesh">If <c>true</c>, runs Unity’s mesh optimization on the combined mesh before returning it.<br/>
    /// <b>Note:</b> This option is only available in the Unity Editor.</param>
    /// <returns>A new <see cref="Mesh"/> containing the combined geometry of all provided mesh filters,
    /// or <c>null</c> if any required mesh is not readable or if parameters are invalid.</returns>

    public static Mesh CombineMeshes(Transform targetTransform, IEnumerable<MeshFilter> meshFilters, out Material[] combinedMaterials, bool generateLightmapUVs = true, bool optimizeMesh = true)
    {
        if (targetTransform == null)
        {
            Debug.LogError($"Parameter {nameof(targetTransform)} cannot be null.");
            combinedMaterials = null;
            return null;
        }
        if (meshFilters == null)
        {
            Debug.LogError($"Parameter {nameof(meshFilters)} cannot be null.");
            combinedMaterials = null;
            return null;
        }
        HashSet<Mesh> tempMeshes = new HashSet<Mesh>();
        List<CombineInstance> combine = new List<CombineInstance>();

        Dictionary<Material, List<CombineInstance>> combineByMaterial = new Dictionary<Material, List<CombineInstance>>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (!meshFilter.sharedMesh.isReadable)
            {
                Debug.LogError($"Couldn't combine meshes because mesh is not readable: {meshFilter.sharedMesh.name}");
                combinedMaterials = null;
                return null;
            }

            bool hasMultipleSubMeshes = meshFilter.sharedMesh.subMeshCount > 1;
            int lastSubmeshIndex = -1;
            for (int subMeshIndex = 0; subMeshIndex < meshFilter.sharedMesh.subMeshCount; ++subMeshIndex)
            {
                if (meshFilter.gameObject.TryGetComponent(out Renderer renderer))
                {
                    Material material;
                    if (subMeshIndex < renderer.sharedMaterials.Length)
                    {
                        material = renderer.sharedMaterials[subMeshIndex];
                        lastSubmeshIndex = subMeshIndex;
                    }
                    else if (lastSubmeshIndex >= 0)
                    {
                        // Fallback - reuse last valid material
                        material = renderer.sharedMaterials[lastSubmeshIndex];
                    }
                    else
                    {
                        // No valid material found yet — skip this submesh
                        continue;
                    }

                    if (!combineByMaterial.TryGetValue(material, out var combineInstances))
                    {
                        combineInstances = new List<CombineInstance>();
                        combineByMaterial.Add(material, combineInstances);
                    }
                    Mesh subMesh;
                    if (hasMultipleSubMeshes)
                    {
                        subMesh = GetSubMesh(meshFilter.sharedMesh, subMeshIndex);
                        tempMeshes.Add(subMesh);
                    }
                    else
                    {
                        subMesh = meshFilter.sharedMesh;
                    }
                    combineInstances.Add(new CombineInstance
                    {
                        mesh = subMesh,
                        transform = targetTransform.worldToLocalMatrix * meshFilter.transform.localToWorldMatrix,
                        lightmapScaleOffset = renderer.lightmapScaleOffset,
                        realtimeLightmapScaleOffset = renderer.realtimeLightmapScaleOffset,
                        subMeshIndex = subMeshIndex
                    });
                }
            }
        }
        uint vertexCount = 0;
        foreach (var combineInstances in combineByMaterial.Values)
        {
            Mesh combineMesh = new Mesh();
            combineMesh.CombineMeshes(combineInstances.ToArray(), true, true, true);
#if UNITY_EDITOR
            if (generateLightmapUVs)
            {
                UnityEditor.Unwrapping.GenerateSecondaryUVSet(combineMesh);
            }
#endif
            vertexCount += (uint)combineMesh.vertexCount;
            combine.Add(new CombineInstance
            {
                mesh = combineMesh,
                transform = Matrix4x4.identity,
                subMeshIndex = 0
            });
        }

        Mesh mesh = new Mesh
        {
            indexFormat = vertexCount > ushort.MaxValue ? IndexFormat.UInt32 : IndexFormat.UInt16
        };
        mesh.CombineMeshes(combine.ToArray(), false, true, true);
#if UNITY_EDITOR
        if (generateLightmapUVs)
        {
            UnityEditor.Unwrapping.GenerateSecondaryUVSet(mesh);
        }
        if (optimizeMesh)
        {
            UnityEditor.MeshUtility.Optimize(mesh);
        }
#endif
        combinedMaterials = new List<Material>(combineByMaterial.Keys).ToArray();

        foreach (var subMesh in tempMeshes)
        {
            if (subMesh)
            {
                Object.DestroyImmediate(subMesh);
            }
        }
        tempMeshes.Clear();

        return mesh;
    }
    /// <summary>Creates Mesh from other one's specified submesh.</summary>
    /// <param name="originalMesh">Original mesh containing specified submesh.</param>
    /// <param name="subMeshIndex">Index of submesh to get.</param>
    public static Mesh GetSubMesh(Mesh originalMesh, int subMeshIndex)
    {
        if (originalMesh == null)
        {
            Debug.LogError("Cannot get submesh for NULL.");
            return null;
        }
        if (subMeshIndex < 0 || subMeshIndex >= originalMesh.subMeshCount)
        {
            Debug.LogError("Submesh index out of range for specified original mesh.");
            return null;
        }

        SubMeshDescriptor subMeshDescriptor = originalMesh.GetSubMesh(subMeshIndex);
        Mesh subMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        bool hasNormals = originalMesh.normals != null && originalMesh.normals.Length >= subMeshDescriptor.firstVertex + subMeshDescriptor.vertexCount;
        bool hasUvs = originalMesh.uv != null && originalMesh.uv.Length >= subMeshDescriptor.firstVertex + subMeshDescriptor.vertexCount;
        for (int i = 0; i < subMeshDescriptor.vertexCount; ++i)
        {
            var vertexIndex = subMeshDescriptor.firstVertex + i;
            vertices.Add(originalMesh.vertices[vertexIndex]);
            if (hasNormals) normals.Add(originalMesh.normals[vertexIndex]);
            if (hasUvs) uvs.Add(originalMesh.uv[vertexIndex]);
        }
        subMesh.SetVertices(vertices);
        if (hasNormals) subMesh.SetNormals(normals);
        if (hasUvs) subMesh.SetUVs(0, uvs);
        List<int> indices = new List<int>(originalMesh.GetIndices(subMeshIndex));

        for (int i = 0; i < subMeshDescriptor.indexCount; ++i)
        {
            indices[i] -= subMeshDescriptor.firstVertex;
        }
        subMesh.SetIndices(indices, subMeshDescriptor.topology, 0);
        return subMesh;
    }
}
