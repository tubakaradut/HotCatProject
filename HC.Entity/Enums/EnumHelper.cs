using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace HC.Entity.Enums
{
    public static class EnumHelper
    {
        //Enum değerlerini Viewlarımızda gösterirken name‘leri değilde, tanımlamış olduğumuz görünür adları ile listelensin istediğinde yani display attribütündeki name getirmek için yazıldı.
        public static string GetDisplayName(Enum value)
        {
            return value.GetType()?
           .GetMember(value.ToString())?.First()?
           .GetCustomAttribute<DisplayAttribute>()?
           .Name;
        }
    }
}
