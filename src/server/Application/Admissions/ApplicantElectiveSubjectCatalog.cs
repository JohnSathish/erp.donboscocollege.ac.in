using System;
using System.Collections.Generic;
using ERP.Application.Admissions.ViewModels;

namespace ERP.Application.Admissions;

/// <summary>
/// Maps stored elective choices (e.g. "MDC 111") to display name and full catalog label for tooltips.
/// Aligns with export/PDF course labels.
/// </summary>
public static class ApplicantElectiveSubjectCatalog
{
    private static readonly Dictionary<string, string> CodeToFullLabel = new(StringComparer.OrdinalIgnoreCase)
    {
        ["MDC 111"] = "MDC 111 — CULTURE AND SOCIETY",
        ["MDC 112"] = "MDC 112 — FUNDAMENTALS OF COMPUTER SYSTEMS",
        ["MDC 116"] = "MDC 116 — INTRODUCTION TO NATIONAL CADET CORPS",
        ["MDC 118"] = "MDC 118 — MATHEMATICS IN DAILY LIFE",
        ["MDC 119"] = "MDC 119 — PHILOSOPHY OF CULTURE",
        ["MDC 110"] = "MDC 110 — COMMERCIAL ARITHMETIC & ELEMENTARY STATISTICS",
        ["MDC 115"] = "MDC 115 — INTRODUCTION TO LIFE SCIENCES",
        ["AEC 120"] = "AEC 120 — ALTERNATIVE ENGLISH",
        ["AEC 123"] = "AEC 123 — MIL-I: GARO",
        ["SEC 131"] = "SEC 131 — MOTIVATION",
        ["SEC 132"] = "SEC 132 — PERSONALITY DEVELOPMENT",
        ["SEC 133"] = "SEC 133 — PUBLIC SPEAKING",
        ["VAC 140"] = "VAC 140 — ENVIRONMENT STUDIES",
    };

    /// <summary>
    /// Builds code, human-readable name, and full description (for tooltips) from a draft field value.
    /// </summary>
    public static ApplicantElectiveSubjectDto FromStoredChoice(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return new ApplicantElectiveSubjectDto(string.Empty, "—", null);
        }

        var trimmed = raw.Trim();

        if (CodeToFullLabel.TryGetValue(trimmed, out var full))
        {
            return ToDto(trimmed, full);
        }

        var codeCandidate = trimmed;
        var dashSep = trimmed.IndexOf(" — ", StringComparison.Ordinal);
        if (dashSep > 0)
        {
            codeCandidate = trimmed[..dashSep].Trim();
        }

        var autoParen = codeCandidate.IndexOf(" (Auto", StringComparison.OrdinalIgnoreCase);
        if (autoParen > 0)
        {
            codeCandidate = codeCandidate[..autoParen].Trim();
        }

        if (CodeToFullLabel.TryGetValue(codeCandidate, out full))
        {
            return ToDto(codeCandidate, full);
        }

        foreach (var kv in CodeToFullLabel)
        {
            if (trimmed.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
            {
                return ToDto(kv.Key, kv.Value);
            }
        }

        return new ApplicantElectiveSubjectDto(codeCandidate, trimmed, null);
    }

    private static ApplicantElectiveSubjectDto ToDto(string code, string fullLabel)
    {
        var namePart = fullLabel;
        var sepIdx = fullLabel.IndexOf(" — ", StringComparison.Ordinal);
        if (sepIdx >= 0 && sepIdx + 3 < fullLabel.Length)
        {
            namePart = fullLabel[(sepIdx + 3)..].Trim();
        }
        else
        {
            namePart = code;
        }

        return new ApplicantElectiveSubjectDto(code, namePart, fullLabel);
    }
}
