using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignments
    {
        public Assignments()
        {
            Submission = new HashSet<Submission>();
        }

        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int AssignmentId { get; set; }
        public int MaxPoint { get; set; }
        public string Contents { get; set; }
        public DateTime DueDate { get; set; }

        public virtual AssignmentCategories Category { get; set; }
        public virtual ICollection<Submission> Submission { get; set; }
    }
}
