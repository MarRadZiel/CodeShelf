using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MeshCombinerSampleController : MonoBehaviour
{
    [SerializeField]
    private GameObject originalMeshes;
    [SerializeField]
    private GameObject combinedMesh;

    [SerializeField]
    private TMPro.TextMeshProUGUI sampleUIText;

    private List<float> frameTimes = new List<float>();
    private const int historyCount = 60;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (originalMeshes.activeInHierarchy)
            {
                originalMeshes.SetActive(false);
                combinedMesh.SetActive(true);
            }
            else
            {
                originalMeshes.SetActive(true);
                combinedMesh.SetActive(false);
            }
            frameTimes.Clear();
        }
    }
    private void FixedUpdate()
    {
        RefreshText();
    }

    private void RefreshText()
    {
        StringBuilder stringBuilder = new StringBuilder($"Mesh combining: {(originalMeshes.activeInHierarchy ? "OFF" : "ON")}\n");
#if UNITY_EDITOR
        if (frameTimes.Count == historyCount)
        {
            frameTimes.RemoveAt(0);
        }
        frameTimes.Add(UnityStats.frameTime);
        float avg = 0.0f;
        foreach (var frameTime in frameTimes)
        {
            avg += frameTime;
        }
        avg /= frameTimes.Count;

        stringBuilder.AppendLine($"Draw calls: {UnityStats.drawCalls}");
        stringBuilder.AppendLine($"Batches: {UnityStats.batches}");
        stringBuilder.AppendLine($"AVG Frame time: {(avg * 1000.0f):F2}ms");
        stringBuilder.AppendLine($"AVG FPS: {Mathf.RoundToInt(1.0f / avg)}");
        stringBuilder.AppendLine($"MIN Frame time: {(frameTimes.Min() * 1000.0f):F2}ms");
        stringBuilder.AppendLine($"AVG FPS: {Mathf.RoundToInt(1.0f / avg)}");
        stringBuilder.AppendLine($"MAX FPS: {Mathf.RoundToInt(1.0f / frameTimes.Min())}");
#endif
        sampleUIText.SetText(stringBuilder.ToString());
    }
}
