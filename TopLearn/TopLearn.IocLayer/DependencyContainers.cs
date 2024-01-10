using Microsoft.Extensions.DependencyInjection;
using TopLearn.Core.Services.Implementations;
using TopLearn.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Convertors;
using TopLearn.Core.Services;

namespace TopLearn.IocLayer
{
    public class DependencyContainers
    {
        public static void RegisterServicse(IServiceCollection service)
        {
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IViewRenderService, RenderViewToString>();
            service.AddScoped<IPermissionService, PermissionService>();
            service.AddTransient<ICourseService, CourseService>();
        }
    }
}
