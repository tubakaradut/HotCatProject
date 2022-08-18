using HC.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Map.Options
{
  public class MaterialMap:BaseMap<Material>
    {
        public MaterialMap()
        {
            ToTable("dbo.Materials");
            Property(x => x.MaterialName).HasMaxLength(255).IsRequired();
            Property(x => x.UnitType).IsRequired();
            Property(x => x.ExpirationDate).IsRequired();
        }                                  
    }
}
