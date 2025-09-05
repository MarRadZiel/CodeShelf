using UnityEditor;
using UnityEngine;

[CustomFileInspector("")]
public class DirectoryInspector : CustomFileInspector
{
    public override bool OverrideInspectorGUI => true;

    private System.IO.DirectoryInfo directoryInfo;

    public DirectoryInspector(CustomFileInspectorContext context) : base(context) { }

    public override void OnEnable()
    {
        directoryInfo = new System.IO.DirectoryInfo(context.assetPath);
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
        if (GUILayout.Button("Show in explorer"))
        {
            EditorUtility.RevealInFinder(context.assetPath);
        }
        EditorGUILayout.SelectableLabel(directoryInfo.FullName);
    }
}