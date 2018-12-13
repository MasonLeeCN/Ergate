using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    public class ServiceInfo
    {
        public string ServiceName { get; set; }

        public string Title { get; set; }

        public ApiVersion Version { get; set; } = new ApiVersion(1, 0);

        public string XmlName { get; set; }
    }
}
