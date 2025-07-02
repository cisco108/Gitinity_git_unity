#!/bin/bash

cat << 'EOF' > .git/hooks/pre-commit
#!/bin/sh

echo -e "pre-commit:\n" >> logs.txt

branch="$(git rev-parse --abbrev-ref HEAD)"
if [ "$branch" = "file-locking" ]; then
    echo $branch >> logs.txt
    exit 0
fi
    
echo $branch >> logs.txt
    
if [ ! -f .allow_commit ]; then
    echo ".allow_commit file missing and got created. Asset validation needs to run again before committing." > .allow_commit
fi

A=$(cat .allow_commit 2>/dev/null)
echo "allow_commit content: $A"

if [ "$A" != "ok" ]; then
    echo "Γ¥î Commit blocked because: $A"
    exit 1
fi

echo "Γ£à Looks good to me!"
EOF
