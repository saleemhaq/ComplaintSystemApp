﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ComplaintApp.Application.Dtos
{
    public class ComplaintDto
    {
        public string ComplaintName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public int CreatorUserId { get; set; }

        public string ComplaintRegarding { get; set; }
    }
}