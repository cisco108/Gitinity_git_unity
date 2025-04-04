# <img src="Assets/git_Un/Documentation/Images/2502_logo.jpg" alt="Alt text" style="width:4cm;vertical-align:middle;" >  Gitinity - A git wrapper for Unity
(Suggestions for another name are welcome.)
### Get it with the package manager from:
``https://github.com/cisco108/2502_Git_Un.git#upm``



# Get Started:

### The gui changed and can be found under tools now:
![Alt text](Assets/git_Un/Documentation/Images/14.png)
![Alt text](Assets/git_Un/Documentation/Images/15.png)
The following example using the old Gui is still applicable.
### 1. Install git_Un to your Unity Project.
### 2. Open the git_Un GUI (Window -> git_Un_GUI)
![Alt text](Assets/git_Un/Documentation/Images/13.png)
### 3. Paste in the http link to an EMPTY (no commits) repository of your preferred hosting platform.
### 4. Press "Setup git Un"
### What happens:
- The project gets initialized as a git repository.
- A .gitignore for unity projects is added.
- The provided remote is added to the repository.
- The entire project (minus gitignored files) is pushed to the remote.
-  (An additional branch is added for managing file locking, which is not implemented yet.)
# How to Use Merge Function: 

## 1. Changes on scene file on <span style="color: #0000FF;">feature</span>
![Alt text](Assets/git_Un/Documentation/Images/1.png)

## 2. Changes on scene file on <span style="color: #FF0000;">master</span>
![Alt text](Assets/git_Un/Documentation/Images/2.png)

## 3. Try to merge <span style="color: #0000FF;">feature</span> in <span style="color: #FF0000;">master</span>
![Alt text](Assets/git_Un/Documentation/Images/5.png)
![Alt text](Assets/git_Un/Documentation/Images/6.png)

## 4. Switch to feature
![Alt text](Assets/git_Un/Documentation/Images/7.png)

## 5. Open Tools → GitinityUI 
![Alt text](Assets/git_Un/Documentation/Images/8.png)

## 6. Select target and feature branch, then press START 
![Alt text](Assets/git_Un/Documentation/Images/9.png)
### Configure path to git-bash.exe if needed.

## 7. Automatic switch to <span style="color: #FF0000;">master</span>
![Alt text](Assets/git_Un/Documentation/Images/10.png)

## 8. Drag and drop prefabs from Assets/DiffObjects_as_Prefabs
![Alt text](Assets/git_Un/Documentation/Images/11.png)

## 9. Result in commits:
![Alt text](Assets/git_Un/Documentation/Images/12.png)

## Everyone who would like to contribute to this, in which ever way, is very welcome to do so. I can imagine this becoming a cool tool.
