﻿namespace DomainDrivers.SmartSchedule.Allocation;

public record CapabilitiesAllocated(
    Guid EventId,
    Guid AllocatedCapabilityId,
    ProjectAllocationsId ProjectId,
    Demands MissingDemands,
    DateTime OccurredAt)
{
    public CapabilitiesAllocated(Guid allocatedCapabilityId, ProjectAllocationsId projectId, Demands missingDemands,
        DateTime occuredAt)
        : this(Guid.NewGuid(), allocatedCapabilityId, projectId, missingDemands, occuredAt)
    {
    }
}