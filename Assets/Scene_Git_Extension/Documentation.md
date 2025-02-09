# Unity Scene g(i/e)t Extension


### interface IGitDiffReader
- returns a IList of GameObjects in the scene that differ

### class DiffGameObjectExtractor : IGitDiffReader
- takes in a git diff as txt
- extracts relevant info -> diff GameObjects
- finds them in the scene

### class PrefabSaver
- takes in IList<GameObjects> of objects in scene 
- saves them as prefab to asset folder

### interface ITerminalInterface
- executes given commands

### class GitBashInterface : ITerminalInterface
- target to git-bash.exe
 
### class GitCommands
- holds all used commands

### interface ICommandBuilder
- builds and returns a command from arguments

### class GitBashCommandBuilder : ICommandBuilder
- target git bash

### class SceneGitGUI
- takes in strings for target and source branch
- fires events from buttons