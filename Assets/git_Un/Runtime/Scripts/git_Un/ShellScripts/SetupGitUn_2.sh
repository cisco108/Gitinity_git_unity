touch .allow_commit

echo "true" >> .allow_commit

echo ".allow_commit" >> .gitignore

mv .git/hooks/pre-commit.sample .git/hooks/pre-commit

cat << 'EOF' > .git/hooks/pre-commit
#!/bin/sh

echo "hello from pre-commit"

A=$(cat .allow_commit 2>/dev/null)
echo "allowed: $A"

if [ "$A" != "true" ]; then
    echo "□~]~L Commit blocked: .allow_commit is not 'true'"
    exit 1
fi

echo "□~\~E Commit allowed"
EOF
        
git add .gitignore

git commit -m 'added gitignore' 

git checkout -b file-locking

