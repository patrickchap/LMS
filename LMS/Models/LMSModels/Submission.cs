using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submission
    {
        public int AssignmentId { get; set; }
        public string StudentId { get; set; }
        public int Score { get; set; }
        public string Contents { get; set; }
        public DateTime Time { get; set; }

        public virtual Assignments Assignment { get; set; }
        public virtual Students Student { get; set; }
    }
}
