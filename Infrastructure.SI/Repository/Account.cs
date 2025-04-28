using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.SI.DTO.Request.Identity;
using Application.SI.DTO.Response;
using Application.SI.Extension.Identity;
using Application.SI.Interface.Identity;
using Application.SI.Reponse;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.SI.Repository
{
    public class Account(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager) : IAccount
    {
        public async Task<ServiceReponse> CreateUserAsync(CreateUserRequestDTO model)
        {
            var user = await FindUserByEmail(model.Email);
            if (user != null)
                return new ServiceReponse(false, "el usuario ya existe");


            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = model.Email,
                PasswordHash = model.Password,
                Email = model.Email,
                Name = model.Name,
            };
            var result = CheckResult(await userManager.CreateAsync(newUser, model.Password));
            if (!result.Flag) return result;
            else return await CreateUserClaims(model);
        }

        private async Task<ServiceReponse> CreateUserClaims(CreateUserRequestDTO model)
        {
            if (string.IsNullOrEmpty(model.Policy)) return new ServiceReponse(false, "Poliza vacia");
            Claim[] userClaims = [];

            if (model.Policy.Equals(Policy.AdminPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Name",model.Name),
                    new Claim("Create", "true"),
                    new Claim("Update", "true"),
                    new Claim("Read", "true"),
                    new Claim("ManagerUser", "true"),
                    new Claim("Delete", "true")
                    ];
            }

            if (model.Policy.Equals(Policy.ManagerPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim("Name",model.Name),
                    new Claim("Create", "true"),
                    new Claim("Update", "true"),
                    new Claim("Read", "true"),
                    new Claim("ManagerUser", "false"),
                    new Claim("Delete", "false")
                    ];
            }
            if (model.Policy.Equals(Policy.UserPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("Name",model.Name),
                    new Claim("Create", "false"),
                    new Claim("Update", "false"),
                    new Claim("Read", "false"),
                    new Claim("ManagerUser", "false"),
                    new Claim("Delete", "false")
                    ];
            }

            var result = CheckResult(await userManager.AddClaimsAsync(await FindUserByEmail(model.Email), userClaims));
            return result.Flag ? new ServiceReponse(true, "Usuario Creado") : result;
            
        }

        public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
        {
            var userList = new List<GetUserWithClaimResponseDTO>();
            var allUsers = await userManager.Users.ToListAsync();
            if (allUsers.Count == 0) return userList;

            foreach (var user in allUsers)
            {
                var currentUser = await userManager.FindByIdAsync(user.Id);
                var getCurrentUserClaims = await userManager.GetClaimsAsync(currentUser);
                if (getCurrentUserClaims.Any())
                    userList.Add(new GetUserWithClaimResponseDTO()
                    {
                        UserId = user.Id,
                        Email = getCurrentUserClaims.FirstOrDefault(_ => _.Type ==ClaimTypes.Email).Value,
                        RoleName = getCurrentUserClaims.FirstOrDefault(_ => _.Type ==ClaimTypes.Role).Value,
                        Name = getCurrentUserClaims.FirstOrDefault(_=> _.Type == "Name").Value,
                        ManagerUser = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type == "ManagerUser").Value),
                        Create = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type == "Create").Value),
                        Read = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type == "Read").Value),
                        Update = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type =="Update").Value),
                        Delete = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(_ => _.Type =="Delete").Value)
                    });
            }

            return userList;
        }

        

        public async Task<ServiceReponse> loginAsync(LoginUserRequestDTO model)
        {
            var user = await FindUserByEmail(model.Email);
            if (user == null) return new ServiceReponse(false, "Usuario no encontrado");
            
            var verifyPassword = await signInManager.CheckPasswordSignInAsync(user, model.Password,false);
            if (!verifyPassword.Succeeded) return new ServiceReponse(false, "Credenciales Incorrectas");

            var result = await signInManager.PasswordSignInAsync(user,model.Password,false,false);
            if (!result.Succeeded) return new ServiceReponse(false, "Error Desconocido Al Ingresar");
            else return new ServiceReponse(true, null);
        }

        public async Task SetUpASync() => await CreateUserAsync(new CreateUserRequestDTO()
        {
            Name = "Admin",
            Email = "admin@admin.com",
            Password = "Admin@123",
            Policy = Policy.AdminPolicy
        });
       

        public async Task<ServiceReponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);   
            if (user == null) return new ServiceReponse(false,"Usuario no encontrado");

            var oldUserClaims = await userManager.GetClaimsAsync(user);
            Claim[] newUserClaims =
                [
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("Name",model.Name),
                    new Claim("Create",model.Create.ToString()),
                    new Claim("Update",model.Update.ToString()),
                    new Claim("Read",model.Read.ToString() ),
                    new Claim("ManagerUser",model.ManagerUser.ToString()),
                    new Claim("Delete",model.Delete.ToString() )
                ];

            var result = await userManager.RemoveClaimsAsync(user, oldUserClaims);
            var response = CheckResult(result);

            if (!response.Flag) return new ServiceReponse(false, response.Message);

            var addNewClaims = await userManager.AddClaimsAsync(user, newUserClaims);
            var outcome = CheckResult(addNewClaims);
            if (outcome.Flag)
                return new ServiceReponse(true, "usuario Actualizado");
            else return outcome;
        }




        private async Task<ApplicationUser> FindUserByEmail(string email)
        => await userManager.FindByEmailAsync(email);

        private ServiceReponse CheckResult(IdentityResult result)
        {
            if (result.Succeeded) return new ServiceReponse(true, null);

            var errors = result.Errors.Select(_ => _.Description);
            return new ServiceReponse( false, string.Join(Environment.NewLine, errors));
        }
    }
}
