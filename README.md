# AD Manager

AD Manager is your forked and rebranded Active Directory management platform for Active Directory administration.
It provides web-based AD administration, delegated permissions, automation rules, auditing, and integrations.

## Status of this fork

- Branding updated to **AD Manager**
- Upstream-specific URL endpoints removed from user-facing paths
- Production-focused deployment guidance included below

## Technology stack

- **Backend/UI runtime:** ASP.NET Core (.NET 8), Blazor Server, MudBlazor
- **Database:** SQLite (default) or SQL Server / MySQL / MariaDB
- **Hosting:** Linux or Windows, typically behind Nginx/Apache/IIS reverse proxy

> Note: This repository is not a Python codebase today. A full Python rewrite is a separate migration project and should be done in phases (see “Python migration path” below).

---

## Install and host on Linux (production)

### 1) Prerequisites

- Ubuntu 22.04+ / Debian 12+
- .NET 8 runtime (`dotnet-runtime-8.0`)
- Nginx
- TLS certificate (Let's Encrypt recommended)

### 2) Install runtime and tools

```bash
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-runtime-8.0 nginx unzip
```

### 3) Deploy app

```bash
sudo mkdir -p /opt/ad-management
sudo mkdir -p /var/lib/ad-management
sudo useradd -r -s /usr/sbin/nologin admanagement || true
sudo chown -R admanagement:admanagement /opt/ad-management /var/lib/ad-management
```

Publish from source (on a build host), then copy the published output to `/opt/ad-management` on the target server:

```bash
dotnet publish ADManager/ADManager.csproj -c Release -o /opt/ad-management
```

> If your local folder layout differs, update the `.csproj` path accordingly.

### 4) Configure systemd service

Create `/etc/systemd/system/ad-management.service`:

```ini
[Unit]
Description=AD Manager
After=network.target

[Service]
WorkingDirectory=/opt/ad-management
ExecStart=/usr/bin/dotnet /opt/ad-management/ADManager.dll
Restart=always
RestartSec=5
User=admanagement
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

> The executable is now `ADManager.dll` to match the renamed project and assembly output.

Enable and start:

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now ad-management
sudo systemctl status ad-management
```

### 5) Configure Nginx reverse proxy

Create `/etc/nginx/sites-available/ad-management`:

```nginx
server {
    listen 80;
    server_name YOUR_DOMAIN;

    location / {
        proxy_pass http://127.0.0.1:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

Enable site:

```bash
sudo ln -s /etc/nginx/sites-available/ad-management /etc/nginx/sites-enabled/ad-management
sudo nginx -t
sudo systemctl reload nginx
```

### 6) Enable HTTPS

```bash
sudo apt-get install -y certbot python3-certbot-nginx
sudo certbot --nginx -d YOUR_DOMAIN
```

### 7) Optional centralized Seq logging

Add optional Seq settings in your `appsettings.Production.json`:

```json
{
  "Logging": {
    "Seq": {
      "Url": "http://your-seq-server:5341",
      "ApiKey": "your-seq-ingestion-api-key"
    }
  }
}
```

---

## Install and host on Windows (production)

### 1) Prerequisites

- Windows Server 2019/2022 or Windows 11
- .NET 8 Hosting Bundle
- IIS with ASP.NET Core Hosting enabled
- TLS certificate for your server hostname

### 2) Install prerequisites

1. Install the **.NET 8 Hosting Bundle** from Microsoft.
2. In **Server Manager** enable:
   - Web Server (IIS)
   - ASP.NET 4.x
   - WebSocket Protocol
   - Management Tools
3. Create a local or domain service account for the application pool identity.

### 3) Publish the application

Run on a build machine or on the server from the repository root:

```powershell
dotnet publish ADManager/ADManager.csproj -c Release -o C:\inetpub\ad-manager
```

### 4) Create the IIS site

1. Open **IIS Manager**.
2. Create a new **Application Pool** named `ADManagerPool` using **No Managed Code**.
3. Set the pool identity to the service account created for the app.
4. Create a new site named `AD Manager` pointing to `C:\inetpub\ad-manager`.
5. Bind the site to `https` with your TLS certificate.

### 5) File system and app settings

- Grant the app pool identity read/write access to the application writable directories.
- Set `ASPNETCORE_ENVIRONMENT=Production`.
- Store secrets outside source control using environment variables or secured configuration transforms.

### 6) Run as a Windows Service (optional)

If you prefer a service instead of IIS, publish to a target directory and register `ADManager.dll` with a service wrapper such as `sc.exe` or NSSM, then place a reverse proxy in front of it if needed.

---

## Production-readiness checklist

- [ ] Run with `ASPNETCORE_ENVIRONMENT=Production`
- [ ] Use HTTPS only (redirect HTTP to HTTPS)
- [ ] Store secrets outside source control (environment variables or secret manager)
- [ ] Configure DB backups and restore test schedule
- [ ] Restrict firewall to 80/443 and management ports only
- [ ] Enable structured logs and central log shipping endpoint (optional)
- [ ] Rotate credentials/API tokens periodically
- [ ] Keep server patches and .NET runtime up to date
- [ ] Verify AD least-privilege service account rights
- [ ] Enable monitoring/alerts for app availability and error rate

---

## Keep receiving upstream features without losing your custom changes

Use a **two-branch strategy**:

- `main` → tracks upstream updates
- `custom/ad-management` → your custom branding and local changes

### One-time setup

```bash
git remote add upstream https://github.com/Blazam-App/BLAZAM.git
git fetch upstream
```

### Update workflow

```bash
# Update your local mirror branch
git checkout main
git fetch upstream
git merge --ff-only upstream/main

# Rebase your customization branch on top of fresh main
git checkout custom/ad-management
git rebase main

# Push updated customization branch
git push origin custom/ad-management --force-with-lease
```

This keeps your custom files isolated and makes conflict resolution predictable on each upstream release.

---

## Python migration path (if you still want full Python)

A safe full migration should be handled in phases:

1. Freeze current feature scope and API contracts
2. Build a Python backend (FastAPI/Django) with matching endpoints
3. Migrate data layer and auth flows
4. Replace UI incrementally (or keep existing UI against new APIs)
5. Run side-by-side validation and cut over gradually

If you want, the next task can scaffold a real Python service in this repo (`/python_backend`) and begin endpoint-by-endpoint migration.
