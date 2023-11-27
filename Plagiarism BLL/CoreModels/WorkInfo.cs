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
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid WorkId { get; set; }
        public virtual Work Work { get; set; }
        public WorkType WorkType { get; set; }
        public string WorkName { get; set; }
    }
}
