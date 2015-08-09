﻿using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Core;
using MITD.PMS.Application.Contracts;
using MITD.PMS.Domain.Model.Employees;
using MITD.PMS.Domain.Model.Periods;
using MITD.PMS.Domain.Model.UnitIndices;
using MITD.PMS.Domain.Model.Units;
using MITD.PMS.Presentation.Contracts;

namespace MITD.PMS.Interface
{
    //[Interceptor(typeof(Interception))]
    public class PeriodUnitServiceFacade : IPeriodUnitServiceFacade
    {
        private readonly IUnitService unitService;
        private readonly IUnitIndexService unitIndexService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper<Unit, UnitInPeriodAssignmentDTO> unitAssignmentMapper;

        private readonly IFilterMapper<Unit, UnitInPeriodDTOWithActions> unitInPeriodDTOWithActionsMapper;
        private readonly IMapper<UnitCustomField, CustomFieldDTO> unitCustomFieldMapper;
        private readonly IFilterMapper<Unit, UnitInPeriodDTO> unitInPeriodDTOMapper;
        private readonly IUnitRepository unitRep;



        public PeriodUnitServiceFacade(IUnitService unitService,
            IMapper<Unit,UnitInPeriodAssignmentDTO> unitAssignmentMapper,
            IFilterMapper<Unit, UnitInPeriodDTOWithActions> unitInPeriodDTOWithActionsMapper,
            IFilterMapper<Unit, UnitInPeriodDTO> unitInPeriodDTOMapper,
            IMapper<UnitCustomField, CustomFieldDTO> unitCustomFieldMapper,
            IUnitRepository unitRep,
            IUnitIndexService unitIndexService,
            IEmployeeRepository employeeRepository)
        {
            this.unitService = unitService;
            this.unitAssignmentMapper = unitAssignmentMapper;
            this.unitInPeriodDTOWithActionsMapper = unitInPeriodDTOWithActionsMapper;
            this.unitInPeriodDTOMapper = unitInPeriodDTOMapper;
            this.unitRep = unitRep;
            this.unitCustomFieldMapper = unitCustomFieldMapper;
            this.unitIndexService = unitIndexService;
            _employeeRepository = employeeRepository;
        }


        public UnitInPeriodDTO AssignUnit(long periodId, UnitInPeriodDTO unitInPeriod)
        {
            var unit = unitService.AssignUnit(
                 new UnitId(new PeriodId(periodId), new SharedUnitId(unitInPeriod.ParentId.Value)),
                new UnitId(new PeriodId(periodId), new SharedUnitId(unitInPeriod.UnitId)),
                unitInPeriod.CustomFields.Select(c => new SharedUnitCustomFieldId(c.Id)).ToList(),
                unitInPeriod.UnitIndices.Select(c => new UnitIndexForUnit(new AbstractUnitIndexId(c.Id), c.ShowforTopLevel, c.ShowforSameLevel, c.ShowforLowLevel)).ToList()

                          );
            return unitInPeriodDTOMapper.MapToModel(unit, new string[] { });
    
        }

        public string RemoveUnit(long periodId, long unitId)
        {
            unitService.RemoveUnit(new PeriodId(periodId), new SharedUnitId(unitId));
            return "Unit with Id " + unitId + " removed";
        }

        public IEnumerable<UnitInPeriodDTOWithActions> GetUnitsWithActions(long periodId)
        {
            var units=unitRep.GetUnits(new PeriodId(periodId));
            return units.Select(u => unitInPeriodDTOWithActionsMapper.MapToModel(u,new string[]{}));
        }

        public IEnumerable<UnitInPeriodDTO> GetUnits(long periodId)
        {
            var units = unitRep.GetUnits(new PeriodId(periodId));
            return units.Select(u => unitInPeriodDTOMapper.MapToModel(u,new string[]{}));
        }

        public UnitInPeriodDTO GetUnit(long periodId, long unitId, string selectedColumns)
        {
            var unit = unitRep.GetBy(new UnitId(new PeriodId(periodId), new SharedUnitId(unitId)));
            var unitDto = unitInPeriodDTOMapper.MapToModel(unit, selectedColumns.Split(','));
         //   unitDto.CustomFields = unit.CustomFields.Select(c => unitCustomFieldMapper.MapToModel(c)).ToList();
          //  var unitindexIdList = unit.UnitIndexList.Select(j => j.UnitIndexId).ToList();
           // var unitIndices = unitIndexService.FindUnitIndices(index => unitindexIdList.Contains(index.Id));
            //todo change this mapping to valid mapping need som work !!!!!!
            //var unitInPeriodUnitIndexDTOList = new List<UnitInPeriodUnitIndexDTO>();
            //foreach (var unitIndex in unitIndices)
            //{
            //    var unitUnitIndex = unit.UnitIndexList.Single(j => j.UnitIndexId == unitIndex.Id);
            //    unitInPeriodUnitIndexDTOList.Add(new UnitInPeriodUnitIndexDTO
            //    {
            //        Id = unitIndex.Id.Id,
            //        IsInquireable = unitIndex.IsInquireable,
            //        Name = unitIndex.Name,
            //        ShowforTopLevel = unitUnitIndex.ShowforTopLevel,
            //        ShowforSameLevel = unitUnitIndex.ShowforSameLevel,
            //        ShowforLowLevel = unitUnitIndex.ShowforLowLevel
            //    });
            //}
            //unitDto.UnitIndices = unitInPeriodUnitIndexDTOList;
            return unitDto;


        }

