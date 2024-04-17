﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nightingale
{
    class StudentGraduatedFactory:IFactory
    {
        public Nightingale CreateNightingale()
        {
            return new StudentGraduated();
        }
    }
}