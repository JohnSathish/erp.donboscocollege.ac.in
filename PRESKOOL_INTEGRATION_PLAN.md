# Preskool Template Integration Plan - Clean Start

## Overview
Create a clean project structure with proper Preskool template integration for:
1. **Admissions System** (admissions.donboscocollege.ac.in)
2. **ERP System** (erp.donboscocollege.ac.in)

## Step-by-Step Plan

### Phase 1: Extract Preskool Core Components
1. ✅ Copy Preskool assets (CSS, JS, images, fonts)
2. ✅ Extract layout components (Header, Sidebar, Layout)
3. ✅ Extract shared services (SidebarService, SettingsService, CommonService, DataService)
4. ✅ Create reusable Preskool layout library

### Phase 2: Create Admissions System
1. Create new Angular app for admissions
2. Integrate Preskool layout
3. Build admission features
4. Connect to existing backend

### Phase 3: Create ERP System  
1. Create new Angular app for ERP
2. Integrate Preskool layout
3. Create role-based dashboards (Admin, Student, Staff, Parent)
4. Implement RBAC
5. Connect to existing backend

## Key Decisions

1. **Keep existing backend** - No changes to server code
2. **Use Preskool template** - Properly extracted and adapted
3. **Separate apps** - Admissions and ERP are independent
4. **Shared layout library** - Preskool components reused in both apps
5. **Type-safe** - Proper TypeScript from the start

## Next Steps

1. Create shared Preskool layout library
2. Set up admissions app structure
3. Set up ERP app structure
4. Integrate Preskool layout in both
5. Build dashboards

