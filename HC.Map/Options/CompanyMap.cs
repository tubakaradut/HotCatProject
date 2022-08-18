using HC.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Map.Options
{
   public class CompanyMap:BaseMap<Company>
    {
        public CompanyMap()
        {
            ToTable("dbo.Companies");
            Property(x => x.CompanyName).HasMaxLength(255).IsRequired();
            Property(x => x.Address).HasMaxLength(255).IsOptional();
            Property(x => x.PhoneNumber).HasMaxLength(255).IsOptional();
            Property(x => x.TaxNo).HasMaxLength(255).IsOptional();
        }
    }
}
