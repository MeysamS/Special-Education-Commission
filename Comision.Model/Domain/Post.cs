using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
    /// <summary>
    /// <span dir="rtl">سمت</span>
    /// </summary>
    public class Post : AuditableEntity<long>
    {
        [Index("IX_PostName", IsUnique = true)]
        public string Name { get; set; }
        public PostType PostType { get; set; }

        public virtual ICollection<MemberDetails> MemberDetails { get; set; }
        /// <summary>
        /// <span dir="rtl">لیست شخص</span>
        /// </summary>
        public virtual ICollection<PostPerson> PostPersons { get; set; }
        /// <summary>
        /// لیست کاربرانی که اختیارات مربوط این پست را دارند
        /// </summary>
        public virtual ICollection<UserPost> UserPosts { get; set; }

        /// <summary>
        /// <span dir="rtl">لیست تایید درخواست</span>
        /// </summary>
        public virtual ICollection<Cartable> Cartables { get; set; }
        public virtual ICollection<Signer> Signers { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }//end Post
}