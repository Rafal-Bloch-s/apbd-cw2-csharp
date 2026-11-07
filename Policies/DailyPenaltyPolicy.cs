
namespace EquipmentRental.Policies;

public class DailyPenaltyPolicy : IPenaltyPolicy
{
    private const decimal DailyRate = 5m;

    public decimal CalculatePenalty(DateTime dueAt, DateTime returnedAt)
    {
        if (returnedAt <= dueAt)
            return 0m;

        double lateDays = (returnedAt - dueAt).TotalDays;
        int startedDays = (int)Math.Ceiling(lateDays);

        return startedDays * DailyRate;
    }
}
