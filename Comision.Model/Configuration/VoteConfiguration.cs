using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class VoteConfiguration : EntityTypeConfiguration<Vote>
    {
        public VoteConfiguration()
        {
            HasKey(c => c.RequestId);
       
            //ارتباط درخواست با رای صادره - یک به صفر
            HasRequired(cu => cu.Request)
                .WithOptional(en => en.Vote)
                .WillCascadeOnDelete(false);
           
            //ارتباط بین شخص با رای - یک به چند
            HasRequired(x => x.Person)
              .WithMany(x => x.Votes)
              .HasForeignKey(x => x.PersonId)
              .WillCascadeOnDelete(false);

            //ارتباط بین سمت با رای - یک به چند
            HasRequired(x => x.Post)
              .WithMany(x => x.Votes)
              .HasForeignKey(x => x.PostId)
              .WillCascadeOnDelete(false);
        }
    }

}
