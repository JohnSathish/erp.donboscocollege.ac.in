namespace ERP.Application.Examinations.Interfaces;

public interface IGradingService
{
    string CalculateGrade(decimal percentage, string? gradingScheme = null);
    decimal? CalculateGPA(IEnumerable<CourseGradeInfo> courseGrades);
    decimal CalculatePercentage(decimal marksObtained, decimal maxMarks);
}

public record CourseGradeInfo(
    decimal Percentage,
    int Credits,
    decimal? GradePoints = null);





