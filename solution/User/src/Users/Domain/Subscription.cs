using Common.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Users.Domain;

public class Subscription : Entity
{
    public required Guid UserId { get; set; }

    public required SubscriptionType SubscriptionType { get; set; }

    public required DateTime StartTime { get; set; }

    public required DateTime EndTime { get; set; }
    
    public User User { get; set; }

    [SetsRequiredMembers]
    public Subscription(Guid userId, SubscriptionType subscriptionType, DateTime startTime, DateTime endTime)
    {
        UserId = userId;
        SubscriptionType = subscriptionType;
        StartTime = startTime;
        EndTime = endTime;
    }
}