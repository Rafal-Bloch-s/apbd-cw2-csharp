
namespace EquipmentRental.Models;

public class Rental
{
    private static int _nextId = 1;

    public int Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }

    public DateTime RentedAt { get; }
    public int DurationDays { get; }
    public DateTime DueAt { get; }
    public DateTime? ReturnedAt { get; private set; }

    public decimal Penalty { get; private set; }

    public bool IsActive => ReturnedAt == null;

    public bool? WasReturnedOnTime =>
        ReturnedAt.HasValue
            ? ReturnedAt.Value <= DueAt
            : null;

    public Rental(User user, Equipment equipment,
                  DateTime rentedAt, int durationDays)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (equipment == null)
            throw new ArgumentNullException(nameof(equipment));

        if (durationDays <= 0)
            throw new ArgumentException("Czas wypożyczenia musi być dodatni.");

        Id = _nextId++;
        User = user;
        Equipment = equipment;

        RentedAt = rentedAt;
        DurationDays = durationDays;
        DueAt = rentedAt.AddDays(durationDays);

        ReturnedAt = null;
        Penalty = 0m;
    }

    public bool IsOverdue(DateTime currentDate)
    {
        return IsActive && currentDate > DueAt;
    }

    public void CompleteReturn(DateTime returnDate, decimal penalty)
    {
        if (!IsActive)
            throw new InvalidOperationException("Sprzęt został już zwrócony.");

        if (returnDate < RentedAt)
            throw new ArgumentException("Data zwrotu nie może być wcześniejsza niż wypożyczenie.");

        if (penalty < 0)
            throw new ArgumentException("Kara nie może być ujemna.");

        ReturnedAt = returnDate;
        Penalty = penalty;
    }
}
