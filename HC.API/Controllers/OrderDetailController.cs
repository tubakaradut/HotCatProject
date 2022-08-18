using AutoMapper;
using HC.API.Filters;
using HC.API.Models.DTOClasses;
using HC.BLL.GenericRepository.ConcRep;
using HC.Entity.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Http;

namespace HC.API.Controllers
{
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
    public class OrderDetailController : BaseApiController
    {
        OrderDetailRepository orderDetailRepository = new OrderDetailRepository();

        /// <summary>
        /// Sipariş Detaylarını Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult OrderDetailList()
        {
            var orders = orderDetailRepository.GetAll();
            List<OrderDetailDTO> orderDetailDTOs = Mapper.Map<List<OrderDetailDTO>>(orders);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(orderDetailDTOs, settings);
        }

        /// <summary>
        /// Sipariş Detayını Getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult BringOrderDetail(int id)
        {
            var orderDetail = orderDetailRepository.FirstOrDefault(x => x.Id == id);
            OrderDetailDTO orderDetailDTO = Mapper.Map<OrderDetailDTO>(orderDetail);
            return Json(orderDetailDTO);
        }

        /// <summary>
        /// Sipariş Detay Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteOrderDetail(int id)
        {
            var orderDetail = orderDetailRepository.Find(id);
            if (orderDetail != null)
            {
                orderDetail.DeletedById = GetUserId();
                orderDetailRepository.Delete(orderDetail);

                return Json("Silme İşlemi Başarılı :)");
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
