
using EquipmentRental.Models;

namespace EquipmentRental.Services;

public class UserService
{
    private readonly List<User> _users = new();

    public void AddUser(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (_users.Exists(item => item.Id == user.Id))
            throw new InvalidOperationException("Użytkownik jest już zarejestrowany.");

        _users.Add(user);
    }

    public IReadOnlyList<User> GetAll()
    {
        return _users.AsReadOnly();
    }

    public User GetById(int id)
    {
        User? user = _users.Find(item => item.Id == id);

        if (user == null)
            throw new KeyNotFoundException($"Nie znaleziono użytkownika o ID {id}.");

        return user;
    }
}
