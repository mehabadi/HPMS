﻿using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using MITD.Core;
using MITD.Domain.Repository;
using MITD.PMS.Presentation.Contracts;
using MITD.PMSAdmin.Application.Contracts;
using MITD.PMSAdmin.Domain.Model.CustomFieldTypes;
using MITD.PMSSecurity.Domain;
using Omu.ValueInjecter;

namespace MITD.PMS.Interface
{

    [Interceptor(typeof(Interception))]
    public class CustomFieldFacadeService : ICustomFieldFacadeService
    {
        #region Fields

        private readonly IMapper<CustomFieldType, CustomFieldDTOWithActions> customFieldWithActionMapper;
        private readonly IMapper<CustomFieldType, CustomFieldDTO> customFieldMapper;
        private readonly IMapper<CustomFieldType, AbstractCustomFieldDescriptionDTO> abstractCustomFieldDescriptionDTOMapper;
        private readonly ICustomFieldService customFieldService;
        private readonly ICustomFieldRepository customFieldRep;

        #endregion

        #region Constructors
        public CustomFieldFacadeService(IMapper<CustomFieldType, CustomFieldDTOWithActions> customFieldWithActionMapper,
                                        IMapper<CustomFieldType, CustomFieldDTO> customFieldMapper,
            //IMapper<CustomFieldType, CustomFieldDTO> abstractCustomFieldDTOMapper,
                                        IMapper<CustomFieldType, AbstractCustomFieldDescriptionDTO> abstractCustomFieldDescriptionDTOMapper,
                                        ICustomFieldService customFieldService,
                                        ICustomFieldRepository customFieldRep)
        {
            this.customFieldWithActionMapper = customFieldWithActionMapper;
            this.customFieldMapper = customFieldMapper;
            this.abstractCustomFieldDescriptionDTOMapper = abstractCustomFieldDescriptionDTOMapper;
            this.customFieldService = customFieldService;
            this.customFieldRep = customFieldRep;

        }
        #endregion

        #region Public methods

        [RequiredPermission(ActionType.ManageCustomFields)]
        public PageResultDTO<CustomFieldDTOWithActions> GetAllCustomFieldes(int pageSize, int pageIndex, QueryStringConditions queryStringConditions)
        {

            var fs = new ListFetchStrategy<CustomFieldType>(Enums.FetchInUnitOfWorkOption.NoTracking);
            var sortBy = QueryConditionHelper.GetSortByDictionary(queryStringConditions.SortBy);
            foreach (var item in sortBy)
            {
                fs.SortCriterias.Add(new StringSortCriteria<CustomFieldType>(item.Key,
                                                                             (item.Value.ToUpper() == "ASC")
                                                                                 ? Enums.SortOrder.Ascending
                                                                               : Enums.SortOrder.Descending));
            }
            fs.OrderBy(x => x.Id);
            fs.WithPaging(pageSize, pageIndex);
            var entityId = getEntityIdFromQueryString(queryStringConditions.Filter);
            customFieldRep.GetAll(
                entityId != null ? Enumeration.FromValue<EntityTypeEnum>(entityId.ToString()) : null, fs);
            var res = new PageResultDTO<CustomFieldDTOWithActions>();
            res.InjectFrom(fs.PageCriteria.PageResult);
            res.Result = fs.PageCriteria.PageResult.Result.Select(r => customFieldWithActionMapper.MapToModel(r)).ToList();
            return res;
        }

        [RequiredPermission(ActionType.CreateCustomField)]
        public CustomFieldDTO AddCustomField(CustomFieldDTO customField)
        {
            var res = customFieldService.AddCustomFieldType(customField.Name, customField.DictionaryName,
                                                                        customField.MinValue, customField.MaxValue,
                                                                        customField.EntityId, customField.TypeId);
            return customFieldMapper.MapToModel(res);
        }

        [RequiredPermission(ActionType.ModifyCustomField)]
        public CustomFieldDTO UpdateCustomField(CustomFieldDTO customField)
        {
            var res = customFieldService.UpdateCustomFieldType(new CustomFieldTypeId(customField.Id), customField.Name,
                customField.DictionaryName, customField.MinValue, customField.MaxValue, customField.EntityId,
                customField.TypeId);
            return customFieldMapper.MapToModel(res);
        }

        [RequiredPermission(ActionType.DeleteCustomField)]
        public string DeleteCustomeField(long id)
        {
            var customFieldType = customFieldRep.GetById(new CustomFieldTypeId(id));
            customFieldService.DeleteCustomField(customFieldType);
            return "customField deleted";
        }

        //[RequiredPermission(ActionType.ShowCustomField)]
        public CustomFieldDTO GetCustomFieldById(long id)
        {
            var customFieldType = customFieldRep.GetById(new CustomFieldTypeId(id));
            return customFieldMapper.MapToModel(customFieldType);
        }

        //[RequiredPermission(ActionType.ShowCustomField)]
        public List<CustomFieldEntity> GetAllCustomFieldEntityType()
        {
            var res = new List<CustomFieldEntity>();
            foreach (var entityTypeEnum in Enumeration.GetAll<EntityTypeEnum>())
            {
                res.Add(new CustomFieldEntity
                    {
                        Id = Convert.ToInt32(entityTypeEnum.Value),
                        Title = entityTypeEnum.DisplayName
                    });
            }
            return res;
        }

        //[RequiredPermission(ActionType.ShowCustomField)]
        public List<CustomFieldDTO> GetAllCustomFields(string entityType)
        {
            var entityCustomFieldList = customFieldRep.GetAll(Enumeration.FromDisplayName<EntityTypeEnum>(entityType));
            return
                entityCustomFieldList.Select(c => customFieldMapper.MapToModel(c))
                  .ToList();
        }

        //[RequiredPermission(ActionType.ShowCustomField)]
        public List<AbstractCustomFieldDescriptionDTO> GetAllCustomFieldsDescription(string entityType)
        {
            //var fs = new ListFetchStrategy<CustomFieldType>(Enums.FetchInUnitOfWorkOption.NoTracking);
            var entityCustomFieldList = customFieldRep.GetAll(Enumeration.FromDisplayName<EntityTypeEnum>(entityType));
            return
                entityCustomFieldList.Select(c => abstractCustomFieldDescriptionDTOMapper.MapToModel(c))
                  .ToList();
        }

        #endregion

        #region Private methods
        private static int? getEntityIdFromQueryString(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return null;
            var res = filter.Split('=');
            return Convert.ToInt32(res[1]);
        }
        #endregion
    }
}
