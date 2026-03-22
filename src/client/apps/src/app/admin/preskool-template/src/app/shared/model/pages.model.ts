export interface pageSelection {
  skip: number;
  limit: number;
}
export interface apiResultFormat {
  data: [];
  totalData: number;
}
export interface userFiles {
  id: number;
  ids: number;
  uploadedFor: string;
  uploadedOn: string;
  uploadedBy: string;
  fileType: string;
  img1: string;
  img2: string;
}
export interface userWallet {
  id: number;
  ids: number;
  reason: string;
  dateTime: string;
  type: string;
  amount: string;
  img: string;
}
export interface userPayment {
  id: number;
  ids: number;
  buyer: string;
  dateTime: string;
  type: string;
  amount: string;
  amount2: string;
  img: string;
  img2: string;
}
export interface userPurchase {
  id: number;
  title: number;
  deliveryOn: string;
  seller: string;
  feedback: string;
  cancel: string;
  amount: string;
  payment: string;
  status: string;
  img: string;
  img2: string;
}
export interface userSales {
  id: number;
  title: number;
  deliveryOn: string;
  seller: string;
  feedback: string;
  cancel: string;
  amount: string;
  payment: string;
  status: string;
  img: string;
  img2: string;
}
export interface billingSetting {
  id: number;
  invoiceNo: number;
  billingDate: string;
  plan: string;
  status: string;
  amount: string;
  action: string;
  img: string;
  img2: string;
}
export interface dataTables {
  isSelected: boolean;
  sNo?: number;
  name?: string;
  position?: string;
  office?: string;
  age?: string;
  salary?: string;
  startDate?: string;
  id?: string;
}
export interface url {
  url: string;
}

export interface Star {
  show?: boolean;
  half?: boolean;
}
export interface academicReasons {
  key?: number;
  createdDate?: string;
  reason?: string;
  role?: string;
  id?: string;
  isSelected?: boolean;
}
export interface transactionModal {
  key?: number;
  status?: string;
  method?: string;
  type?: string;
  date?: string;
  amount?: string;
  description?: string;
  tid?: string;
  isSelected?: boolean;
}
export interface accountIncomeModal {
  key?: number;
  isSelected?: boolean;
  invoiceId?: string;
  incomeName?: string;
  description?: string;
  source?: string;
  date?: string;
  amount?: string;
  invoiceNo?: string;
  paymentMethod?: string;
}
export interface accountInvoiceModal {
  key?: number;
  isSelected?: boolean;
  invoiceNumber?: string;
  date?: string;
  description?: string;
  amount?: number;
  paymentMethod?: string;
  dueDate?: string;
  status?: string;
}
export interface cities {
  sNo: number;
  isSelected: boolean;
  id?: number;
  cityName?: string;
  stateName?: string;
  countryName?: string;
  status?: string;
}
export interface classes {
  sNo: number;
  isSelected: boolean;
  id?: number;
  class?: string;
  section?: string;
  noOfStudents?: string;
  noOfSubjects?: string;
  status: string;
}
export interface classeHomework {
  sNo: number;
  isSelected: boolean;
  id?: number;
  class?: string;
  section?: string;
  subject?: string;
  homeworkDate?: string;
  submissionDate?: string;
  createdBy?: string;
  status: string;
  img: string;
}
export interface transportVehicle {
  sNo: number;
  isSelected: boolean;
  id?: number;
  vehicleNo?: string;
  vehicleModel?: string;
  madeofYear?: string;
  registrationNo?: string;
  chassisNo?: string;
  gps?: string;
  name?: string;
  status: string;
  phone: string;
  img: string;
  address: string;
  statusClass: string;
  driverLicenseNo: string;
}
export interface users {
  sNo: number;
  isSelected: boolean;
  id?: number;
  class?: string;
  section?: string;
  name?: string;
  dateOfJoined?: string;
  status: string;
  img: string;
}
export interface rolesPermission {
  sNo: number;
  isSelected: boolean;
  roleName?: number;
  createdOn?: string;
}

