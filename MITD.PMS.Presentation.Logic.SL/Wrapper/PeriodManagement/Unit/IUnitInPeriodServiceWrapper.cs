﻿using System.Collections.Generic;
using MITD.Presentation;
using MITD.PMS.Presentation.Contracts;
using System;

namespace MITD.PMS.Presentation.Logic
{
    public interface IUnitInPeriodServiceWrapper : IServiceWrapper
    {
        void GetAllUnitInPeriod(Action<IList<UnitInPeriodDTO>, Exception> action, long periodId);
        void GetUnitInPeriod(Action<UnitInPeriodDTO, Exception> action, long periodId, long unitId);
        void AddUnitInPeriod(Action<UnitInPeriodAssignmentDTO, Exception> action, UnitInPeriodAssignmentDTO jobPositionInPeriod);
        void AddUnitInPeriod(Action<JobInPeriodDTO, Exception> action, long periodId, UnitInPeriodDTO unitInPeriodDto);

        void UpdateUnitInPeriod(Action<UnitInPeriodDTO, Exception> action, long periodId,
            UnitInPeriodDTO unitInPeriodDto);
        void DeleteUnitInPeriod(Action<string, Exception> action, long periodId, long jobPositionId);
        void GetAllUnits(Action<List<UnitInPeriodDTO>,Exception> action,long periodId);
        void GetUnitsWithActions(Action<List<UnitInPeriodDTOWithActions>, Exception> action, long periodId);
    }
}
