using LoanApproval.Application.UseCases.Loan.Commands.SubmitLoan;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace LoanApproval.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoanController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitLoan([FromBody] SubmitLoanRequestType request)
    {
        var result = await _mediator.Send(new SubmitLoanCommand
        {
            Request = request
        });

        // TODO return non 20x response for Success = false result
        
        return Ok(result);
    }
}