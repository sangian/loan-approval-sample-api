namespace LoanApproval.Application.Services;

public interface ILicensePlateLockService
{
    public Task<bool> TryLock(string licensePlate);
    public Task<bool> TryUnlock(string licensePlate);
}