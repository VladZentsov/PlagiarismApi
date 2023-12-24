﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.CoreModels
{
    public class CompareResult
    {
        public string WorkName { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserGroupName { get; set; }
        public List<IdenticalLines> IdenticalLines { get; set; }
        public ParsedWork WorkToCompare { get; set; }
        public double MatchPercentage { get; set; }
    }
}
