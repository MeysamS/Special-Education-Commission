using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            HasKey(k => k.Id);

            Property(a => a.CentralOrganizationId).IsOptional();
            Property(a => a.BranchProvinceId).IsOptional();
            Property(a => a.UniversityId).IsOptional();

            HasOptional(x => x.CentralOrganization)
                .WithMany(x => x.Persons)
                .HasForeignKey(x => x.CentralOrganizationId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.BranchProvince)
                .WithMany(x => x.Persons)
                .HasForeignKey(x => x.BranchProvinceId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.University)
                .WithMany(x => x.Persons)
                .HasForeignKey(x => x.UniversityId)
                .WillCascadeOnDelete(false);
        }
    }
}
