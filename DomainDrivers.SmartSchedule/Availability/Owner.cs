namespace DomainDrivers.SmartSchedule.Availability;

public record Owner(Guid? OwnerId)
{
    public static Owner None()
    {
        return new Owner((Guid?)null);
    }
}