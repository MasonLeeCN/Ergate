using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    public abstract class BaseEvent
    {
        public Guid RequestId { get; } = Guid.NewGuid();

        public DateTime CurrentTime { get; } = DateTime.Now;
    }
}
