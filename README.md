# WASM gRPC Prototype (C# / .NET 8)

This is a WebAssembly-based prototype built with .NET 8 and Uno Platform.

## Requirements

This setup guide assumes you are running **Kubuntu 24.04** with most development tools preinstalled.

**Note:** For detailed setup instructions on Windows, please refer to [docs/windows.md](docs/windows.md).

Only the following tools need to be manually installed:

```bash
# Install .NET 8 SDK locally (obviously — it's a .NET project)
curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 8.0 --install-dir ~/.dotnet

# Add .NET tools and SDK path to environment variables
cat <<EOF >> ~/.bashrc

export DOTNET_ROOT="\$HOME/.dotnet"
export PATH="\$HOME/.dotnet:\$HOME/.dotnet/tools:\$PATH"
EOF

# Install WebAssembly tools workload (needed for building .NET WASM projects)
dotnet workload install wasm-tools

# Install static file server (used by watch.sh to serve the frontend)
dotnet tool install --global dotnet-serve

# Install Caddy (a lightweight reverse proxy to serve the frontend over HTTPS)
sudo apt install caddy

# Install inotify-tools (used by watch.sh to detect file changes)
sudo apt install inotify-tools

# Install PowerShell (used in build targets for cross-platform file generation)
sudo apt install -y wget apt-transport-https software-properties-common && \
source /etc/os-release && \
wget -q https://packages.microsoft.com/config/ubuntu/$VERSION_ID/packages-microsoft-prod.deb && \
sudo dpkg -i packages-microsoft-prod.deb && \
rm packages-microsoft-prod.deb && \
sudo apt update && \
sudo apt install -y powershell
```

**Note:** This setup guide is tailored specifically for Kubuntu 24.04 and assumes a base system with most common development tools preinstalled. If you're using a different Linux distribution, some commands or package names may differ. This list is not exhaustive—feel free to improve or extend it as needed.

## Dev Certificates

In the `cert/` directory, run:

```bash
./create.sh
```

## Frontend Dev Server

To start the auto-rebuilding and serving frontend:

```bash
cd frontend
./watch.sh
```

This watches tracked source files (`*.cs`, `*.html`, `*.css`, `*.js`, `*.json`) and serves via `dotnet-serve`.

## Backend

Open the `WasmPrototype.sln` solution in JetBrains Rider and run the `Backend.csproj` project.
