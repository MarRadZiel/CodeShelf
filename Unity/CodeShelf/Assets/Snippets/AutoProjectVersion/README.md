# Auto Project Version
[AutoProjectVersion](Editor/AutoProjectVersion.cs) automatically assigns your Unity project version based on the Git repository history.

## 📦 Installation
AutoProjectVersion is lightweight and plug-and-play. Here’s how to set it up:

1. Copy [`AutoProjectVersion.cs`](Editor/AutoProjectVersion.cs) and [`AutoProjectVersionSettings.cs`](Editor/AutoProjectVersionSettings.cs) into an `Editor` folder in your Unity project.
2. Ensure your project is under Git version control.
3. Create Git tags for major/minor version changes (e.g., `v1.0`, `v1.1`).

_Note: `AutoProjectVersionSettings.cs` defines settings asset used for customization._
## 🚀 Usage

### Automatic  
The version updates on project load or before build (based on settings).
### Manual  
  - Set version: `Tools → Versioning → Set version`  
  - Export changelog: `Tools → Versioning → Export changelog`
  - Open settings: `Tools → Versioning → Settings`

## 📌 Version Format
The version format is:  
`Major`.`Minor`.`Patch`.`Build`  
- **Major** - breaking changes, taken from the latest version tag,  
- **Minor** - minor changes, taken from the latest version tag,  
- **Patch** - number of commits since the last version tag,  
- **Build** - total number of commits in the repository  

The version tag should be in the following format: `v[Major].[Minor]`

### Example
If the last tag is `v1.4` and there have been 7 commits since that tag, with 120 commits total, the version will be:  
`1.4.7.120`

## ⚙️ Customization
AutoProjectVersion automatically generates settings asset in `Assets/Settings/AutoProjectVersionSettings.asset` if missing.  
You can also access it manually via: `Tools → Versioning → Settings`  
Options:
- `Set Version On Project Load` – Update version when Unity loads the project
- `Set Version Before Build` – Update version before each build
- `Include Dates In Changelog Entries` – Add commit dates in changelog
- `Changelog Sorting` – Choose between `NewestFirst` or `OldestFirst`

## 📑 Changelog Export
`AutoProjectVersion` allows generating a changelog based on Git commits and exporting it to a text file.

- If a version tag exists, the changelog will include commits since that tag.
- If no tag exists, the changelog will include all commits. 
- Format and sorting are customizable via settings

### Sample exported changelog with `NewestFirst` sorting
<pre>
v0.0.0.5
Added ByteSize project.
Create README.md
Added PolishWorkCalendar project.
Added BitPacking project.
Initial commit
</pre>
### Sample exported changelog with `OldestFirst` sorting including dates  
<pre>
v0.0.0.5
2025-08-14 Initial commit
2025-08-15 Added BitPacking project.
2025-08-15 Added PolishWorkCalendar project.
2025-08-15 Create README.md
2025-08-22 Added ByteSize project.
</pre>

---

✅ Tested with Unity 6000.0 LTS