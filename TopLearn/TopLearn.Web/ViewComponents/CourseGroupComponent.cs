using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.ViewComponents
{
    public class CourseGroupComponent : ViewComponent
    {
        #region Constructor
        private readonly ICourseService _courseService;

        public CourseGroupComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("CourseGroup" , _courseService.GetAllGroup()));
        }
    }
}
