using System.Collections.Generic;

namespace HC.API.Models.DTOClasses
{
    public class PageRoleDTO
    {
        public int PageId { get; set; }
        public List<int> RoleList { get; set; }
    }
}