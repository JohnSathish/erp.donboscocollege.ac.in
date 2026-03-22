import { Injectable, signal, computed } from '@angular/core';

export interface NavigationItem {
  label: string;
  icon: string;
  route?: string;
  children?: NavigationItem[];
  expanded?: boolean;
}

@Injectable({ providedIn: 'root' })
export class AdminNavigationService {
  private readonly navigationItems = signal<NavigationItem[]>([
    {
      label: 'Home',
      icon: '🏠',
      route: '/admin',
    },
    {
      label: 'Dashboard',
      icon: '📊',
      route: '/admin/dashboard',
    },
    {
      label: 'Online Admission',
      icon: '📝',
      expanded: false,
      children: [
        {
          label: 'Admission workflow',
          icon: '📋',
          route: '/admin/admissions/workflow',
        },
        {
          label: 'Entrance Exam Management',
          icon: '✏️',
          route: '/admin/admissions/exams',
        },
        {
          label: 'Application Form',
          icon: '📋',
          route: '/admin/admissions/applications',
        },
        {
          label: 'Admitted Students',
          icon: '🎓',
          route: '/admin/admissions/admitted-students',
        },
        {
          label: 'Document Verification System',
          icon: '✅',
          route: '/admin/admissions/verification',
        },
        {
          label: 'Merit List & Selection',
          icon: '🏆',
          route: '/admin/admissions/merit-list',
        },
            {
              label: 'Direct Admission',
              icon: '🎓',
              route: '/admin/admissions/direct-admission',
            },
            {
              label: 'Send Individual Offer',
              icon: '✉️',
              route: '/admin/admissions/send-offer',
            },
        {
          label: 'Admission Approval',
          icon: '✓',
          route: '/admin/admissions/approval',
        },
        {
          label: 'Payment Verification',
          icon: '💰',
          route: '/admin/admissions/payment-verification',
        },
      ],
    },
    {
      label: 'Offline Admission',
      icon: '📄',
      route: '/admin/admissions/offline',
    },
    {
      label: 'Student Management',
      icon: '👥',
      expanded: false,
      children: [
        {
          label: 'Student List',
          icon: '📋',
          route: '/admin/students/list',
        },
        {
          label: 'Class & Section Allocation',
          icon: '🏫',
          route: '/admin/students/class-allocation',
        },
        {
          label: 'Student Attendance',
          icon: '📅',
          route: '/admin/students/attendance',
        },
        {
          label: 'Student Promotion',
          icon: '⬆️',
          route: '/admin/students/promotion',
        },
        {
          label: 'ID Card / Certificates',
          icon: '🪪',
          route: '/admin/students/id-cards',
        },
      ],
    },
    {
      label: 'Staff Management',
      icon: '👤',
      expanded: false,
      children: [
        {
          label: 'Staff List',
          icon: '📋',
          route: '/admin/staff/list',
        },
        {
          label: 'Teaching Staff',
          icon: '👨‍🏫',
          route: '/admin/staff/teaching',
        },
        {
          label: 'Non-Teaching Staff',
          icon: '👨‍💼',
          route: '/admin/staff/non-teaching',
        },
        {
          label: 'Attendance',
          icon: '📅',
          route: '/admin/staff/attendance',
        },
        {
          label: 'Leave Management',
          icon: '🏖️',
          route: '/admin/staff/leave',
        },
      ],
    },
    {
      label: 'Academics',
      icon: '📚',
      expanded: false,
      children: [
        {
          label: 'Lesson Plan',
          icon: '📖',
          route: '/admin/academics/lesson-plan',
        },
        {
          label: 'Syllabus',
          icon: '📑',
          route: '/admin/academics/syllabus',
        },
        {
          label: 'Time Table',
          icon: '⏰',
          route: '/admin/academics/timetable',
        },
        {
          label: 'Homework',
          icon: '📝',
          route: '/admin/academics/homework',
        },
        {
          label: 'Question Bank',
          icon: '❓',
          route: '/admin/academics/question-bank',
        },
      ],
    },
    {
      label: 'Examination',
      icon: '📝',
      expanded: false,
      children: [
        {
          label: 'Assessments',
          icon: '📋',
          route: '/admin/examination/assessments',
        },
        {
          label: 'Marks Entry',
          icon: '✍️',
          route: '/admin/examination/marks-entry',
        },
        {
          label: 'Result Processing',
          icon: '📊',
          route: '/admin/examination/result-processing',
        },
        {
          label: 'Report Cards',
          icon: '📄',
          route: '/admin/examination/report-cards',
        },
        {
          label: 'Exam Schedule',
          icon: '📅',
          route: '/admin/examination/schedule',
        },
      ],
    },
    {
      label: 'Fee Management',
      icon: '💰',
      expanded: false,
      children: [
        {
          label: 'Fee Collection',
          icon: '💵',
          route: '/admin/fees/collection',
        },
        {
          label: 'Fee Reports',
          icon: '📊',
          route: '/admin/fees/reports',
        },
        {
          label: 'Concession / Waiver',
          icon: '🎫',
          route: '/admin/fees/concession',
        },
        {
          label: 'Pending Dues',
          icon: '⚠️',
          route: '/admin/fees/pending-dues',
        },
      ],
    },
    {
      label: 'Communication',
      icon: '📢',
      expanded: false,
      children: [
        {
          label: 'Notice Board',
          icon: '📌',
          route: '/admin/communication/notice-board',
        },
        {
          label: 'SMS / Email',
          icon: '📧',
          route: '/admin/communication/sms-email',
        },
        {
          label: 'Circulars',
          icon: '📜',
          route: '/admin/communication/circulars',
        },
        {
          label: 'Parent Notifications',
          icon: '🔔',
          route: '/admin/communication/parent-notifications',
        },
      ],
    },
    {
      label: 'Front Office',
      icon: '🏢',
      expanded: false,
      children: [
        {
          label: 'Visitor Management',
          icon: '👤',
          route: '/admin/front-office/visitors',
        },
        {
          label: 'Enquiry Register',
          icon: '❓',
          route: '/admin/front-office/enquiry',
        },
        {
          label: 'Complaint Register',
          icon: '📝',
          route: '/admin/front-office/complaints',
        },
      ],
    },
    {
      label: 'Transport',
      icon: '🚌',
      expanded: false,
      children: [
        {
          label: 'Vehicle Details',
          icon: '🚗',
          route: '/admin/transport/vehicles',
        },
        {
          label: 'Routes',
          icon: '🗺️',
          route: '/admin/transport/routes',
        },
        {
          label: 'Driver Information',
          icon: '👨‍✈️',
          route: '/admin/transport/drivers',
        },
      ],
    },
    {
      label: 'Library',
      icon: '📚',
      expanded: false,
      children: [
        {
          label: 'Book List',
          icon: '📖',
          route: '/admin/library/books',
        },
        {
          label: 'Issue / Return',
          icon: '📥',
          route: '/admin/library/issue-return',
        },
        {
          label: 'Reports',
          icon: '📊',
          route: '/admin/library/reports',
        },
      ],
    },
    {
      label: 'Hostel',
      icon: '🏠',
      expanded: false,
      children: [
        {
          label: 'Hostel Rooms',
          icon: '🛏️',
          route: '/admin/hostel/rooms',
        },
        {
          label: 'Student Allocation',
          icon: '👥',
          route: '/admin/hostel/student-allocation',
        },
        {
          label: 'Hostel Fees',
          icon: '💰',
          route: '/admin/hostel/fees',
        },
      ],
    },
    {
      label: 'Settings',
      icon: '⚙️',
      expanded: false,
      children: [
        {
          label: 'User Management',
          icon: '👥',
          route: '/admin/settings/users',
        },
        {
          label: 'Roles & Permissions',
          icon: '🔐',
          route: '/admin/settings/roles',
        },
        {
          label: 'Academic Year Settings',
          icon: '📅',
          route: '/admin/settings/academic-year',
        },
      ],
    },
  ]);

  readonly items = this.navigationItems.asReadonly();

  toggleExpanded(index: number): void {
    const items = [...this.navigationItems()];
    if (items[index].children) {
      items[index].expanded = !items[index].expanded;
      this.navigationItems.set(items);
    }
  }
}
