# 📁 Asset Version Control Tool

## 🚀 How to Use

1. **Ensure File Location**
    - Only assets located in the `VersionControlledAssets` folder are managed by this tool.
    - Place your assets (e.g., `.mat`, `.prefab`, `.asset`, etc.) into this folder to enable version control.

2. **Select an Asset**
    - In the Unity **Project view**, click on any asset inside the `VersionControlledAssets` folder.
    - A custom window will open, showing available Git versions for that file.

3. **Switch Versions**
    - Use the dropdown to select a previous commit of the file.
    - Click **"Switch Version"** to check out that version from Git.
    - The current file will be **overwritten** with the selected version.
        - ⚠️ You **must keep the same file name**. Renaming or duplicating it will break version control tracking.

4. **Save New Changes**
    - After editing the asset, click **"Save Changes"** to commit it to Git.
    - A commit message is automatically generated with the selected version hash.

## 📝 Notes

- Only files **under version control** and located inside the `VersionControlledAssets` directory will trigger the versioning popup.
- Changes are committed with Git behind the scenes. Make sure your repo is initialized and accessible from your system's terminal.
