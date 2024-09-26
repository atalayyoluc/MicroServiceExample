using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Common.Interfaces.Repositories;
using CustomerService.Domain.Entities;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries;

public record GetCustomerDetailQuery
(int CutomerId) : IRequest<User>;

public class GetCustomerDetailQueryHandler : IRequestHandler<GetCustomerDetailQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetCustomerDetailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetCustomerDetailQuery query, CancellationToken cancellationToken)
    {
        var customer = await _userRepository.GetUserByIdAsync(query.CutomerId);
        if (customer == null)
        {
            throw new Exception("Müþteri bulunamadi");
        }
        return customer;
    }
}