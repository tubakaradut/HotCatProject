using AutoMapper;
using Common;
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
   
    public class MaterialController : BaseApiController
    {
        MaterialRepository materialRepository = new MaterialRepository();
        ProductMaterialRepository productMaterialRepository = new ProductMaterialRepository();
        AppUserRepository appUserRepository = new AppUserRepository();


        /// <summary>
        /// Aktif Malzemeleri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin),nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier), nameof(AppUserRoleEnum.Salesman))]
        [HttpGet]
        public IHttpActionResult ActivesMaterialList()
        {
            var materials = materialRepository.GetActives(new string[] { "ProductMaterials" });
            List<MaterialDTO> materialDTOs = Mapper.Map<List<MaterialDTO>>(materials);

            foreach (var materialDTOItem in materialDTOs)
            {
                materialDTOItem.UnitTypeName = EnumHelper.GetDisplayName(materialDTOItem.UnitType);

                materialDTOItem.ProductMaterials = materialDTOItem.ProductMaterials.Where(x => x.IsActive).ToList();
            }

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(materialDTOs, settings);
        }


        /// <summary>
        /// Malzeme Ekleme
        /// </summary>
        /// <param name="materialDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpPost]
        public IHttpActionResult AddMaterial(MaterialDTO materialDTO)
        {
            if (materialDTO != null)
            {
                Material material = Mapper.Map<Material>(materialDTO);
                material.CreatedById = GetUserId();
                Material savedMaterial = materialRepository.Add(material);
                return Json(savedMaterial);
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Malzeme Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpDelete]
        public IHttpActionResult DeleteMaterial(int id)
        {
            var material = materialRepository.Find(id);
            if (material != null)
            {
                material.DeletedById = GetUserId();
                materialRepository.Delete(material);

                return Json("Silme İşlemi Başarılı :)");
            }
            else
            {
                return BadRequest();
            }
        }



        /// <summary>
        /// Malzeme Güncelleme
        /// </summary>
        /// <param name="materialDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpPut]
        public IHttpActionResult UpdateMaterial(MaterialDTO materialDTO)
        {
            int isSuccess = 0;
            if (materialDTO != null)
            {
                Material material = Mapper.Map<Material>(materialDTO);
                material.ModifiedById = GetUserId();
                isSuccess = materialRepository.Update(material);
            }
            return Json(isSuccess);
        }


        /// <summary>
        /// Malzeme Talep Etme
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpGet]
        public IHttpActionResult RequestMaterial(int productId)
        {
            List<ProductMaterial> productMaterials = productMaterialRepository.Where(x => x.ProductId == productId && x.IsActive, new string[] { "Material" });

            List<AppUser> appUsers = appUserRepository.Where(x => x.AppUserRoles.Any(y => y.Role.Name == AppUserRoleEnum.Salesman.ToString()) && x.IsActive, new string[] { "Employee" });

            string materialList = string.Empty;

            foreach (var productMaterial in productMaterials)
            {
                if (productMaterial.Material.MinStock >= productMaterial.Material.CurrentStock)
                {
                    materialList += $"Malzeme Adı: {productMaterial.Material.MaterialName}<br/> Minimum Stok Miktarı: {productMaterial.Material.MinStock}<br/> Mevcut Stok Miktarı: {productMaterial.Material.CurrentStock}<br/><br/>";
                }
            }

            foreach (var appUser in appUsers)
            {
                if (appUser.Employee != null)
                {
                    MailSender.SendEmail(appUser.Employee.Email, "Minimum Stok Bilgisi", $"Aşağıdaki malzemelerin stok miktarı minimum stok miktarının altına düşmüştür!<br/> {materialList}");
                }
            }
            return Json("Malzeme Talep İşlemi Tamamlanmıştır.");
        }
    }
}