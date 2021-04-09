using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Courses
    {
        public Courses()
        {
            Classes = new HashSet<Classes>();
        }

        public string Name { get; set; }
        public int Number { get; set; }
        public string CatalogId { get; set; }
        public string DeptAbbr { get; set; }

        public virtual Departments DeptAbbrNavigation { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
    }
}
