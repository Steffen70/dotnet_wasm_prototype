## Windows Setup Instructions
### 1. Prerequisites

Install [Chocolatey](https://chocolatey.org/install) if you haven’t already. Open PowerShell **as Administrator** and run the install command.

---

### 2. Install Required Tools

Run the following commands to install all dependencies:

```pwsh
choco install vscode
choco install git.install
choco install dotnet-8.0-sdk
choco install powershell-core
choco install openssl
choco install caddy
choco install brave
```

---

### 3. Install .NET Tools

After installing the .NET SDK:

```pwsh
dotnet workload install wasm-tools
dotnet tool install --global dotnet-serve
```

---

### 4. Running the Project

#### Host the Frontend

```pwsh
$env:FRONTEND_PORT=8444
cd ./frontend
dotnet publish -c Release -o "./Publish" -p:EmitSourceMapping=true -p:EmitDebugInformation=true
dotnet serve -p $env:FRONTEND_PORT -d ./Publish/wwwroot
```

This will serve the published frontend locally at `http://localhost:8444`.

---

#### Run the Backend

Open a **new PowerShell window**:

```pwsh
$env:API_PORT=8445
cd ./backend
dotnet run -c Release
```

---

#### Start the Reverse Proxy (Caddy)

Ensure you are in the `frontend` directory:

```pwsh
cd ./frontend
caddy run --config Caddyfile --adapter caddyfile --environ
```

This proxies `https://localhost:8443` to the frontend/backend.

---

### 5. Trusted Certificate for HTTPS

To avoid browser warnings with the HTTPS proxy:

1. Navigate to `../cert/README.md` and follow instructions to install `root_ca.crt` into the **Windows Trusted Root Certification Authorities**.
2. You may need to restart your browser for changes to take effect.

---

### 6. Open in Browser

Once everything is running:

* Open **Brave Browser** (installed via Chocolatey)
* Navigate to: [https://localhost:8443](https://localhost:8443)