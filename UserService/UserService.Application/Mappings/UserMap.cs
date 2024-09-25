using CustomerService.Application.Common.Models.Users;
using CustomerService.Domain.Entities;
using Mapster;

namespace CustomerService.Application.Mappings;

public class UserMap : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(UserRegister, HashedPassword), User>()
            .Map(dest => dest, src => src.Item1)
            .Map(dest => dest, src => src.Item2);
    }
}
