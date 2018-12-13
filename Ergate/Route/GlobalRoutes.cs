using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ergate
{
    public class GlobalRoutes : IApplicationModelConvention
    {
        private readonly AttributeRouteModel prefix;

        public GlobalRoutes(IRouteTemplateProvider routeTemplateProvider)
        {
            prefix = new AttributeRouteModel(routeTemplateProvider);
            prefix.Template += "/[controller]";
        }

        //接口的Apply方法
        public void Apply(ApplicationModel application)
        {
            //遍历所有的 Controller
            foreach (var controller in application.Controllers)
            {
                var items = controller.Selectors.ToList();
                if (items.Any())
                {
                    foreach (var model in items)
                    {
                        model.AttributeRouteModel = prefix;
                    }
                }
            }
        }
    }
}
