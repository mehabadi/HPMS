﻿
using System.Collections.Generic;
using MITD.Core;
using MITD.PMS.Domain.Model.Employees;
using MITD.PMS.Domain.Model.Periods;
using MITD.PMS.Domain.Model.Units;


namespace MITD.PMS.Application.Contracts
{
    public interface IUnitService : IService
    {

        void RemoveInquirer(PeriodId periodId, SharedUnitId unitId, EmployeeId employeeId);
        List<UnitInquiryConfigurationItem> GetInquirySubjectWithInquirer(UnitId unitId);
        void UpdateInquirers(EmployeeId inquirySubjectEmployeeId, UnitId unitId,long unitIndexInPeiodUnit);
        Unit UpdateUnit(UnitId unitId, List<SharedUnitCustomFieldId> customFieldIdList, IList<UnitIndexForUnit> unitIndexList);
   //     Unit AssignUnit(UnitId unitId, List<SharedUnitCustomFieldId> customFieldIdList, IList<UnitIndexForUnit> unitIndexList);

        Unit AssignUnit(UnitId parentId, UnitId unitId, List<SharedUnitCustomFieldId> customFieldIdList,
            IList<UnitIndexForUnit> unitIndexList);
        Unit AssignUnit(PeriodId periodId, SharedUnitId sharedUnitId, SharedUnitId parentSharedUnitId);
        void RemoveUnit(PeriodId periodId, SharedUnitId sharedUnitId);
        List<Unit> GetAllParentUnits(Period period);
        List<Unit> GetAllUnitByParentId(UnitId id);
        UnitId GetUnitIdBy(Period period, SharedUnitId sharedUnitId);
        Unit GetUnitBy(UnitId unitId);
        List<UnitId> GetAllUnitId(Period period);
        void ManageVerifiers(EmployeeId employeeId, UnitId unitId);
        void RemoveVerifiers(EmployeeId employeeId, UnitId unitId);
    }
}
