# Preskool Template Integration Guide

## 📁 File Placement Structure

### Step 1: Extract Template Files

When you extract the Preskool template, you'll typically find these folders:
- `assets/` (CSS, JS, images, fonts)
- `src/` (Angular source files)
- `angular.json` or similar config files

### Step 2: Place Template Files

#### **A. Template Source Files (Reference)**
```
src/client/apps/src/app/admin/
└── preskool-template/          ← CREATE THIS FOLDER
    ├── assets/                  ← Copy template assets here
    │   ├── css/
    │   ├── js/
    │   ├── images/
    │   └── fonts/
    ├── components/              ← Copy Angular components here (for reference)
    └── README.md                ← Template documentation
```

#### **B. Public Assets (Static Files)**
```
src/client/apps/public/
└── preskool-assets/             ← CREATE THIS FOLDER
    ├── css/                     ← Copy CSS files here
    ├── js/                      ← Copy JS files here
    ├── images/                  ← Copy images here
    └── fonts/                   ← Copy fonts here
```

#### **C. Angular Components (Integration)**
```
src/client/apps/src/app/admin/
├── dashboard/
│   ├── admin-dashboard-preskool.component.ts    ← NEW: Preskool dashboard
│   ├── admin-dashboard-preskool.component.html ← NEW: Preskool dashboard HTML
│   └── admin-dashboard-preskool.component.scss ← NEW: Preskool dashboard styles
└── shared/
    └── preskool/                ← CREATE THIS FOLDER
        ├── widgets/             ← Reusable widgets from template
        ├── layouts/             ← Layout components
        └── components/         ← Shared components
```

## 📋 Integration Steps

### Step 1: Copy Assets to Public Folder

1. **Create the folder:**
   ```powershell
   mkdir src\client\apps\public\preskool-assets
   ```

2. **Copy these folders from Preskool template:**
   - `assets/css/` → `public/preskool-assets/css/`
   - `assets/js/` → `public/preskool-assets/js/`
   - `assets/images/` → `public/preskool-assets/images/`
   - `assets/fonts/` → `public/preskool-assets/fonts/`

### Step 2: Copy Template Reference Files

1. **Create the folder:**
   ```powershell
   mkdir src\client\apps\src\app\admin\preskool-template
   ```

2. **Copy the entire Preskool template structure here** (for reference and component extraction)

### Step 3: Update index.html

Add Preskool CSS and JS files to `src/client/apps/src/index.html`:

```html
<!-- Add before closing </head> tag -->
<link rel="stylesheet" href="/preskool-assets/css/style.css">
<!-- Add any other Preskool CSS files here -->

<!-- Add before closing </body> tag -->
<script src="/preskool-assets/js/app.js"></script>
<!-- Add any other Preskool JS files here -->
```

### Step 4: Create New Dashboard Component

We'll create a new Preskool-styled dashboard component that integrates with your existing API.

## 🎯 What to Copy from Preskool Template

### Essential Files:
1. **CSS Files:**
   - Main stylesheet (usually `style.css` or `main.css`)
   - Component-specific CSS files
   - Icon fonts CSS (if different from current)

2. **JavaScript Files:**
   - Main app.js
   - Chart libraries (if different from ApexCharts)
   - Calendar libraries
   - Form validation libraries

3. **Images:**
   - Logo files
   - Avatar placeholders
   - Dashboard icons/images
   - Any UI element images

4. **Fonts:**
   - Custom fonts used in template

### Components to Extract:
1. **Dashboard Widgets:**
   - KPI cards
   - Chart containers
   - Notification widgets
   - Calendar widget
   - Todo list widget
   - Attendance widgets
   - Fees collection widgets

2. **Layout Components:**
   - Header/Navbar
   - Sidebar
   - Footer (if needed)

3. **Shared Components:**
   - Buttons
   - Cards
   - Tables
   - Forms
   - Modals

## ⚠️ Important Notes

1. **Don't Overwrite Existing Files:**
   - Keep your current `admin-shell.component.ts/html`
   - Keep your current `admin-dashboard.component.ts/html` (we'll create a new one)
   - Keep your existing routes

2. **Dependency Check:**
   - Check if Preskool uses different chart libraries (you have ApexCharts)
   - Check if Preskool uses different date pickers
   - Check if Preskool uses different form libraries

3. **Angular Version Compatibility:**
   - Preskool might be built with an older Angular version
   - We'll need to convert components to Angular 20 standalone components
   - We'll need to update to your current routing structure

## 🚀 Next Steps After File Placement

Once you've placed the files, I'll help you:
1. ✅ Extract and convert Preskool dashboard HTML to Angular component
2. ✅ Integrate with your existing API services
3. ✅ Update routing to use new dashboard
4. ✅ Ensure all widgets work with your backend
5. ✅ Test mobile responsiveness
6. ✅ Fix any styling conflicts

## 📝 Checklist

- [ ] Created `preskool-template/` folder in `admin/`
- [ ] Created `preskool-assets/` folder in `public/`
- [ ] Copied CSS files to `public/preskool-assets/css/`
- [ ] Copied JS files to `public/preskool-assets/js/`
- [ ] Copied images to `public/preskool-assets/images/`
- [ ] Copied fonts to `public/preskool-assets/fonts/`
- [ ] Copied template source files to `admin/preskool-template/`
- [ ] Updated `index.html` with CSS/JS references

---

**Ready?** Once you've placed the files, let me know and I'll start the integration! 🎉



