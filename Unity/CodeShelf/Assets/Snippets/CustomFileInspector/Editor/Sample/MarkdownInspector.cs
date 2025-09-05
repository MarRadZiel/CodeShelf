using UnityEditor;
using UnityEngine;

/// <summary>Sample Markdown inspector supporting some basic tags.</summary>
[CustomFileInspector("md")]
public class MarkdownInspector : CustomFileInspector
{
    public override bool OverrideInspectorGUI => true;

    [SerializeField]
    private bool renderedPreview = true;

    private TextAsset mdTextAsset;
    private string text;

    private readonly int[] headerSizes = new int[] { 22, 18, 16, 14, 12, 10 };
    private readonly Color codeColor = Color.yellow;
    private readonly Color blockColor = Color.cyan;
    private readonly Color preColor = Color.magenta;

    public MarkdownInspector(CustomFileInspectorContext context) : base(context) { }

    public override void OnEnable()
    {
        mdTextAsset = context.target as TextAsset;
        text = mdTextAsset.text;
        ParseHeaders();
        ParseFormattingTags();
        ParseBlocks();
        ParseLinks();
        AdjustWhiteSpaces();

        void ParseHeaders()
        {
            text = text.Replace("######", "<h6>").Replace("#####", "<h5>").Replace("####", "<h4>").Replace("###", "<h3>").Replace("##", "<h2>").Replace("#", "<h1>");

            for (int i = 0; i < headerSizes.Length; ++i)
            {
                string headerTag = $"<h{i + 1}>";
                int headerStart = text.IndexOf(headerTag);
                do
                {
                    if (headerStart >= 0)
                    {
                        int headerEnd = text.IndexOf('\n', headerStart);
                        text = text[..headerStart] + $"\n\n<b><size={headerSizes[i]}>{text[(headerStart + 4)..headerEnd].Trim()}</size></b>" + text[headerEnd..];
                        headerStart = text.IndexOf(headerTag, headerEnd + 1);
                    }
                }
                while (headerStart >= 0);
            }

        }
        void ParseFormattingTags()
        {
            string tag = "***";
            int startTag = text.IndexOf(tag);
            do
            {
                if (startTag >= 0)
                {
                    int endTag = text.IndexOf(tag, startTag + 3);

                    text = text[..startTag] + $"<b><i>{text[(startTag + 3)..endTag].Trim()}</i></b>" + text[(endTag + 3)..];

                    startTag = text.IndexOf(tag, endTag + 3);
                }
            }
            while (startTag >= 0);

            tag = "**";
            startTag = text.IndexOf(tag);
            do
            {
                if (startTag >= 0)
                {
                    int endTag = text.IndexOf(tag, startTag + tag.Length);

                    text = text[..startTag] + $"<b>{text[(startTag + tag.Length)..endTag].Trim()}</b>" + text[(endTag + tag.Length)..];

                    startTag = text.IndexOf(tag, endTag + tag.Length);
                }
            }
            while (startTag >= 0);

            tag = "__";
            startTag = text.IndexOf(tag);
            do
            {
                if (startTag >= 0)
                {
                    int endTag = text.IndexOf(tag, startTag + tag.Length);

                    text = text[..startTag] + $"<b>{text[(startTag + tag.Length)..endTag].Trim()}</b>" + text[(endTag + tag.Length)..];

                    startTag = text.IndexOf(tag, endTag + tag.Length);
                }
            }
            while (startTag >= 0);

            tag = "*";
            startTag = text.IndexOf(tag);
            do
            {
                if (startTag >= 0)
                {
                    int endTag = text.IndexOf(tag, startTag + tag.Length);

                    text = text[..startTag] + $"<i>{text[(startTag + tag.Length)..endTag].Trim()}</i>" + text[(endTag + tag.Length)..];

                    startTag = text.IndexOf(tag, endTag + tag.Length);
                }
            }
            while (startTag >= 0);

            tag = "_";
            startTag = text.IndexOf(tag);
            do
            {
                if (startTag >= 0)
                {
                    int endTag = text.IndexOf(tag, startTag + tag.Length);

                    text = text[..startTag] + $"<i>{text[(startTag + tag.Length)..endTag].Trim()}</i>" + text[(endTag + tag.Length)..];

                    startTag = text.IndexOf(tag, endTag + tag.Length);
                }
            }
            while (startTag >= 0);

            text = text.Replace("<ins>", "<u>").Replace("</ins>", "</u>");
        }
        void ParseLinks()
        {
            string tag = "[";
            int startTag = text.IndexOf(tag);
            do
            {
                if (startTag >= 0)
                {
                    int titleEndTag = text.IndexOf(']', startTag + tag.Length);
                    if (titleEndTag > 0)
                    {
                        if (titleEndTag + 1 < text.Length && text[titleEndTag + 1] == '(')
                        {
                            int endTag = text.IndexOf(")", startTag + tag.Length);
                            text = text[..startTag] + $"<u>{text[(startTag + tag.Length)..titleEndTag].Trim()}</u>" + text[(endTag + tag.Length)..];
                            startTag = text.IndexOf(tag, endTag + tag.Length);
                        }
                        else
                        {
                            startTag = text.IndexOf(tag, titleEndTag);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            while (startTag >= 0);
        }
        void ParseBlocks()
        {
            string tag = "```";
            int startTag = text.IndexOf(tag);
            string codeColorString = $"#{ColorUtility.ToHtmlStringRGB(codeColor)}";
            do
            {
                if (startTag >= 0)
                {
                    int endStartTag = text.IndexOf("\n", startTag + tag.Length);
                    int endTag = text.IndexOf(tag, endStartTag + tag.Length);

                    text = text[..startTag] + $"<br><color={codeColorString}>{text[endStartTag..endTag].Trim()}</color><br>" + text[(endTag + tag.Length)..];

                    startTag = text.IndexOf(tag, endTag + tag.Length);
                }
            }
            while (startTag >= 0);

            tag = "`";
            startTag = text.IndexOf(tag);
            string blockColorString = $"#{ColorUtility.ToHtmlStringRGB(blockColor)}";
            do
            {
                if (startTag >= 0)
                {
                    int endTag = text.IndexOf(tag, startTag + tag.Length);
                    text = text[..startTag] + $"<color={blockColorString}>{text[(startTag + tag.Length)..endTag].Trim()}</color>" + text[(endTag + tag.Length)..];

                    startTag = text.IndexOf(tag, endTag + tag.Length);
                }
            }
            while (startTag >= 0);
            text = text.Replace("<pre>", $"<color=#{ColorUtility.ToHtmlStringRGB(preColor)}>").Replace("</pre>", "</color>");
        }
        void AdjustWhiteSpaces()
        {
            text = text.Trim();
            text = text.Replace("\r\n", "\n");
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