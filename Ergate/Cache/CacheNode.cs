﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate.Cache
{
    public class CacheNode<T>
    {
        public string Key { get; set; }

        public T Data { get; set; }

        public TimeSpan CacheTime { get; set; }
    }
}
