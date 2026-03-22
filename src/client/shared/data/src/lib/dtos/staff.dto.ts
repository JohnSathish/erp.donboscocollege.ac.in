// Staff Module DTOs

export interface StaffMemberDto {
  id: string;
  employeeNumber: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  mobileNumber: string;
  dateOfBirth: string;
  gender: string;
  department?: string | null;
  designation?: string | null;
  employeeType?: string | null;
  joinDate: string;
  exitDate?: string | null;
  status: string;
  address?: string | null;
  emergencyContactName?: string | null;
  emergencyContactNumber?: string | null;
  qualifications?: string | null;
  specialization?: string | null;
  createdOnUtc: string;
  createdBy?: string | null;
  updatedOnUtc?: string | null;
  updatedBy?: string | null;
}

export interface StaffMembersListResponse {
  staff: StaffMemberDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

// Request DTOs
export interface CreateStaffMemberPayload {
  employeeNumber: string;
  firstName: string;
  lastName: string;
  email: string;
  mobileNumber: string;
  dateOfBirth: string;
  gender: string;
  department?: string | null;
  designation?: string | null;
  employeeType?: string | null;
  joinDate: string;
  address?: string | null;
  emergencyContactName?: string | null;
  emergencyContactNumber?: string | null;
  qualifications?: string | null;
  specialization?: string | null;
  createdBy?: string | null;
}

export interface UpdateStaffMemberPayload {
  firstName: string;
  lastName: string;
  email: string;
  mobileNumber: string;
  department?: string | null;
  designation?: string | null;
  employeeType?: string | null;
  address?: string | null;
  emergencyContactName?: string | null;
  emergencyContactNumber?: string | null;
  qualifications?: string | null;
  specialization?: string | null;
  updatedBy?: string | null;
}

