#!/bin/bash

# Clean the output directory
rm -rf ./Publish

# Publish the project to the output directory
dotnet publish -c Release -o ./Publish -v:d
