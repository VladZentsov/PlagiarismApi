using Plagiarism_BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.CoreModels
{
    public class WorkInfo: BaseModel
    {
        public int UserId { get; set; }
        public int WorkId { get; set; }
        public WorkType WorkType { get; set; }
    }
}
