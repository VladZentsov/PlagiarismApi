using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.CoreModels
{
    public class IdenticalLines
    {
        public int CurrentWorkLineNumber { get; set; }
        public List<int>? WorkToCompareLineNumbers { get; set; } 
    }
}
