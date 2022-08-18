using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace HC.API.Controllers
{
    public class BaseApiController : ApiController
    {
        //Burada oluşturdugumuz methodları her controllerde kullanacagımız için ara bir controller oluşturduk.Kalıtım verdiği bütün controllerde buradaki methodlar kullanılabilecek.Bu sayede gereksiz kod tekrarları kalabalığı olmayacak.

        //Swagger üzerinde bu methodun görünmesine gerek olmadığı için aşağıdaki attribute eklendi

        [ApiExplorerSettings(IgnoreApi = true)]
        public int GetUserId()  //sisteme giriş yapmış kullanıcının Idsini getirmek için
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = Convert.ToInt32(identity.Claims.FirstOrDefault(x => x.Type == "id").Value);
            return userId;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<string> GetUserRoles() ////sisteme giriş yapmış kullanıcının rollerini getirmek için
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<string> userRoles = identity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x=>x.Value).ToList();
            return userRoles;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetUserName() ////sisteme giriş yapmış kullanıcının kullanıcı ismini getirmek için
        {
            var identity = (ClaimsIdentity)User.Identity;
            string username = identity.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            return username;
        }
    }
}
