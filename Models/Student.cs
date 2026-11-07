
namespace EquipmentRental.Models;

public class Student : User
{
    public override string UserType => "Student";

    public override int MaxActiveRentals => 2;

    public Student(string firstName, string lastName)
        : base(firstName, lastName)
    {
    }
}