        public PageResultDTO<UnitInPeriodDTOWithActions> GetAllUnits(long periodId, int pageSize, int pageIndex, QueryStringConditions queryStringConditions, string selectedColumns)
        {
            var result = GetAllUnitWithActions(periodId, "");
            var pResDto = new PageResultDTO<UnitInPeriodDTOWithActions>
            {
                CurrentPage = 0,
                PageSize = 10,
                Result = result,
                TotalCount = result.Count,
                TotalPages = 1
            };
            return pResDto;
        }

        public List<UnitInPeriodDTOWithActions> GetAllUnitWithActions(long periodId, string selectedColumns)
        {
            var res = unitRep.GetUnits(new PeriodId(periodId));
            return res.Select(r => unitInPeriodDTOWithActionsMapper.MapToModel(r, selectedColumns.Split(','))).ToList();
        }


        public UnitInPeriodDTO UpdateUnit(long periodId, UnitInPeriodDTO unitInPeriod)
        {
            var unit = unitService.UpdateUnit(new UnitId(new PeriodId(periodId), new SharedUnitId(unitInPeriod.UnitId)),
                unitInPeriod.CustomFields.Select(c => new SharedUnitCustomFieldId(c.Id)).ToList(),
                unitInPeriod.UnitIndices.Select(c => new UnitIndexForUnit(new AbstractUnitIndexId(c.Id), c.ShowforTopLevel, c.ShowforSameLevel, c.ShowforLowLevel)).ToList()
                );
            return unitInPeriodDTOMapper.MapToModel(unit, new string[] { });
        }


        public List<InquirySubjectWithInquirersDTO> GetInquirySubjectsWithInquirers(long periodId, long unitId)
        {
            var inquirySubjectWIthInquirersList = new List<InquirySubjectWithInquirersDTO>();
            var configurationItems =
                unitService.GetInquirySubjectWithInquirer(new UnitId(new PeriodId(periodId),
                    new SharedUnitId(unitId)));
            var inquirySubjectWithinquirers = configurationItems.GroupBy(c => c.Id.InquirySubjectId);
            foreach (var inquirySubjectWithinquirer in inquirySubjectWithinquirers)
            {
                var inquirySubject = _employeeRepository.GetBy(inquirySubjectWithinquirer.Key);
                var inquirySubjectInquirerDTO = new InquirySubjectWithInquirersDTO
                {
                    EmployeeName = inquirySubject.FullName,
                    EmployeeNo = inquirySubject.Id.EmployeeNo,
                };
                inquirySubjectInquirerDTO.CustomInquirers = new List<InquirerDTO>();
                inquirySubjectInquirerDTO.Inquirers = new List<InquirerDTO>();

                foreach (var itm in inquirySubjectWithinquirer)
                {
                    var inquirer = _employeeRepository.GetBy(itm.Id.InquirerId);
                    var inquirerJobPositionName = unitRep.GetBy(itm.Id.InquirerUnitId).Name;
                    if (itm.IsAutoGenerated)
                    {

                        inquirySubjectInquirerDTO.Inquirers.Add(new InquirerDTO
                        {
                            EmployeeNo = inquirer.Id.EmployeeNo,
                            FullName = inquirer.FullName,
                            IsPermitted = itm.IsPermitted,
                            EmployeeJobPositionId = itm.Id.InquirerUnitId.SharedUnitId.Id,
                            EmployeeJobPositionName = inquirerJobPositionName
                        });
                    }
                    else
                    {

                        inquirySubjectInquirerDTO.CustomInquirers.Add(new InquirerDTO
                        {
                            EmployeeNo = inquirer.Id.EmployeeNo,
                            FullName = inquirer.FullName,
                            EmployeeJobPositionId = itm.Id.InquirerUnitId.SharedUnitId.Id,
                            EmployeeJobPositionName = inquirerJobPositionName
                        });
                    }

                }

                inquirySubjectWIthInquirersList.Add(inquirySubjectInquirerDTO);


            }
            return inquirySubjectWIthInquirersList;

        }


        
    }
}
