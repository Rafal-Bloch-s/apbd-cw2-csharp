
using EquipmentRental.Enums;
using EquipmentRental.Models;
using EquipmentRental.Policies;

namespace EquipmentRental.Services;

public class RentalService
{
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly IPenaltyPolicy _penaltyPolicy;
    private readonly List<Rental> _rentals = new();

    public RentalService(
        EquipmentService equipmentService,
        UserService userService,
        IPenaltyPolicy penaltyPolicy)
    {
        _equipmentService = equipmentService
            ?? throw new ArgumentNullException(nameof(equipmentService));

        _userService = userService
            ?? throw new ArgumentNullException(nameof(userService));

        _penaltyPolicy = penaltyPolicy
            ?? throw new ArgumentNullException(nameof(penaltyPolicy));
    }

    public Rental RentEquipment(
        int userId,
        int equipmentId,
        DateTime rentedAt,
        int durationDays)
    {
        User user = _userService.GetById(userId);
        Equipment equipment = _equipmentService.GetById(equipmentId);

        if (durationDays <= 0)
            throw new ArgumentException("Czas wypożyczenia musi być dodatni.");

        if (equipment.Status != EquipmentStatus.Available)
            throw new InvalidOperationException(
                "Sprzęt nie jest dostępny do wypożyczenia.");

        int activeCount = 0;

        foreach (Rental rental in _rentals)
        {
            if (rental.User.Id == userId && rental.IsActive)
                activeCount++;
        }

        if (activeCount >= user.MaxActiveRentals)
            throw new InvalidOperationException(
                $"Użytkownik osiągnął limit {user.MaxActiveRentals} aktywnych wypożyczeń.");

        Rental newRental = new Rental(
            user, equipment, rentedAt, durationDays);

        equipment.Rent();
        _rentals.Add(newRental);

        return newRental;
    }

    public Rental ReturnEquipment(int rentalId, DateTime returnDate)
    {
        Rental rental = GetById(rentalId);

        if (!rental.IsActive)
            throw new InvalidOperationException(
                "To wypożyczenie zostało już zakończone.");

        if (returnDate < rental.RentedAt)
            throw new ArgumentException(
                "Data zwrotu nie może być wcześniejsza niż wypożyczenie.");

        if (rental.Equipment.Status != EquipmentStatus.Rented)
            throw new InvalidOperationException(
                "Sprzęt nie ma statusu wypożyczonego.");

        decimal penalty = _penaltyPolicy.CalculatePenalty(
            rental.DueAt, returnDate);

        rental.CompleteReturn(returnDate, penalty);
        rental.Equipment.Return();

        return rental;
    }

    public IReadOnlyList<Rental> GetAll()
    {
        return _rentals.AsReadOnly();
    }

    public IReadOnlyList<Rental> GetActiveRentalsForUser(int userId)
    {
        _userService.GetById(userId);

        List<Rental> activeRentals = new();

        foreach (Rental rental in _rentals)
        {
            if (rental.User.Id == userId && rental.IsActive)
                activeRentals.Add(rental);
        }

        return activeRentals.AsReadOnly();
    }

    public IReadOnlyList<Rental> GetOverdueRentals(DateTime currentDate)
    {
        List<Rental> overdueRentals = new();

        foreach (Rental rental in _rentals)
        {
            if (rental.IsOverdue(currentDate))
                overdueRentals.Add(rental);
        }

        return overdueRentals.AsReadOnly();
    }

    public Rental GetById(int id)
    {
        Rental? rental = _rentals.Find(item => item.Id == id);

        if (rental == null)
            throw new KeyNotFoundException(
                $"Nie znaleziono wypożyczenia o ID {id}.");

        return rental;
    }
}
