# Unity Scene Git Extension

## Interfaces

### <span style="color: #90EE90;">IGitDiffReader</span>
- Returns an `IList` of `GameObjects` in the scene that differ

### <span style="color: #90EE90;">ITerminalInterface</span>
- Executes given commands

### <span style="color: #90EE90;">ICommandBuilder</span>
- Builds and returns a command from arguments

## Classes

### DiffGameObjectExtractor : <span style="color: #90EE90;">IGitDiffReader</span>
- Takes in a git diff as text
- Extracts relevant info → diff `GameObjects`
- Finds them in the scene

### PrefabSaver
- Takes in `IList<GameObjects>` of objects in scene
- Saves them as prefabs to asset folder

### GitBashInterface : <span style="color: #90EE90;">ITerminalInterface</span>
- Target: git-bash.exe

### GitCommands
- Holds all used commands

### GitBashCommandBuilder : <span style="color: #90EE90;">ICommandBuilder</span>
- Target: Git Bash

### SceneGitGUI
- Takes in strings for target and source branch
- Fires events from buttons
