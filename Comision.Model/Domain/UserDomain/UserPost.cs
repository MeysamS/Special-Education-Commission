using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;

namespace Comision.Model.Domain.UserDomain
{
    /// <summary>
    ///  اختیارات و وظایفی که به کاربر اختصاص داده می شود
    /// برای سطح دسترسی داده ای
    /// </summary>
    public class UserPost : AuditableEntity<long>
    {
        [Index("IX_UserPosts", IsUnique = true, Order = 1)]
        public long UserId { get; set; }
        public virtual User User { get; set; }

        [Index("IX_UserPosts", IsUnique = true, Order = 2)]
        public long PostId { get; set; }
        public virtual Post Post { get; set; }

        [Index("IX_UserPosts", IsUnique = true, Order = 3)]
        public long? CentralOrganizationId { get; set; }
        public virtual CentralOrganization CentralOrganization { get; set; }

        [Index("IX_UserPosts", IsUnique = true, Order = 4)]
        public long? BranchProvinceId { get; set; }
        public virtual BranchProvince BranchProvince { get; set; }

        [Index("IX_UserPosts", IsUnique = true, Order = 5)]
        public long? UniversityId { get; set; }
        public virtual University University { get; set; }

        [Index("IX_UserPosts", IsUnique = true, Order = 6)]
        public long? CollegeId { get; set; }
        public virtual College College { get; set; }

        [Index("IX_UserPosts", IsUnique = true, Order = 7)]
        public long? EducationalGroupId { get; set; }
        public virtual EducationalGroup EducationalGroup { get; set; }

        [Index("IX_UserPosts", IsUnique = true, Order = 8)]
        public long? FieldofStudyId { get; set; }
        public virtual FieldofStudy FieldofStudy { get; set; }

    }
}
