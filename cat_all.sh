#!/bin/bash

# Loop through and print filename + contents
for file in $(git ls-files); do
    echo -e "\n===== FILE: $file ====="
    cat "$file"
done
