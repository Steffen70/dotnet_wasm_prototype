#!/bin/bash

# Clean the output directory
rm -rf ./Publish

# Publish the project to the output directory
export API_URL="https://localhost:8443/"
dotnet publish -c Release -o ./Publish -p:ApiUrl=$API_URL -p:EmitSourceMapping=true -p:EmitDebugInformation=true

# Strip sourceMappingURL comment to prevent browser from requesting .js.map
find ./Publish -type f -name "*.js" -exec sed -i '/sourceMappingURL/d' {} +