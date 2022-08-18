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
using System.Configuration;
using System.Web.Http;

namespace HC.API.Controllers
{
    [AuthorizeRoles(nameof(AppUserRoleEnum.Manager), nameof(AppUserRoleEnum.Admin))]
    public class EmployeeController : BaseApiController
    {
        EmployeeRepository employeeRepository = new EmployeeRepository();
        AppUserRepository appUserRepository = new AppUserRepository();
        AppUserRoleRepository appUserRoleRepository = new AppUserRoleRepository();

        /// <summary>
        /// Tüm Çalışanları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult EmployeeList()
        {
            var employees = employeeRepository.GetAll();
            List<EmployeeDTO> employeeDTOs = Mapper.Map<List<EmployeeDTO>>(employees);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(employeeDTOs, settings);
        }

        /// <summary>
        /// Aktif Çalışanları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ActivesEmployeeList()
        {
            var employees = employeeRepository.GetActives(new string[] { "Department" });

            List<EmployeeDTO> employeeDTOs = Mapper.Map<List<EmployeeDTO>>(employees);

            foreach (var employeeDTO in employeeDTOs)
            {
                if (employeeDTO.Department == null)
                {
                    employeeDTO.Department = new DepartmentDTO();
                    employeeDTO.Department.DepartmentName = "-";
                }
            }

            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(employeeDTOs, settings);
        }
        /// <summary>
        /// Pasif Çalışanları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult PasivesEmployeeList()
        {
            var employees = employeeRepository.GetPassives();
            List<EmployeeDTO> employeeDTOs = Mapper.Map<List<EmployeeDTO>>(employees);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(employeeDTOs, settings);
        }
        /// <summary>
        /// GÜncellenen  Çalışanları Listeleme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ModifiedsEmployeeList()
        {
            var employees = employeeRepository.GetModifieds();
            List<EmployeeDTO> employeeDTOs = Mapper.Map<List<EmployeeDTO>>(employees);
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore//ilişkilendirmede sonsuz döngüye girmesini engellemek için
            };
            return Json(employeeDTOs, settings);
        }
        /// <summary>
        /// Çalışan Getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult BringEmployee(int id)
        {
            var employee = employeeRepository.FirstOrDefault(x => x.Id == id);
            EmployeeDTO employeeDTO = Mapper.Map<EmployeeDTO>(employee);
            return Json(employeeDTO);
        }

        /// <summary>
        /// Çalışan Ekleme
        /// </summary>
        /// <param name="employeeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                if (employeeDTO != null)
                {
                    Employee employee = Mapper.Map<Employee>(employeeDTO);

                    employee.CreatedById = GetUserId();

                    Employee savedEmployee = employeeRepository.Add(employee);

                    if (savedEmployee != null && employeeDTO.IsCreateAppUser)
                    {
                        AppUser appUser = new AppUser();
                        appUser.UserName = $"{savedEmployee.FirstName.Replace(" ", "")}.{savedEmployee.LastName.Replace(" ","")}".ToLower();
                        appUser.Password = Guid.NewGuid().ToString().Substring(0, 6);
                        appUser.CreatedById = GetUserId();
                        appUser.EmployeeId = savedEmployee.Id;
                        appUser.IsActive = true; // Kullanıcının kendi hesabını onaylaması için normal değeri false olmalı ve mail adresine gelen maile gidip linke tıklayarak yeni şifre oluşturması gerekiyor.

                        AppUser savedAppUser = appUserRepository.Add(appUser);

                        if (employeeDTO.AppUserRoles.Count > 0)
                        {
                            foreach (var roleId in employeeDTO.AppUserRoles)
                            {
                                AppUserRole appUserRole = new AppUserRole();
                                appUserRole.AppUserId = savedAppUser.Id;
                                appUserRole.RoleId = roleId;
                                appUserRole.CreatedById = GetUserId();
                                appUserRoleRepository.Add(appUserRole);
                            }
                        }

                        if (savedAppUser != null)
                        {
                            //webconfigte appSettings içine yazdığımız UIUrl(value=adresi) burada kullanıyoruz.
                            string UIUrl = ConfigurationManager.AppSettings["UIUrl"];

                            // UI tarafında Şifre Oluşturma sayfası yapıldıktan sonra bu link yollanacak.
                            //$"Linki tıklayıp şifrenizi belirledikten sonra {savedAppUser.UserName} kullanıcı adınız ile sisteme giriş yapabilirsiniz.<br/><br/> {UIUrl}/Authentication/CreatePassword/" + savedAppUser.ActivationCode

                            MailSender.SendEmail(savedEmployee.Email, "Üyelik Aktivasyon", $"{savedAppUser.UserName} kullanıcı adınız ve şifreniz {savedAppUser.Password} ile sisteme giriş yapabilirsiniz.");
                        }
                    }

                    return Json(savedEmployee);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Çalışan Silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteEmployee(int id)
        {
            var employee = employeeRepository.Find(id);
            if (employee != null)
            {
                employee.DeletedById = GetUserId();

                employeeRepository.Delete(employee);

                return Json("Silme İşlemi Başarılı");
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Çalışan Güncelleme
        /// </summary>
        /// <param name="employeeDTO"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateEmployee(EmployeeDTO employeeDTO)
        {
            int isSuccess = 0;

            if (employeeDTO != null)
            {
                Employee employee = Mapper.Map<Employee>(employeeDTO);
                employee.ModifiedById = GetUserId();

                isSuccess = employeeRepository.Update(employee);
            }
            return Json(isSuccess);
        }

    }
}
