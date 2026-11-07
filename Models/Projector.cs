
namespace EquipmentRental.Models;

public class Projector : Equipment
{
    public int BrightnessLumens { get; }
    public string Resolution { get; }

    public Projector(string name, string manufacturer,
                     int brightnessLumens, string resolution)
        : base(name, manufacturer)
    {
        if (brightnessLumens <= 0)
            throw new ArgumentException("Jasność musi być dodatnia.");

        if (string.IsNullOrWhiteSpace(resolution))
            throw new ArgumentException("Rozdzielczość nie może być pusta.");

        BrightnessLumens = brightnessLumens;
        Resolution = resolution;
    }

    public override string GetDetails()
    {
        return $"Projektor: {Name}, {Manufacturer}, jasność: {BrightnessLumens} lm, rozdzielczość: {Resolution}";
    }
}
