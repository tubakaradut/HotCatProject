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
using System.Web.UI.WebControls;

namespace HC.API.Controllers
{
    public class CafeTableController : BaseApiController
    {
        CafeTableRepository cafeTableRepository = new CafeTableRepository();


        /// <summary>
        /// Tüm Masaları Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpGet]
        public IHttpActionResult CafeTableList()
        {
            var tables = cafeTableRepository.GetAll();
            List<CafeTableDTO> cafeTableDTOs = Mapper.Map<List<CafeTableDTO>>(tables);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(cafeTableDTOs, settings);
        }


        /// <summary>
        /// Aktif Masaları Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
        [HttpGet]
        public IHttpActionResult ActivesCafeTableList()
        {
            var tables = cafeTableRepository.GetActives().OrderBy(x => x.TableLocation).ToList();

            List<CafeTableDTO> cafeTableDTOs = Mapper.Map<List<CafeTableDTO>>(tables);

            foreach (var cafeTableDTO in cafeTableDTOs)
            {
                cafeTableDTO.TableLocationName = Enum.GetName(typeof(TableLocation), cafeTableDTO.TableLocation);

                cafeTableDTO.TableLocationDisplayName = EnumHelper.GetDisplayName(cafeTableDTO.TableLocation);

                cafeTableDTO.TableStatusDisplayName = EnumHelper.GetDisplayName(cafeTableDTO.TableStatus);
            }

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(cafeTableDTOs, settings);

        }


        /// <summary>
        /// Pasif Masaları Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpGet]
        public IHttpActionResult PasivesCafeTableList()
        {
            var tables = cafeTableRepository.GetPassives();

            List<CafeTableDTO> cafeTableDTOs = Mapper.Map<List<CafeTableDTO>>(tables);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(cafeTableDTOs, settings);

        }


        /// <summary>
        /// Güncellenen Masaları Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpGet]
        public IHttpActionResult ModifiedsCafeTableList()
        {
            var tables = cafeTableRepository.GetModifieds();
            List<CafeTableDTO> cafeTableDTOs = Mapper.Map<List<CafeTableDTO>>(tables);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(cafeTableDTOs, settings);

        }


        /// <summary>
        /// Masa Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
        [HttpGet]
        public IHttpActionResult BringCafeTable(int id)
        {
            var table = cafeTableRepository.FirstOrDefault(x => x.Id == id);
            CafeTableDTO cafeTableDTO = Mapper.Map<CafeTableDTO>(table);
            return Json(cafeTableDTO);

        }


        /// <summary>
        /// Masa Ekleme
        /// </summary>
        /// <param name="cafeTableDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpPost]
        public IHttpActionResult AddCafeTable(CafeTableDTO cafeTableDTO)
        {
            try
            {
                if (cafeTableDTO != null)
                {
                    CafeTable cafeTable = Mapper.Map<CafeTable>(cafeTableDTO);
                    cafeTable.CreatedById = GetUserId();
                    CafeTable savedCafeTable = cafeTableRepository.Add(cafeTable);
                    return Json(savedCafeTable);
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
        /// Masa Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpDelete]
        public IHttpActionResult DeleteCafeTable(int id)
        {
            var table = cafeTableRepository.Find(id);
            if (table != null)
            {
                table.DeletedById = GetUserId();
                cafeTableRepository.Delete(table);
                return Json("Silme İşlemi Başarılı :)");
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Masa Güncelleme
        /// </summary>
        /// <param name="cafeTableDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpPut]
        public IHttpActionResult UpdateCafeTable(CafeTableDTO cafeTableDTO)
        {
            int isSuccess = 0;
            if (cafeTableDTO != null)
            {
                CafeTable cafeTable = Mapper.Map<CafeTable>(cafeTableDTO);
                cafeTable.ModifiedById = GetUserId();
                isSuccess = cafeTableRepository.Update(cafeTable);
            }
            return Json(isSuccess);
        }

    }
}
