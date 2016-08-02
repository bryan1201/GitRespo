﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeDB
{
    interface IFactory
    {
        IUser CreateUser();
        IDept CreateDept();
    }
}
