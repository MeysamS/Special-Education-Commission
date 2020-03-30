using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class MemberMasterConfiguration : EntityTypeConfiguration<MemberMaster>
    {
        public MemberMasterConfiguration()
        {
            HasKey(k => k.Id);

            HasRequired(x => x.University)
                .WithMany(x => x.MemberMasters)
                .HasForeignKey(x => x.UniversityId)
                .WillCascadeOnDelete(false);
        }
    }
}
