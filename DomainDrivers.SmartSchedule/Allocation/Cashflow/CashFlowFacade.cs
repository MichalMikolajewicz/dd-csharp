﻿using DomainDrivers.SmartSchedule.Shared;
using Microsoft.EntityFrameworkCore;

namespace DomainDrivers.SmartSchedule.Allocation.Cashflow;

public class CashFlowFacade
{
    private readonly ICashflowDbContext _cashflowDbContext;
    private readonly IEventsPublisher _eventsPublisher;
    private readonly TimeProvider _timeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CashFlowFacade(ICashflowDbContext cashflowDbContext, IEventsPublisher eventsPublisher,
        TimeProvider timeProvider, IUnitOfWork unitOfWork)
    {
        _cashflowDbContext = cashflowDbContext;
        _eventsPublisher = eventsPublisher;
        _timeProvider = timeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task AddIncomeAndCost(ProjectAllocationsId projectId, Income income, Cost cost)
    {
        await _unitOfWork.InTransaction(async () =>
        {
            var cashflow = await _cashflowDbContext.Cashflows.FindAsync(projectId);
            if (cashflow == null)
            {
                cashflow = new Cashflow(projectId);
                await _cashflowDbContext.Cashflows.AddAsync(cashflow);
            }

            cashflow.Update(income, cost);
            await _eventsPublisher.Publish(new EarningsRecalculated(projectId, cashflow.Earnings(),
                _timeProvider.GetUtcNow().DateTime));
        });
    }

    public async Task<Earnings> Find(ProjectAllocationsId projectId)
    {
        var byId = await _cashflowDbContext.Cashflows.SingleAsync(x => x.ProjectId == projectId);
        return byId.Earnings();
    }

    public async Task<IDictionary<ProjectAllocationsId, Earnings>> FindAllEarnings()
    {
        return (await _cashflowDbContext.Cashflows.ToListAsync())
            .ToDictionary(x => x.ProjectId, x => x.Earnings());
    }
}