# Unity Scene g(i/e)t Extension


### interface GitDiffReader
- returns a IList of GameObjects in the scene that differ

### class DiffGameObjectExtractor : GitDiffReader
- takes in a git diff as txt
- extracts relevant info -> diff GameObjects
- finds them in the scene

### class PrefabSaver
- takes in IList<GameObjects> of objects in scene 
- saves them as prefab to asset folder
