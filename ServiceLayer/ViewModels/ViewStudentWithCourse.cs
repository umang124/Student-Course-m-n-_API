using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ViewModels
{
    public class ViewStudentWithCourse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<int> CourseIds { get; set; }
        public List<string> CourseNames { get; set; }

    }
}
