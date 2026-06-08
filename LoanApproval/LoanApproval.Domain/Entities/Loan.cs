using LoanApproval.Domain.Enums;

namespace LoanApproval.Domain.Entities;

public class Loan
{
    public long Id  { get; set; }
    public long CustomerId { get; set; }
    public Customer? Customer { get; set; }
    
    public decimal MaxRetailPrice { get; set; }
    public decimal DownPayment { get; set; }
    
    public required string VehicleLicensePlate { get; set; }
    public required string VehicleYear { get; set; }
    public required string VehicleMachineNumber { get; set; }

    public LoanStatus LoanStatus { get; set; } = LoanStatus.Submitted;
    public DateTime? LastSubmittedAt { get; set; }
    public DateTime? LastApprovedAt { get; set; }
    public DateTime? LastRejectedAt { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public IList<LoanStatusHistory> LoanStatusHistory { get; set; } = new List<LoanStatusHistory>();
}