using LoanApproval.Application.Services;

namespace LoanApproval.Infrastructure.Services;

public sealed class LicensePlateLockService : ILicensePlateLockService
{
    public Task<bool> TryLock(string licensePlate)
    {
        return Task.FromResult(true);
    }

    public Task<bool> TryUnlock(string licensePlate)
    {
        return Task.FromResult(true);
    }
}