using CustomerService.Application.Common.Models.Identity;
using Mapster;

namespace CustomerService.Application.Mappings;

public class AuthMap : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(AuthenticateResult, AccessToken), AuthenticateResult>()
              .Map(dest => dest, src => src.Item1)
              .Map(dest => dest.AccessToken, src => src.Item2);
    }
}
