﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.PMS.Integration.Data.Contract.DTO
{
    public class EmployeeDTO
    {
        public String Name { get; set; }

        public String Family { get; set; }

        public String PersonnelCode { get; set; }

        public long OrganID { get; set; }
    }
}
