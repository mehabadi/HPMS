﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.PMS.Integration.Data.Contract.DTO
{
    public class UnitIntegrationDTO
    {
        public int ID { get; set; }

        public String UnitName { get; set; }

        public long? ParentID { get; set; }

        public Guid TransferId { get; set; }


    }
}
