﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SI.DTO.Response
{
    public class BaseUserClaimsDTO
    {

        public string UserId { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public bool ManagerUser {  get; set; }
        public bool Read {  get; set; }
        public bool Delete { get; set; }
        public bool Update { get; set; }
        public bool Create { get; set; }
    }
}
