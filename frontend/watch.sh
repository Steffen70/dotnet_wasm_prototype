#!/bin/bash

FRONTEND_PORT=8443

WATCHED_EXTENSIONS=("cs" "html" "css" "js" "json")
EXT_PATTERN=$(IFS="|" ; echo "${WATCHED_EXTENSIONS[*]}")
SERVER_PID=0
PIPE_FILE="/tmp/dotnet_serve_output"

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

    # Create a named pipe for cleaner output and easier control
    rm -f "$PIPE_FILE"
    mkfifo "$PIPE_FILE"

    awk '
        BEGIN {
            RESET = "\033[0m"
        }

        /\033\[33mStarting server/ {
            print RESET $0
            next
        }

        /^Listening on:/ {
            print RESET $0
            next
        }

        /\033\[92m  http:\/\/localhost:/ {
            print RESET $0
            next
        }

        /Press CTRL\+C to exit/ {
            print RESET $0
            next
        }

        /Request finished/ {
            sub(/^[ \t]+/, "")
            print
        }
    ' < "$PIPE_FILE" &

    unbuffer dotnet serve -p $FRONTEND_PORT -d ./Publish/wwwroot \
        -h "Cross-Origin-Embedder-Policy: require-corp" \
        -h "Cross-Origin-Opener-Policy: same-origin" \
        > "$PIPE_FILE" 2>&1 &

    SERVER_PID=$!
}

# Cleanup function
cleanup() {
    if [[ "$CLEANED_UP" == "1" ]]; then
        return
    fi
    CLEANED_UP=1

    echo "Cleaning up..."
    if [[ $SERVER_PID -ne 0 ]]; then
        kill "$SERVER_PID" 2>/dev/null
        wait "$SERVER_PID" 2>/dev/null
    fi
    rm -f "$PIPE_FILE"
    exit 0
}


trap cleanup SIGINT SIGTERM EXIT

rebuild_and_serve
echo "Watching for changes in git-tracked *.{${EXT_PATTERN}} files..."

while true; do
    inotifywait -e modify,create,delete $(get_tracked_files) >/dev/null 2>&1
    echo "Change detected."
    rebuild_and_serve
done
