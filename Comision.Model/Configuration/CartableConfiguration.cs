using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{

    public class CartableConfiguration : EntityTypeConfiguration<Cartable>
    {
        public CartableConfiguration()
        {
            HasKey(c => c.Id);
          //  AutomaticMigrationsEnabled = true;

            //ارتباط بین درخواست با تاييد درخواست - یک به چند
            HasRequired(x => x.Request)
              .WithMany(x => x.Cartables)
              .HasForeignKey(x => x.RequestId)
              .WillCascadeOnDelete(false);

            //ارتباط بین شخص با تاييد درخواست - یک به چند
            HasRequired(x => x.Person)
              .WithMany(x => x.Cartables)
              .HasForeignKey(x => x.PersonId)
              .WillCascadeOnDelete(false);

            // ارتباط بین پرسنلی از طرف از جدول کارتابل با جدول شخص
            HasRequired(x => x.PersonOnBehalfof)
            .WithMany(x => x.CartablesBehalfof)
            .HasForeignKey(x => x.PersonIdOnBehalfof)
            .WillCascadeOnDelete(false);


            //ارتباط بین سمت با تاييد درخواست - یک به چند
            HasRequired(x => x.Post)
              .WithMany(x => x.Cartables)
              .HasForeignKey(x => x.PostId)
              .WillCascadeOnDelete(false);
        }
    }
}
