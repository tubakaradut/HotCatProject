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
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
    public class DepartmentController : BaseApiController
    {
        DepartmentRepository departmentRepository = new DepartmentRepository();

        /// <summary>
        /// Tüm Departmanları Getir
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult DepartmentList()
        {
            var departments = departmentRepository.GetAll();
            List<DepartmentDTO> departmentDTOs = Mapper.Map<List<DepartmentDTO>>(departments);

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(departmentDTOs, settings);
        }
        /// <summary>
        /// Aktif Departmanları Getir
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ActivesDepartmentList()
        {
            var departments = departmentRepository.GetActives(new string[] { "Company" });

            List<DepartmentDTO> departmentDTOs = Mapper.Map<List<DepartmentDTO>>(departments);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(departmentDTOs, settings);
        }

        /// <summary>
        /// Pasif Departmanları Getir
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult PasivesDepartmentList()
        {
            var departments = departmentRepository.GetPassives();
            List<DepartmentDTO> departmentDTOs = Mapper.Map<List<DepartmentDTO>>(departments);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(departmentDTOs, settings);
        }
        /// <summary>
        /// Güncellenen Departmanları Getir
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ModifiedsDepartmentList()
        {
            var departments = departmentRepository.GetModifieds();
            List<DepartmentDTO> departmentDTOs = Mapper.Map<List<DepartmentDTO>>(departments);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(departmentDTOs, settings);
        }
        /// <summary>
        /// Departman Getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult BringDepartment(int id)
        {
            Department department = departmentRepository.FirstOrDefault(x => x.Id == id);
            DepartmentDTO departmentDTO = Mapper.Map<DepartmentDTO>(department);
            return Json(departmentDTO);
        }

        /// <summary>
        /// Departman Ekle
        /// </summary>
        /// <param name="departmentDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                if (departmentDTO != null)
                {
                    Department department = Mapper.Map<Department>(departmentDTO);
                    department.CreatedById = GetUserId();
                    Department savedDepartment=departmentRepository.Add(department);
                    return Json(savedDepartment);
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
      /// Departman Silme
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteDepartment(int id)
        {
            var department = departmentRepository.Find(id);
            if (department != null)
            {
                department.DeletedById = GetUserId();
                departmentRepository.Delete(department);
                return Json("Silme İşlemi Başarılı");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Departman Güncelleme
        /// </summary>
        /// <param name="departmentDTO"></param>
        /// <returns></returns>
        [HttpPut]

        public IHttpActionResult UpdateDepartment(DepartmentDTO departmentDTO)
        {
            int isSuccess = 0;
            if (departmentDTO!=null)
            {
                Department department = Mapper.Map<Department>(departmentDTO);
                department.ModifiedById = GetUserId();
                isSuccess= departmentRepository.Update(department);
            }
            
            return Json(isSuccess);
        }

    }
}
