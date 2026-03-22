using ERP.Application.Examinations.Interfaces;

namespace ERP.Infrastructure.Examinations;

public class GradingService : IGradingService
{
    // Standard grading scheme (can be made configurable)
    private static readonly Dictionary<string, (decimal MinPercentage, string Grade, decimal? GradePoints)> StandardGradingScheme = new()
    {
        { "A+", (90m, "A+", 10.0m) },
        { "A", (80m, "A", 9.0m) },
        { "B+", (70m, "B+", 8.0m) },
        { "B", (60m, "B", 7.0m) },
        { "C+", (55m, "C+", 6.0m) },
        { "C", (50m, "C", 5.0m) },
        { "D", (45m, "D", 4.0m) },
        { "P", (40m, "P", 4.0m) }, // Pass
        { "F", (0m, "F", 0.0m) }   // Fail
    };

    public string CalculateGrade(decimal percentage, string? gradingScheme = null)
    {
        // For now, use standard grading scheme
        // Can be extended to support different schemes based on gradingScheme parameter
        var scheme = StandardGradingScheme;

        foreach (var (grade, (minPercentage, gradeValue, _)) in scheme.OrderByDescending(x => x.Value.MinPercentage))
        {
            if (percentage >= minPercentage)
            {
                return gradeValue;
            }
        }

        return "F";
    }

    public decimal? CalculateGPA(IEnumerable<CourseGradeInfo> courseGrades)
    {
        var grades = courseGrades.ToList();
        if (!grades.Any())
            return null;

        var totalGradePoints = 0m;
        var totalCredits = 0;

        foreach (var grade in grades)
        {
            var gradePoints = grade.GradePoints ?? GetGradePoints(grade.Percentage);
            totalGradePoints += gradePoints * grade.Credits;
            totalCredits += grade.Credits;
        }

        if (totalCredits == 0)
            return null;

        return totalGradePoints / totalCredits;
    }

    public decimal CalculatePercentage(decimal marksObtained, decimal maxMarks)
    {
        if (maxMarks <= 0)
            return 0;

        return (marksObtained / maxMarks) * 100;
    }

    private static decimal GetGradePoints(decimal percentage)
    {
        var scheme = StandardGradingScheme;
        foreach (var (_, (minPercentage, _, gradePoints)) in scheme.OrderByDescending(x => x.Value.MinPercentage))
        {
            if (percentage >= minPercentage && gradePoints.HasValue)
            {
                return gradePoints.Value;
            }
        }

        return 0m;
    }
}





