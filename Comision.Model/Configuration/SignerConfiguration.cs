using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class SignerConfiguration : EntityTypeConfiguration<Signer>
    {
        public SignerConfiguration()
        {
            HasKey(c => c.Id);            

            //ارتباط بین سمت با امضا کننده گان - یک به چند
            HasRequired(x => x.Post)
              .WithMany(x => x.Signers)
              .HasForeignKey(x => x.PostId)
              .WillCascadeOnDelete(false);
        }
    }
}
