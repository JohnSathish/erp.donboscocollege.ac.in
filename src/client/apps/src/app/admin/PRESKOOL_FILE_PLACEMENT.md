# Preskool Template - Quick File Placement Guide

## 🎯 Where to Place Your Preskool Template Files

### Option 1: If Preskool is a ZIP file

1. **Extract the ZIP** to a temporary folder (e.g., `C:\Temp\preskool-template\`)

2. **Identify the structure:**
   - Look for `assets/` folder (CSS, JS, images, fonts)
   - Look for `src/` or `app/` folder (Angular components)
   - Look for HTML files (dashboard pages)

### Option 2: If Preskool is already extracted

Navigate to where you extracted it and follow the steps below.

---

## 📂 Step-by-Step File Placement

### **STEP 1: Create Folders**

Run these commands in PowerShell (from project root `D:\Projects\ERP`):

```powershell
# Create public assets folder
New-Item -ItemType Directory -Force -Path "src\client\apps\public\preskool-assets"
New-Item -ItemType Directory -Force -Path "src\client\apps\public\preskool-assets\css"
New-Item -ItemType Directory -Force -Path "src\client\apps\public\preskool-assets\js"
New-Item -ItemType Directory -Force -Path "src\client\apps\public\preskool-assets\images"
New-Item -ItemType Directory -Force -Path "src\client\apps\public\preskool-assets\fonts"

# Create template reference folder
New-Item -ItemType Directory -Force -Path "src\client\apps\src\app\admin\preskool-template"
```

### **STEP 2: Copy Assets**

**From Preskool template, copy:**

```
Preskool/assets/css/*     →  src/client/apps/public/preskool-assets/css/
Preskool/assets/js/*      →  src/client/apps/public/preskool-assets/js/
Preskool/assets/images/*  →  src/client/apps/public/preskool-assets/images/
Preskool/assets/fonts/*   →  src/client/apps/public/preskool-assets/fonts/
```

**PowerShell commands (adjust paths as needed):**

```powershell
# Replace "C:\Path\To\Preskool" with your actual Preskool template path
$preskoolPath = "C:\Path\To\Preskool"

# Copy CSS
Copy-Item -Path "$preskoolPath\assets\css\*" -Destination "src\client\apps\public\preskool-assets\css\" -Recurse -Force

# Copy JS
Copy-Item -Path "$preskoolPath\assets\js\*" -Destination "src\client\apps\public\preskool-assets\js\" -Recurse -Force

# Copy Images
Copy-Item -Path "$preskoolPath\assets\images\*" -Destination "src\client\apps\public\preskool-assets\images\" -Recurse -Force

# Copy Fonts
Copy-Item -Path "$preskoolPath\assets\fonts\*" -Destination "src\client\apps\public\preskool-assets\fonts\" -Recurse -Force
```

### **STEP 3: Copy Template Source (for reference)**

**Copy the entire Preskool template structure:**

```
Preskool/*  →  src/client/apps/src/app/admin/preskool-template/
```

**PowerShell command:**

```powershell
# Replace "C:\Path\To\Preskool" with your actual Preskool template path
$preskoolPath = "C:\Path\To\Preskool"
Copy-Item -Path "$preskoolPath\*" -Destination "src\client\apps\src\app\admin\preskool-template\" -Recurse -Force
```

---

## 📋 What I Need From You

After you place the files, please tell me:

1. **Main CSS file name:** What's the main CSS file? (e.g., `style.css`, `main.css`, `app.css`)
2. **Main JS file name:** What's the main JS file? (e.g., `app.js`, `main.js`, `bundle.js`)
3. **Dashboard HTML file:** Which HTML file is the admin dashboard? (e.g., `index.html`, `admin-dashboard.html`)
4. **Chart library:** Does Preskool use ApexCharts or another library?
5. **Any errors:** If you see any errors while copying files

---

## ✅ Verification

After copying, verify the structure looks like this:

```
src/client/
├── apps/
│   ├── public/
│   │   └── preskool-assets/          ← NEW
│   │       ├── css/
│   │       ├── js/
│   │       ├── images/
│   │       └── fonts/
│   └── src/
│       └── app/
│           └── admin/
│               └── preskool-template/ ← NEW
│                   └── (all Preskool files)
```

---

## 🚀 Next Steps

Once files are placed:
1. ✅ I'll update `index.html` to include CSS/JS
2. ✅ I'll extract the dashboard HTML and convert to Angular component
3. ✅ I'll integrate with your existing API
4. ✅ I'll ensure mobile responsiveness
5. ✅ I'll test everything works

**Ready? Place the files and let me know!** 🎉



