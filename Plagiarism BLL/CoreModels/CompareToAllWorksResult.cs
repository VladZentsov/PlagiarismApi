using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.CoreModels
{
    public class CompareToAllWorksResult
    {
        public ParsedWork CurrentWork { get; set; }
        public List<CompareResult> CompareResults { get; set; } = new List<CompareResult>();
    }
}
