using LoanApproval.Application.Repositories;
using LoanApproval.Domain.Entities;
using LoanApproval.Domain.Enums;
using LoanApproval.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanApproval.Infrastructure.Repositories;

public sealed class LoanRepository : ILoanRepository
{
    private readonly AppWriteDbContext _writeDbContext;
    private readonly AppQueryDbContext _queryDbContext;

    public LoanRepository(AppWriteDbContext writeDbContext,
        AppQueryDbContext queryDbContext)
    {
        _writeDbContext = writeDbContext;
        _queryDbContext = queryDbContext;
    }
    public async Task<ICollection<Loan>> GetLoansByCustomerId(long customerId, CancellationToken cancellationToken = default)
    {
        return await _queryDbContext.Loans
            .Where(l => l.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Loan?> GetLoanById(long loanId, CancellationToken cancellationToken = default)
    {
        return await _queryDbContext.Loans
            .Where(l => l.Id == loanId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsLicensePlateAvailable(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await _queryDbContext.Loans
            .AnyAsync(l => l.LoanStatus != LoanStatus.Submitted 
                           || l.LoanStatus != LoanStatus.Approved, cancellationToken);
    }

    public bool Submit(Loan loan, CancellationToken cancellationToken = default)
    {
        loan.LoanStatus =  LoanStatus.Submitted;
        loan.LastSubmittedAt = DateTime.UtcNow;
        
        loan.LoanStatusHistory.Add(new LoanStatusHistory()
        {
            Status = LoanStatus.Submitted,
            StatusUpdatedAt = loan.LastSubmittedAt ?? DateTime.UtcNow
        });
        
        _writeDbContext.Add(loan);

        return true;
    }

    public async Task<bool> Approve(long loanId, long approverId, CancellationToken cancellationToken = default)
    {
        var loan = await _writeDbContext.Loans
            .Where(l => l.Id == loanId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (loan is null) return false;
        
        loan.LoanStatus = LoanStatus.Approved;
        loan.LastApprovedAt = DateTime.UtcNow;
        
        loan.LoanStatusHistory.Add(new LoanStatusHistory()
        {
            Status = LoanStatus.Approved,
            StatusUpdatedAt = loan.LastSubmittedAt ?? DateTime.UtcNow
        });

        return true;
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        return await _writeDbContext.SaveChangesAsync(cancellationToken);
    }
}