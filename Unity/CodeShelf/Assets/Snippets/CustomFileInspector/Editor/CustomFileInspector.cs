using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>Attribute used for specifying file extensions supported by speciffic <see cref="CustomFileInspector"/>.</summary>
[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public class CustomFileInspectorAttribute : System.Attribute
{
    /// <summary>Supported file extensions separated with commas.</summary>
    public string FileExtensions { get; private set; }
    /// <summary>Creates a new instance of this Attribute.</summary>
    /// <param name="fileExtension">Supported file extensions separated with commas.</param>
    public CustomFileInspectorAttribute(string fileExtension)
    {
        FileExtensions = fileExtension;
    }
}

/// <summary>Base class for custom file inspectors.</summary>
public abstract class CustomFileInspector
{
    /// <summary>Inspector context.</summary>
    protected CustomFileInspectorContext context;
    /// <summary>Should this CustomFileInspector's OnHeaderGUI method be used instead of default one.</summary>
    public virtual bool OverrideHeaderGUI => false;
    /// <summary>Should this CustomFileInspector's OnInspectorGUI method be used instead of default one.</summary>
    public virtual bool OverrideInspectorGUI => false;

    /// <summary>Creates a new instance of CustomFileInspector.</summary>
    /// <param name="context">Inspector context.</param>
    public CustomFileInspector(CustomFileInspectorContext context)
    {
        this.context = context;
    }
    public virtual void OnEnable() { }
    /// <summary>Implement this function to make a custom header GUI.</summary>
    /// Note that <see cref="OverrideHeaderGUI"/> must be set to true in order to use this implementation.</summary>
    public virtual void OnHeaderGUI() { }
    /// <summary>Implement this function to make a custom inspector.<br/>
    /// Note that <see cref="OverrideInspectorGUI"/> must be set to true in order to use this implementation.</summary>
    public virtual void OnInspectorGUI() { }
}

/// <summary>Registry of all CustomFileInspector types.</summary>
public static class CustomFileInspectorRegistry
{
    private static readonly Dictionary<string, System.Type> _registeredEditors = new Dictionary<string, System.Type>();

    static CustomFileInspectorRegistry()
    {
        var editorTypes = TypeCache.GetTypesWithAttribute<CustomFileInspectorAttribute>();
        foreach (var editorType in editorTypes)
        {
            var attribute = editorType.GetCustomAttribute<CustomFileInspectorAttribute>();
            if (attribute != null)
            {
                string fileExtension = attribute.FileExtensions.ToLower().Trim();
                var extensions = fileExtension.Split(',');
                foreach (var extension in extensions)
                {
                    if (!_registeredEditors.TryAdd(extension.Trim().TrimStart('.'), editorType))
                    {
                        Debug.LogError($"Multiple custom file inspectors detected for \"{extension}\" extension.");
                    }
                }
            }
        }
    }

    /// <summary>Tries to create an instance of CustomFileInspector valid for the specified context.</summary>
    /// <param name="context">Inspector context.</param>
    /// <param name="inspector">CustomFileInspector valid for the specified context or null if there is no valid one in the registry.</param>
    /// <returns>True if inspector was created. False otherwise.</returns>
    public static bool TryGetInspectorForContext(CustomFileInspectorContext context, out CustomFileInspector inspector)
    {
        var extension = System.IO.Path.GetExtension(context.assetPath).ToLower().Trim().TrimStart('.');
        if (_registeredEditors.TryGetValue(extension, out var editorType))
        {
            try
            {
                inspector = System.Activator.CreateInstance(editorType, context) as CustomFileInspector;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
        inspector = null;
        return false;
    }
}

/// <summary>Context for the <see cref="CustomFileInspector"/>.</summary>
public class CustomFileInspectorContext
{
    /// <summary>A SerializedObject representing the object or objects being inspected.</summary>
    public readonly SerializedObject serializedObject;
    /// <summary>The object being inspected.</summary>
    public readonly Object target;
    /// <summary>Asset path of the object being inspected.</summary>
    public readonly string assetPath;

    /// <summary>Creates a new instance of CustomFileInspectorContext.</summary>
    /// <param name="editor">Editor of the object being inspected.</param>
    public CustomFileInspectorContext(Editor editor) : this(editor.serializedObject, editor.target) { }
    /// <summary>Creates a new instance of CustomFileInspectorContext.</summary>
    /// <param name="serializedObject">A SerializedObject representing the object or objects being inspected.</param>
    /// <param name="target">The object being inspected.</param>
    public CustomFileInspectorContext(SerializedObject serializedObject, Object target)
    {
        this.serializedObject = serializedObject;
        this.target = target;
        this.assetPath = AssetDatabase.GetAssetPath(target);
    }
}


[CustomEditor(typeof(DefaultAsset), true)]
public class CustomDefaultAssetEditor : Editor
{
    private CustomFileInspector customInspector;

    private void OnEnable()
    {
        if (customInspector == null)
        {
            var context = new CustomFileInspectorContext(this);
            if (CustomFileInspectorRegistry.TryGetInspectorForContext(context, out customInspector))
            {
                customInspector.OnEnable();
            }
        }
    }
    protected override void OnHeaderGUI()
    {
        if (customInspector != null && customInspector.OverrideHeaderGUI)
        {
            customInspector.OnHeaderGUI();
        }
        else
        {
            base.OnHeaderGUI();
        }
    }
    public override void OnInspectorGUI()
    {
        if (customInspector != null && customInspector.OverrideInspectorGUI)
        {
            customInspector.OnInspectorGUI();
        }
        else
        {
            base.OnInspectorGUI();
        }
    }
}
[CustomEditor(typeof(TextAsset), true)]
public class CustomTextAssetEditor : Editor
{
    private CustomFileInspector customInspector;
    private TextAsset textAsset;

    private void OnEnable()
    {
        if (customInspector == null)
        {
            var context = new CustomFileInspectorContext(this);
            if (CustomFileInspectorRegistry.TryGetInspectorForContext(context, out customInspector))
            {
                customInspector.OnEnable();
            }
        }

        if (customInspector == null || !customInspector.OverrideInspectorGUI)
        {
            textAsset = (TextAsset)target;
        }
    }
    protected override void OnHeaderGUI()
    {
        if (customInspector != null && customInspector.OverrideHeaderGUI)
        {
            customInspector.OnHeaderGUI();
        }
        else
        {
            base.OnHeaderGUI();
        }
    }
    public override void OnInspectorGUI()
    {
        if (customInspector != null && customInspector.OverrideInspectorGUI)
        {
            customInspector.OnInspectorGUI();
        }
        else
        {
            base.OnInspectorGUI();
            EditorGUILayout.TextArea(textAsset.text, GUI.skin.label);
        }
    }
}