export interface approveRequest {
  isSelected: boolean;
  id?: number;
  submittedBy: string;
  leaveType: string;
  role: string;
  leaveDate: string;
  noOfDays: string;
  appliedOn: string;
  authority: string;
  status: string;
}
export interface classReport {
  sNo: number;
  isSelected: boolean;
  id?: number;
  class?: string;
  section?: string;
  noOfStudents?: string;
  name: string;
  gender: string;
  dateOfJoin: string;
  parent: string;
  admissionNo: string;
  rollNo: string;
  parentimg: string;
  img: string;
  status: string;
}
export interface classReport {
  sNo: number;
  isSelected: boolean;
  id?: number;
  class?: string;
  section?: string;
  noOfStudents?: string;
  name: string;
  gender: string;
  dateOfJoin: string;
  parent: string;
  admissionNo: string;
  rollNo: string;
  parentimg: string;
  img: string;
  status: string;
}
export interface studentReport {
  sNo: number;
  isSelected: boolean;
  id?: number;
  class?: string;
  section?: string;
  dob?: string;
  name: string;
  gender: string;
  dateOfJoin: string;
  parent: string;
  admissionNo: string;
  rollNo: string;
  parentimg: string;
  img: string;
  status: string;
}
export interface FeesRecord {
  id?: number;
  feesGroup: string;
  feesType: string;
  feesCode: string;
  dueDate: string;
  amount: number;
  status: string;
  refId: string;
  mode: string;
  datePaid: string;
  discount: string | number; 
  fine: number;
  isSelected?:boolean;
}

