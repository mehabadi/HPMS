﻿using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Data.NH;
using MITD.Domain.Repository;
using MITD.PMS.Common;
using MITD.PMSAdmin.Domain.Model.CustomFieldTypes;
using MITD.PMSAdmin.Domain.Model.Jobs;
using MITD.PMSAdmin.Domain.Model.JobIndices;
using NHibernate.Linq;

namespace MITD.PMSAdmin.Persistence.NH
{
    public class CustomFieldRepository : NHRepository, ICustomFieldRepository
    {
        private NHRepository<CustomFieldType> rep; 

        public CustomFieldRepository(NHUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            init();
        }

        public CustomFieldRepository(IUnitOfWorkScope unitOfWorkScope)
            : base(unitOfWorkScope)
        {
            init();
        }

        private void init()
        {
            rep =  new NHRepository<CustomFieldType>(unitOfWork);
        }

        public CustomFieldTypeId GetNextId()
        {
            using (var ctx=Session.SessionFactory.OpenStatelessSession())
            {
                ctx.BeginTransaction();
                var res = ctx.CreateSQLQuery("Select next value for dbo.CustomFieldTypeSeq").UniqueResult<long>();
                return new CustomFieldTypeId(res);
            }
        }

        public void FindBy(EntityTypeEnum entityId, ListFetchStrategy<CustomFieldType> fs)
        {
            if (entityId == null)
                rep.GetAll(fs);
            else
                rep.Find(c => Equals(c.EntityId, entityId), fs);

        }

        public void Add(CustomFieldType customFieldType)
        {
            rep.Add(customFieldType);
        }

        public void UpdateCustomFieldType(CustomFieldType customFieldType)
        {
           rep.Update(customFieldType);
        }

        public CustomFieldType GetById(CustomFieldTypeId customFieldTypeId)
        {
            return rep.FindByKey(customFieldTypeId);
            
        }

        public List<CustomFieldType> GetAllCustomField(AbstractJobIndexId jobIndexId)
        {
            var jobIndex = Session.Query<JobIndex>().Single(j => j.Id == jobIndexId);
            return Session.Query<CustomFieldType>().Where(c => jobIndex.CustomFieldTypeIdList.Contains(c.Id)).ToList();
        }

        public List<CustomFieldType> GetAllCustomField(JobId jobId)
        {
            var job = Session.Query<Job>().Single(j => j.Id == jobId);
            return Session.Query<CustomFieldType>().Where(c => job.CustomFieldTypeIdList.Contains(c.Id)).ToList();

           
        }

        public void GetAll(EntityTypeEnum entityType, ListFetchStrategy<CustomFieldType> fs)
        {
            if (entityType == null)
                rep.GetAll(fs);
            else
                rep.Find(c => c.EntityId==entityType, fs);
        }

        public List<CustomFieldType> GetAll(EntityTypeEnum entityType)
        {
           return rep.Find(c => c.EntityId == entityType).ToList();
        }

        public IList<CustomFieldType> Find(System.Linq.Expressions.Expression<Func<CustomFieldType, bool>> predicate)
        {
            return rep.Find(predicate);
        }

        public void DeleteCustomField(CustomFieldType customField)
        {
            rep.Delete(customField);
        }


        public CustomFieldTypeException ConvertException(Exception exp)
        {
            string typeName = "";
            string keyName = "";
            if (NHExceptionDetector.IsDublicateException(exp, out typeName, out keyName))
                return new CustomFieldTypeDuplicateException("CustomFieldType", "DictionaryName");
            if (NHExceptionDetector.IsDeleteHasRelatedDataException(exp, out typeName, out keyName))
                return new CustomFieldTypeDeleteException("CustomFieldType", "Id");
            throw new Exception();
        }

        public CustomFieldTypeException TryConvertException(Exception exp)
        {
            CustomFieldTypeException res = null;
            try
            {
                res = ConvertException(exp);
            }
            catch (Exception e)
            {

            }
            return res;
        }
    }
}
