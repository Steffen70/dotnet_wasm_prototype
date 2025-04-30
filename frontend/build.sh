#!/bin/bash

# Clean the output directory
rm -rf ./Publish

# Publish the project to the output directory
export API_URL="https://localhost:8444/"
dotnet publish -c Release -o ./Publish -v:d -p:ApiUrl=$API_URL