export interface attendanceReport {
  isSelected: boolean;
  id?: number;
  studentDate: string;
  percent: string;
  p: string;
  l: string;
  a: string;
  h: string;
  f: string;
  '01m': string;
  '02t': string;
  '03w': string;
  '04t': string;
  '05f': string;
  '06s': string;
  '07s': string;
  '08m': string;
  '09t': string;
  '10w': string;
  '11t': string;
  '12f': string;
  '13s': string;
  '14s': string;
  '15m': string;
  '16t': string;
  '17w': string;
  '18t': string;
  '19f': string;
  '20s': string;
  '21s': string;
  '22m': string;
  '23t': string;
  img: string;
  bgDark: boolean;
  bgSuccess: boolean;
}
export interface blogCategories {
  isSelected: boolean;
  id?: number;
  category: string;
  addedOn: string;
  status: string;
}
export interface blogComments {
  isSelected: boolean;
  id?: number;
  comment: string;
  date: string;
  review: string;
  blog: string;
  by: string;
}
export interface blogTags {
  isSelected: boolean;
  id?: number;
  tags: string;
  addedOn: string;
  status: string;
}
export interface contactMessages {
  isSelected: boolean;
  id: number;
  ids: string;
  name: string;
  phone: string;
  email: string;
  message: string;
  date: string;
  img: string;
}
export interface countries {
  isSelected: boolean;
  id: number;
  ids: string;
  countryName: string;
  countryCode: string;
  status: string;
}
export interface dailyAttendance {
  id?: number;
  class: string;
  section: string;
  totalPresent: string;
  totalAbsent: string;
  presentPercentage: string;
  absentPercentage: string;
}
export interface deleteAccount {
  isSelected: boolean;
  id?: number;
  ids: string;
  name: string;
  requisitionDate: string;
  deleteRequestDate: string;
  status: string;
  img: string;
}
export interface departments {
  isSelected: boolean;
  id?: number;
  ids: string;
  department: string;
  status: string;
}
export interface designation {
  isSelected: boolean;
  id?: number;
  ids: string;
  designation: string;
  status: string;
}
export interface classRoomData {
  key?: number;
  isSelected?: boolean;
  id?: string;
  status?: string;
  capacity?: string;
  roomNo?: string;
}
export interface classSyllabusData {
  key?: number;
  isSelected?: boolean;
  id?: string;
  status?: string;
  createdDate?: string;
  subjectGroup?: string;
  section?: string;
  class?: string;
}
export interface classSubjectData {
  key?: number;
  isSelected?: boolean;
  id?: string;
  status?: string;
  type?: string;
  code?: string;
  name?: string;
}
export interface classSectionData {
  key?: number;
  isSelected?: boolean;
  id?: string;
  status?: string;
  sectionName?: string;
}
export interface classRoutineData {
  key?: number;
  isSelected?: boolean;
  id?: string;
  classRoom?: string;
  endTime?: string;
  startTime?: string;
  day?: string;
  subject?: string;
  teacher?: string;
  section?: string;
  class?: string;
}
export interface hostelroom {
  sNo: number;
  isSelected: boolean;
  id?: number;
  roomNo?: string;
  hostelName?: string;
  roomType?: string;
  noofBed?: string;
  amount?: string;
}
export interface hostelroomtype {
  sNo: number;
  isSelected: boolean;
  id?: number;
  roomNo?: string;
  hostelName?: string;
  roomType?: string;
  noofBed?: string;
  description?: string;
}
export interface hostelroomtype {
  sNo: number;
  isSelected: boolean;
  id?: number;
  inTake?: string;
  hostelName?: string;
  hostelType?: string;
  address?: string;
  description?: string;
}
export interface holidays {
  sNo: number;
  isSelected: boolean;
  id?: number;
  holidayTitle?: string;
  date?: string;
  description?: string;
  address?: string;
  dstatus?: string;
}
export interface feesGroup {
  isSelected: boolean;
  id?: number;
  ids: string;
  feesGroup: string;
  description: string;
  status: string;
}
export interface feesMaster {
  isSelected: boolean;
  id?: number;
  ids: string;
  feesGroup: string;
  feesType: string;
  dueDate: string;
  amount: string;
  fineType: string;
  fineAmount: string;
  status: string;
}
export interface feesReport {
  isSelected: boolean;
  id?: number;
  ids: string;
  feesGroup: string;
  feesCode: string;
  dueDate: string;
  amount: string;
  status: string;
  refId: string;
  mode: string;
  datePaid: string;
  discount: string;
  fine: string;
  balance: string;
  statusEmpty: boolean;
}
export interface feesType {
  isSelected: boolean;
  id?: number;
  ids?: number;
  feesType: string;
  feesCode: string;
  feesGroup: string;
  description: string;
  status: string;
}
export interface grade {
  isSelected: boolean;
  id?: number;
  ids?: number;
  grade: string;
  percentage: string;
  gradePoints: string;
  status: string;
}
export interface feesCollectData {
  key?: number;
  isSelected?: boolean;
  section?: string;
  class?: string;
  email?: string;
  parentClass?: string;
  parent?: string;
  parentImage?: string;
  studentImage?: string;
  studentClass?: string;
  student?: string;
  rollNo?: string;
  admNo?: string;
  statusClass?: string;
  status?: string;
  lastDate?: string;
  amount?: string;
  view?: string;
}
export interface guardianList {
  sNo: number;
  isSelected: boolean;
  id?: number;
  name?: string;
  Addedon?: string;
  ChildImage?: string;
  GuardianImage?: string;
  email: string;
  phone: string;
  class: string;
  Child: string;
}
export interface gradeReport {
  isSelected: boolean;
  id?: number;
  ids?: number;
  admissionNo: string;
  studentName: string;
  rollNo: string;
  english: string;
  spanish: string;
  physics: string;
  chemistry: string;
  maths: string;
  computer: string;
  envScience: string;
  total: string;
  percent: string;
  grade: string;
  img: string;
}
export interface parentList {
  sNo: number;
  isSelected: boolean;
  id?: number;
  name?: string;
  Addedon?: string;
  ChildImage?: string;
  GuardianImage?: string;
  email: string;
  phone: string;
  class: string;
  Child: string;
  ParentImage?: string;
}
export interface payroll {
  sNo: number;
  isSelected: boolean;
  id?: number;
  name?: string;
  details?: string;
  amount: string;
  phone: string;
  status: string;
  designation: string;
  department?: string;
}
export interface players {
  sNo: number;
  isSelected: boolean;
  id?: number;
  year?: string;
  coachName?: string;
  name?: string;
  sports: string;
  dateofJoin: string;
  designation: string;
  department?: string;
  img: string;
  img2: string;
}
export interface permission {
  sNo: number;
  isSelected: boolean;
  id?: number;
  modules?: string;
  created?: string;
  view?: string;
  edit: string;
  delete: string;
  allowAll: string;
}

