
namespace EquipmentRental.Models;

public abstract class User
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public abstract string UserType { get; }
    public abstract int MaxActiveRentals { get; }

    protected User(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Imię nie może być puste.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Nazwisko nie może być puste.");

        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
    }

    public override string ToString()
    {
        return $"ID: {Id}, {FirstName} {LastName}, typ: {UserType}, limit: {MaxActiveRentals}";
    }
}
