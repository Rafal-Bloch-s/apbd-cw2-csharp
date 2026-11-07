
namespace EquipmentRental.Policies;

public interface IPenaltyPolicy
{
    decimal CalculatePenalty(DateTime dueAt, DateTime returnedAt);
}