export interface membershipTransactions {
  isSelected?: boolean;
  id: number;
  ids: string;
  providerName: string;
  planType: string;
  transactionDate: string;
  amount: string;
  paymentMethod: string;
  startDate: string;
  endDate: string;
  status: string;
}
export interface pages {
  isSelected: boolean;
  id?: number;
  page: string;
  pageSlug: string;
  key: string;
}

export interface faqData {
  isSelected: boolean;
  key?: number;
  category?: string;
  answers?: string;
  questions?: string;
  id?: string;
}
export interface feesAssignData {
  isSelected: boolean;
  key?: number;
  category?: string;
  gender?: string;
  amount?: string;
  section?: string;
  class?: string;
  feesType?: string;
  feesGroup?: string;
  sNo?: string;
}
export interface expenseData {
  isSelected: boolean;
  key?: number;
  id?: string;
  paymentMethod?: string;
  invoiceNo?: string;
  amount?: string;
  date?: string;
  category?: string;
  description?: string;
  expenseName?: string;
}
export interface expenseCategoryData {
  isSelected: boolean;
  key?: number;
  id?: string;
  category?: string;
  description?: string;
}
export interface examAttendance {
  isSelected: boolean;
  key?: number;
  id?: string;
  studentName?: string;
  rollNo?: string;
  english?: string;
  spanish?: string;
  physics?: string;
  chemistry?: string;
  maths?: string;
  computer?: string;
  envScience?: string;
  img?: string;
}
export interface examResult {
  isSelected: boolean;
  key?: number;
  id?: string;
  studentName?: string;
  admissionNo?: string;
  img?: string;
  roll?: string;
  result?: string;
  grade?: string;
  english?: number | undefined;
  spanish?: number;
  physics?: number;
  chemistry?: number;
  maths?: number;
  computer?: number;
  envScience?: number;
  percent?: number;
  total?: number;
}
export interface examSchedule {
  isSelected: boolean;
  key?: number;
  id?: string;
  minMarks?: string;
  maxMarks?: string;
  roomNo?: string;
  duration?: string;
  endTime?: string;
  startTime?: string;
  examDate?: string;
  subject?: string;
}
export interface examData {
  isSelected: boolean;
  key?: number;
  id?: string;
  examName?: string;
  examDate?: string;
  endTime?: string;
  startTime?: string;
}
export interface language {
  isSelected: boolean;
  key?: number;
  status?: string;
  rtl?: string;
  code?: string;
  language?: string;
}
export interface libraryMemberList {
  isSelected: boolean;
  key?: number;
  id?: string;
  mobile?: string;
  dateofJoin?: string;
  email?: string;
  cardNo?: string;
  img?: string;
  name?: string;
}
export interface bookIssueList {
  isSelected: boolean;
  key?: number;
  id?: string;
  issueRemarks?: string;
  img?: string;
  bookReturned?: string;
  booksIssued?: string;
  class?: string;
  dueDate?: string;
  issueTo?: string;
  dateofIssue?: string;
}
export interface bookList {
  isSelected: boolean;
  key?: number;
  id?: string;
  postDate?: string;
  price?: string;
  available?: string;
  qty?: string;
  rackNo?: string;
  subject?: string;
  author?: string;
  publisher?: string;
  bookNo?: string;
  bookName?: string;
}
export interface listLeaves {
  isSelected: boolean;
  key?: number;
  id?: string;
  status?: string;
  leaveType?: string;
}
export interface LeaveReport {
  isSelected: boolean;
  key?: number;
  specialAvailable?: string;
  specialUsed?: string;
  paternityAvailable?: string;
  paternityUsed?: string;
  maternityAvailable?: string;
  maternityUsed?: string;
  casualAvailable?: string;
  casualUsed?: string;
  medicalAvailable?: string;
  medicalUsed?: string;
  avatar?: string;
  rollNo?: string;
  studentName?: string;
  admissionNo?: string;
}
export interface states {
  sNo: number;
  isSelected: boolean;
  id?: number;
  stateName: string;
  countryName: string;
  status: string;
}
export interface sports {
  isSelected: boolean;
  id?: number;
  ids?: number;
  tags: string;
  addedOn: string;
  status: string;
  img: string;
}
export interface staffAttendance {
  attendance: string;
  isSelected: boolean;
  id?: number;
  ids?: number;
  name: string;
  department: string;
  designation: string;
  img: string;
  key: string;
}
export interface staffDayWise {
  isSelected: boolean;
  id?: number;
  sNo: string;
  ids: string;
  name: string;
  department: string;
  role: string;
  attendance: string;
  img: string;
}
export interface staffLeaves {
  isSelected: boolean;
  id?: number;
  leaveType: string;
  leaveDate: string;
  noOfDays: string;
  appliedOn: string;
  status: string;
}
export interface staffPayroll {
  isSelected: boolean;
  id?: number;
  viewPayslip: string;
  ids: string;
  salaryFor: string;
  date: string;
  paymentMethod: string;
  netSalary: string;
}
export interface teacherAttendance {
  attendance: string;
  isSelected: boolean;
  id?: number;
  ids?: number;
  name: string;
  class: string;
  img: string;
  key: string;
}
export interface scheduleClass {
  isSelected: boolean;
  key?: number;
  id?: string;
  status?: string;
  endTime?: string;
  startTime?: string;
  type?: string;
}
export interface staff {
  sNo: number;
  isSelected: boolean;
  name?: number;
  id?: string;
  email?: string;
  phone?: string;
  department?: string;
  designation?: string;
  dateOfJoin?: string;
  img?: string;
}
export interface studentAttendanceType {
  sNo: number;
  id?: number;
  date?: string;
  name?: string;
  img?: string;
  class?: string;
  dob?: string;
  fname?: string;
  img2?: string;
  count?: string;
}
export interface staffsAttendance {
  id: number;
  dateMonth: string;
  jun: string;
  jul: string;
  aug: string;
  sep: string;
  oct: string;
  nov: string;
  dec: string;
  jan: string;
  feb: string;
  mar: string;
  apr: string;
  bgDark?: string;
  bgsuccess?: string;
  bginfo?: string;
}
export interface studentLeaveAttendance {
  key: number;
  date: string;
  jun: string;
  jul: string;
  aug: string;
  sep: string;
  oct: string;
  nov: string;
  dec: string;
  jan: string;
  feb: string;
  mar: string;
  apr: string;
  bgDark?: string;
  bgsuccess?: string;
  bginfo?: string;
}
export interface staffReport {
  isSelected: boolean;
  id?: number;
  studentDate: string;
  percent: string;
  p: string;
  l: string;
  a: string;
  h: string;
  f: string;
  '01m': string;
  '02t': string;
  '03w': string;
  '04t': string;
  '05f': string;
  '06s': string;
  '07s': string;
  '08m': string;
  '09t': string;
  '10w': string;
  '11t': string;
  '12f': string;
  '13s': string;
  '14s': string;
  '15m': string;
  '16t': string;
  '17w': string;
  '18t': string;
  '19f': string;
  '20s': string;
  '21s': string;
  '22m': string;
  '23t': string;
  img: string;
  bgDark: boolean;
  bgSuccess: boolean;
}
export interface studentAttendance {
  admissionNo: string;
  isSelected: boolean;
  id?: number;
  rollNo?: number;
  class: string;
  department: string;
  designation: string;
  img: string;
  section: string;
  name: string;
  notes: string;
  attendance: string;
  present: string;
  Late: string;
  Absent: string;
  Holiday: string;
  Halfday: string;
  key: string;
}
export interface teacherDayWise {
  id: number;
  sNo: string;
  ids: string;
  name: string;
  subject: string;
  attendance: string;
  img: string;
}
export interface studentDayWise {
  key: number;
  id: string;
  rollNo: string;
  name: string;
  attendance: string;
  img: string;
}
export interface teacherLeaves {
  id: number;
  leaveType: string;
  leaveDate: string;
  noOfDays: string;
  appliedOn: string;
  status: string;
}
export interface teacherReport {
  id?: number;
  studentDate: string;
  percentage: string;
  present: string;
  late: string;
  absent: string;
  halfDay: string;
  fieldTrip: string;
  '01M': string;
  '02T': string;
  '03W': string;
  '04T': string;
  '05F': string;
  '06S': string;
  '07S': string;
  '08M': string;
  '09T': string;
  '10W': string;
  '11T': string;
  '12F': string;
  '13S': string;
  '14S': string;
  '15M': string;
  '16T': string;
  '17W': string;
  '18T': string;
  '19F': string;
  '20S': string;
  '21S': string;
  '22M': string;
  '23T': string;
  img: string;
}
export interface teachersList {
  isSelected: boolean;
  id?: number;
  ids?: string;
  name: string;
  class: string;
  subject: string;
  email: string;
  phone: string;
  dateOfJoin: string;
  status: string;
  img: string;
}
export interface teacherSalary {
  isSelected: boolean;
  id?: number;
  ids?: string;
  salaryFor: string;
  date: string;
  paymentMethod: string;
  netSalary: string;
}
export interface testimonials {
  sNo: number;
  id?: number;
  role?: string;
  content?: string;
  img?: string;
  dateAdded?: string;
  isSelected: boolean;
  name:string;
}
export interface transportassignvehicle {
  sNo: number;
  id?: number;
  route?: string;
  pickupPoint?: string;
  img?: string;
  vehicle?: string;
  isSelected: boolean;
  name:string;
  phone:string;
  status:string;
  statusClass:string;
}
export interface transportPickupPoints {
  sNo: number;
  id?: number;
  pickupPoint?: string;
  img?: string;
  vehicle?: string;
  isSelected: boolean;
  status:string;
  statusClass:string;
  addedOn:string;
}
export interface transportRoutes {
  sNo: number;
  id?: number;
  routes?: string;
  img?: string;
  status?: string;
  isSelected: boolean;
  statusClass:string;
  addedOn:string;
}
export interface studentlist {
  sNo: number;
  id?: number;
  promotion?: string;
  result?: string;
  AdmissionNo?: string;
  isSelected: boolean;
  RollNo:string;
  name:string;
  class:string;
  section:string;
  gender:string;
  status:string;
  DateofJoin:string;
  DOB:string;
  imgSrc:string;
}
export interface videocall {
  sNo: number;
  img: string;
  name: string;
}
export interface callHistory {
  sNo: number;
  username: string;
  phoneNumber: string;
  callType: string;
  duration: string;
  dateTime: string;
  img: string;
  video: boolean;
  audio: boolean;
  isSelected?: boolean;
}
export interface file {
  sNo: number;
  name: string;
  lastModified: string;
  size: string;
  ownedMember: string;
  modified: string;
  memberimg: string;
  folderimg: string;
  isSelected?: boolean;
}