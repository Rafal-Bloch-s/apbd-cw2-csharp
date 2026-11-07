
using EquipmentRental.Enums;
using EquipmentRental.Models;

namespace EquipmentRental.Services;

public class EquipmentService
{
    private readonly List<Equipment> _equipment = new();

    public void AddEquipment(Equipment equipment)
    {
        if (equipment == null)
            throw new ArgumentNullException(nameof(equipment));

        if (_equipment.Exists(item => item.Id == equipment.Id))
            throw new InvalidOperationException("Sprzęt jest już zarejestrowany.");

        _equipment.Add(equipment);
    }

    public IReadOnlyList<Equipment> GetAll()
    {
        return _equipment.AsReadOnly();
    }

    public IReadOnlyList<Equipment> GetAvailable()
    {
        List<Equipment> available = new();

        foreach (Equipment item in _equipment)
        {
            if (item.Status == EquipmentStatus.Available)
            {
                available.Add(item);
            }
        }

        return available.AsReadOnly();
    }

    public Equipment GetById(int id)
    {
        Equipment? equipment = _equipment.Find(item => item.Id == id);

        if (equipment == null)
            throw new KeyNotFoundException($"Nie znaleziono sprzętu o ID {id}.");

        return equipment;
    }

    public void SendToService(int id)
    {
        Equipment equipment = GetById(id);
        equipment.SendToService();
    }

    public void CompleteService(int id)
    {
        Equipment equipment = GetById(id);
        equipment.CompleteService();
    }
}
