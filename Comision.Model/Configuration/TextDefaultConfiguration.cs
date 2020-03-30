using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
   public class TextDefaultConfiguration:EntityTypeConfiguration<TextDefault>
   {
       public TextDefaultConfiguration()
       {
           this.HasKey(x => x.Id);

            this.HasRequired(x=>x.University)
                .WithMany(x=>x.TextDefaults)
                .HasForeignKey(x=>x.UnivercityId)
                .WillCascadeOnDelete(false);
                

       }
    }
}
