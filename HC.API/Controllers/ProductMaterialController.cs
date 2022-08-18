using AutoMapper;
using HC.API.Filters;
using HC.API.Models.DTOClasses;
using HC.BLL.GenericRepository.ConcRep;
using HC.Entity.Enums;
using HC.Entity.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace HC.API.Controllers
{
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
    public class ProductMaterialController : BaseApiController
    {
        ProductMaterialRepository productMaterialRepository = new ProductMaterialRepository();

        /// <summary>
        /// Ürünler ile Malzeme İlişkilendirme
        /// </summary>
        /// <param name="productMaterialDTOs"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddProductMaterial(List<ProductMaterialDTO> productMaterialDTOs)
        {
            if (productMaterialDTOs != null && productMaterialDTOs.Count > 0)
            {
                ProductMaterial savedProductMaterial = new ProductMaterial();

                foreach (var productMaterialDTO in productMaterialDTOs)
                {
                    ProductMaterial productMaterial = Mapper.Map<ProductMaterial>(productMaterialDTO);
                    productMaterial.CreatedById = GetUserId();
                    savedProductMaterial = productMaterialRepository.Add(productMaterial);
                }
                return Json(savedProductMaterial);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
