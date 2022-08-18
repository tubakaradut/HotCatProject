using AutoMapper;
using HC.API.Models.DTOClasses;
using HC.Entity.Model;

namespace HC.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //AutoMapper: 2 nesneyi birbirine eşlemeyi sağlayan kütüphanedir. DTO lar ile Entityleri maplemek(eşlestirmek) için aşağıdaki konfigürasyonu kullanıyoruz. ReverseMap ters yönlü de mapleme imkanı sağlıyor.

            CreateMap<AppUser, LoginDTO>().ReverseMap();
            CreateMap<AppUser, AppUserDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Employee, AppUser>().ReverseMap();
            CreateMap<Page, PageDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<CafeTable, CafeTableDTO>().ReverseMap();
            CreateMap<Company, CompanyDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Material, MaterialDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Receipt, ReceiptDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<ProductMaterial, ProductMaterialDTO>().ReverseMap();
        }
    }
}