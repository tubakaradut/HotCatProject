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
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin),nameof(AppUserRoleEnum.Cashier))]
    public class ReceiptController : BaseApiController
    {
        ReceiptRepository receiptRepository = new ReceiptRepository();
        CompanyRepository companyRepository = new CompanyRepository();

        /// <summary>
        /// Tüm Fişleri listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ReceiptList()
        {
            var receipts = receiptRepository.GetAll();
            List<ReceiptDTO> receiptDTOs = Mapper.Map<List<ReceiptDTO>>(receipts);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(receiptDTOs, settings);
        }
        /// <summary>
        /// Aktif Fişleri listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ActivesReceiptList()
        {
            var receipts = receiptRepository.GetActives();
            List<ReceiptDTO> receiptDTOs = Mapper.Map<List<ReceiptDTO>>(receipts);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(receiptDTOs, settings);
        }
        /// <summary>
        /// Pasif Fişleri listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult PasivesReceiptList()
        {
            var receipts = receiptRepository.GetPassives();
            List<ReceiptDTO> receiptDTOs = Mapper.Map<List<ReceiptDTO>>(receipts);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(receiptDTOs, settings);
        }
        /// <summary>
        /// Güncellenen Fişleri listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ModifiedsReceiptList()
        {
            var receipts = receiptRepository.GetPassives();
            List<ReceiptDTO> receiptDTOs = Mapper.Map<List<ReceiptDTO>>(receipts);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(receiptDTOs, settings);
        }


        /// <summary>
        /// Fiş Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult BringReceipt(int id)
        {
            Receipt receipt = receiptRepository.FirstOrDefault(x => x.Id == id, new string[] { "Order.OrderDetails.Product" });

            receipt.Order.OrderDetails = receipt.Order.OrderDetails.OrderBy(x => x.Id).ToList();

            foreach (var orderDetail in receipt.Order.OrderDetails)
            {
                orderDetail.Product.Description = orderDetail.Product.Description != null ? orderDetail.Product.Description : "Açıklama Yok";
            }

            ReceiptDTO receiptDTO = Mapper.Map<ReceiptDTO>(receipt);

            if (receiptDTO != null)
            {
                Company company = companyRepository.FirstOrDefault(x => x.Id == receipt.CompanyId);
                CompanyDTO companyDTO = Mapper.Map<CompanyDTO>(company);
                receiptDTO.Company = companyDTO;
            }


            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(receiptDTO, settings);
        }


        /// <summary>
        /// Fiş Ekleme
        /// </summary>
        /// <param name="receiptDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReceipt(ReceiptDTO receiptDTO)
        {
            try
            {
                if (receiptDTO != null)
                {
                    Receipt receipt = Mapper.Map<Receipt>(receiptDTO);
                    receipt.CreatedById = GetUserId();
                    Receipt savedReceipt = receiptRepository.Add(receipt);
                    return Json(savedReceipt);
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
        /// Fiş Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteReceipt(int id)
        {
            var receipt = receiptRepository.Find(id);
            if (receipt != null)
            {
                receipt.DeletedById = GetUserId();
                receiptRepository.Delete(receipt);
                return Json("Silme İşlemi Başarılı");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Fiş Güncelleme
        /// </summary>
        /// <param name="receiptDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReceipt(ReceiptDTO receiptDTO)
        {
            int isSuccess = 0;
            if (receiptDTO != null)
            {
                Receipt receipt = Mapper.Map<Receipt>(receiptDTO);
                receipt.ModifiedById = GetUserId();
                isSuccess = receiptRepository.Update(receipt);
            }
            return Json(isSuccess);
        }

    }
}

