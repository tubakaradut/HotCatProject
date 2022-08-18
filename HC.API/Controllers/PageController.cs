using AutoMapper;
using HC.API.Filters;
using HC.API.Models.DTOClasses;
using HC.BLL.GenericRepository.ConcRep;
using HC.Entity.Enums;
using HC.Entity.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HC.API.Controllers
{
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
    public class PageController : BaseApiController
    {
        PageRepository pageRepository = new PageRepository();
        PageRoleRepository pageRoleRepository = new PageRoleRepository();

        /// <summary>
        /// Sistemdeki Kullanıcının Yetkisine Bağlı Görebileceği Sayfaları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<PageDTO> PageList()
        {
            List<string> userRoles = GetUserRoles();

            List<Page> pages = pageRepository.Where(x => x.PageRoles.Any(y => userRoles.Contains(y.Role.Name) && y.IsActive) && x.IsActive);

            List<PageDTO> pageDTOs = Mapper.Map<List<PageDTO>>(pages);

            return pageDTOs;
        }

        /// <summary>
        /// Sistemdeki Kullanıcının Yetkisine Bağlı Görebileceği Anasayfayı Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PageDTO GetMainPage()
        {
            List<string> userRoles = GetUserRoles();

            Page pages = pageRepository.FirstOrDefault(x => x.PageRoles.Any(y => userRoles.Contains(y.Role.Name) && y.IsActive) && x.IsActive && x.IsMainPage);

            PageDTO pageDTOs = Mapper.Map<PageDTO>(pages);

            return pageDTOs;
        }

        /// <summary>
        /// Aktif Sayfaları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ActivesPageList()
        {
            var pages = pageRepository.GetActives(new string[] { "PageRoles" });

            List<PageDTO> pagesDTOs = Mapper.Map<List<PageDTO>>(pages);


            foreach (var page in pages)
            {
                string roleNames = string.Empty;

                foreach (AppUserRoleEnum role in Enum.GetValues(typeof(AppUserRoleEnum)))
                {
                    bool isUsedRole = page.PageRoles.Any(x => x.RoleId == (int)role);
                    if (isUsedRole)
                    {
                        roleNames += roleNames == string.Empty ? EnumHelper.GetDisplayName(role) : ", " + EnumHelper.GetDisplayName(role);
                    }
                }

                PageDTO pageDTO = pagesDTOs.FirstOrDefault(x => x.Id == page.Id);
                pageDTO.Roles = roleNames;
            }

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(pagesDTOs, settings);
        }

        /// <summary>
        /// Sayfa ile Rolleri ilişkilendirme
        /// </summary>
        /// <param name="pageRoleDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPageRole(PageRoleDTO pageRoleDTO)
        {
            if (pageRoleDTO != null && pageRoleDTO.RoleList.Count > 0)
            {
                PageRole savedPageRole = new PageRole();
                foreach (var role in pageRoleDTO.RoleList)
                {
                    PageRole pageRole = new PageRole();
                    pageRole.PageId = pageRoleDTO.PageId;
                    pageRole.RoleId = role;
                    pageRole.CreatedById = GetUserId();

                    savedPageRole = pageRoleRepository.Add(pageRole);
                }
                return Json(savedPageRole);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
