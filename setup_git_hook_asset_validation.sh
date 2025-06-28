#!/bin/bash

cat << 'EOF' > .git/hooks/pre-commit
#!/bin/sh

echo "hello from pre-commit"

A=$(cat .allow_commit 2>/dev/null)
echo "allowed: $A"

if [ "$A" != "true" ]; then
    echo "❌ Commit blocked: Asset validation failed."
    exit 1
fi

echo "✅ Looks good to me!"
EOF

