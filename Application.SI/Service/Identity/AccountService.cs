using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.SI.DTO.Request.Identity;
using Application.SI.DTO.Response;
using Application.SI.Interface.Identity;
using Application.SI.Reponse;

namespace Application.SI.Service.Identity
{
    public class AccountService(IAccount account) : IAccountService
    {
        public async Task<ServiceReponse> CreateUserAsync(CreateUserRequestDTO model)
            =>  await account.CreateUserAsync(model);
        

        public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
            => await account.GetUsersWithClaimsAsync();

        public async Task<ServiceReponse> LoginAsync(LoginUserRequestDTO model)
        => await account.loginAsync(model);

        public async Task SetUpAsync()
        => await account.SetUpASync();

        public async Task<ServiceReponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
        => await account.UpdateUserAsync(model);
    }
}
