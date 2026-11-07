
using EquipmentRental.Enums;

namespace EquipmentRental.Models;

public abstract class Equipment
{
    private static int _nextId = 1;

    public int Id { get; }
    public string Name { get; }
    public string Manufacturer { get; }
    public EquipmentStatus Status { get; private set; }

    protected Equipment(string name, string manufacturer)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nazwa sprzętu nie może być pusta.");

        if (string.IsNullOrWhiteSpace(manufacturer))
            throw new ArgumentException("Producent nie może być pusty.");

        Id = _nextId++;
        Name = name;
        Manufacturer = manufacturer;
        Status = EquipmentStatus.Available;
    }

    public void Rent()
    {
        if (Status != EquipmentStatus.Available)
            throw new InvalidOperationException("Sprzęt nie jest dostępny.");

        Status = EquipmentStatus.Rented;
    }

    public void Return()
    {
        if (Status != EquipmentStatus.Rented)
            throw new InvalidOperationException("Sprzęt nie jest wypożyczony.");

        Status = EquipmentStatus.Available;
    }

    public void SendToService()
    {
        if (Status != EquipmentStatus.Available)
            throw new InvalidOperationException("Nie można oddać tego sprzętu do serwisu.");

        Status = EquipmentStatus.InService;
    }

    public void CompleteService()
    {
        if (Status != EquipmentStatus.InService)
            throw new InvalidOperationException("Sprzęt nie znajduje się w serwisie.");

        Status = EquipmentStatus.Available;
    }

    public abstract string GetDetails();
}
