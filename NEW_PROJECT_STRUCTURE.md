# New Clean Project Structure with Preskool Template

## Project Architecture

```
ERP-New/
├── src/
│   ├── server/                    # Backend API (keep existing)
│   │   ├── Api/                   # Main API
│   │   ├── Application/           # Business logic
│   │   ├── Domain/                # Domain models
│   │   └── Infrastructure/        # Data access
│   │
│   └── client/                    # Frontend
│       ├── admissions/            # Admissions System (admissions.donboscocollege.ac.in)
│       │   ├── src/
│       │   │   ├── app/
│       │   │   │   ├── features/
│       │   │   │   │   ├── application/      # Application submission
│       │   │   │   │   ├── verification/      # Document verification
│       │   │   │   │   ├── exam/              # Entrance exam
│       │   │   │   │   ├── merit-list/        # Merit list
│       │   │   │   │   └── payment/           # Payment processing
│       │   │   │   ├── shared/
│       │   │   │   │   ├── layout/            # Preskool layout components
│       │   │   │   │   ├── components/        # Reusable components
│       │   │   │   │   └── services/          # API services
│       │   │   │   └── app.config.ts
│       │   │   └── assets/
│       │   │       └── preskool/              # Preskool assets
│       │   └── package.json
│       │
│       └── erp/                   # ERP System (erp.donboscocollege.ac.in)
│           ├── src/
│           │   ├── app/
│           │   │   ├── features/
│           │   │   │   ├── admin/             # Admin dashboard & features
│           │   │   │   ├── student/           # Student dashboard & features
│           │   │   │   ├── staff/             # Staff/Teacher dashboard
│           │   │   │   └── parent/            # Parent dashboard
│           │   │   ├── shared/
│           │   │   │   ├── layout/            # Preskool layout (header, sidebar)
│           │   │   │   ├── components/        # Shared components
│           │   │   │   ├── guards/            # Route guards (RBAC)
│           │   │   │   └── services/          # API services
│           │   │   └── app.config.ts
│           │   └── assets/
│           │       └── preskool/              # Preskool assets
│           └── package.json
│
└── preskool-template/            # Original Preskool template (reference)
    └── src/app/                  # Keep as reference
```

## Integration Plan

### Phase 1: Setup Preskool Template Foundation
1. Copy Preskool assets (CSS, JS, images, fonts) to both projects
2. Extract and adapt Preskool layout components (header, sidebar, layout)
3. Set up Preskool services (SidebarService, SettingsService, etc.)
4. Configure Preskool routes properly

### Phase 2: Admissions System
1. Create clean admissions app structure
2. Integrate Preskool layout
3. Build admission-specific features
4. Connect to existing backend APIs

### Phase 3: ERP System
1. Create ERP app structure with role-based dashboards
2. Integrate Preskool layout
3. Build role-specific dashboards (Admin, Student, Staff, Parent)
4. Implement module-based access control
5. Connect to existing backend APIs

## Key Improvements Over Previous Approach

1. **Clean Separation**: Admissions and ERP are completely separate apps
2. **Proper Template Integration**: Preskool components properly extracted and reused
3. **Type Safety**: Proper TypeScript types from the start
4. **Modular Structure**: Each feature is self-contained
5. **RBAC Ready**: Role-based access control built in from the start

