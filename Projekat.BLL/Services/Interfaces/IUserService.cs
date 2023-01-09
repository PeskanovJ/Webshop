using Projekat.Shared.Common;
using Projekat.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponsePackage<bool>> RegisterAdmin(AdminDTO userDTO);
        Task<ResponsePackage<bool>> RegisterUser(UserDTO userDTO);
        ResponsePackage<ProfileDTO> LoginUser(LoginDTO loginDTO);
        ResponsePackage<ProfileDTO> ActivateUser(Guid guid);
        Task<ResponsePackage<bool>> ForgotPassword(string email);
        ResponsePackage<bool> ResetPassword(PasswordResetDTO passwordResetDTO);
        ResponsePackage<ProfileDTO> GetProfile(string email);
        ResponsePackage<bool> UpdateProfile(ProfileDTO profileDTO);
        ResponsePackage<bool> FollowItem(int userId,int itemId);
        ResponsePackage<bool> UnFollowItem(int userId, int itemId);
    }
}
