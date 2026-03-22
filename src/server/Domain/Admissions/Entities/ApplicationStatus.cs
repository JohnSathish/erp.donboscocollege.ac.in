namespace ERP.Domain.Admissions.Entities;

public enum ApplicationStatus
{
    Submitted = 0,
    Approved = 1,
    Rejected = 2,
    WaitingList = 3,
    EntranceExam = 4,
    Enrolled = 5,
    /// <summary>Admin granted direct admission; awaiting admission fee payment.</summary>
    DirectAdmissionGranted = 6,
    /// <summary>Admission fee paid (direct path). Next: approve / enroll per policy.</summary>
    AdmissionFeePaid = 7,
}






