using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class MemberDetailsConfiguration : EntityTypeConfiguration<MemberDetails>
    {
        public MemberDetailsConfiguration()
        {
            HasKey(k => k.Id);

            HasRequired(x => x.MemberMaster)
                .WithMany(x => x.MemberDetails)
                .HasForeignKey(x => x.MemberMasterId)
                .WillCascadeOnDelete(false);

            //ارتباط بین شخص با اعضای شورا و کمیسیون
            //HasRequired(x => x.Person)
            //  .WithMany(x => x.MemberDetails)
            //  .HasForeignKey(x => x.PersonId)
            //  .WillCascadeOnDelete(false);

        }
    }
}
