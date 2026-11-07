
namespace EquipmentRental.Models;

public class Laptop : Equipment
{
    public int RamGb { get; }
    public int StorageGb { get; }

    public Laptop(string name, string manufacturer,
                  int ramGb, int storageGb)
        : base(name, manufacturer)
    {
        if (ramGb <= 0 || storageGb <= 0)
            throw new ArgumentException("Parametry laptopa muszą być dodatnie.");

        RamGb = ramGb;
        StorageGb = storageGb;
    }

    public override string GetDetails()
    {
        return $"Laptop: {Name}, {Manufacturer}, RAM: {RamGb} GB, dysk: {StorageGb} GB";
    }
}
