using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(c => c.Id);
           
            HasRequired(cu => cu.Person)
                .WithOptional(en => en.User)
                .WillCascadeOnDelete(false);

            HasRequired(cu => cu.Authentication)
                .WithMany(en => en.Users)
                .HasForeignKey(fk=>fk.AuthenticationId)
                .WillCascadeOnDelete(false);

        }
    }
}
