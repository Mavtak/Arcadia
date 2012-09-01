﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class IdGenerator
    {
        private int lastIdUsed = 0;
        public IdGenerator()
        {
        }
        public int NextInt()
        {
            lock (this)
            {
                return ++lastIdUsed;
            }
        }
        public string NextString()
        {
            lock (this)
            {
                return (++lastIdUsed).ToString();
            }
        }
    }
}
