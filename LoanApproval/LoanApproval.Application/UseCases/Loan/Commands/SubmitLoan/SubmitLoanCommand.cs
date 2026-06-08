using Mediator;

namespace LoanApproval.Application.UseCases.Loan.Commands.SubmitLoan;

public sealed class SubmitLoanCommand : ICommand<SubmitLoanResponseType>
{
    public required SubmitLoanRequestType Request { get; set; }
}

public sealed class SubmitLoanRequestType
{
    public long CustomerId { get; set; }
    public decimal MaxRetailPrice { get; set; }
    public decimal DownPayment { get; set; }
    public required string VehicleLicensePlate { get; set; }
    public required string VehicleYear { get; set; }
    public required string VehicleMachineNumber { get; set; }
}

public sealed class SubmitLoanResponseType
{
    public bool Success { get; set; } = true;
    public long? LoanId { get; set; }
    public long? CustomerId { get; set; }
    public decimal? MaxRetailPrice { get; set; }
    public decimal? DownPayment { get; set; }
    public string? VehicleLicensePlate { get; set; }
    public string? VehicleYear { get; set; }
    public string? VehicleMachineNumber { get; set; }
    public string? LoanStatus { get; set; }
}