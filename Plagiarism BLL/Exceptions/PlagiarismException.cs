using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.Exceptions
{
    public class PlagiarismException: Exception
    {
        public PlagiarismException() { }

        public PlagiarismException(string message) : base(message) { }

        public PlagiarismException(string message, Exception innerException) : base(message, innerException) { }
    }
}
