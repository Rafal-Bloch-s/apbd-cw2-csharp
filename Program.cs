
using EquipmentRental.Models;
using EquipmentRental.Services;
using EquipmentRental.Policies;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Tworzenie serwisów
EquipmentService equipmentService = new EquipmentService();
UserService userService = new UserService();

IPenaltyPolicy penaltyPolicy = new DailyPenaltyPolicy();

RentalService rentalService = new RentalService(
    equipmentService,
    userService,
    penaltyPolicy
);

ReportService reportService = new ReportService(
    equipmentService,
    userService,
    rentalService
);

Console.WriteLine("======================================");
Console.WriteLine(" SYSTEM WYPOŻYCZALNI SPRZĘTU UCZELNI");
Console.WriteLine("======================================");

// 1. Rejestracja użytkowników

Console.WriteLine("\n=== 1. UŻYTKOWNICY ===");

Student student = new Student("Jan", "Kowalski");
Employee employee = new Employee("Anna", "Nowak");

userService.AddUser(student);
userService.AddUser(employee);

foreach (User user in userService.GetAll())
{
    Console.WriteLine(user);
}

// 2. Rejestracja sprzętu

Console.WriteLine("\n=== 2. DODAWANIE SPRZĘTU ===");

equipmentService.AddEquipment(
    new Laptop("Latitude 5420", "Dell", 16, 512));

equipmentService.AddEquipment(
    new Camera("EOS R50", "Canon", 24, 2));

equipmentService.AddEquipment(
    new Projector("EB-X49", "Epson", 3600, "1024x768"));

equipmentService.AddEquipment(
    new Laptop("ThinkPad T14", "Lenovo", 32, 1000));

equipmentService.AddEquipment(
    new Projector("VPL-PHZ51", "Sony", 5800, "1920x1200"));

Console.WriteLine("Zarejestrowano 5 urządzeń.");

// 3. Oznaczenie sprzętu jako niedostępnego

Console.WriteLine("\n=== 3. SPRZĘT W SERWISIE ===");

equipmentService.SendToService(4);

Console.WriteLine("ThinkPad T14 został oddany do serwisu.");

// 4. Wyświetlanie wszystkich urządzeń

Console.WriteLine("\n=== 4. LISTA SPRZĘTU ===");

foreach (Equipment item in equipmentService.GetAll())
{
    Console.WriteLine(
        $"ID {item.Id}: {item.GetDetails()} | {item.Status}");
}

// 5. Wyświetlanie dostępnego sprzętu

Console.WriteLine("\n=== 5. DOSTĘPNY SPRZĘT ===");

foreach (Equipment item in equipmentService.GetAvailable())
{
    Console.WriteLine($"{item.Id}. {item.Name}");
}

// 6. Poprawne wypożyczenia

Console.WriteLine("\n=== 6. POPRAWNE WYPOŻYCZENIA ===");

DateTime startDate = new DateTime(2026, 11, 1);

Rental rental1 = rentalService.RentEquipment(
    student.Id, 1, startDate, 3);

Rental rental2 = rentalService.RentEquipment(
    student.Id, 2, startDate, 2);

Rental rental3 = rentalService.RentEquipment(
    employee.Id, 3, startDate, 4);

foreach (Rental rental in rentalService.GetAll())
{
    Console.WriteLine(
        $"Wypożyczenie #{rental.Id}: " +
        $"{rental.User.FirstName} {rental.User.LastName} - " +
        $"{rental.Equipment.Name}, " +
        $"termin: {rental.DueAt:yyyy-MM-dd}");
}

// 7. Próba przekroczenia limitu studenta

Console.WriteLine("\n=== 7. PRZEKROCZENIE LIMITU ===");

try
{
    rentalService.RentEquipment(
        student.Id, 5, startDate, 3);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Odrzucono: {ex.Message}");
}

// 8. Próba wypożyczenia zajętego sprzętu

Console.WriteLine("\n=== 8. SPRZĘT JUŻ WYPOŻYCZONY ===");

try
{
    rentalService.RentEquipment(
        employee.Id, 1, startDate, 3);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Odrzucono: {ex.Message}");
}

// 9. Próba wypożyczenia sprzętu w serwisie

Console.WriteLine("\n=== 9. SPRZĘT NIEDOSTĘPNY ===");

try
{
    rentalService.RentEquipment(
        employee.Id, 4, startDate, 3);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Odrzucono: {ex.Message}");
}

// 10. Aktywne wypożyczenia użytkownika

Console.WriteLine("\n=== 10. WYPOŻYCZENIA STUDENTA ===");

foreach (Rental rental in
    rentalService.GetActiveRentalsForUser(student.Id))
{
    Console.WriteLine(
        $"#{rental.Id}: {rental.Equipment.Name}");
}

// 11. Zwrot w terminie

Console.WriteLine("\n=== 11. ZWROT W TERMINIE ===");

rentalService.ReturnEquipment(
    rental1.Id,
    new DateTime(2026, 11, 4));

Console.WriteLine($"Sprzęt: {rental1.Equipment.Name}");
Console.WriteLine($"Terminowo: {rental1.WasReturnedOnTime}");
Console.WriteLine($"Kara: {rental1.Penalty} zł");
Console.WriteLine($"Status: {rental1.Equipment.Status}");

// 12. Zwrot po terminie

Console.WriteLine("\n=== 12. ZWROT PO TERMINIE ===");

rentalService.ReturnEquipment(
    rental2.Id,
    new DateTime(2026, 11, 5, 12, 0, 0));

Console.WriteLine($"Sprzęt: {rental2.Equipment.Name}");
Console.WriteLine($"Terminowo: {rental2.WasReturnedOnTime}");
Console.WriteLine($"Kara: {rental2.Penalty} zł");
Console.WriteLine($"Status: {rental2.Equipment.Status}");

// 13. Próba ponownego zwrotu

Console.WriteLine("\n=== 13. PODWÓJNY ZWROT ===");

try
{
    rentalService.ReturnEquipment(
        rental1.Id,
        new DateTime(2026, 11, 6));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Odrzucono: {ex.Message}");
}

// 14. Przeterminowane wypożyczenia

Console.WriteLine("\n=== 14. ZALEGŁE WYPOŻYCZENIA ===");

DateTime reportDate = new DateTime(2026, 11, 7);

foreach (Rental rental in
    rentalService.GetOverdueRentals(reportDate))
{
    Console.WriteLine(
        $"#{rental.Id}: {rental.Equipment.Name}, " +
        $"termin: {rental.DueAt:yyyy-MM-dd}");
}

// 15. Raport końcowy

Console.WriteLine();
Console.WriteLine(reportService.GenerateReport(reportDate));

Console.WriteLine("=== KONIEC DEMONSTRACJI ===");
