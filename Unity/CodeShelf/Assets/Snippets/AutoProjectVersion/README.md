# Auto Project Version
[AutoProjectVersion](Editor/AutoProjectVersion.cs) automatically assigns your Unity project version based on the Git repository history.

## 📌 Version Format
The version format is:  
`Major`.`Minor`.`Patch`.`Build`  
- **Major** - breaking changes, taken from the latest version tag,  
- **Minor** - minor changes, taken from the latest version tag,  
- **Patch** - number of commits since the last version tag,  
- **Build** - total number of commits in the repository

### Example
If the last tag is `v1.4` and there have been 7 commits since that tag, with 120 commits total, the version will be:  
`1.4.7.120`

## 📑 Export Changelog
`AutoProjectVersion` allows exporting a changelog to a text file.

- If a version tag exists, the changelog will include commits since that tag.
- If no tag exists, the changelog will include all commits.


### Sample Exported Changelog
<pre>
v0.0.0.5
Added ByteSize project.
Create README.md
Added PolishWorkCalendar project.
Added BitPacking project.
Initial commit
</pre>

## ⚙️ Installation

1. Copy `AutoProjectVersion.cs` into an `Editor` folder in your Unity project.
2. Ensure your project is under Git version control.
3. Create Git tags for major/minor version changes (e.g., `v1.0`, `v1.1`).

## 🚀 Usage

- **Automatic:** The version is updated before each build.
- **Manual:**  
  - Set version: `Tools → Versioning → Set version`  
  - Export changelog: `Tools → Versioning → Export change log`

---

✅ Tested with Unity 6000.0 LTS