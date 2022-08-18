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
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin), nameof(AppUserRoleEnum.Waiter), nameof(AppUserRoleEnum.Cashier))]
    public class OrderController : BaseApiController
    {
        OrderRepository orderRepository = new OrderRepository();
        CafeTableRepository cafeTableRepository = new CafeTableRepository();
        ProductRepository productRepository = new ProductRepository();
        OrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        MaterialRepository materialRepository = new MaterialRepository();
        AppUserRepository appUserRepository = new AppUserRepository();
        ReceiptRepository receiptRepository = new ReceiptRepository();

        /// <summary>
        ///Tüm Siparişleri Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult OrderList()
        {
            var orders = orderRepository.GetAll();
            List<OrderDTO> orderDTOs = Mapper.Map<List<OrderDTO>>(orders);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(orderDTOs, settings);
        }

        /// <summary>
        ///Aktif Siparişleri Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ActivesOrderList()
        {
            var orders = orderRepository.GetActives();
            List<OrderDTO> orderDTOs = Mapper.Map<List<OrderDTO>>(orders);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(orderDTOs, settings);
        }

        /// <summary>
        ///Pasif Siparişleri Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult PasivesOrderList()
        {
            var orders = orderRepository.GetPassives();
            List<OrderDTO> orderDTOs = Mapper.Map<List<OrderDTO>>(orders);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(orderDTOs, settings);
        }

        /// <summary>
        ///Güncellenen Siparişleri Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ModifiedsOrderList()
        {
            var orders = orderRepository.GetModifieds();
            List<OrderDTO> orderDTOs = Mapper.Map<List<OrderDTO>>(orders);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(orderDTOs, settings);
        }

        /// <summary>
        /// Sipariş Getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult BringOrder(int id)
        {
            var order = orderRepository.FirstOrDefault(x => x.Id == id);
            OrderDTO orderDTO = Mapper.Map<OrderDTO>(order);
            return Json(orderDTO);
        }

        /// <summary>
        /// Sipariş Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteOrder(int id)
        {
            var order = orderRepository.Find(id);
            if (order != null)
            {
                order.DeletedById = GetUserId();
                orderRepository.Delete(order);
                return Json("Silme İşlemi Başarılı");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Sipariş Güncelleme
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateOrder(OrderDTO orderDTO)
        {
            int isSuccess = 0;
            if (orderDTO != null)
            {
                Order order = Mapper.Map<Order>(orderDTO);
                order.ModifiedById = GetUserId();
                isSuccess = orderRepository.Update(order);
            }

            return Json(isSuccess);
        }



        /// <summary>
        /// Sipariş Ekle
        /// </summary>
        /// <param name="addOrderDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrder(AddOrderDTO addOrderDTO)
        {
            try
            {
                if (addOrderDTO != null)
                {

                    List<AppUser> appUsers = appUserRepository.Where(x => x.AppUserRoles.Any(y => y.Role.Name == AppUserRoleEnum.Manager.ToString()) && x.IsActive, new string[] { "Employee" });


                    Order savedOrder = new Order();

                    CafeTable cafeTable = cafeTableRepository.FirstOrDefault(x => x.Id == addOrderDTO.CafeTableId && x.IsActive, new string[] { "Orders", "Orders.OrderDetails", "Orders.OrderDetails.Product", "Orders.OrderDetails.Product.ProductMaterials", "Orders.OrderDetails.Product.ProductMaterials.Material" });

                    if (cafeTable != null)
                    {
                        //Masa Arızalı ise

                        if (cafeTable.TableStatus == TableStatus.NotAvailable)
                        {
                            return BadRequest("Masa arızalı olduğu için sipariş verilemez!");
                        }


                        //Masa Dolu ise

                        if (cafeTable.TableStatus == TableStatus.Filled)
                        {
                            Order order = cafeTable.Orders?.FirstOrDefault(x => x.OrderStatus == OrderStatus.Active && x.IsActive);

                            if (order != null)
                            {
                                //sipariş detaylarından aynı üründen tekrar söylenmiş mi diye kontrol edilir çünkü aynı üründen tekrar sipariş verilmiş ise ürünün quantity si artırılır ve total price değişir.
                                List<OrderDetail> orderDetails = order.OrderDetails;
                                if (orderDetails != null && orderDetails.Count > 0)
                                {
                                    int updatedOrderDetail = default(int);
                                    OrderDetail savedOrderDetail = null;

                                    foreach (var orderDetailDTO in addOrderDTO.OrderDetails)
                                    {
                                        var isExistOrderDetailByProductId = orderDetails?.FirstOrDefault(x => x.ProductId == orderDetailDTO.ProductId && x.IsActive);

                                        if (isExistOrderDetailByProductId != null)
                                        {

                                            // sipariş verilen ürününün içindeki malzeme eşleşmelerinin içinde dönüyoruz
                                            int savedMaterial = 0;
                                            foreach (var productMaterial in isExistOrderDetailByProductId.Product.ProductMaterials)
                                            {
                                                productMaterial.Material.CurrentStock -= (productMaterial.MaterialQuantity * orderDetailDTO.Quantity);

                                                if (productMaterial.Material.CurrentStock >= 0)
                                                {
                                                    productMaterial.Material.ModifiedById = GetUserId();
                                                    savedMaterial = materialRepository.Update(productMaterial.Material);

                                                    // stok miktarı minimum stok miktarının altına düştüğünde mail gönderilir.
                                                    if (productMaterial.Material.MinStock >= productMaterial.Material.CurrentStock)
                                                    {
                                                        foreach (var appUser in appUsers)
                                                        {
                                                            if (appUser.Employee != null)
                                                            {

                                                                MailSender.SendEmail(appUser.Employee.Email, "Minimum Stok Bilgisi", $"Aşağıdaki malzemenin stok miktarı minimum stok miktarının altına düşmüştür!<br/> Malzeme Adı: {productMaterial.Material.MaterialName}<br/> Minimum Stok Miktarı: {productMaterial.Material.MinStock}<br/> Mevut Stok Miktarı: {productMaterial.Material.CurrentStock}");
                                                            }
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    return BadRequest("Stok miktarı yetersiz!");
                                                }
                                            }

                                            if (savedMaterial == 1)
                                            {
                                                isExistOrderDetailByProductId.Quantity += orderDetailDTO.Quantity;
                                                isExistOrderDetailByProductId.TotalPrice = (isExistOrderDetailByProductId.Quantity * isExistOrderDetailByProductId.Product.UnitPrice);
                                                isExistOrderDetailByProductId.ModifiedById = GetUserId();

                                                updatedOrderDetail = orderDetailRepository.Update(isExistOrderDetailByProductId);
                                            }
                                        }
                                        else  //Eğer üründen yok ise ürün sipariş detay tablosuna eklenir.
                                        {
                                            Product product = productRepository.FirstOrDefault(x => x.Id == orderDetailDTO.ProductId && x.IsActive, new string[] { "ProductMaterials", "ProductMaterials.Material" });

                                            if (product != null)
                                            {
                                                // sipariş verilen ürününün içindeki malzeme eşleşmelerinin içinde dönüyoruz
                                                int savedMaterial = 0;
                                                foreach (var productMaterial in product.ProductMaterials)
                                                {
                                                    productMaterial.Material.CurrentStock -= (productMaterial.MaterialQuantity * orderDetailDTO.Quantity);

                                                    if (productMaterial.Material.CurrentStock >= 0)
                                                    {
                                                        productMaterial.Material.ModifiedById = GetUserId();
                                                        savedMaterial = materialRepository.Update(productMaterial.Material);


                                                        // stok miktarı minimum stok miktarının altına düştüğünde mail gönderilir.
                                                        if (productMaterial.Material.MinStock >= productMaterial.Material.CurrentStock)
                                                        {
                                                            foreach (var appUser in appUsers)
                                                            {
                                                                if (appUser.Employee != null)
                                                                {

                                                                    MailSender.SendEmail(appUser.Employee.Email, "Minimum Stok Bilgisi", $"Aşağıdaki malzemenin stok miktarı minimum stok miktarının altına düşmüştür!<br/> Malzeme Adı: {productMaterial.Material.MaterialName}<br/> Minimum Stok Miktarı: {productMaterial.Material.MinStock}<br/> Mevut Stok Miktarı: {productMaterial.Material.CurrentStock}");
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return BadRequest("Stok miktarı yetersiz!");

                                                    }
                                                }

                                                if (savedMaterial == 1)
                                                {
                                                    OrderDetail orderDetail = new OrderDetail();
                                                    orderDetail.OrderId = order.Id;
                                                    orderDetail.ProductId = product.Id;
                                                    orderDetail.AppUserId = GetUserId();
                                                    orderDetail.Quantity = orderDetailDTO.Quantity;
                                                    orderDetail.TotalPrice = (product.UnitPrice * orderDetailDTO.Quantity);
                                                    orderDetail.CreatedById = GetUserId();

                                                    savedOrderDetail = orderDetailRepository.Add(orderDetail);
                                                }
                                            }
                                        }
                                    }

                                    if (updatedOrderDetail > 0 || savedOrderDetail != null)
                                    {
                                        return Json("Siparişleriniz Kaydedilmiştir :)");
                                    }
                                }
                            }
                        }


                        //Masa Boş ise

                        if (addOrderDTO.OrderDetails != null && addOrderDTO.OrderDetails.Count > 0)
                        {
                            foreach (var orderDetailDTO in addOrderDTO.OrderDetails)
                            {
                                Product product = productRepository.FirstOrDefault(x => x.Id == orderDetailDTO.ProductId && x.IsActive, new string[] { "ProductMaterials", "ProductMaterials.Material" });

                                if (product != null)
                                {
                                    // sipariş verilen ürününün içindeki malzeme eşleşmelerinin içinde dönüyoruz
                                    int savedMaterial = 0;
                                    foreach (var productMaterial in product.ProductMaterials)
                                    {
                                        productMaterial.Material.CurrentStock -= (productMaterial.MaterialQuantity * orderDetailDTO.Quantity);

                                        if (productMaterial.Material.CurrentStock >= 0)
                                        {
                                            productMaterial.Material.ModifiedById = GetUserId();
                                            savedMaterial = materialRepository.Update(productMaterial.Material);

                                            // stok miktarı minimum stok miktarının altına düştüğünde mail gönderilir.
                                            if (productMaterial.Material.MinStock >= productMaterial.Material.CurrentStock)
                                            {
                                                foreach (var appUser in appUsers)
                                                {
                                                    if (appUser.Employee != null)
                                                    {

                                                        MailSender.SendEmail(appUser.Employee.Email, "Minimum Stok Bilgisi", $"Aşağıdaki malzemenin stok miktarı minimum stok miktarının altına düşmüştür!<br/> Malzeme Adı: {productMaterial.Material.MaterialName}<br/> Minimum Stok Miktarı: {productMaterial.Material.MinStock}<br/> Mevut Stok Miktarı: {productMaterial.Material.CurrentStock}");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return BadRequest("Stok miktarı yetersiz!");
                                        }
                                    }

                                    Order orderEntity = new Order();    //Sipariş oluşturulur.
                                    orderEntity.OrderName = $"{cafeTable.TableName} Sipariş";
                                    orderEntity.OrderNumber = Guid.NewGuid().ToString().Substring(0, 8);
                                    orderEntity.OrderStatus = OrderStatus.Active;
                                    orderEntity.CafeTableId = cafeTable.Id;
                                    orderEntity.CreatedById = GetUserId();
                                    savedOrder = orderRepository.Add(orderEntity);

                                    if (savedMaterial == 1)
                                    {
                                        OrderDetail orderDetail = new OrderDetail();  //Sipariş Detayları oluşturulur.
                                        orderDetail.OrderId = savedOrder.Id;
                                        orderDetail.ProductId = product.Id;
                                        orderDetail.AppUserId = GetUserId();
                                        orderDetail.Quantity = orderDetailDTO.Quantity;
                                        orderDetail.TotalPrice = (product.UnitPrice * orderDetailDTO.Quantity);
                                        orderDetail.CreatedById = GetUserId();

                                        OrderDetail savedOrderDetail = orderDetailRepository.Add(orderDetail);

                                        if (savedOrderDetail != null)
                                        {
                                            cafeTable.TableStatus = TableStatus.Filled;
                                            cafeTable.ModifiedById = GetUserId();
                                            int isSuccess = cafeTableRepository.Update(cafeTable);
                                        }
                                    }
                                    else
                                    {
                                        savedOrder.IsActive = false;
                                        savedOrder.OrderStatus = OrderStatus.Canceled;
                                        savedOrder.ModifiedById = GetUserId();
                                        orderRepository.Update(savedOrder);
                                    }
                                }
                            }
                        }

                    }
                    return Json("Siparişleriniz Kaydedilmiştir :)");
                }
                else
                {
                    return BadRequest("Üzgünüz İşlem Yapılamadı!");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);

            }
        }


        /// <summary>
        /// Masa Id'sine Göre Sipariş Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetOrderByTableId(int id)
        {
            var order = orderRepository.FirstOrDefault(x => x.CafeTableId == id && x.IsActive, new string[] { "OrderDetails", "OrderDetails.Product" });

            if (order != null && order.OrderDetails != null)
            {
                order.OrderDetails = order?.OrderDetails?.Where(x => x.IsActive).ToList();
            }

            OrderDTO orderDTO = Mapper.Map<OrderDTO>(order);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };

            return Json(orderDTO, settings);
        }

        /// <summary>
        /// Sipariş Kapatma
        /// </summary>
        /// <param name="completeOrderDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CompleteOrder(CompleteOrderDTO completeOrderDTO)
        {
            Order order = orderRepository.FirstOrDefault(x => x.CafeTableId == completeOrderDTO.CafeTableId && x.IsActive, new string[] { "OrderDetails" });

            order.OrderDetails = order.OrderDetails.Where(x => x.IsActive).ToList();

            CafeTable cafeTable = cafeTableRepository.FirstOrDefault(x => x.Id == completeOrderDTO.CafeTableId && x.IsActive && x.TableStatus == TableStatus.Filled);

            foreach (var orderDetail in order.OrderDetails)
            {
                orderDetail.IsActive = false;
                orderDetail.ModifiedById = GetUserId();
                orderDetailRepository.Update(orderDetail);
            }

            order.OrderStatus = OrderStatus.Completed;
            order.ModifiedById = GetUserId();
            order.IsActive = false;

            int updatedOrder = orderRepository.Update(order);

            if (updatedOrder > 0)
            {
                cafeTable.TableStatus = TableStatus.Empty;
                cafeTable.ModifiedById = GetUserId();

                int updatedCafeTable = cafeTableRepository.Update(cafeTable);

                if (updatedCafeTable > 0)
                {
                    int userId = GetUserId();
                    AppUser appUser = appUserRepository.FirstOrDefault(x => x.Id == userId && x.IsActive, new string[] { "Employee.Department" });

                    // indirimli fiyat hesaplama
                    decimal totalPrice = completeOrderDTO.Discount > 0 ? (order.OrderDetails.Sum(x => x.TotalPrice) - (order.OrderDetails.Sum(x => x.TotalPrice) * (completeOrderDTO.Discount / 100))) : order.OrderDetails.Sum(x => x.TotalPrice);

                    // fiş oluşturma
                    Receipt receipt = new Receipt();
                    receipt.ReceiptNumber = Guid.NewGuid().ToString().Substring(0, 8);
                    receipt.Discount = completeOrderDTO.Discount;
                    receipt.TotalPrice = totalPrice;
                    receipt.PaymentType = (PaymentType)completeOrderDTO.PaymentTypeId;
                    receipt.CompanyId = appUser?.Employee?.Department?.CompanyId;
                    receipt.OrderId = order.Id;
                    receipt.CreatedById = userId;

                    Receipt savedReceipt = receiptRepository.Add(receipt);

                    return Json(savedReceipt);
                }
            }
            else
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    orderDetail.IsActive = true;
                    orderDetail.ModifiedById = null;
                    orderDetailRepository.Update(orderDetail);
                }
            }

            return Json(updatedOrder);
        }

        /// <summary>
        /// Kapatılan Sipariş Raporlarını Getirme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCompleteOrderReport()
        {
            List<Order> completeorder = orderRepository.Where(x => x.OrderStatus == OrderStatus.Completed, new string[] { "CafeTable" });
            List<OrderDTO> completeorderDTOs = Mapper.Map<List<OrderDTO>>(completeorder);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };

            return Json(completeorderDTOs, settings);
        }
        /// <summary>
        /// Sipariş Id ye göre Sipariş Detay Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetOrderDetailsByOrderId(int id)
        {
            List<OrderDetail> orderDetails = orderDetailRepository.Where(x => x.OrderId == id, new string[] { "Product" });
            List<OrderDetailDTO> orderDetailDTOs = Mapper.Map<List<OrderDetailDTO>>(orderDetails);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(orderDetailDTOs, settings);
        }

    }
}
