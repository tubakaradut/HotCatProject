using HC.API.Filters;
using HC.API.Models.DTOClasses;
using HC.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace HC.API.Controllers
{
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin),nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
    public class EnumController : ApiController   //Tüm Enumların Display Name'leri getirebilmek için oluşturuldu.
    {
        /// <summary>
        /// Tüm Masa Lokasyonlarını Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult TableLocationList()
        {
            List<TableLocationDTO> tableLocationDTOs = new List<TableLocationDTO>();
            foreach (TableLocation tableLocation in Enum.GetValues(typeof(TableLocation)))
            {
                tableLocationDTOs.Add(new TableLocationDTO()
                {
                    Name = tableLocation.ToString(),
                    DisplayName = EnumHelper.GetDisplayName(tableLocation),
                    Id = (int)tableLocation
                });
            }

            return Json(tableLocationDTOs);
        }


        /// <summary>
        /// Tüm Masa Durumlarını Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult TableStatusList()
        {
            List<TableStatusDTO> tableStatusDTO = new List<TableStatusDTO>();

            foreach (TableStatus tableStatus in Enum.GetValues(typeof(TableStatus)))
            {
                if (tableStatus != TableStatus.Filled)
                {
                    tableStatusDTO.Add(new TableStatusDTO()
                    {
                        Name = tableStatus.ToString(),
                        DisplayName = EnumHelper.GetDisplayName(tableStatus),
                        Id = (int)tableStatus
                    });
                }
            }

            return Json(tableStatusDTO);
        }

        /// <summary>
        /// Tüm Birim Tiplerini Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult UnitTypeList()
        {
            List<UnitTypeDTO> unitTypeDTOs = new List<UnitTypeDTO>();
            foreach (UnitType unitType in Enum.GetValues(typeof(UnitType)))
            {
                unitTypeDTOs.Add(new UnitTypeDTO()
                {
                    Name = unitType.ToString(),
                    DisplayName = EnumHelper.GetDisplayName(unitType),
                    Id = (int)unitType
                });
            }

            return Json(unitTypeDTOs);
        }

        /// <summary>
        /// Tüm Ödeme Tiplerini Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult PaymentTypeList()
        {
            List<PaymentTypeDTO> paymentTypeDTOs = new List<PaymentTypeDTO>();
            foreach (PaymentType paymentType in Enum.GetValues(typeof(UnitType)))
            {
                paymentTypeDTOs.Add(new PaymentTypeDTO()
                {
                    Name = paymentType.ToString(),
                    DisplayName = EnumHelper.GetDisplayName(paymentType),
                    Id = (int)paymentType
                });
            }

            return Json(paymentTypeDTOs);
        }

        /// <summary>
        /// Tüm Rolleri Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult RoleList()
        {
            List<RoleDTO> roleDTOs = new List<RoleDTO>();
            foreach (AppUserRoleEnum role in Enum.GetValues(typeof(AppUserRoleEnum)))
            {
                roleDTOs.Add(new RoleDTO()
                {
                    Name = role.ToString(),
                    DisplayName = EnumHelper.GetDisplayName(role),
                    Id = (int)role
                });
            }

            return Json(roleDTOs);
        }


    }
}
