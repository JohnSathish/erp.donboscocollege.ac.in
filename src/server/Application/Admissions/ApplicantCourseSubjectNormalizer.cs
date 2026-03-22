using System.Collections.Generic;
using ERP.Application.Admissions.DTOs;

namespace ERP.Application.Admissions;

/// <summary>
/// Maps legacy/abbreviated major labels to the canonical display name used in the portal and PDFs.
/// </summary>
public static class ApplicantCourseSubjectNormalizer
{
    private static readonly Dictionary<string, string> MajorCanonicalByKey = new(StringComparer.OrdinalIgnoreCase)
    {
        ["POL. SCIENCE"] = "POLITICAL SCIENCE",
        ["POL-SCIENCE"] = "POLITICAL SCIENCE",
        ["POL SCIENCE"] = "POLITICAL SCIENCE",
    };

    public static void Normalize(ApplicantApplicationDraftDto dto)
    {
        if (dto.Courses is null)
        {
            return;
        }

        dto.Courses.MajorSubject = ToCanonicalMajorName(dto.Courses.MajorSubject);
    }

    public static string ToCanonicalMajorName(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }

        var trimmed = raw.Trim();
        return MajorCanonicalByKey.TryGetValue(trimmed, out var full) ? full : trimmed;
    }
}
