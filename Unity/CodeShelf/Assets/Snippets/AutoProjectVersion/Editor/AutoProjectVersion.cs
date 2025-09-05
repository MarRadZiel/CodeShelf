using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AutoProjectVersion : IPreprocessBuildWithReport
{
    private const string defaultSettingsAssetPath = "Assets/Settings/AutoProjectVersionSettings.asset";
    private static AutoProjectVersionSettings _settings;

    static AutoProjectVersion()
    {
        LoadSettingsIfNeeded();
    }

    [InitializeOnLoadMethod]
    public static void OnProjectLoaded()
    {
        LoadSettingsIfNeeded();
        if (_settings.SetVersionOnProjectLoad)
        {
            SetVersion();
        }
    }
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        LoadSettingsIfNeeded();
        if (_settings.SetVersionBeforeBuild)
        {
            SetVersion();
        }
    }

    [MenuItem("Tools/Versioning/Settings")]
    private static void GoToSettings()
    {
        LoadSettingsIfNeeded();

        Selection.activeObject = _settings;
    }
    [MenuItem("Tools/Versioning/Set version")]
    public static void SetVersion()
    {
        LoadSettingsIfNeeded();

        GetCurrentVersion(out int major, out int minor, out int patch, out int build, out _);

        var version = $"{major}.{minor}.{patch}.{build}";
        if (!string.Equals(PlayerSettings.bundleVersion, version))
        {
            Debug.Log($"Project version changed: {version}");
            PlayerSettings.bundleVersion = version;
            AssetDatabase.SaveAssets();
        }
    }
    [MenuItem("Tools/Versioning/Export changelog")]
    public static void ExportChangelog()
    {
        LoadSettingsIfNeeded();

        StringBuilder changelog = new StringBuilder();

        GetCurrentVersion(out int major, out int minor, out int patch, out int build, out string lastVersionCommit);

        var version = $"{major}.{minor}.{patch}.{build}";
        changelog.AppendLine($"v{version}");
        StringBuilder gitCommand = new StringBuilder("log");
        if (_settings.ChangelogSorting == AutoProjectVersionSettings.ChangelogSortType.OldestFirst)
        {
            gitCommand.Append(" --reverse");
        }
        gitCommand.Append(" --pretty=format:");
        if (_settings.IncludeDatesInChangelogEntries)
        {
            gitCommand.Append("\"%ad %s\" --date=short");
        }
        else
        {
            gitCommand.Append("\"%s\"");
        }
        if (lastVersionCommit != null)
        {
            gitCommand.Append($" {lastVersionCommit}..HEAD");
        }
        changelog.AppendLine(ExecuteGitCommand(gitCommand.ToString()));

        var path = EditorUtility.SaveFilePanel("Save changelog", "", "changelog.txt", "txt");
        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                System.IO.File.WriteAllText(path, changelog.ToString());
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    private static void LoadSettingsIfNeeded()
    {
        if (_settings == false)
        {
            string[] assetGUIDs = AssetDatabase.FindAssets($"t:{nameof(AutoProjectVersionSettings)}");
            if (assetGUIDs != null && assetGUIDs.Length > 0)
            {
                _settings = AssetDatabase.LoadAssetAtPath<AutoProjectVersionSettings>(AssetDatabase.GUIDToAssetPath(assetGUIDs[0]));
            }
            else
            {
                _settings = AssetDatabase.LoadAssetAtPath(defaultSettingsAssetPath, typeof(AutoProjectVersionSettings)) as AutoProjectVersionSettings;
                if (_settings == false)
                {
                    _settings = ScriptableObject.CreateInstance<AutoProjectVersionSettings>();
                    if (!AssetDatabase.IsValidFolder("Assets/Settings")) AssetDatabase.CreateFolder("Assets", "Settings");
                    AssetDatabase.CreateAsset(_settings, AssetDatabase.GenerateUniqueAssetPath(defaultSettingsAssetPath));
                    AssetDatabase.SaveAssets();
                    Selection.activeObject = _settings;
                }
            }
            if (_settings == false)
            {
                Debug.LogError($"{nameof(AutoProjectVersionSettings)} asset couldn't be loaded or created.");
            }
        }
    }

    private static void GetCurrentVersion(out int major, out int minor, out int patch, out int build, out string lastVersionCommit)
    {
        major = minor = patch = build = 0;
        lastVersionCommit = null;
        if (TryGetLastVersionTag(out var maj, out var min, out var tag))
        {
            major = maj;
            minor = min;
            lastVersionCommit = GetTagCommit(tag);
            string numberOfCommitsFromTag = ExecuteGitCommand($"rev-list {lastVersionCommit}..HEAD --count");
            int.TryParse(numberOfCommitsFromTag, out patch);
        }
        string totalNumberOfCommits = ExecuteGitCommand($"rev-list --all --count");
        int.TryParse(totalNumberOfCommits, out build);
    }
    private static bool TryGetLastVersionTag(out int major, out int minor, out string lastTag)
    {
        lastTag = null;
        major = minor = 0;

        // Get the latest tag (highest version), strip spaces, sort semantically in reverse, take first
        string tags = ExecuteGitCommand("tag --sort=-v:refname");
        if (string.IsNullOrWhiteSpace(tags))
        {
            return false;
        }
        var tagsSplitted = tags.Split('\n');
        if (tagsSplitted.Length > 0)
        {
            var parts = tagsSplitted[0].TrimStart('v').Split('.');
            if (parts.Length >= 2 && int.TryParse(parts[0], out major) && int.TryParse(parts[1], out minor))
            {
                lastTag = tagsSplitted[0];
                return true;
            }
        }
        return false;
    }
    private static string GetTagCommit(string tag)
    {
        return ExecuteGitCommand($"rev-list -n 1 {tag}");
    }
    private static string ExecuteGitCommand(string parameters)
    {
        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        using (var p = new System.Diagnostics.Process())
        {
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = "git";
            p.StartInfo.Arguments = parameters;
            p.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    outputBuilder.AppendLine(e.Data);
                }
            };
            p.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    errorBuilder.AppendLine(e.Data);
                }
            };

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();

            if (errorBuilder.Length > 0)
            {
                Debug.LogWarning($"Git error: {errorBuilder}");
            }
        }
        return outputBuilder.Replace("\r\n", "\n").Replace("\r", "\n").ToString().Trim();
    }
}
