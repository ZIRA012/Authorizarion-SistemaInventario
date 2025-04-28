using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.SI.DTO.Request.Identity;
using Application.SI.DTO.Response;
using Application.SI.Reponse;

namespace Application.SI.Interface.Identity
{
    public interface IAccount
    {
        Task<ServiceReponse> loginAsync(LoginUserRequestDTO model);
        Task<ServiceReponse> CreateUserAsync(CreateUserRequestDTO model);
        Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync();
        Task SetUpASync();
        Task<ServiceReponse> UpdateUserAsync(ChangeUserClaimRequestDTO model);
        //Task SaveActivity(ActivityTrackerRequestDTO model);

        //Task<IEnumerable<ActivityTrackerResponseDTO>> GetActivityAsync();
    }
}
