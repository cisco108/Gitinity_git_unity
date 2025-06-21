touch .allow_commit

echo "true" >> .allow_commit

echo ".allow_commit" >> .gitignore
      
git add .gitignore

git commit -m 'added gitignore' 

git checkout -b file-locking

