using AutoMapper;
using HC.API.Filters;
using HC.API.Models.DTOClasses;
using HC.API.Util;
using HC.BLL.GenericRepository.ConcRep;
using HC.Entity.Enums;
using HC.Entity.Model;
using HC.UTILS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HC.API.Controllers
{
   
    public class AuthenticationController : BaseApiController
    {
        AppUserRepository appUserRepository = new AppUserRepository();

        /// <summary>
        /// Swagger da Kullanabilmek için token üretiyor.
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IHttpActionResult CreateToken(LoginDTO loginDTO)
        {
            string passwordEncryption = Cryptography.Encryption(loginDTO.Password);

            AppUser appUser = appUserRepository.FirstOrDefault(x => x.UserName == loginDTO.UserName && x.Password == loginDTO.Password && x.IsActive, new string[] { "AppUserRoles", "AppUserRoles.Role" });

            if (appUser != null)
            {
                List<string> appUserRoles = appUser.AppUserRoles.Select(x => x.Role.Name).ToList();

                AppUserDTO appUserDTO = Mapper.Map<AppUserDTO>(appUser);

                appUserDTO.RoleNames = appUserRoles;

                var accessToken = AuthUtil.Token(appUserDTO);

                return Ok(accessToken);
            }
            else
            {
                return BadRequest();
            }
        }
      
        /// <summary>
        /// Şİfre Oluşturma
        /// </summary>
        /// <param name="activationCode"></param>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult CreatePassword(Guid activationCode)
        {

            if (activationCode != null)
            {
                AppUser appUser = appUserRepository.FirstOrDefault(x => x.ActivationCode == activationCode && x.IsActive == false);

                if (appUser != null)
                {
                    appUser.IsActive = true;
                    appUser.ActivationCode = null;
                    appUser.ModifiedById = appUser.Id;
                    int isSuccess = appUserRepository.Update(appUser);

                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Siatemdeki Kullanıcı Adını Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserNameInSystem()
        {
            string username = GetUserName();
            return Ok(username);
        }


        /// <summary>
        /// Sistemdeki Kullanıcı Rollari Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserRolesInSystem()
        {
            List<string> userRoles = GetUserRoles();
            return Ok(userRoles);
        }
    }
}
