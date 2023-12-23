﻿using Plagiarism_BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int UniversityYear { get; set; }
        public string GroupName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
