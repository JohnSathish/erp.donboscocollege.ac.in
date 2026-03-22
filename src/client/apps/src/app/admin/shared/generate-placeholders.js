const fs = require('fs');
const path = require('path');

const components = [
  { path: 'students/list', name: 'admin-student-list', title: 'Student List', icon: '👥' },
  { path: 'students/class-allocation', name: 'admin-class-allocation', title: 'Class & Section Allocation', icon: '🏫' },
  { path: 'students/attendance', name: 'admin-student-attendance', title: 'Student Attendance', icon: '📅' },
  { path: 'students/promotion', name: 'admin-student-promotion', title: 'Student Promotion', icon: '⬆️' },
  { path: 'students/id-cards', name: 'admin-id-cards', title: 'ID Card / Certificates', icon: '🪪' },
  { path: 'staff/list', name: 'admin-staff-list', title: 'Staff List', icon: '👤' },
  { path: 'staff/teaching', name: 'admin-teaching-staff', title: 'Teaching Staff', icon: '👨‍🏫' },
  { path: 'staff/non-teaching', name: 'admin-non-teaching-staff', title: 'Non-Teaching Staff', icon: '👨‍💼' },
  { path: 'staff/attendance', name: 'admin-staff-attendance', title: 'Staff Attendance', icon: '📅' },
  { path: 'staff/leave', name: 'admin-staff-leave', title: 'Leave Management', icon: '🏖️' },
  { path: 'academics/lesson-plan', name: 'admin-lesson-plan', title: 'Lesson Plan', icon: '📖' },
  { path: 'academics/syllabus', name: 'admin-syllabus', title: 'Syllabus', icon: '📑' },
  { path: 'academics/timetable', name: 'admin-timetable', title: 'Time Table', icon: '⏰' },
  { path: 'academics/homework', name: 'admin-homework', title: 'Homework', icon: '📝' },
  { path: 'academics/question-bank', name: 'admin-question-bank', title: 'Question Bank', icon: '❓' },
  { path: 'examination/schedule', name: 'admin-exam-schedule', title: 'Exam Schedule', icon: '📅' },
  { path: 'examination/marks-entry', name: 'admin-marks-entry', title: 'Marks Entry', icon: '✍️' },
  { path: 'examination/result-processing', name: 'admin-result-processing', title: 'Result Processing', icon: '📊' },
  { path: 'examination/report-cards', name: 'admin-report-cards', title: 'Report Cards', icon: '📄' },
  { path: 'fees/collection', name: 'admin-fee-collection', title: 'Fee Collection', icon: '💵' },
  { path: 'fees/reports', name: 'admin-fee-reports', title: 'Fee Reports', icon: '📊' },
  { path: 'fees/concession', name: 'admin-fee-concession', title: 'Concession / Waiver', icon: '🎫' },
  { path: 'fees/pending-dues', name: 'admin-pending-dues', title: 'Pending Dues', icon: '⚠️' },
  { path: 'communication/notice-board', name: 'admin-notice-board', title: 'Notice Board', icon: '📌' },
  { path: 'communication/sms-email', name: 'admin-sms-email', title: 'SMS / Email', icon: '📧' },
  { path: 'communication/circulars', name: 'admin-circulars', title: 'Circulars', icon: '📜' },
  { path: 'communication/parent-notifications', name: 'admin-parent-notifications', title: 'Parent Notifications', icon: '🔔' },
  { path: 'front-office/visitors', name: 'admin-visitor-management', title: 'Visitor Management', icon: '👤' },
  { path: 'front-office/enquiry', name: 'admin-enquiry-register', title: 'Enquiry Register', icon: '❓' },
  { path: 'front-office/complaints', name: 'admin-complaint-register', title: 'Complaint Register', icon: '📝' },
  { path: 'transport/vehicles', name: 'admin-vehicle-details', title: 'Vehicle Details', icon: '🚗' },
  { path: 'transport/routes', name: 'admin-routes', title: 'Routes', icon: '🗺️' },
  { path: 'transport/drivers', name: 'admin-driver-information', title: 'Driver Information', icon: '👨‍✈️' },
  { path: 'library/books', name: 'admin-book-list', title: 'Book List', icon: '📖' },
  { path: 'library/issue-return', name: 'admin-issue-return', title: 'Issue / Return', icon: '📥' },
  { path: 'library/reports', name: 'admin-library-reports', title: 'Library Reports', icon: '📊' },
  { path: 'hostel/rooms', name: 'admin-hostel-rooms', title: 'Hostel Rooms', icon: '🛏️' },
  { path: 'hostel/student-allocation', name: 'admin-hostel-student-allocation', title: 'Student Allocation', icon: '👥' },
  { path: 'hostel/fees', name: 'admin-hostel-fees', title: 'Hostel Fees', icon: '💰' },
  { path: 'settings/users', name: 'admin-user-management', title: 'User Management', icon: '👥' },
  { path: 'settings/roles', name: 'admin-roles-permissions', title: 'Roles & Permissions', icon: '🔐' },
  { path: 'settings/academic-year', name: 'admin-academic-year-settings', title: 'Academic Year Settings', icon: '📅' },
];

const baseDir = __dirname.replace(/\\shared$/, '');

function toPascalCase(str) {
  // Remove 'admin-' prefix and convert to PascalCase
  const withoutPrefix = str.replace(/^admin-/, '');
  return 'Admin' + withoutPrefix
    .split('-')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join('');
}

function generateComponent(comp) {
  const dir = path.join(baseDir, comp.path);
  const className = toPascalCase(comp.name.replace('admin-', ''));
  const componentName = comp.name;
  
  const content = `import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaceholderComponent } from '../../shared/placeholder.component';

@Component({
  selector: 'app-${componentName}',
  standalone: true,
  imports: [CommonModule, PlaceholderComponent],
  template: \`<app-placeholder icon="${comp.icon}" title="${comp.title}" description="This feature is under development and will be available soon."></app-placeholder>\`,
})
export class ${className}Component {}
`;

  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }

  const filePath = path.join(dir, `${componentName}.component.ts`);
  fs.writeFileSync(filePath, content, 'utf8');
  console.log(`Updated: ${filePath}`);
}

components.forEach(generateComponent);
console.log('Done!');

