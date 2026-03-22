export const routes = {

  //AUTHENTICATION ROUTES//
  login: '/login',
  login2: '/login-2',
  login3: '/login-3',
  register: '/register',
  register2: '/register-2',
  register3: '/register-3',
  emailVerification: '/email-verification',
  emailVerification2: '/email-verification-2',
  emailVerification3: '/email-verification-3',
  forgotPassword: '/forgot-password',
  forgotPassword2: '/forgot-password-2',
  forgotPassword3: '/forgot-password-3',
  lockScreen: '/lock-screen',
  twoStepVerification: '/two-step-verification',
  success: '/reset-password-success',
  success2: '/reset-password-success-2',
  success3: '/reset-password-success-3',
  twoStepVerification2: '/two-step-verification-2',
  twoStepVerification3: '/two-step-verification-3',
  resetPassword: '/reset-password',
  resetPassword2: '/reset-password-2',
  resetPassword3: '/reset-password-3',

  // ERROR PAGES//
   error404: '/error/error-404',
  error500: '/error/error-500',

  // FEATURE ROUTES//

  //dashboard //
  adminDashboard: '/preskool/dashboard',
  parentDashboard: '/preskool/parent-dashboard',
  teacherDashboard: '/preskool/teacher-dashboard',
  studentDashboard: '/preskool/student-dashboard',

  //Application//
   chat: '/application/chat',
  email: '/application/email',
  fileManager: '/application/file-manager',
  callHistory: '/application/call-history',
  fileArchived: '/application/file-archived',
  fileDocument: '/application/file-document',
  fileFavorites: '/application/file-favourites',
  fileManagerDeleted: '/application/file-manager-deleted',
  fileRecent: '/application/file-recent',
  audioCall: '/application/audio-call',
  videoCall: '/application/video-call',
  toDo: '/application/todo',
  notes: '/application/notes',
  fileShared: '/application/file-shared',
  calendar: '/application/calendar',

  // Academic Section//
  academicReasons: '/academic-reasons',
  classRoom: '/class-room',
  classTimeTable: '/class-time-table',
  classSyllabus: '/class-syllabus',
  classSubject: '/class-subject',
  classSection: '/class-section',
  classRoutine: '/class-routine',
  classHomeWork: '/class-home-work',

  // Classes//
  scheduleClasses: '/classes/schedule-classes',
  classes: '/classes/class-list',

  // Examinations//
  grade: '/examinations/grade',
  examResults: '/examinations/exam-results',
  examAttendance: '/examinations/exam-attendance',
  examSchedule: '/examinations/exam-schedule',
  examList: '/examinations/exam-list',

  rolesPermission: '/roles-permission',
  users: '/users',
  deleteAccount: '/delete-account',
  permission: '/permission',

   // Teachers//
  addTeacher: '/admin/staff/list',
  editTeacher: '/admin/staff/list',
  teacherRoutine: '/admin/staff/list',
  teacherLeaves: '/admin/staff/list',
  teacherSalary: '/admin/staff/list',
  teachersList: '/admin/staff/list',
  teacherGrid: '/admin/staff/list',
  teacherDetails: '/admin/staff/list',
  teacherLibrary: '/admin/library/list',

  // Students//
  addStudent: '/admin/students/list',
  editStudent: '/admin/students/list',
  studentDetails: '/admin/students/list',
  studentFees: '/admin/fees/collection',
  studentLeaves: '/admin/students/list',
  studentList: '/admin/students/list',
  studentsList: '/admin/students/list',
  studentsGrid: '/admin/students/list',
  studentGrid: '/admin/students/list',
  studentPromotion: '/admin/students/list',
  studentLibrary: '/admin/library/list',
  studentTimeTable: '/admin/students/list',
  studentResult: '/admin/students/list',

  // Parents / Guardians//
  guardiansList: '/guardians/guardian-list',
  guardiansGrid: '/guardians/guardian-grid',
  parentGrid: '/parents/parent-grid',
  parentList: '/parents/parents-list',

  approveRequest: '/leaves/approve-request',
  listLeaves: '/leaves/list-leaves',

  departments: '/departments',
  designation: '/designation',
  holiday: '/holidays',
  payRoll: '/payroll',

  // Staff Management//
  addStaff: '/staff/add-staff',
  editStaff: '/staff/edit-staff',
  staffDetails: '/staff/staff-details',
  staffLeaves: '/staff/staff-leaves',
  staffPayroll: '/staff/staff-payroll',
  staffList: '/staff/staff-list',

  // Attendance//
  teacherAttendance: '/admin/staff/attendance',
  staffAttendance: '/admin/staff/attendance',
  staffsAttendance: '/admin/staff/attendance',
  studentAttendance: '/admin/students/attendance',

  attendanceReport: '/attendance-report',
  classReport: '/class-report',
  dailyAttendance: '/daily-attendance',
  gradeReport: '/grade-report',
  feesReport: '/fees-report',
  staffDayWise: '/staff-day-wise',
  studentAttendanceType: '/student-attendance-type',
  staffReport: '/staff-report',
  leaveReport: '/leave-report',
  teacherDayWise: '/teacher-day-wise',
  teacherReport: '/teacher-report',
  studentDayWise: '/student-day-wise',
  studentReport: '/student-report',

  // App Settings
  appSettings: '/app-settings',
  customFields: '/app-settings/custom-fields',
  invoiceSettings: '/app-settings/invoice-settings',

  // General Settings
  generalSettings: '/general-settings',
  connectedApps: '/general-settings/connected-apps',
  profileSettings: '/general-settings/profile-settings',
  securitySettings: '/general-settings/security-settings',
  notificationsSettings: '/general-settings/notification-settings',

  // Academic Settings
  academicSettings: '/academic-settings',
  schoolSettings: '/academic-settings/school-settings',
  religion: '/academic-settings/religion',

  // Other Settings
  otherSettings: '/other-settings',
  banIpAddress: '/other-settings/ban-ip-address',
  storage: '/other-settings/storage',

  // System Settings
  systemSettings: '/system-settings',
  emailSettings: '/system-settings/email-settings',
  emailTemplates: '/system-settings/email-templates',
  gdprCookies: '/system-settings/gdpr-cookies',
  otpSettings: '/system-settings/otp-settings',
  smsSettings: '/system-settings/sms-settings',

  // Website Settings
  websiteSettings: '/website-settings',
  localization: '/website-settings/localization',
  companySetting: '/website-settings/company-settings',
  companySettings: '/website-settings/company-settings', // duplicate?
  socialAuthentication: '/website-settings/social-authentication',
  preferences: '/website-settings/preferences',
  prefixes: '/website-settings/prefixes',
  language: '/website-settings/language-settings',

  //finance settings//
  financeAccounts: '/accounts',
  transactions: '/accounts/transactions',
  income: '/accounts/income',
  invoice: '/accounts/invoice',
  accountsInvoices: '/accounts/accounts-invoices',
  addInvoice: '/accounts/add-invoice',
  editInvoice: '/accounts/edit-invoice',
  expenses: '/accounts/expenses',
  expensesCategory: '/accounts/expenses-category',

   contactMessages: '/contact-messages',
  ticketDetails: '/tickets/ticket-details',
  ticketGrid: '/tickets/ticket-grid',
  ticketsList: '/tickets/ticket',
   paymentGateway: '/financial-settings/payment-gateways',
  taxRates: '/financial-settings/tax-rates',


  allBlog: '/blog/all-blog',
  blogCategories: '/blog/blog-categories',
  blogComments: '/blog/blog-comments',
  blogTags: '/blog/blog-tags',
  faq: '/faq',
  testimonials: '/testimonials',
   activities: '/pages/activities',
  profile: '/admin/profile',
  comingSoon: '/pages/coming-soon',
  underMaintenance: '/pages/under-maintenance',
  blankPage: '/pages/blank-page',
  pagesMain: '/pages',
  cities: '/location/cities',
  countries: '/location/countries',
  states: '/location/states',
  // management //
  transportVehicle: '/transport/transport-vehicle',
  transportVehicleDrivers: '/transport/transport-vehicle-drivers',
  transportAssignVehicle: '/transport/transport-assign-vehicle',
  transportPickupPoints: '/transport/transport-pickup-points',
  transportRoutes: '/transport/trasnsport-routes', // NOTE: 'trasnsport' typo – confirm if intentional
  hostelRooms: '/hostel/hostel-rooms',
  hostelRoomType: '/hostel/hostel-room-type',
  hostelList: '/hostel/hostel-list',
  feesGroup: '/fees-collection/fees-group',
  feesMaster: '/fees-collection/fees-master',
  feesType: '/fees-collection/fees-type',
  collectFees: '/admin/fees/collection',
  feesAssign: '/admin/fees/collection',
  players: '/players',
  sports: '/sports',
  membershipAddons: '/membership-addons',
  membershipPlans: '/membership-plans',
  membershipTransactions: '/membership-transactions',
  issueBook: '/library/issue-book',
  returnBook: '/library/return-book',
  books: '/library/books',
  libraryMembers: '/library/library-members',

   events: '/admin/events',
  noticeBoard: '/admin/communication/notice-board',


  // ui interface//
  uiAvatar: '/base-ui/ui-avatar',
  alerts: '/base-ui/ui-alerts',
  uiCards: '/base-ui/ui-cards',
  uiButtons: '/base-ui/ui-buttons',
  uiAccordion: '/base-ui/ui-accordion',
  uiPopovers: '/base-ui/ui-popovers',
  uiPlaceholders: '/base-ui/ui-placeholders',
  uiBadges: '/base-ui/ui-badges',
  uiBreadcrumb: '/base-ui/ui-breadcrumb',
  uiButtonsGroup: '/base-ui/ui-buttons-group',
  uiCarousel: '/base-ui/ui-carousel',
  uiDropdowns: '/base-ui/ui-dropdowns',
  uiGrid: '/base-ui/ui-grid',
  uiImages: '/base-ui/ui-images',
  uiLightbox: '/base-ui/ui-lightbox',
  uiMedia: '/base-ui/ui-media',
  uiModals: '/base-ui/ui-modals',
  uiNavTabs: '/base-ui/ui-nav-tabs',
  uiOffcanvas: '/base-ui/ui-offcanvas',
  uiPagination: '/base-ui/ui-pagination',
  uiProgress: '/base-ui/ui-progress',
  uiRangeSlider: '/base-ui/ui-rangeslider',
  uiSpinner: '/base-ui/ui-spinner',
  uiSweetAlerts: '/base-ui/ui-sweetalerts',
  uiToasts: '/base-ui/ui-toasts',
  uiTooltips: '/base-ui/ui-tooltips',
  uiTypography: '/base-ui/ui-typography',
  uiVideo: '/base-ui/ui-video',
  uiBorders: '/base-ui/ui-borders',
  uiColors: '/base-ui/ui-colors',
  
  // Advanced UI
  counter: '/advanced-ui/ui-counter',
  clipboard: '/advanced-ui/ui-clipboard',
  ribbon: '/advanced-ui/ui-ribbon',
  rating: '/advanced-ui/ui-rating',
  textEditor: '/advanced-ui/ui-text-editor',
  scrollbar: '/advanced-ui/ui-scrollbar',
  timeline: '/advanced-ui/ui-timeline',
  dragDrop: '/advanced-ui/ui-drag-drop',

  // Charts
  chartApex: '/charts/chart-apex',
  chartNg2: '/charts/chart-ng2',
  chartPrime: '/charts/prime-ng',

  // Tables
  basicTable: '/table/tables-basic',
  dataTable: '/table/data-basic',

  // Forms
  formBasicInputs: '/forms/form-basic-inputs',
  formInputsGroups: '/forms/form-input-groups',
  formHorizontal: '/forms/form-horizontal',
  formVertical: '/forms/form-vertical',
  formMask: '/forms/form-mask',
  formValidation: '/forms/form-validation',
  formSelect: '/forms/form-select',
  formFileUpload: '/forms/form-fileupload',
  formCheckboxRadios: '/forms/form-checkbox-radios',
  formWizard: '/forms/form-wizard',
  formElements: '/forms/form-elements',
  formGridGutters: '/forms/form-grid-gutters',
  formSelect2: '/forms/form-select-2',
  formFloatingLabels: '/forms/form-floating-labels',

  // Icons
  iconFontAwesome: '/icons/icon-fontawesome',
  iconFeather: '/icons/icon-feather',
  iconIonic: '/icons/icon-ionic',
  iconMaterial: '/icons/icon-material',
  iconPe7: '/icons/icon-pe7',
  iconSimpleline: '/icons/icon-simpleline',
  iconWeather: '/icons/icon-weather',
  iconThemify: '/icons/icon-themify',
  iconTypicon: '/icons/icon-typicon',
  iconFlag: '/icons/icon-flag',

// Layout
  layoutDefault: '/layout-default',
  layoutMini: '/layout-mini',
  layoutRtl: '/dashboard/layout-rtl',
  layoutBox: '/layout-box',
  layoutDark: '/layout-dark',

} as const satisfies Record<string, string>;