﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Application.SI.Extension.Identity
{
    public class ApplicationUser : IdentityUser   
    {
        public string Name { get; set; }
    }
}
