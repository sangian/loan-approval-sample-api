using LoanApproval.Domain.Enums;

namespace LoanApproval.Domain.Entities;

public class LoanStatusHistory
{
    public long Id { get; set; }
    public long LoanId { get; set; }
    public Loan? Loan { get; set; }
    
    public required LoanStatus Status { get; set; }
    public required DateTime StatusUpdatedAt { get; set; }
    
    public long? ApproverId { get; set; }
    public Employee? Approver { get; set; }
}