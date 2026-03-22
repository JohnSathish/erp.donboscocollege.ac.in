# Entrance Exam Registration Process - Complete Workflow

## Overview
The Entrance Exam Registration system allows applicants to register for entrance exams, tracks their attendance, and records their scores. This document explains the complete workflow from exam creation to score entry.

## Process Flow

### 1. **Exam Creation** (Admin)
- Admin creates an entrance exam with:
  - Exam Name & Code (e.g., "BSC2025")
  - Exam Date & Time (Start/End)
  - Venue & Address
  - Maximum Capacity
  - Description & Instructions
- Exam status: Active/Inactive
- Initial registrations count: 0

**API Endpoint:** `POST /api/admissions/entrance-exams`

---

### 2. **Registration Eligibility Check**
Before an applicant can register, the system checks:
- ✅ Exam is **Active** (`IsActive = true`)
- ✅ Exam has **available slots** (`CurrentRegistrations < MaxCapacity`)
- ✅ Exam date is **not in the past** (`ExamDate >= Today`)
- ✅ Applicant is **not already registered** for this exam

**Validation Logic:**
```csharp
public bool CanRegister()
{
    return IsActive && 
           CurrentRegistrations < MaxCapacity && 
           ExamDate >= DateTime.UtcNow.Date;
}
```

---

### 3. **Applicant Registration** (Admin or System)
When an applicant registers for an exam:

**API Endpoint:** `POST /api/admissions/entrance-exams/{examId}/register/{applicantId}`

**What Happens:**
1. System validates exam eligibility (checks `CanRegister()`)
2. Checks if applicant is already registered
3. Creates new `ExamRegistration` record with:
   - `ExamId` - Links to the exam
   - `ApplicantAccountId` - Links to the applicant
   - `RegisteredOnUtc` - Timestamp
   - `RegisteredBy` - Admin who registered (optional)
4. **Generates Hall Ticket Number** automatically:
   - Format: `HT-{ExamCode}-{ApplicantUniqueId}-{Last4DigitsOfId}`
   - Example: `HT-BSC2025-APP001-3a4f`
5. **Increments exam's registration count** (`CurrentRegistrations++`)
6. Saves registration to database

**Example:**
```
Exam: BSC2025 (Max Capacity: 100)
Before: CurrentRegistrations = 50
After Registration: CurrentRegistrations = 51
```

---

### 4. **Viewing Registrations** (Admin)
Admin can view all registrations for an exam:

**API Endpoint:** `GET /api/admissions/entrance-exams/{examId}/registrations`

**Information Displayed:**
- Hall Ticket Number
- Applicant Name & Unique ID
- Registration Date
- Attendance Status (Present/Absent)
- Exam Score
- Actions (View Applicant Details)

**Filters Available:**
- Search by name, ID, or hall ticket
- Filter by attendance status (Present/Absent/All)

---

### 5. **Marking Attendance** (Admin - On Exam Day)
On the day of the exam, admin marks attendance:

**API Endpoint:** `PATCH /api/admissions/entrance-exams/{examId}/registrations/{registrationId}/attendance`

**What Happens:**
1. Admin marks each registration as Present or Absent
2. System records:
   - `IsPresent` (true/false)
   - `AttendanceMarkedOnUtc` (timestamp)
   - `AttendanceMarkedBy` (admin username)
3. Updates registration record

**Note:** Attendance can be marked before or after the exam date.

---

### 6. **Entering Exam Scores** (Admin - After Exam)
After the exam is conducted, admin enters scores:

**API Endpoint:** `PATCH /api/admissions/entrance-exams/{examId}/registrations/{registrationId}/score`

**What Happens:**
1. Admin enters the score (decimal number, cannot be negative)
2. System records:
   - `Score` (the actual score)
   - `ScoreEnteredOnUtc` (timestamp)
   - `ScoreEnteredBy` (admin username)
3. Updates registration record

**Validation:**
- Score must be >= 0
- Can be entered multiple times (updates existing score)

---

## Data Model

### ExamRegistration Entity
```csharp
- Id: Guid (unique registration ID)
- ExamId: Guid (links to EntranceExam)
- ApplicantAccountId: Guid (links to StudentApplicantAccount)
- HallTicketNumber: string (auto-generated, e.g., "HT-BSC2025-APP001-3a4f")
- IsPresent: bool (attendance status)
- Score: decimal? (exam score, nullable)
- RegisteredOnUtc: DateTime (when registered)
- RegisteredBy: string? (who registered)
- AttendanceMarkedOnUtc: DateTime? (when attendance was marked)
- AttendanceMarkedBy: string? (who marked attendance)
- ScoreEnteredOnUtc: DateTime? (when score was entered)
- ScoreEnteredBy: string? (who entered score)
```

