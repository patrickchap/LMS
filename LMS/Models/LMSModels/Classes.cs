using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Classes
    {
        public Classes()
        {
            AssignmentCategories = new HashSet<AssignmentCategories>();
            Enrolled = new HashSet<Enrolled>();
        }

        public string CourseCatalogId { get; set; }
        public string Semester { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ClassId { get; set; }
        public string TaughtBy { get; set; }
        public uint Year { get; set; }
        public string Location { get; set; }

        public virtual Courses CourseCatalog { get; set; }
        public virtual Professors TaughtByNavigation { get; set; }
        public virtual ICollection<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual ICollection<Enrolled> Enrolled { get; set; }
    }
}
