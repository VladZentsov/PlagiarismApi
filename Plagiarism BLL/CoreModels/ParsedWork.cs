using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.CoreModels
{
    public class ParsedWork:BaseModel
    {
        public string[] CodeLines { get; set; }
    }
}
