using CustomerService.Application.Common.Interfaces;
using CustomerService.Domain.Entities;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries;

public record GetCustomerDetailQuery
(int CutomerId) : IRequest<User>;

public class GetCustomerDetailQueryHandler : IRequestHandler<GetCustomerDetailQuery, User>
{
    private readonly IApplicationDbContext _context;

    public GetCustomerDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetCustomerDetailQuery query, CancellationToken cancellationToken)
    {
        var customer = _context.Users.FirstOrDefault(u => u.Id == query.CutomerId);
        if (customer == null)
        {
            throw new Exception("Müþteri bulunamadi");
        }
        return customer;
    }
}