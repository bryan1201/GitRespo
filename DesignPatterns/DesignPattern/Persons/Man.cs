﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persons
{
    class Man : IPerson
    {
        public void Accept(IAction visitor)
        {
            try
            {
                visitor.GetManConclusion(this);

            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
