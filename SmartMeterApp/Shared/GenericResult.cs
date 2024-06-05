using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class GenericResult
    {
        public GenericResult()
        {
            Errors = new List<string>();
        }

        public bool Success
        {
            get { return !Errors.Any(); }
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public object Value { get; set; }

        public List<string> Errors { get; set; }
    }
}
