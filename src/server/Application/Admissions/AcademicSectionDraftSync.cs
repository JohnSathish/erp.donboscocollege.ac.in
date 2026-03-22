using ERP.Application.Admissions.DTOs;

namespace ERP.Application.Admissions;

/// <summary>
/// Keeps legacy <see cref="AcademicSection.ClassXII"/> in sync with <see cref="AcademicSection.ClassXiiSubjects"/>
/// during migration to the board/stream + dynamic rows model.
/// </summary>
public static class AcademicSectionDraftSync
{
    public static void HydrateFromLegacy(AcademicSection? academics)
    {
        if (academics is null)
        {
            return;
        }

        if (academics.ClassXiiSubjects is { Count: > 0 })
        {
            return;
        }

        if (academics.ClassXII is not { Count: > 0 })
        {
            return;
        }

        foreach (var row in academics.ClassXII)
        {
            if (string.IsNullOrWhiteSpace(row.Subject) && string.IsNullOrWhiteSpace(row.Marks))
            {
                continue;
            }

            academics.ClassXiiSubjects.Add(new ClassXiiSubjectRowDto
            {
                Subject = row.Subject,
                Marks = row.Marks,
                EntryMode = "manual"
            });
        }
    }

    public static void PushToLegacy(AcademicSection? academics)
    {
        if (academics is null || academics.ClassXiiSubjects is not { Count: > 0 })
        {
            return;
        }

        academics.ClassXII = academics.ClassXiiSubjects
            .Select(x => new SubjectMarkDto
            {
                Subject = x.Subject,
                Marks = x.Marks
            })
            .ToList();
    }

    /// <summary>Ensures both representations match (e.g. after loading old JSON for export).</summary>
    public static void SyncBidirectional(AcademicSection? academics)
    {
        HydrateFromLegacy(academics);
        PushToLegacy(academics);
    }
}
