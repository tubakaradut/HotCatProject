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
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HC.API.Controllers
{
    public class CompanyController : BaseApiController
    {
        CompanyRepository companyRepository = new CompanyRepository();

        /// <summary>
        /// Tüm Şirketleri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpGet]
        public IHttpActionResult CompanyList()
        {
            var companies = companyRepository.GetAll();
            List<CompanyDTO> companyDTOs = Mapper.Map<List<CompanyDTO>>(companies);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(companyDTOs, settings);
        }


        /// <summary>
        /// Aktif Şirketleri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Cashier))]
        [HttpGet]
        public IHttpActionResult ActivesCompanyList()
        {

            var companies = companyRepository.GetActives();
            List<CompanyDTO> CompanyDTOs = Mapper.Map<List<CompanyDTO>>(companies);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(CompanyDTOs, settings);

        }


        /// <summary>
        /// Pasif Şirketleri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpGet]
        public IHttpActionResult PasivesCompanyList()
        {
            var companies = companyRepository.GetPassives();

            List<CompanyDTO> companyDTOs = Mapper.Map<List<CompanyDTO>>(companies);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(companyDTOs, settings);

        }


        /// <summary>
        /// Güncellenen Şirketleri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpGet]
        public IHttpActionResult ModifiedsCompanyList()
        {
            var companies = companyRepository.GetModifieds();
            List<CompanyDTO> companyDTOs = Mapper.Map<List<CompanyDTO>>(companies);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(companyDTOs, settings);

        }


        /// <summary>
        /// Şİrket Ekle
        /// </summary>
        /// <param name="companyDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpPost]
        public IHttpActionResult AddCompany(CompanyDTO companyDTO)
        {
            try
            {
                if (companyDTO != null)
                {
                    Company company = Mapper.Map<Company>(companyDTO);
                    company.CreatedById = GetUserId();
                    Company savedCompany = companyRepository.Add(company);
                    return Json(savedCompany);
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
        /// Şİrket Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpDelete]
        public IHttpActionResult DeleteCompany(int id)
        {
            var company = companyRepository.Find(id);
            if (company != null)
            {
                company.DeletedById = GetUserId();
                companyRepository.Delete(company);
                return Json("Silme İşlemi Başarılı");
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Şirket Güncelleme
        /// </summary>
        /// <param name="companyDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpPut]
        public IHttpActionResult Updatecompany(CompanyDTO companyDTO)
        {
            int isSuccess = 0;
            if (companyDTO != null)
            {
                Company company = Mapper.Map<Company>(companyDTO);
                company.ModifiedById = GetUserId();
                isSuccess = companyRepository.Update(company);
            }

            return Json(isSuccess);
        }


        /// <summary>
        /// Şirket Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin),nameof(AppUserRoleEnum.Cashier))]
        [HttpGet]
        public IHttpActionResult BringCompany(int id)
        {
            var company = companyRepository.FirstOrDefault(X => X.Id == id);
            CompanyDTO companyDTO = Mapper.Map<CompanyDTO>(company);
            return Json(companyDTO);
        }
    }
}
