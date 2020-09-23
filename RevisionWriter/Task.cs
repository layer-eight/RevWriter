using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevisionWriter
{
    public class Task : IComparable<Task>
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string ProjectID { get; set; }
        public string Duration { get; set; }

        public int CompareTo(Task other)
        {
            return this.Date.CompareTo(other.Date);
        }
    }
}
