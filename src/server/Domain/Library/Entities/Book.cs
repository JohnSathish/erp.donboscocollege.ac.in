namespace ERP.Domain.Library.Entities;

public class Book
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Isbn { get; private set; } = string.Empty;

    public string Title { get; private set; } = string.Empty;

    public string Author { get; private set; } = string.Empty;

    public string? Publisher { get; private set; }

    public int? PublicationYear { get; private set; }

    public string? Category { get; private set; }

    public string? Language { get; private set; }

    public int TotalCopies { get; private set; }

    public int AvailableCopies { get; private set; }

    public decimal? Price { get; private set; }

    public string? Location { get; private set; } // Shelf/Rack location

    public string? Description { get; private set; }

    public BookStatus Status { get; private set; } = BookStatus.Available;

    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public string? CreatedBy { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public string? UpdatedBy { get; private set; }

    private Book() { } // For EF Core

    public Book(
        string isbn,
        string title,
        string author,
        int totalCopies,
        string? publisher = null,
        int? publicationYear = null,
        string? category = null,
        string? language = null,
        decimal? price = null,
        string? location = null,
        string? description = null,
        string? createdBy = null)
    {
        Isbn = isbn;
        Title = title;
        Author = author;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
        Publisher = publisher;
        PublicationYear = publicationYear;
        Category = category;
        Language = language;
        Price = price;
        Location = location;
        Description = description;
        Status = BookStatus.Available;
        CreatedBy = createdBy;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string title,
        string author,
        string? publisher,
        int? publicationYear,
        string? category,
        string? language,
        decimal? price,
        string? location,
        string? description,
        string? updatedBy = null)
    {
        Title = title;
        Author = author;
        Publisher = publisher;
        PublicationYear = publicationYear;
        Category = category;
        Language = language;
        Price = price;
        Location = location;
        Description = description;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateCopies(int totalCopies, string? updatedBy = null)
    {
        var difference = totalCopies - TotalCopies;
        TotalCopies = totalCopies;
        AvailableCopies = Math.Max(0, AvailableCopies + difference);
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void DecreaseAvailableCopies(int count = 1)
    {
        if (AvailableCopies >= count)
        {
            AvailableCopies -= count;
            if (AvailableCopies == 0)
            {
                Status = BookStatus.Unavailable;
            }
            UpdatedOnUtc = DateTime.UtcNow;
        }
    }

    public void IncreaseAvailableCopies(int count = 1)
    {
        AvailableCopies = Math.Min(TotalCopies, AvailableCopies + count);
        if (AvailableCopies > 0 && Status == BookStatus.Unavailable)
        {
            Status = BookStatus.Available;
        }
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateStatus(BookStatus status, string? updatedBy = null)
    {
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}

public enum BookStatus
{
    Available,
    Unavailable,
    Lost,
    Damaged
}




