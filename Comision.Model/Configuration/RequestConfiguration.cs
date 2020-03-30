using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{

    public class RequestConfiguration : EntityTypeConfiguration<Request>
    {
        public RequestConfiguration()
        {
            HasKey(c => c.Id);

            //ارتباط بین شخص با درخواست - یک به چند
            HasRequired(x => x.Person)
              .WithMany(x => x.Requests)
              .HasForeignKey(x => x.PersonId)
              .WillCascadeOnDelete(false);

            //ارتباط بین گروه یا مستر اعضا با درخواست - یک به چند
            HasRequired(x => x.MemberMaster)
              .WithMany(x => x.Requests)
              .HasForeignKey(x => x.MemberMasterId)
              .WillCascadeOnDelete(false);
           
        }
    }
}
