using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AutoProjectVersionSettings : ScriptableObject
{
    [Header("Auto Project Version")]
    [SerializeField]
    [Tooltip("Version will be automatically set when the project loads.")]
    private bool _setVersionOnProjectLoad = false;
    public bool SetVersionOnProjectLoad => _setVersionOnProjectLoad;
    [SerializeField]
    [Tooltip("Version will be automatically set befor building the project.")]
    private bool _setVersionBeforeBuild = false;
    public bool SetVersionBeforeBuild => _setVersionBeforeBuild;

    [Header("Changelog Export")]
    [SerializeField]
    [Tooltip("Changelog entries sorting type.")]
    private ChangelogSortType _changelogSorting;
    public ChangelogSortType ChangelogSorting => _changelogSorting;
    [SerializeField]
    [Tooltip("Should changelog entries include a date.")]
    private bool _includeDatesInChangelogEntries = false;
    public bool IncludeDatesInChangelogEntries => _includeDatesInChangelogEntries;

    [System.Serializable]
    public enum ChangelogSortType
    {
        NewestFirst,
        OldestFirst
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AutoProjectVersionSettings))]
public class AutoProjectVersionSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Set version", "Sets version for the project.")))
        {
            AutoProjectVersion.SetVersion();
        }
        if (GUILayout.Button(new GUIContent("Export changelog", "Exports changelog to the specified text file.")))
        {
            AutoProjectVersion.ExportChangelog();
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif