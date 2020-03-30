using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class UniversityConfiguration : EntityTypeConfiguration<University>
    {
        public UniversityConfiguration()
        {
            HasKey(c => c.Id);
            Property(a => a.Name).IsRequired().IsUnicode(true).HasColumnType("nvarchar").HasMaxLength(100);

            //ارتباط بین واحد مرکز استانی با دانشگاه - یک به چند
            HasRequired(x => x.BranchProvince)
              .WithMany(x => x.Universities)
              .HasForeignKey(x => x.BranchProvinceId)
              .WillCascadeOnDelete(false);
        }
    }
}
