
using System.Text;
using EquipmentRental.Enums;
using EquipmentRental.Models;

namespace EquipmentRental.Services;

public class ReportService
{
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly RentalService _rentalService;

    public ReportService(
        EquipmentService equipmentService,
        UserService userService,
        RentalService rentalService)
    {
        _equipmentService = equipmentService
            ?? throw new ArgumentNullException(nameof(equipmentService));

        _userService = userService
            ?? throw new ArgumentNullException(nameof(userService));

        _rentalService = rentalService
            ?? throw new ArgumentNullException(nameof(rentalService));
    }

    public string GenerateReport(DateTime reportDate)
    {
        IReadOnlyList<Equipment> equipment = _equipmentService.GetAll();
        IReadOnlyList<User> users = _userService.GetAll();
        IReadOnlyList<Rental> rentals = _rentalService.GetAll();

        int available = 0;
        int rented = 0;
        int inService = 0;

        foreach (Equipment item in equipment)
        {
            switch (item.Status)
            {
                case EquipmentStatus.Available:
                    available++;
                    break;

                case EquipmentStatus.Rented:
                    rented++;
                    break;

                case EquipmentStatus.InService:
                    inService++;
                    break;
            }
        }

        int activeRentals = 0;
        int completedRentals = 0;
        int returnedOnTime = 0;
        int returnedLate = 0;

        decimal totalPenalties = 0m;

        foreach (Rental rental in rentals)
        {
            if (rental.IsActive)
            {
                activeRentals++;
            }
            else
            {
                completedRentals++;

                if (rental.WasReturnedOnTime == true)
                    returnedOnTime++;
                else
                    returnedLate++;
            }

            totalPenalties += rental.Penalty;
        }

        int overdueRentals =
            _rentalService.GetOverdueRentals(reportDate).Count;

        StringBuilder report = new StringBuilder();

        report.AppendLine("=== RAPORT WYPOŻYCZALNI ===");
        report.AppendLine($"Data raportu: {reportDate:yyyy-MM-dd}");

        report.AppendLine();
        report.AppendLine("--- UŻYTKOWNICY ---");
        report.AppendLine($"Liczba użytkowników: {users.Count}");

        report.AppendLine();
        report.AppendLine("--- SPRZĘT ---");
        report.AppendLine($"Wszystkie urządzenia: {equipment.Count}");
        report.AppendLine($"Dostępne: {available}");
        report.AppendLine($"Wypożyczone: {rented}");
        report.AppendLine($"W serwisie: {inService}");

        report.AppendLine();
        report.AppendLine("--- WYPOŻYCZENIA ---");
        report.AppendLine($"Wszystkie wypożyczenia: {rentals.Count}");
        report.AppendLine($"Aktywne: {activeRentals}");
        report.AppendLine($"Zakończone: {completedRentals}");
        report.AppendLine($"Przeterminowane aktywne: {overdueRentals}");
        report.AppendLine($"Zwrócone terminowo: {returnedOnTime}");
        report.AppendLine($"Zwrócone po terminie: {returnedLate}");

        report.AppendLine();
        report.AppendLine("--- KARY ---");
        report.AppendLine($"Łączne naliczone kary: {totalPenalties:0.00} zł");

        return report.ToString();
    }
}
