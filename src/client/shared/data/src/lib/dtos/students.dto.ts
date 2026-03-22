// Student Management Module DTOs

export interface StudentDto {
  id: string;
  studentNumber: string;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  mobileNumber: string;
  photoUrl?: string | null;
  programId?: string | null;
  programCode?: string | null;
  majorSubject?: string | null;
  minorSubject?: string | null;
  shift: string;
  academicYear: string;
  admissionNumber?: string | null;
  enrollmentDate: string;
  status: string;
  createdOnUtc: string;
  createdBy?: string | null;
  updatedOnUtc?: string | null;
  updatedBy?: string | null;
}

export interface StudentsListResponse {
  students: StudentDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

// Request DTOs
export interface UpdateStudentProfilePayload {
  fullName: string;
  email: string;
  mobileNumber: string;
  shift: string;
  programId?: string | null;
  programCode?: string | null;
  majorSubject?: string | null;
  minorSubject?: string | null;
  photoUrl?: string | null;
}

export interface UpdateStudentStatusPayload {
  status: string;
}

export interface ConvertApplicantToStudentPayload {
  applicantAccountId: string;
  studentNumber: string;
  academicYear: string;
  programId?: string | null;
  programCode?: string | null;
}




