# 📁 Custom File Inspector
`CustomFileInspector` is a Unity Editor extension that enables custom inspectors for specific file types using a clean, attribute-driven architecture.  
It allows you to override Unity’s default inspector behavior for assets like `.md`, `.csv`, or even directories — without touching Unity’s core editor code.

## 💡 Why Use This?
Unity’s default inspector for text and default assets is limited. This framework lets you tailor the editor experience for specific file types — whether you're previewing markdown, inspecting logs, or managing directories — all with minimal setup.

## ✨ Features
- 🧩 Attribute-based registration  
  Easily register custom inspectors using `[CustomFileInspector("md,csv")]` - case-insensitive with optional dots.
- 🧠 Context-aware rendering  
  Access `serializedObject`, `target`, and `assetPath` via `CustomFileInspectorContext`.
- 🎨 Flexible GUI overrides  
  Choose whether to override Unity’s header and/or inspector GUI.
- 🔄 Dynamic inspector resolution  
  Automatically selects the correct inspector based on file extension.
- 🗂️ Supports `DefaultAsset` and `TextAsset`  
  Works seamlessly with Unity’s built-in asset types.

## 🚀 Getting Started
1. Create a Custom Inspector
```csharp
///<summary>Custom inspector for .log files</summary>
[CustomFileInspector("log")]
public class LogInspector : CustomFileInspector
{
    public LogInspector(CustomFileInspectorContext context) : base(context) { }

    public override bool OverrideInspectorGUI => true;

    public override void OnInspectorGUI()
    {
        var textAsset = context.target as TextAsset;
        EditorGUILayout.LabelField("Log Preview", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(textAsset.text, GUI.skin.label);
    }
}
```

2. Drop your `.log` file into Unity
- Unity will automatically use your custom inspector for `.log` files.
- You can also support multiple extensions: `[CustomFileInspector("log,txt")]`.

## 🧪 Examples
- 📄 [MarkdownInspector.cs](Editor/Sample/MarkdownInspector.cs)  
Renders `.md` files with a simplified styled preview. [Sample](Editor/Sample/sampleMarkdown.md) `.md` file is included.   
- 📁 [DirectoryInspector.cs](Editor/Sample/DirectoryInspector.cs)  
Handles folders as `DefaultAsset` and displays their full path and provides button to open them in the explorer.

## 🧠 How It Works
- `CustomFileInspectorRegistry` scans all types with `[CustomFileInspectorAttribute]`.
- It maps file extensions to inspector types.
- `CustomDefaultAssetEditor` and `CustomTextAssetEditor` delegate rendering to the correct inspector if one is registered.
- Each inspector receives a `CustomFileInspectorContext` with full access to Unity’s editor state.

## 🛠️ Extending
You can create inspectors for any file type Unity recognizes as `DefaultAsset` or `TextAsset`. Just inherit from `CustomFileInspector`, implement your GUI logic, and register with the `CustomFileInspectorAttribute`.

## ⚠️ Notes
- Only one inspector per extension is supported.
- If multiple inspectors are registered for the same extension, Unity will log an error.

---

✅ Tested with Unity 6000.0 LTS