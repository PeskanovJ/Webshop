using Microsoft.Extensions.Configuration;
using Projekat.BLL.Services.Interfaces;
using Projekat.DAL;
using Projekat.DAL.Model;
using Projekat.DAL.Repository;
using Projekat.DAL.Repository.IRepository;
using Projekat.Shared.Common;
using Projekat.Shared.Constants;
using Projekat.Shared.DTOs;
using Projk.BLL.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


namespace Projekat.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        
        public UserService(IUnitOfWork uow, IEmailService emailService,IConfiguration configuration)
        {
            _uow = uow;
            _emailService = emailService;
            _configuration= configuration;
        }

        public ResponsePackage<ProfileDTO> ActivateUser(Guid guid)
        {
            User u = _uow.User.GetFirstOrDefault(u => u.ActivationGuid == Guid.Parse(guid.ToString().ToUpper()));
            u.ActivationGuid = Guid.Empty;
            _uow.User.Update(u);
            _uow.Save();

            return new ResponsePackage<ProfileDTO>(new ProfileDTO()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,

            },ResponseStatus.OK,"Account activated");


        }

        public ResponsePackage<ProfileDTO> LoginUser(LoginDTO loginDTO)
        {
            User u = _uow.User.GetFirstOrDefault(u => u.Email == loginDTO.Email);

            if (u.Password.SequenceEqual(PasswordHasher.GenerateSaltedHash(Encoding.ASCII.GetBytes(loginDTO.Password), u.Salt)))
            {
                if (u.ActivationGuid == Guid.Empty)
                {
                    return new ResponsePackage<ProfileDTO>(new ProfileDTO
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        ProfileUrl= u.ProfileUrl,

                    }, ResponseStatus.OK, "Login successful");
                }
                else
                {
                    return new ResponsePackage<ProfileDTO>(null, ResponseStatus.AccountNotActivated, "Account is not activated");
                }
            }
            else
                return new ResponsePackage<ProfileDTO>(null, ResponseStatus.NotFound, "There was an error with login");
        }

        private bool MailExists(string email)
        {
           User u= _uow.User.GetFirstOrDefault(u => u.Email == email);
            if (u != null)
                return true;
            else
                return false; 
        }

        private bool PhoneExists(string phone)
        {
            User u = _uow.User.GetFirstOrDefault(u => u.PhoneNumber == phone);
            if (u != null)
                return true;
            else
                return false;
        }

        private bool AccountActivated(string email)
        {
            User u = _uow.User.GetFirstOrDefault(u => u.Email == email);
            if (u.ActivationGuid == Guid.Empty)
                return true;
            else
                return false;
        }

        public async Task<ResponsePackage<bool>> RegisterUser(UserDTO userDTO)
        {
            if (MailExists(userDTO.Email))
            {
                return new ResponsePackage<bool>(false, ResponseStatus.InvalidEmail, "Email already exists");
            }
            if (PhoneExists(userDTO.PhoneNumber))
            {
                return new ResponsePackage<bool>(false, ResponseStatus.InvalidPhoneNo, "Phone number already exists");
            }

            User newUser = new User();
            newUser.FirstName = userDTO.FirstName;
            newUser.LastName = userDTO.LastName;
            newUser.Email = userDTO.Email;
            newUser.PhoneNumber = userDTO.PhoneNumber;
            byte[] salt = PasswordHasher.GenerateSalt();
            newUser.Salt = salt;
            newUser.Password = PasswordHasher.GenerateSaltedHash(Encoding.ASCII.GetBytes(userDTO.Password), salt);
            newUser.Role = SD.Roles.User;
            newUser.Created = DateTime.Now;
            newUser.ActivationGuid = Guid.NewGuid();
            newUser.ProfileUrl = "";
            try
            {
                _uow.User.Add(newUser);
                _uow.Save();

                var emailContent = $"<p>Zdravo {newUser.FirstName} {newUser.LastName},</p>";
                emailContent += $"<p>Vas nalog je uspešno napravljen. Kliknite na link ispod da biste ga aktivirali.</p>";
                emailContent += $"<a href='{_configuration["ActivateAccountUrl"]}{newUser.ActivationGuid}'>Aktiviraj nalog</a>";



                var success = await _emailService.SendMailAsync(new Shared.Common.EmailData()
                {
                    To = newUser.Email,
                    Content = emailContent,
                    IsContentHtml = true,
                    Subject = "Aktivacija naloga"
                });


                if(success)
                    return new ResponsePackage<bool>(success,ResponseStatus.OK,"User registered succesfully");
                else
                    return new ResponsePackage<bool>(success, ResponseStatus.InternalServerError, "There was an error while registering new user");
            }
            catch (Exception ex) {
                return new ResponsePackage<bool>(false, ResponseStatus.InternalServerError, ex.Message);
            }
            
        }

        public async Task<ResponsePackage<bool>> ForgotPassword(string email)
        {
            if (MailExists(email))
            {
                if (AccountActivated(email))
                {
                    User u = _uow.User.GetFirstOrDefault(u => u.Email == email);
                    u.PasswordGuid = Guid.NewGuid();
                    _uow.User.Update(u);
                    _uow.Save();

                    var emailContent = $"<p>Zdravo {u.FirstName} {u.LastName},</p>";
                    emailContent += $"<p>Vas nalog je uspešno napravljen. Kliknite na link ispod da biste ga aktivirali.</p>";
                    emailContent += $"<a href='{_configuration["ResetPasswordUrl"]}{u.PasswordGuid}'>Resetuj lozinku</a>";

                    var success = await _emailService.SendMailAsync(new Shared.Common.EmailData()
                    {
                        To = u.Email,
                        Content = emailContent,
                        IsContentHtml = true,
                        Subject = "Reset lozinke"
                    });

                    if (success)
                        return new ResponsePackage<bool>(success, ResponseStatus.OK, "Reset mail sent");
                    else
                        return new ResponsePackage<bool>(success, ResponseStatus.InternalServerError, "There was an error");
                }
                else return new ResponsePackage<bool>(false, ResponseStatus.AccountNotActivated, "Account is not activated");

            }
            else
                return new ResponsePackage<bool>(false, ResponseStatus.InvalidEmail, "Mail does not exist");
        }

        public ResponsePackage<bool> ResetPassword(PasswordResetDTO passwordResetDTO)
        {
            User u = _uow.User.GetFirstOrDefault(u => u.PasswordGuid == passwordResetDTO.PasswordGuid);
            if (u != null) { 
                u.PasswordGuid = Guid.Empty;
                byte[] salt = PasswordHasher.GenerateSalt();
                u.Salt = salt;
                u.Password = PasswordHasher.GenerateSaltedHash(Encoding.ASCII.GetBytes(passwordResetDTO.Password), salt);

                _uow.User.Update(u);
                _uow.Save();
                return new ResponsePackage<bool>(true, ResponseStatus.OK, "Password reseted succesfully");
            }
            else
            {
                return new ResponsePackage<bool>(false, ResponseStatus.InvalidPasswordGuid, "Password reset link is not active anymore");
            }
        }

        public ResponsePackage<ProfileDTO> GetProfile(string email)
        {
            User u = _uow.User.GetFirstOrDefault(u=>u.Email== email);
            return new ResponsePackage<ProfileDTO>(new ProfileDTO()
            {
                Email = u.Email,
                FirstName=u.FirstName, LastName=u.LastName,
                PhoneNumber=u.PhoneNumber,
                ProfileUrl=u.ProfileUrl,
                Created=u.Created,
            },ResponseStatus.OK,"Profile");
            
            
        }

        public ResponsePackage<bool> UpdateProfile(ProfileDTO profileDTO)
        {
            User u = _uow.User.GetFirstOrDefault(u => u.Email == profileDTO.Email);
            if (profileDTO.ProfileUrl != null) 
                u.ProfileUrl = profileDTO.ProfileUrl;
            u.FirstName=profileDTO.FirstName;
            u.LastName= profileDTO.LastName;
            u.PhoneNumber=profileDTO.PhoneNumber;
            
            _uow.User.Update(u);
            _uow.Save();

            return new ResponsePackage<bool>(true, ResponseStatus.OK, "Profile changed");
        }
    }
}
