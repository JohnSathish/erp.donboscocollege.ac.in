// Examinations Module DTOs

export interface AssessmentDto {
  id: string;
  courseId: string;
  courseName?: string;
  academicTermId: string;
  academicTermName?: string;
  name: string;
  code: string;
  type: string;
  maxMarks: number;
  passingMarks: number;
  totalWeightage: number;
  classSectionId?: string | null;
  classSectionName?: string | null;
  scheduledDate?: string | null;
  duration?: string | null;
  instructions?: string | null;
  status: string;
  isPublished: boolean;
  publishedOnUtc?: string | null;
  publishedBy?: string | null;
  components: AssessmentComponentDto[];
  createdOnUtc: string;
  createdBy?: string | null;
}

export interface AssessmentComponentDto {
  id: string;
  assessmentId: string;
  name: string;
  code?: string | null;
  maxMarks: number;
  passingMarks: number;
  weightage: number;
  displayOrder: number;
  instructions?: string | null;
}

export interface AssessmentSummaryDto {
  id: string;
  courseId: string;
  courseName?: string;
  academicTermId: string;
  academicTermName?: string;
  name: string;
  code: string;
  type: string;
  maxMarks: number;
  passingMarks: number;
  totalWeightage: number;
  classSectionId?: string | null;
  classSectionName?: string | null;
  scheduledDate?: string | null;
  status: string;
  isPublished: boolean;
  publishedOnUtc?: string | null;
  createdOnUtc: string;
}

export interface MarkEntryDto {
  id: string;
  assessmentComponentId: string;
  assessmentComponentName?: string;
  studentId: string;
  studentName?: string;
  studentRollNumber?: string;
  marksObtained: number;
  percentage?: number | null;
  grade?: string | null;
  remarks?: string | null;
  status: string;
  isAbsent: boolean;
  isExempted: boolean;
  enteredOnUtc?: string | null;
  enteredBy?: string | null;
  approvedOnUtc?: string | null;
  approvedBy?: string | null;
  createdOnUtc: string;
}

export interface ResultSummaryDto {
  id: string;
  studentId: string;
  studentNumber: string;
  studentName: string;
  academicTermId: string;
  academicTermName: string;
  totalMarks: number;
  maxMarks: number;
  percentage: number;
  grade?: string | null;
  gpa?: number | null;
  totalCredits: number;
  earnedCredits: number;
  status: string;
  isPublished: boolean;
  publishedOnUtc?: string | null;
  publishedBy?: string | null;
  courseResults: CourseResultDto[];
}

export interface CourseResultDto {
  id: string;
  courseId: string;
  courseName: string;
  assessmentId?: string | null;
  totalMarks: number;
  maxMarks: number;
  percentage: number;
  grade?: string | null;
  gradePoints?: number | null;
  credits: number;
  isPassed: boolean;
}

// Request DTOs
export interface CreateAssessmentPayload {
  courseId: string;
  academicTermId: string;
  name: string;
  code: string;
  type: string;
  maxMarks: number;
  passingMarks: number;
  totalWeightage: number;
  classSectionId?: string | null;
  scheduledDate?: string | null;
  duration?: string | null;
  instructions?: string | null;
  components?: AssessmentComponentRequestDto[] | null;
  createdBy?: string | null;
}

export interface AssessmentComponentRequestDto {
  name: string;
  maxMarks: number;
  passingMarks: number;
  weightage: number;
  displayOrder: number;
  code?: string | null;
  instructions?: string | null;
}

export interface EnterMarksPayload {
  assessmentComponentId: string;
  studentId: string;
  marksObtained: number;
  isAbsent?: boolean;
  isExempted?: boolean;
  remarks?: string | null;
  enteredBy?: string | null;
}

export interface BulkEnterMarksPayload {
  assessmentComponentId: string;
  studentMarks: StudentMarkRequestDto[];
  enteredBy?: string | null;
}

export interface StudentMarkRequestDto {
  studentId: string;
  marksObtained: number;
  isAbsent?: boolean;
  isExempted?: boolean;
  remarks?: string | null;
}

export interface PublishAssessmentPayload {
  publishedBy?: string | null;
}

export interface ApproveMarksPayload {
  approvedBy?: string | null;
}

export interface ProcessResultsPayload {
  studentId: string;
  academicTermId: string;
  processedBy?: string | null;
}

