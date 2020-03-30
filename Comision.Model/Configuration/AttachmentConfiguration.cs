using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
   public class AttachmentConfiguration:EntityTypeConfiguration<Attachment>
   {
       public AttachmentConfiguration()
       {
           this.HasKey(x => x.Id);

            this.HasRequired(x=>x.Request)
                .WithMany(x=>x.Attachments)
                .HasForeignKey(x=>x.RequestId)
                .WillCascadeOnDelete(false);
       }
    }
}
