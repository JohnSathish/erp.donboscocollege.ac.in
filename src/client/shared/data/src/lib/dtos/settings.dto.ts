export interface ProgramDto {
  id: string;
  code: string;
  name: string;
  description?: string | null;
  level: string;
  durationYears: number;
  totalCredits: number;
  isActive: boolean;
  createdOnUtc: string;
  createdBy?: string | null;
  updatedOnUtc?: string | null;
  updatedBy?: string | null;
}

export interface ProgramsListResponse {
  programs: ProgramDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface CourseDto {
  id: string;
  code: string;
  name: string;
  description?: string | null;
  programId?: string | null;
  programName?: string | null;
  creditHours: number;
  prerequisites?: string | null;
  isActive: boolean;
  createdOnUtc: string;
  createdBy?: string | null;
  updatedOnUtc?: string | null;
  updatedBy?: string | null;
}

export interface CoursesListResponse {
  courses: CourseDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface FeeComponentDto {
  id: string;
  name: string;
  description?: string | null;
  amount: number;
  isOptional: boolean;
  installmentNumber?: number | null;
  dueDateUtc?: string | null;
  displayOrder: number;
  createdOnUtc: string;
  createdBy?: string | null;
  updatedOnUtc?: string | null;
  updatedBy?: string | null;
}

export interface FeeStructureDto {
  id: string;
  name: string;
  description?: string | null;
  programId?: string | null;
  programName?: string | null;
  academicYear: string;
  isActive: boolean;
  effectiveFromUtc: string;
  effectiveToUtc?: string | null;
  totalAmount: number;
  components: FeeComponentDto[];
  createdOnUtc: string;
  createdBy?: string | null;
  updatedOnUtc?: string | null;
  updatedBy?: string | null;
}

export interface FeeStructuresListResponse {
  feeStructures: FeeStructureDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}


