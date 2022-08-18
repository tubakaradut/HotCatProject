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
    public class CategoryController : BaseApiController
    {
        CategoryRepository categoryRepository = new CategoryRepository();


        /// <summary>
        /// Tüm Kategorileri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpGet]
        public IHttpActionResult CategoryList()
        {
            var categories = categoryRepository.GetAll();
            List<CategoryDTO> categoryDTOs = Mapper.Map<List<CategoryDTO>>(categories);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(categoryDTOs, settings);
        }


        /// <summary>
        /// Aktif Kategorileri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
        [HttpGet]
        public IHttpActionResult ActivesCategoryList()
        {
            var categories = categoryRepository.GetActives(new string[] { "Products", "Products.ProductMaterials", "Products.ProductMaterials.Material" });

            List<CategoryDTO> categoryDTOs = Mapper.Map<List<CategoryDTO>>(categories);

            foreach (var categoryDTOItem in categoryDTOs)
            {
                categoryDTOItem.Description = categoryDTOItem.Description !=null ? categoryDTOItem.Description : "Açıklama Yok";

                CategoryDTO categoryDTO = categoryDTOs.FirstOrDefault(x => x.Id == categoryDTOItem.ParentId);

                if (categoryDTO != null)
                {
                    categoryDTOItem.ParentCategoryName = categoryDTO.CategoryName;
                }
                else
                {
                    categoryDTOItem.ParentCategoryName = "-";
                }

                categoryDTOItem.Products = categoryDTOItem.Products.Where(x => x.IsActive).ToList();

                foreach (var productDTO in categoryDTOItem.Products)
                {
                    productDTO.Description = productDTO.Description != null ? productDTO.Description : "Açıklama Yok";

                    ProductMaterialDTO productMaterialDTO = productDTO.ProductMaterials?.OrderBy(x => x.Material.CurrentStock)?.FirstOrDefault(x => x.IsActive);

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
            }

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(categoryDTOs, settings);
        }



        /// <summary>
        /// Pasif Kategorileri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpGet]
        public IHttpActionResult PasivesCategoryList()
        {
            var categories = categoryRepository.GetPassives();
            List<CategoryDTO> categoryDTOs = Mapper.Map<List<CategoryDTO>>(categories);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(categoryDTOs, settings);
        }



        /// <summary>
        /// Güncellenen Kategorileri Listeleme
        /// </summary>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman))]
        [HttpGet]
        public IHttpActionResult ModifiedsCategoryList()
        {
            var categories = categoryRepository.GetModifieds();
            List<CategoryDTO> categoryDTOs = Mapper.Map<List<CategoryDTO>>(categories);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(categoryDTOs, settings);
        }



        /// <summary>
        /// Kategori Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Salesman), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
        [HttpGet]
        public IHttpActionResult BringCategory(int id)
        {
            var category = categoryRepository.FirstOrDefault(X => X.Id == id);
            CategoryDTO categoryDTO = Mapper.Map<CategoryDTO>(category);
            return Json(categoryDTO);
        }



        /// <summary>
        /// Kategori Ekleme
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpPost]
        public IHttpActionResult AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO != null)
                {
                    Category category = Mapper.Map<Category>(categoryDTO);
                    category.CreatedById = GetUserId();
                    Category savedCategory = categoryRepository.Add(category);
                    return Json(savedCategory);
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
        /// Kategori Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpDelete]
        public IHttpActionResult DeleteCategory(int id)
        {
            var category = categoryRepository.Find(id);
            if (category != null)
            {
                category.DeletedById = GetUserId();
                categoryRepository.Delete(category);

                return Json("Silme İşlemi Başarılı :)");
            }
            else
            {
                return BadRequest();
            }
        }



        /// <summary>
        /// Kategori Güncelleme
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
        [HttpPut]
        public IHttpActionResult UpdateCategory(CategoryDTO categoryDTO)
        {
            int isSuccess = 0;
            if (categoryDTO != null)
            {
                Category category = Mapper.Map<Category>(categoryDTO);
                category.ModifiedById = GetUserId();
                isSuccess = categoryRepository.Update(category);
            }
            return Json(isSuccess);
        }

    }
}
