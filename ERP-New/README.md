# Don Bosco College ERP System - Clean Architecture

## Project Structure

```
ERP-New/
├── src/
│   ├── server/                    # Backend API (.NET Core)
│   │   ├── Api/                   # Main API with scalability enhancements
│   │   ├── Application/           # Business logic
│   │   ├── Domain/                # Domain models
│   │   └── Infrastructure/        # Data access, caching, queuing
│   │
│   └── client/                    # Frontend (Angular)
│       ├── shared/
│       │   └── preskool/          # Shared Preskool layout library
│       ├── admissions/            # Admissions System
│       │   └── src/app/           # admissions.donboscocollege.ac.in
│       └── erp/                   # ERP System
│           └── src/app/           # erp.donboscocollege.ac.in
│
├── docs/                          # Documentation
└── README.md                      # This file
```

## Domains

- **Admissions**: `admissions.donboscocollege.ac.in`
- **ERP**: `erp.donboscocollege.ac.in`

## Scalability Features

✅ API Rate Limiting
✅ Database Connection Pooling
✅ Response Compression
✅ Health Checks
✅ Request Queuing (Background Jobs)
✅ Redis Caching (Phase 2)
✅ CDN Ready

## Getting Started

1. Backend: `cd src/server/Api && dotnet run`
2. Admissions Frontend: `cd src/client/admissions && npm start`
3. ERP Frontend: `cd src/client/erp && npm start`

