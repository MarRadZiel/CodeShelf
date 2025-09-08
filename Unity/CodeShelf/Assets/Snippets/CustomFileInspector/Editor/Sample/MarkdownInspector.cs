using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>Sample Markdown inspector supporting some basic tags.</summary>
[CustomFileInspector("md")]
public class MarkdownInspector : CustomFileInspector
{
    public override bool OverrideInspectorGUI => true;

    private bool renderedPreview = true;
    private TextAsset mdTextAsset;
    private string text;

    // Formatting settings
    private readonly int[] headerSizes = new int[] { 22, 18, 16, 14, 12, 10 };
    private readonly Color codeColor = Color.yellow;
    private readonly Color blockColor = Color.cyan;
    private readonly Color preColor = Color.magenta;
    private readonly Color blockQuoteColor = Color.grey;

    public MarkdownInspector(CustomFileInspectorContext context) : base(context) { }

    public override void OnEnable()
    {
        mdTextAsset = context.target as TextAsset;
        text = mdTextAsset.text;
        text = text.Replace("\r\n", "\n");

        Parse();
        AdjustWhiteSpaces();

        void Parse()
        {
            string blockQuoteColorString = $"#{ColorUtility.ToHtmlStringRGB(blockQuoteColor)}";
            string codeColorString = $"#{ColorUtility.ToHtmlStringRGB(codeColor)}";
            string blockColorString = $"#{ColorUtility.ToHtmlStringRGB(blockColor)}";
            string preColorString = $"#{ColorUtility.ToHtmlStringRGB(preColor)}";

            // Parse # headers
            for (int i = 6; i >= 1; i--)
            {
                string replacement = $"\n\n<b><size={headerSizes[i - 1]}>$1</size></b>";
                text = Regex.Replace(text, $"^{new string('#', i)} (.+)$", replacement, RegexOptions.Multiline);
            }

            // Bold + Italic  *** ***
            text = Regex.Replace(text, @"\*\*\*(.+?)\*\*\*", "<b><i>$1</i></b>");

            // Bold ** ** and __ __
            text = Regex.Replace(text, @"\*\*(.+?)\*\*", "<b>$1</b>");
            text = Regex.Replace(text, @"__(.+?)__", "<b>$1</b>");

            // Italic * * and _ _
            text = Regex.Replace(text, @"(?<!\*)\*(?!\*)(.+?)(?<!\*)\*(?!\*)", "<i>$1</i>");
            text = Regex.Replace(text, @"(?<!_)_(?!_)(.+?)(?<!_)_(?!_)", "<i>$1</i>");

            // Underline with '<ins>' tag
            text = text.Replace("<ins>", "<u>").Replace("</ins>", "</u>");

            // Horizontal line '---'
            text = Regex.Replace(text, @"^(-{3,}|\*{3,})$", "────────────", RegexOptions.Multiline);

            // Block quote '>'
            text = Regex.Replace(text, @"^> (.*?)$", $"<color={blockQuoteColorString}>| $1</color>", RegexOptions.Multiline);

            // Multiline code blocks ``` ```
            text = Regex.Replace(text, @"```(?:\w*\n)?([\s\S]*?)```\n", $"<br><color={codeColorString}>$1</color><br>");

            // Inline code ` `
            text = Regex.Replace(text, @"`(.+?)`", $"<color={blockColorString}>$1</color>");

            // '<pre>' tags
            text = text.Replace("<pre>", $"<color={preColorString}>").Replace("</pre>", "</color>");

            // Links '[text](url)'
            text = Regex.Replace(text, @"\[(.+?)\]\((.+?)\)", "<u>$1</u>");

            // Lists
            text = Regex.Replace(text, @"^\s*[-*] (.*?)$", "• $1", RegexOptions.Multiline);

            // Handle supported HTML tags
            text = Regex.Replace(text, @"<em>(.*?)</em>", "<i>$1</i>", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<strong>(.*?)</strong>", "<b>$1</b>", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
        }
        void AdjustWhiteSpaces()
        {
            text = text.Trim();
            do
            {
                int textLength = text.Length;
                text = text.Replace("\n\n\n", "\n\n");
                int textLengthAfter = text.Length;

                if (textLength == textLengthAfter)
                {
                    break;
                }
            }
            while (true);

            text = text.Replace("\n\n", "<NEWLINE>");
            text = text.Replace("\n", "<br>");
            text = text.Replace("<NEWLINE>", "\n\n");
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.EndDisabledGroup();
        renderedPreview = GUILayout.Toggle(renderedPreview, new GUIContent(renderedPreview ? "Switch to basic view" : "Switch to rendered view"), GUI.skin.button);
        EditorGUI.BeginDisabledGroup(true);
        if (renderedPreview)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };
            EditorGUILayout.TextArea(text, style);
        }
        else
        {
            EditorGUILayout.TextArea(mdTextAsset.text, GUI.skin.label);
        }
    }
}