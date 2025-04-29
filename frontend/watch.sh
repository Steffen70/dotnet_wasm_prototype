#!/bin/bash

FRONTEND_PORT=8443

WATCHED_EXTENSIONS=("cs" "html" "css" "js" "json")
EXT_PATTERN=$(IFS="|" ; echo "${WATCHED_EXTENSIONS[*]}")
SERVER_PID=0

get_tracked_files() {
    git ls-files | grep -E "\.(${EXT_PATTERN})$"
}

rebuild_and_serve() {
    if [[ $SERVER_PID -ne 0 ]]; then
        echo "Stopping server (PID $SERVER_PID)..."
        kill "$SERVER_PID" 2>/dev/null
        wait "$SERVER_PID" 2>/dev/null
    fi

    echo "Rebuilding project..."
    ./build.sh

    echo "Starting server..."
    python3 -m http.server $FRONTEND_PORT --directory ./Publish/wwwroot &
    SERVER_PID=$!
}

# Cleanup function
cleanup() {
    echo "Cleaning up..."
    if [[ $SERVER_PID -ne 0 ]]; then
        kill "$SERVER_PID" 2>/dev/null
        wait "$SERVER_PID" 2>/dev/null
    fi
    exit 0
}

# Trap Ctrl+C and other exits
trap cleanup SIGINT SIGTERM EXIT

# Initial build
rebuild_and_serve

echo "Watching for changes in git-tracked *.{${EXT_PATTERN}} files..."

while true; do
    inotifywait -e modify,create,delete $(get_tracked_files) >/dev/null 2>&1
    echo "Change detected."
    rebuild_and_serve
done
