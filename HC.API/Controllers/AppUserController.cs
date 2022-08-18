using AutoMapper;
using HC.API.Filters;
using HC.API.Models.DTOClasses;
using HC.BLL.GenericRepository.ConcRep;
using HC.Entity.Enums;
using HC.Entity.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace HC.API.Controllers
{
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
    public class AppUserController : BaseApiController
    {
        AppUserRepository appUserRepository = new AppUserRepository();

        /// <summary>
        /// Tüm Kullanıcıları Listeleme
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult AppUserList()
        {
            var users = appUserRepository.GetAll();
            List<AppUserDTO> appUserDTOs = Mapper.Map<List<AppUserDTO>>(users);

           
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore //Serialize işlemi sırasında birbirlerini döngüsel olarak referans gösteren iki navigation property’den dolayı serialize işlemi bir loop’a girmekte ve bu sebeple ilgili serializer bu döngüyü nasıl handle edebileceğini bilmediği için “Self referencing loop detected for property” hatasını vermektedir.Sonsuz döngüye girmesini engellemek için ve  “Self referencing loop detected for property” hatasını almamak için ıgnore yapılır.
            };
            return Json(appUserDTOs, settings);
        }
        /// <summary>
        /// Aktif Kullanıcıları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ActivesAppUserList()
        {
            var users = appUserRepository.GetActives();
            List<AppUserDTO> appUserDTOs = Mapper.Map<List<AppUserDTO>>(users);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//sonsuz döngüye girmesini engellemek için
            };
            return Json(appUserDTOs, settings);
        }

        /// <summary>
        /// Pasif Kullanıcıları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult PasivesAppUserList()
        {
            var users = appUserRepository.GetPassives();
            List<AppUserDTO> appUserDTOs = Mapper.Map<List<AppUserDTO>>(users);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//sonsuz döngüye girmesini engellemek için
            };
            return Json(appUserDTOs, settings);
        }
        /// <summary>
        /// Güncellenen Kullanıcıları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ModifiedsAppUserList()
        {
            var users = appUserRepository.GetModifieds();
            List<AppUserDTO> appUserDTOs = Mapper.Map<List<AppUserDTO>>(users);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//sonsuz döngüye girmesini engellemek için
            };
            return Json(appUserDTOs, settings);
        }

        /// <summary>
        /// Kullanıcı Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult BringAppUser(int id)
        {
            var appuser = appUserRepository.FirstOrDefault(x => x.Id == id);
            AppUserDTO appUserDTO = Mapper.Map<AppUserDTO>(appuser);
            return Json(appUserDTO);
        }

        /// <summary>
        /// Kullanıcı Ekleme
        /// </summary>
        /// <param name="appUserDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAppUser(AppUserDTO appUserDTO)
        {
            try
            {
                if (appUserDTO != null)
                {
                    AppUser appUser = Mapper.Map<AppUser>(appUserDTO);
                    appUser.CreatedById = GetUserId();
                    appUserRepository.Add(appUser);
                    return Json(AppUserList());
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Kullanıcı Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAppUser(int id)
        {
            var user = appUserRepository.Find(id);
            if (user != null)
            {
                user.DeletedById = GetUserId();
                appUserRepository.Delete(user);

                return Json("Silme İşlemi Başarılı");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Kullanıcı Güncelleme
        /// </summary>
        /// <param name="appUserDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAppUser(AppUserDTO appUserDTO)
        {
            int isSuccess = 0;
            if (appUserDTO != null)
            {
                AppUser appUser = Mapper.Map<AppUser>(appUserDTO);
                appUser.ModifiedById = GetUserId();

                isSuccess = appUserRepository.Update(appUser);
            }

            return Json(isSuccess);
        }

    }
}
