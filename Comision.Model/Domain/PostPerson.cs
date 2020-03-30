using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
    /// <summary>
    /// <span dir="rtl">”„  ‘Œ’</span>
    /// </summary>
    public class PostPerson : AuditableEntity<long>
    {

        public long PersonId { get; set; }
        public virtual Person Person { get; set; }

        [Index("IX_PostPersons", IsUnique = true, Order = 1)]
        public long PostId { get; set; }
        public virtual Post Post { get; set; }

        [Index("IX_PostPersons", IsUnique = true, Order = 2)]
        public long? CentralOrganizationId { get; set; }
        public virtual CentralOrganization CentralOrganization { get; set; }

        [Index("IX_PostPersons", IsUnique = true, Order = 3)]
        public long? BranchProvinceId { get; set; }
        public virtual BranchProvince BranchProvince { get; set; }

        [Index("IX_PostPersons", IsUnique = true, Order = 4)]
        public long? UniversityId { get; set; }
        public virtual University University { get; set; }

        [Index("IX_PostPersons", IsUnique = true, Order = 5)]
        public long? CollegeId { get; set; }
        public virtual College College { get; set; }

        [Index("IX_PostPersons", IsUnique = true, Order = 6)]
        public long? EducationalGroupId { get; set; }
        public virtual EducationalGroup EducationalGroup { get; set; }

        [Index("IX_PostPersons", IsUnique = true, Order = 7)]
        public long? FieldofStudyId { get; set; }
        public virtual FieldofStudy FieldofStudy { get; set; }

        public PostPerson()
        {
        }

        public PostPerson(long personId, long postId, long levelId, PostType postType)
        {
            PersonId = personId;
            PostId = postId;
            if (postType == PostType.CentralOrganization)
                CentralOrganizationId = levelId;
            else if (postType == PostType.BranchProvince)
                BranchProvinceId = levelId;
            else if (postType == PostType.University)
                UniversityId = levelId;
            else if (postType == PostType.College)
                CollegeId = levelId;
            else if (postType == PostType.EducationalGroup)
                EducationalGroupId = levelId;
            else if (postType == PostType.FieldofStudy)
                FieldofStudyId = levelId;
        }

    }//end PostPerson
}