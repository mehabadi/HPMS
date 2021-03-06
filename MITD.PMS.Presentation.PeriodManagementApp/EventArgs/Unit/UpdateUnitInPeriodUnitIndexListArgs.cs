﻿ using MITD.PMS.Presentation.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MITD.PMS.Presentation.PeriodManagementApp
{
    public class UpdateUnitInPeriodUnitIndexListArgs
    {
        public UpdateUnitInPeriodUnitIndexListArgs(List<UnitInPeriodUnitIndexDTO> unitInPeriodUnitIndices, long periodId, long unitId)
        {
            this.UnitInPeriodUnitIndices =unitInPeriodUnitIndices;
            PeriodId = periodId;
            this.UnitId = unitId;
        }
        public List<UnitInPeriodUnitIndexDTO> UnitInPeriodUnitIndices
        {
            get; private set; 
        }

        public long PeriodId
        {
            get;
            private set;
        }

        public long UnitId
        {
            get;
            private set;
        }
    }
}
