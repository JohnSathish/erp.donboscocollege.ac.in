export interface RegisterStudentApplicantRequest {
  fullName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  mobileNumber: string;
}

export interface RegisterStudentApplicantResponse {
  uniqueId: string;
  temporaryPassword: string;
}
