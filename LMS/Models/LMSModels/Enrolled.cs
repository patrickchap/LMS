﻿using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Enrolled
    {
        public string StudentId { get; set; }
        public int ClassId { get; set; }
        public string Grade { get; set; }

        public virtual Classes Class { get; set; }
        public virtual Students Student { get; set; }
    }
}
