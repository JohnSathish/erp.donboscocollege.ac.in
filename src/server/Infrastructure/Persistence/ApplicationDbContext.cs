using ERP.Domain.Admissions.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<StudentApplicantAccount> StudentApplicantAccounts => Set<StudentApplicantAccount>();
    public DbSet<ApplicantRefreshToken> ApplicantRefreshTokens => Set<ApplicantRefreshToken>();
    public DbSet<ApplicantApplicationDraft> ApplicantApplicationDrafts => Set<ApplicantApplicationDraft>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<EntranceExam> EntranceExams => Set<EntranceExam>();
    public DbSet<ExamRegistration> ExamRegistrations => Set<ExamRegistration>();
    public DbSet<Program> Programs => Set<Program>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<FeeStructure> FeeStructures => Set<FeeStructure>();
    public DbSet<FeeComponent> FeeComponents => Set<FeeComponent>();
    public DbSet<MeritScore> MeritScores => Set<MeritScore>();
    public DbSet<AdmissionOffer> AdmissionOffers => Set<AdmissionOffer>();
    public DbSet<AdmissionWorkflowSettings> AdmissionWorkflowSettings => Set<AdmissionWorkflowSettings>();
    public DbSet<OfflineFormIssuance> OfflineFormIssuances => Set<OfflineFormIssuance>();
    public DbSet<ClassXiiSubjectMaster> ClassXiiSubjectMasters => Set<ClassXiiSubjectMaster>();
    public DbSet<ERP.Domain.Students.Entities.Student> Students => Set<ERP.Domain.Students.Entities.Student>();
    public DbSet<ERP.Domain.Students.Entities.Guardian> Guardians => Set<ERP.Domain.Students.Entities.Guardian>();
    public DbSet<ERP.Domain.Students.Entities.AcademicRecord> AcademicRecords => Set<ERP.Domain.Students.Entities.AcademicRecord>();
    public DbSet<ERP.Domain.Students.Entities.CourseEnrollment> CourseEnrollments => Set<ERP.Domain.Students.Entities.CourseEnrollment>();
    public DbSet<ERP.Domain.Students.Entities.StudentTransfer> StudentTransfers => Set<ERP.Domain.Students.Entities.StudentTransfer>();
    public DbSet<ERP.Domain.Students.Entities.StudentExit> StudentExits => Set<ERP.Domain.Students.Entities.StudentExit>();
    public DbSet<ERP.Domain.Students.Entities.DisciplineRecord> DisciplineRecords => Set<ERP.Domain.Students.Entities.DisciplineRecord>();
    public DbSet<ERP.Domain.Students.Entities.CounselingRecord> CounselingRecords => Set<ERP.Domain.Students.Entities.CounselingRecord>();
    public DbSet<ERP.Domain.Fees.Entities.Invoice> Invoices => Set<ERP.Domain.Fees.Entities.Invoice>();
    public DbSet<ERP.Domain.Fees.Entities.InvoiceLine> InvoiceLines => Set<ERP.Domain.Fees.Entities.InvoiceLine>();
    public DbSet<ERP.Domain.Fees.Entities.Payment> Payments => Set<ERP.Domain.Fees.Entities.Payment>();
    public DbSet<ERP.Domain.Fees.Entities.Receipt> Receipts => Set<ERP.Domain.Fees.Entities.Receipt>();
    public DbSet<ERP.Domain.Fees.Entities.Refund> Refunds => Set<ERP.Domain.Fees.Entities.Refund>();
    public DbSet<ERP.Domain.Fees.Entities.Scholarship> Scholarships => Set<ERP.Domain.Fees.Entities.Scholarship>();
    public DbSet<ERP.Domain.Attendance.Entities.AttendanceSession> AttendanceSessions => Set<ERP.Domain.Attendance.Entities.AttendanceSession>();
    public DbSet<ERP.Domain.Attendance.Entities.AttendanceRecord> AttendanceRecords => Set<ERP.Domain.Attendance.Entities.AttendanceRecord>();
    public DbSet<ERP.Domain.Attendance.Entities.AttendanceDeviceEvent> AttendanceDeviceEvents => Set<ERP.Domain.Attendance.Entities.AttendanceDeviceEvent>();
    public DbSet<ERP.Domain.Academics.Entities.AcademicTerm> AcademicTerms => Set<ERP.Domain.Academics.Entities.AcademicTerm>();
    public DbSet<ERP.Domain.Academics.Entities.ClassSection> ClassSections => Set<ERP.Domain.Academics.Entities.ClassSection>();
    public DbSet<ERP.Domain.Academics.Entities.TimetableSlot> TimetableSlots => Set<ERP.Domain.Academics.Entities.TimetableSlot>();
    public DbSet<ERP.Domain.Academics.Entities.Room> Rooms => Set<ERP.Domain.Academics.Entities.Room>();
    public DbSet<ERP.Domain.Examinations.Entities.Assessment> Assessments => Set<ERP.Domain.Examinations.Entities.Assessment>();
    public DbSet<ERP.Domain.Examinations.Entities.AssessmentComponent> AssessmentComponents => Set<ERP.Domain.Examinations.Entities.AssessmentComponent>();
    public DbSet<ERP.Domain.Examinations.Entities.MarkEntry> MarkEntries => Set<ERP.Domain.Examinations.Entities.MarkEntry>();
    public DbSet<ERP.Domain.Examinations.Entities.ResultSummary> ResultSummaries => Set<ERP.Domain.Examinations.Entities.ResultSummary>();
    public DbSet<ERP.Domain.Examinations.Entities.CourseResult> CourseResults => Set<ERP.Domain.Examinations.Entities.CourseResult>();
    public DbSet<ERP.Domain.Staff.Entities.StaffMember> StaffMembers => Set<ERP.Domain.Staff.Entities.StaffMember>();
    public DbSet<ERP.Domain.Library.Entities.Book> Books => Set<ERP.Domain.Library.Entities.Book>();
    public DbSet<ERP.Domain.Library.Entities.BookIssue> BookIssues => Set<ERP.Domain.Library.Entities.BookIssue>();
    public DbSet<ERP.Domain.Transport.Entities.Vehicle> Vehicles => Set<ERP.Domain.Transport.Entities.Vehicle>();
    public DbSet<ERP.Domain.Hostel.Entities.HostelRoom> HostelRooms => Set<ERP.Domain.Hostel.Entities.HostelRoom>();
    public DbSet<ERP.Domain.Hostel.Entities.RoomAllocation> RoomAllocations => Set<ERP.Domain.Hostel.Entities.RoomAllocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}


