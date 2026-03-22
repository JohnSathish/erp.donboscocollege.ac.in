export interface ApplicantProfileDto {
  accountId: string;
  uniqueId: string;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  mobileNumber: string;
  shift: string;
  photoUrl?: string | null;
  createdOnUtc: string;
}

export interface ApplicantDocumentStatusDto {
  name: string;
  status: string;
  description: string;
  isComplete: boolean;
}

export interface ApplicantNotificationDto {
  title: string;
  message: string;
  createdOnUtc: string;
}

export interface ApplicantDashboardDto {
  profile: ApplicantProfileDto;
  documents: ApplicantDocumentStatusDto[];
  notifications: ApplicantNotificationDto[];
}
