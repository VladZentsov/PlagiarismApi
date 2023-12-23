using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.CoreModels
{
    public class CompareWorksResult
    {
        public List<IdenticalLines> IdenticalLines { get; set; }
        public ParsedWork CurrentWork { get; set; }
        public ParsedWork WorkToCompare { get; set; }
        public double MatchPercentage { get; set; }
    }
}
