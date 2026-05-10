# AD-Management – Hosting, Setup & Upstream-Sync Guide

## 1. Install & Host on a Linux Server

### Prerequisites
| Requirement | Version |
|---|---|
| OS | Ubuntu 22.04 / Debian 12 (or any modern Linux) |
| .NET Runtime | 8.0 |
| Web server | Nginx **or** Apache (acts as reverse proxy) |
| Database | SQLite (zero-config) **or** MySQL/MariaDB/SQL Server |
| SSL certificate | Let's Encrypt (recommended) or self-signed |

---

### Step 1 – Install .NET 8 Runtime

```bash
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update && sudo apt-get install -y dotnet-runtime-8.0
```

---

### Step 2 – Create a dedicated service user

```bash
sudo useradd -r -s /usr/sbin/nologin admanagement
```

---

### Step 3 – Get the application

**Option A – Download a published build from GitHub Releases**

```bash
RELEASE_URL="https://github.com/khanalrahul7912/BLAZAM/releases/latest/download/linux-x64.zip"
sudo mkdir -p /opt/admanagement
sudo wget -qO /tmp/admanagement.zip "$RELEASE_URL"
sudo unzip /tmp/admanagement.zip -d /opt/admanagement
```

**Option B – Build from source**

```bash
# Requires .NET SDK 8
git clone https://github.com/khanalrahul7912/BLAZAM.git
cd BLAZAM
dotnet publish BLAZAM/BLAZAM.csproj -c Release -r linux-x64 --self-contained false -o /opt/admanagement
```

---

### Step 4 – Create data and config directories

```bash
sudo mkdir -p /var/lib/admanagement
sudo chown -R admanagement:admanagement /opt/admanagement /var/lib/admanagement
```

---

### Step 5 – Configure `appsettings.json`

Edit `/opt/admanagement/appsettings.json` (or create an override):

```json
{
  "ConnectionStrings": {
    "AppConnection": "Data Source=/var/lib/admanagement/admanagement.db"
  },
  "Kestrel": {
    "Endpoints": {
      "Http": { "Url": "http://localhost:5000" }
    }
  },
  "Logging": {
    "LogLevel": { "Default": "Warning" }
  }
}
```

For **MySQL** replace `AppConnection` with:

```
"Server=localhost;Database=admanagement;User=adm_user;Password=CHANGE_ME;"
```

---

### Step 6 – Create a systemd service

```bash
sudo tee /etc/systemd/system/admanagement.service > /dev/null <<'EOF'
[Unit]
Description=AD-Management Web Application
After=network.target

[Service]
WorkingDirectory=/opt/admanagement
ExecStart=/usr/bin/dotnet /opt/admanagement/BLAZAM.dll
Restart=always
RestartSec=10
SyslogIdentifier=admanagement
User=admanagement
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
EOF

sudo systemctl daemon-reload
sudo systemctl enable --now admanagement
sudo systemctl status admanagement
```

---

### Step 7 – Configure Nginx reverse proxy

```bash
sudo apt-get install -y nginx
sudo tee /etc/nginx/sites-available/admanagement > /dev/null <<'EOF'
server {
    listen 80;
    server_name YOUR_DOMAIN_OR_IP;

    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection "upgrade";
        proxy_set_header   Host $host;
        proxy_set_header   X-Real-IP $remote_addr;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_read_timeout 300s;
    }
}
EOF

sudo ln -s /etc/nginx/sites-available/admanagement /etc/nginx/sites-enabled/
sudo nginx -t && sudo systemctl reload nginx
```

> **Add HTTPS with Let's Encrypt:**
> ```bash
> sudo apt install certbot python3-certbot-nginx
> sudo certbot --nginx -d YOUR_DOMAIN
> ```

---

### Step 8 – First-run wizard

1. Open `https://YOUR_DOMAIN` in your browser.
2. The setup wizard will walk you through:
   - Database connection
   - AD / LDAP connection details
   - Creating the first admin account
3. In **Settings → Application**, change the **App Name** to `AD-Management` if needed.

---

## 2. Keeping Your Fork in Sync With Upstream

You want to keep your customisations **on a stable branch** and pull upstream improvements **without overwriting them**.

### One-time setup – add the upstream remote

```bash
git remote add upstream https://github.com/Blazam-App/BLAZAM.git
git fetch upstream
```

### Recommended branching strategy

```
upstream/main  ──────────────── (tracked read-only)
                  ↘ merge/squash
main           ─── [upstream changes merged here, no custom code]
                  ↘ rebase / cherry-pick
custom/adm     ─── [your AD-Management customisations live here]
```

| Branch | Purpose |
|---|---|
| `main` | Mirror of upstream – only upstream merges go here |
| `custom/adm` | **Your working branch** – all rebranding & custom features |

### Workflow to pull a new upstream release

```bash
# 1. Fetch latest from upstream
git fetch upstream

# 2. Fast-forward your main to upstream/main
git checkout main
git merge --ff-only upstream/main

# 3. Rebase your custom branch on top of the new main
git checkout custom/adm
git rebase main

# 4. Resolve any conflicts, then push
git push origin custom/adm --force-with-lease
```

> **Tip:** If the upstream release is large and conflicts are many, use
> `git cherry-pick` to selectively apply only the upstream commits you want.

### Protecting your customisations

Your customisations are confined to a small set of files:

| File | What was changed |
|---|---|
| `BLAZAMDatabase/Models/AppSettings.cs` | Default app name → "AD-Management" |
| `BLAZAM/Pages/_Host.cshtml` | Reconnect dialog text |
| `BLAZAMGui/UI/Modals/AboutAppModalContent.razor` | Removed blazam.org links |
| `BLAZAM/Pages/Privacy.razor` | Rebranded BLAZAM → AD-Management |
| `BLAZAMGui/UI/Outputs/AppDocumentationButton.razor` | Docs URL → GitHub wiki |
| `BLAZAMGui/Navs/Buttons/AppUserButton.razor` | Docs URL → GitHub wiki |
| `BLAZAMServices/Background/ApplicationNewsService.cs` | Disabled upstream news API |

Because these are focused, minimal changes, rebase conflicts will be easy to resolve.

---

## 3. Updating the App Name at Runtime

Even after deployment you can change the displayed name without redeploying:

1. Log in as a **Super Admin**.
2. Navigate to **Settings → Application**.
3. Change **App Name** to `AD-Management` (or any label you prefer).
4. Save – the change takes effect immediately site-wide.
