﻿using System;
using MITD.PMS.Presentation.Contracts;
using MITD.Presentation;

namespace MITD.PMS.Integration.PMS.Contract
{
    public partial interface IUnitIndexServiceWrapper : IServiceWrapper
    {
        #region Not Use
        //void GetUnitIndexCategory(Action<UnitIndexCategoryDTO, Exception> action, long id);
        //void AddUnitIndexCategory(Action<UnitIndexCategoryDTO, Exception> action, UnitIndexCategoryDTO unit);
        //void UpdateUnitIndexCategory(Action<UnitIndexCategoryDTO, Exception> action, UnitIndexCategoryDTO unit);
        //void GetAllUnitIndexGroup(Action<PageResultDTO<UnitIndexCategoryDTOWithActions>, Exception> action, int pageSize, int pageIndex);
        //void DeleteUnitIndexCategory(Action<string, Exception> action, long id); 
        #endregion
    }
}
