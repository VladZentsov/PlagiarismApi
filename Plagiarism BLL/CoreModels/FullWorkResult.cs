using Plagiarism_BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.CoreModels
{
    public class FullWorkResult:BaseModel
    {
        public string Code { get; set; }
        public WorkType WorkType { get; set; }
        public string WorkName { get; set; }
    }
}
