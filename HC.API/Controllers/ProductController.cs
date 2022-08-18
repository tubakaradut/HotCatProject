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
    public class ProductController : BaseApiController
    {
        ProductRepository productRepository = new ProductRepository();
        ProductMaterialRepository productMaterialRepository = new ProductMaterialRepository();
        AppUserRepository appUserRepository = new AppUserRepository();


        /// <summary>
        /// Tüm Ürünleri Listeleme
        /// </summary>
        /// <returns></returns>

        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpGet]
        public IHttpActionResult ProductList()
        {
            var products = productRepository.GetAll();
            List<ProductDTO> productDTOs = Mapper.Map<List<ProductDTO>>(products);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(productDTOs, settings);

        }



        /// <summary>
        /// Aktif Ürünleri Listeleme
        /// </summary>
        /// <returns></returns>

        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman), nameof(AppUserRoleEnum.Cashier), nameof(AppUserRoleEnum.Waiter))]
        [HttpGet]
        public IHttpActionResult ActivesProductList()
        {
            var products = productRepository.GetActives(new string[] { "Category" });
            List<ProductDTO> productDTOs = Mapper.Map<List<ProductDTO>>(products);


            foreach (var productDTO in productDTOs)
            {
                productDTO.Description = productDTO.Description != null ? productDTO.Description : "Açıklama Yok";

                List<ProductMaterial> productMaterials = productMaterialRepository.Where(x => x.ProductId == productDTO.Id && x.IsActive, new string[] { "Material" });

                List<ProductMaterialDTO> productMaterialDTOs = Mapper.Map<List<ProductMaterialDTO>>(productMaterials);


                ProductMaterialDTO productMaterialDTO = productMaterialDTOs?.OrderBy(x => x.Material.CurrentStock)?.FirstOrDefault(x => x.IsActive);

                if (productMaterialDTO != null && productMaterialDTO.Material != null)
                {
                    decimal currentProductQuantity = Math.Floor((productMaterialDTO.Material.CurrentStock / productMaterialDTO.MaterialQuantity));

                    productDTO.ProductQuantity = currentProductQuantity;
                }
                else
                {
                    productDTO.ProductQuantity = default(decimal);
                }
            }


            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(productDTOs, settings);

        }



        /// <summary>
        /// Pasif Ürünleri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpGet]
        public IHttpActionResult PasivesProductList()
        {
            var products = productRepository.GetPassives();
            List<ProductDTO> productDTOs = Mapper.Map<List<ProductDTO>>(products);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(productDTOs, settings);
        }



        /// <summary>
        /// Güncellenen Ürünleri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpGet]
        public IHttpActionResult ModifiedsProductList()
        {
            var products = productRepository.GetModifieds();
            List<ProductDTO> productDTOs = Mapper.Map<List<ProductDTO>>(products);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(productDTOs, settings);
        }



        /// <summary>
        /// Ürün Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
        [HttpGet]
        public IHttpActionResult BringProduct(int id)
        {
            var product = productRepository.FirstOrDefault(x => x.Id == id);
            ProductDTO productDTO = Mapper.Map<ProductDTO>(product);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(productDTO, settings);
        }



        /// <summary>
        /// Ürün Ekleme
        /// </summary>
        /// <param name="productDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpPost]
        public IHttpActionResult AddProduct(ProductDTO productDTO)
        {
            try
            {
                if (productDTO != null)
                {
                    Product product = Mapper.Map<Product>(productDTO);
                    product.CreatedById = GetUserId();
                    Product savedData = productRepository.Add(product);
                    return Json(savedData);
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
        /// Ürün Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpDelete]
        public IHttpActionResult DeleteProduct(int id)
        {
            var product = productRepository.Find(id);
            if (product != null)
            {
                product.DeletedById = GetUserId();
                productRepository.Delete(product);
                return Json("Silme İşlemi Başarılı :)");
            }
            else
            {
                return BadRequest();
            }
        }



        /// <summary>
        /// Ürün Güncelleme
        /// </summary>
        /// <param name="productDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpPut]
        public IHttpActionResult UpdateProduct(ProductDTO productDTO)
        {
            int isSuccess = 0;
            if (productDTO != null)
            {
                Product product = Mapper.Map<Product>(productDTO);
                product.ModifiedById = GetUserId();
                isSuccess = productRepository.Update(product);
            }
            return Json(isSuccess);
        }



        /// <summary>
        /// Ürüne Göre Malzeme Talep Etme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman), nameof(AppUserRoleEnum.Cashier), nameof(AppUserRoleEnum.Waiter))]
        [HttpGet]
        public IHttpActionResult RequestProduct(int id)
        {
            List<ProductMaterial> productMaterials = productMaterialRepository.Where(x => x.ProductId == id && x.IsActive, new string[] {"Material" });

            List<AppUser> appUsers = appUserRepository.Where(x => x.AppUserRoles.Any(y => y.Role.Name == AppUserRoleEnum.Manager.ToString()) && x.IsActive, new string[] { "Employee" });

            Product product = productRepository.FirstOrDefault(x => x.Id == id && x.IsActive);

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
                    string productName = string.Empty;

                    if (product != null)
                    {
                        productName = product.ProductName;
                    }

                    MailSender.SendEmail(appUser.Employee.Email, "Minimum Stok Bilgisi", $"{productName} ürününe ait malzemelerin stok miktarı minimum stok miktarının altına düşmüştür!<br/> {materialList}");
                }
            }
            return Json("Ürüne Göre Malzeme Talep İşlemi Tamamlanmıştır.");
        }

    }
}