### EntranceExam Entity (Relevant Fields)
```csharp
- Id: Guid
- ExamCode: string (e.g., "BSC2025")
- ExamName: string
- ExamDate: DateTime
- MaxCapacity: int (maximum registrations allowed)
- CurrentRegistrations: int (current count)
- IsActive: bool (can accept registrations)
```

---

## Complete Workflow Example

### Scenario: BSC 2025 Entrance Exam

**Step 1: Admin Creates Exam**
```
POST /api/admissions/entrance-exams
{
  "examName": "Bachelor of Science Entrance Exam 2025",
  "examCode": "BSC2025",
  "examDate": "2025-06-15",
  "examStartTime": "09:00",
  "examEndTime": "12:00",
  "venue": "Main Hall",
  "maxCapacity": 100
}
Result: Exam created with CurrentRegistrations = 0
```

**Step 2: Applicant Registers**
```
POST /api/admissions/entrance-exams/{examId}/register/{applicantId}
Result: 
- Registration created
- Hall Ticket: HT-BSC2025-APP001-3a4f
- CurrentRegistrations = 1
```

**Step 3: More Applicants Register**
```
50 applicants register
CurrentRegistrations = 50
Available slots: 50
```

**Step 4: Exam Day - Mark Attendance**
```
PATCH /api/admissions/entrance-exams/{examId}/registrations/{registrationId}/attendance
{
  "isPresent": true
}
Result: IsPresent = true, AttendanceMarkedOnUtc = now
```

**Step 5: After Exam - Enter Scores**
```
PATCH /api/admissions/entrance-exams/{examId}/registrations/{registrationId}/score
{
  "score": 85.5
}
Result: Score = 85.5, ScoreEnteredOnUtc = now
```

---

## Key Features

### ✅ Automatic Hall Ticket Generation
- Format: `HT-{ExamCode}-{ApplicantUniqueId}-{Last4Digits}`
- Unique for each registration
- Generated automatically when registration is created

### ✅ Capacity Management
- Exam tracks `CurrentRegistrations` vs `MaxCapacity`
- Prevents over-registration
- Auto-increments/decrements when registrations are added/removed

### ✅ Attendance Tracking
- Can mark Present or Absent
- Tracks who marked attendance and when
- Can be filtered in the registrations list

### ✅ Score Management
- Stores decimal scores
- Tracks who entered the score and when
- Can be updated if needed

### ✅ Registration Validation
- Prevents duplicate registrations
- Checks exam availability before allowing registration
- Validates exam is active and not past

---

## Admin UI Workflow

1. **View Exams List** → See all entrance exams
2. **Click "View"** → See exam details
3. **Click "Registrations"** → See all registrations for that exam
4. **Mark Attendance** → Click on each registration to mark Present/Absent
5. **Enter Scores** → Click on each registration to enter exam score
6. **View Applicant** → Click "View Applicant" to see full applicant details

---

## Important Notes

⚠️ **Registration Count**: The `CurrentRegistrations` field is automatically managed. When you add a registration, it increments. When you delete a registration, it decrements.

⚠️ **Hall Ticket**: Generated automatically and cannot be manually changed.

⚠️ **Score Entry**: Can be done multiple times (updates existing score). Score cannot be negative.

⚠️ **Attendance**: Can be marked as Present or Absent. Default is Absent (false).

⚠️ **Exam Status**: Only Active exams can accept new registrations. Inactive exams cannot accept registrations even if they have capacity.

---

## API Endpoints Summary

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/entrance-exams` | POST | Create new exam |
| `/entrance-exams/{id}` | GET | Get exam details |
| `/entrance-exams/{id}/edit` | PUT | Update exam |
| `/entrance-exams/{examId}/register/{applicantId}` | POST | Register applicant |
| `/entrance-exams/{examId}/registrations` | GET | List registrations |
| `/entrance-exams/{examId}/registrations/{registrationId}/attendance` | PATCH | Mark attendance |
| `/entrance-exams/{examId}/registrations/{registrationId}/score` | PATCH | Enter score |

---

## Database Relationships

```
EntranceExam (1) ──── (Many) ExamRegistration (Many) ──── (1) StudentApplicantAccount
     │                              │                              │
     │                              │                              │
  ExamId                    ApplicantAccountId              Account Details
  ExamCode                  HallTicketNumber                UniqueId, Name, Email
  MaxCapacity               IsPresent                      MobileNumber
  CurrentRegistrations      Score
  IsActive                  RegisteredOnUtc
```

---

This system provides a complete workflow for managing entrance exams from creation through registration, attendance, and score entry.









