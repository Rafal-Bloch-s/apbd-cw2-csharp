
namespace EquipmentRental.Models;

public class Camera : Equipment
{
    public int Megapixels { get; }
    public int OpticalZoom { get; }

    public Camera(string name, string manufacturer,
                  int megapixels, int opticalZoom)
        : base(name, manufacturer)
    {
        if (megapixels <= 0 || opticalZoom <= 0)
            throw new ArgumentException("Parametry kamery muszą być dodatnie.");

        Megapixels = megapixels;
        OpticalZoom = opticalZoom;
    }

    public override string GetDetails()
    {
        return $"Kamera: {Name}, {Manufacturer}, matryca: {Megapixels} MP, zoom optyczny: {OpticalZoom}x";
    }
}
