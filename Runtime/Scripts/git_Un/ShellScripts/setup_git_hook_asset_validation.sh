#!/bin/bash

cat << 'EOF' > .git/hooks/pre-commit
#!/bin/sh

if [ ! -f .allow_commit ]; then
    echo ".allow_commit file missing and got created. Asset validation needs to run again before committing." > .allow_commit
fi

A=$(cat .allow_commit 2>/dev/null)
echo "allow_commit content: $A"

if [ "$A" != "true" ]; then
    echo "❌ Commit blocked because: $A"
    exit 1
fi

echo "✅ Looks good to me!"
EOF

