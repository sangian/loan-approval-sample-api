using LoanApproval.Domain.Entities;

namespace LoanApproval.Application.Repositories;

public interface ILoanRepository
{
    public Task<ICollection<Loan>> GetLoansByCustomerId(long customerId, CancellationToken cancellationToken = default);
    public Task<Loan?> GetLoanById(long loanId, CancellationToken cancellationToken = default);
    public Task<bool> IsLicensePlateAvailable(string licensePlate, CancellationToken cancellationToken = default);
    public bool Submit(Loan loan, CancellationToken cancellationToken = default);
    public Task<bool> Approve(long loanId, long approverId, CancellationToken cancellationToken = default);
    public Task<int> SaveChanges(CancellationToken cancellationToken = default);
}