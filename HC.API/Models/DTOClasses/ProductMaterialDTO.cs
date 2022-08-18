namespace HC.API.Models.DTOClasses
{
    public class ProductMaterialDTO
    {
        public ProductMaterialDTO()
        {
            IsActive = true;
        }

        public decimal MaterialQuantity { get; set; }
        public bool IsActive { get; set; }


        public int ProductId { get; set; }
        public virtual ProductDTO Product { get; set; }

        public int MaterialId { get; set; }
        public virtual MaterialDTO Material { get; set; }

        

    }
}