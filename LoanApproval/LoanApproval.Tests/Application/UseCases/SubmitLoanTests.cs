using LoanApproval.Application.Repositories;
using LoanApproval.Application.Services;
using LoanApproval.Application.UseCases.Loan.Commands.SubmitLoan;
using LoanApproval.Domain.Entities;
using NSubstitute;

namespace LoanApproval.Tests.Application.UseCases;

public class SubmitLoanTests
{
    [Fact]
    public async Task SubmitLoan_ShouldSubmitSuccessfully()
    {
        var loanRepository = Substitute.For<ILoanRepository>();
        var licensePlateLockService = Substitute.For<ILicensePlateLockService>();

        var command = new SubmitLoanCommand
        {
            Request = new SubmitLoanRequestType
            {
                CustomerId = 1,
                MaxRetailPrice = 250_000_000m,
                DownPayment = 50_000_000m,
                VehicleLicensePlate = "B 1234 ABC",
                VehicleYear = "2026",
                VehicleMachineNumber = "NUM-1001"
            }
        };

        loanRepository.IsLicensePlateAvailable(command.Request.VehicleLicensePlate, Arg.Any<CancellationToken>())
            .Returns(true);
        loanRepository.Submit(Arg.Any<Loan>(), Arg.Any<CancellationToken>())
            .Returns(true);
        licensePlateLockService.TryLock(command.Request.VehicleLicensePlate).Returns(true);
        licensePlateLockService.TryUnlock(command.Request.VehicleLicensePlate).Returns(true);

        var handler = new SubmitLoanCommandHandler(loanRepository, licensePlateLockService);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal(command.Request.CustomerId, result.CustomerId);
        Assert.Equal(command.Request.MaxRetailPrice, result.MaxRetailPrice);
        Assert.Equal(command.Request.DownPayment, result.DownPayment);
        Assert.Equal(command.Request.VehicleLicensePlate, result.VehicleLicensePlate);
        Assert.Equal(command.Request.VehicleYear, result.VehicleYear);
        Assert.Equal(command.Request.VehicleMachineNumber, result.VehicleMachineNumber);

        loanRepository.Received(1).Submit(
            Arg.Is<Loan>(l =>
                l.CustomerId == command.Request.CustomerId &&
                l.MaxRetailPrice == command.Request.MaxRetailPrice &&
                l.DownPayment == command.Request.DownPayment &&
                l.VehicleLicensePlate == command.Request.VehicleLicensePlate &&
                l.VehicleYear == command.Request.VehicleYear &&
                l.VehicleMachineNumber == command.Request.VehicleMachineNumber),
            Arg.Any<CancellationToken>());
        await loanRepository.Received(1).SaveChanges(Arg.Any<CancellationToken>());
        await licensePlateLockService.Received(1).TryLock(command.Request.VehicleLicensePlate);
        await licensePlateLockService.Received(1).TryUnlock(command.Request.VehicleLicensePlate);
    }

    [Fact]
    public async Task SubmitLoan_ShouldFailForExistingLicensePlate()
    {
        var loanRepository = Substitute.For<ILoanRepository>();
        var licensePlateLockService = Substitute.For<ILicensePlateLockService>();

        var command = new SubmitLoanCommand
        {
            Request = new SubmitLoanRequestType
            {
                CustomerId = 1,
                MaxRetailPrice = 250_000_000m,
                DownPayment = 50_000_000m,
                VehicleLicensePlate = "B 1234 ABC",
                VehicleYear = "2026",
                VehicleMachineNumber = "NUM-1001"
            }
        };

        loanRepository.IsLicensePlateAvailable(command.Request.VehicleLicensePlate, Arg.Any<CancellationToken>())
            .Returns(false);

        var handler = new SubmitLoanCommandHandler(loanRepository, licensePlateLockService);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Null(result.CustomerId);
        Assert.Null(result.VehicleLicensePlate);

        loanRepository.DidNotReceive().Submit(Arg.Any<Loan>(), Arg.Any<CancellationToken>());
        await loanRepository.DidNotReceive().SaveChanges(Arg.Any<CancellationToken>());
        await licensePlateLockService.DidNotReceive().TryLock(Arg.Any<string>());
        await licensePlateLockService.DidNotReceive().TryUnlock(Arg.Any<string>());
    }

    [Fact]
    public async Task SubmitLoan_RaceCondition_ShouldFailWhenLicensePlateIsLocked()
    {
        var loanRepository = Substitute.For<ILoanRepository>();
        var licensePlateLockService = Substitute.For<ILicensePlateLockService>();

        var command = new SubmitLoanCommand
        {
            Request = new SubmitLoanRequestType
            {
                CustomerId = 1,
                MaxRetailPrice = 250_000_000m,
                DownPayment = 50_000_000m,
                VehicleLicensePlate = "B 1234 ABC",
                VehicleYear = "2026",
                VehicleMachineNumber = "NUM-1001"
            }
        };

        loanRepository.IsLicensePlateAvailable(command.Request.VehicleLicensePlate, Arg.Any<CancellationToken>())
            .Returns(true);
        licensePlateLockService.TryLock(command.Request.VehicleLicensePlate).Returns(false);

        var handler = new SubmitLoanCommandHandler(loanRepository, licensePlateLockService);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Null(result.CustomerId);
        Assert.Null(result.VehicleLicensePlate);

        await licensePlateLockService.Received(1).TryLock(command.Request.VehicleLicensePlate);
        loanRepository.DidNotReceive().Submit(Arg.Any<Loan>(), Arg.Any<CancellationToken>());
        await loanRepository.DidNotReceive().SaveChanges(Arg.Any<CancellationToken>());
        await licensePlateLockService.DidNotReceive().TryUnlock(Arg.Any<string>());
    }
}
