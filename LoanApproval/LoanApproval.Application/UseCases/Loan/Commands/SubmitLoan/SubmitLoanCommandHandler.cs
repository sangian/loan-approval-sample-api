using LoanApproval.Application.Repositories;
using LoanApproval.Application.Services;
using Mediator;

namespace LoanApproval.Application.UseCases.Loan.Commands.SubmitLoan;

public sealed class SubmitLoanCommandHandler : ICommandHandler<SubmitLoanCommand, SubmitLoanResponseType>
{
    private readonly ILoanRepository _loanRepository;
    private readonly ILicensePlateLockService _licensePlateLockService;

    public SubmitLoanCommandHandler(ILoanRepository loanRepository,
        ILicensePlateLockService licensePlateLockService)
    {
        _loanRepository = loanRepository;
        _licensePlateLockService = licensePlateLockService;
    }
    public async ValueTask<SubmitLoanResponseType> Handle(SubmitLoanCommand command, CancellationToken cancellationToken)
    {
        if (await _loanRepository.IsLicensePlateAvailable(command.Request.VehicleLicensePlate, cancellationToken))
        {
            if (await _licensePlateLockService.TryLock(command.Request.VehicleLicensePlate))
            {
                var loan = new Domain.Entities.Loan
                {
                    CustomerId = command.Request.CustomerId,
                    MaxRetailPrice = command.Request.MaxRetailPrice,
                    DownPayment = command.Request.DownPayment,
                    VehicleLicensePlate = command.Request.VehicleLicensePlate,
                    VehicleYear = command.Request.VehicleYear,
                    VehicleMachineNumber = command.Request.VehicleMachineNumber
                };
                
                if (_loanRepository.Submit(loan))
                {
                    await _loanRepository.SaveChanges(cancellationToken);
                    await _licensePlateLockService.TryUnlock(command.Request.VehicleLicensePlate);
                    
                    return new SubmitLoanResponseType
                    {
                        Success = true,
                        CustomerId = loan.CustomerId,
                        MaxRetailPrice = loan.MaxRetailPrice,
                        DownPayment = loan.DownPayment,
                        VehicleLicensePlate = loan.VehicleLicensePlate,
                        VehicleYear = loan.VehicleYear,
                        VehicleMachineNumber =  loan.VehicleMachineNumber
                    };
                }
                
                await _licensePlateLockService.TryUnlock(command.Request.VehicleLicensePlate);
            }
        }

        return new SubmitLoanResponseType
        {
            Success = false
        };
    }
}