using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services.Implementations
{
    public class CourseService : ICourseService
    {
        #region Constructor
        private readonly TopLearnContext _context;
        public CourseService(TopLearnContext context)
        {
            _context = context;
        }
        #endregion
        public List<CourseGroup> GetAllGroup()
        {
            return _context.CourseGroups.ToList();
        }
    }
}
