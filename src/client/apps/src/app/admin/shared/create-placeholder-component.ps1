# Script to create placeholder components
$components = @(
    @{Path="admissions/verification"; Name="admin-document-verification"; Title="Document Verification System"; Icon="✅"},
    @{Path="admissions/merit-list"; Name="admin-merit-list"; Title="Merit List & Selection"; Icon="🏆"},
    @{Path="admissions/approval"; Name="admin-admission-approval"; Title="Admission Approval"; Icon="✓"},
    @{Path="admissions/payment"; Name="admin-payment-verification"; Title="Payment Verification"; Icon="💰"},
    @{Path="students/list"; Name="admin-student-list"; Title="Student List"; Icon="👥"},
    @{Path="students/class-allocation"; Name="admin-class-allocation"; Title="Class & Section Allocation"; Icon="🏫"},
    @{Path="students/attendance"; Name="admin-student-attendance"; Title="Student Attendance"; Icon="📅"},
    @{Path="students/promotion"; Name="admin-student-promotion"; Title="Student Promotion"; Icon="⬆️"},
    @{Path="students/id-cards"; Name="admin-id-cards"; Title="ID Card / Certificates"; Icon="🪪"},
    @{Path="staff/list"; Name="admin-staff-list"; Title="Staff List"; Icon="👤"},
    @{Path="staff/teaching"; Name="admin-teaching-staff"; Title="Teaching Staff"; Icon="👨‍🏫"},
    @{Path="staff/non-teaching"; Name="admin-non-teaching-staff"; Title="Non-Teaching Staff"; Icon="👨‍💼"},
    @{Path="staff/attendance"; Name="admin-staff-attendance"; Title="Staff Attendance"; Icon="📅"},
    @{Path="staff/leave"; Name="admin-staff-leave"; Title="Leave Management"; Icon="🏖️"},
    @{Path="academics/lesson-plan"; Name="admin-lesson-plan"; Title="Lesson Plan"; Icon="📖"},
    @{Path="academics/syllabus"; Name="admin-syllabus"; Title="Syllabus"; Icon="📑"},
    @{Path="academics/timetable"; Name="admin-timetable"; Title="Time Table"; Icon="⏰"},
    @{Path="academics/homework"; Name="admin-homework"; Title="Homework"; Icon="📝"},
    @{Path="academics/question-bank"; Name="admin-question-bank"; Title="Question Bank"; Icon="❓"},
    @{Path="examination/schedule"; Name="admin-exam-schedule"; Title="Exam Schedule"; Icon="📅"},
    @{Path="examination/marks-entry"; Name="admin-marks-entry"; Title="Marks Entry"; Icon="✍️"},
    @{Path="examination/result-processing"; Name="admin-result-processing"; Title="Result Processing"; Icon="📊"},
    @{Path="examination/report-cards"; Name="admin-report-cards"; Title="Report Cards"; Icon="📄"},
    @{Path="fees/collection"; Name="admin-fee-collection"; Title="Fee Collection"; Icon="💵"},
    @{Path="fees/reports"; Name="admin-fee-reports"; Title="Fee Reports"; Icon="📊"},
    @{Path="fees/concession"; Name="admin-fee-concession"; Title="Concession / Waiver"; Icon="🎫"},
    @{Path="fees/pending-dues"; Name="admin-pending-dues"; Title="Pending Dues"; Icon="⚠️"},
    @{Path="communication/notice-board"; Name="admin-notice-board"; Title="Notice Board"; Icon="📌"},
    @{Path="communication/sms-email"; Name="admin-sms-email"; Title="SMS / Email"; Icon="📧"},
    @{Path="communication/circulars"; Name="admin-circulars"; Title="Circulars"; Icon="📜"},
    @{Path="communication/parent-notifications"; Name="admin-parent-notifications"; Title="Parent Notifications"; Icon="🔔"},
    @{Path="front-office/visitors"; Name="admin-visitor-management"; Title="Visitor Management"; Icon="👤"},
    @{Path="front-office/enquiry"; Name="admin-enquiry-register"; Title="Enquiry Register"; Icon="❓"},
    @{Path="front-office/complaints"; Name="admin-complaint-register"; Title="Complaint Register"; Icon="📝"},
    @{Path="transport/vehicles"; Name="admin-vehicle-details"; Title="Vehicle Details"; Icon="🚗"},
    @{Path="transport/routes"; Name="admin-routes"; Title="Routes"; Icon="🗺️"},
    @{Path="transport/drivers"; Name="admin-driver-information"; Title="Driver Information"; Icon="👨‍✈️"},
    @{Path="library/books"; Name="admin-book-list"; Title="Book List"; Icon="📖"},
    @{Path="library/issue-return"; Name="admin-issue-return"; Title="Issue / Return"; Icon="📥"},
    @{Path="library/reports"; Name="admin-library-reports"; Title="Library Reports"; Icon="📊"},
    @{Path="hostel/rooms"; Name="admin-hostel-rooms"; Title="Hostel Rooms"; Icon="🛏️"},
    @{Path="hostel/student-allocation"; Name="admin-hostel-student-allocation"; Title="Student Allocation"; Icon="👥"},
    @{Path="hostel/fees"; Name="admin-hostel-fees"; Title="Hostel Fees"; Icon="💰"},
    @{Path="settings/users"; Name="admin-user-management"; Title="User Management"; Icon="👥"},
    @{Path="settings/roles"; Name="admin-roles-permissions"; Title="Roles & Permissions"; Icon="🔐"},
    @{Path="settings/academic-year"; Name="admin-academic-year-settings"; Title="Academic Year Settings"; Icon="📅"}
)

foreach ($comp in $components) {
    $dir = "src/client/apps/src/app/admin/$($comp.Path)"
    $name = $comp.Name
    $title = $comp.Title
    $icon = $comp.Icon
    
    $tsContent = @"
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-$name',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: \`<app-placeholder icon="$icon" title="$title" description="This feature is under development and will be available soon."></app-placeholder>\`,
})
export class ${($name -replace '-', '' -replace 'admin', 'Admin' -split '-' | ForEach-Object { $_.Substring(0,1).ToUpper() + $_.Substring(1) } -join '')}Component {}
"@
    
    New-Item -ItemType Directory -Force -Path $dir | Out-Null
    $tsContent | Out-File -FilePath "$dir/$name.component.ts" -Encoding UTF8
    Write-Host "Created: $dir/$name.component.ts"
}













