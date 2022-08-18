namespace HC.API.Models.DTOClasses
{
    public class PageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? ParentId { get; set; }
        public bool IsExistSubMenu { get; set; }
        public bool IsMainPage { get; set; }

        public string Roles { get; set; }

    }
}