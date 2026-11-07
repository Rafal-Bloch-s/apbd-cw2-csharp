
namespace EquipmentRental.Models;

public class Employee : User
{
    public override string UserType => "Employee";

    public override int MaxActiveRentals => 5;

    public Employee(string firstName, string lastName)
        : base(firstName, lastName)
    {
    }
}
