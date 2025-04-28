using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.SI.DTO.Request.Identity;
using Application.SI.DTO.Response;
using Application.SI.Reponse;

namespace Application.SI.Service.Identity
{
    public interface IAccountService
    {
        Task<ServiceReponse> LoginAsync(LoginUserRequestDTO model);
        Task<ServiceReponse> CreateUserAsync(CreateUserRequestDTO model);

        Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync();
        Task SetUpAsync();
        Task<ServiceReponse> UpdateUserAsync(ChangeUserClaimRequestDTO model);
    }
}
