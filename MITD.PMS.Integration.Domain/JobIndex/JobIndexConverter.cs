﻿using System;
using System.Collections.Generic;
using System.Linq;
using MITD.Core;
using MITD.PMS.Integration.Data.Contract.DataProvider;
using MITD.PMS.Integration.Data.Contract.DTO;
using MITD.PMS.Integration.PMS.Contract;
using MITD.PMS.Presentation.Contracts;
using MITD.PMS.Integration.Core;

namespace MITD.PMS.Integration.Domain
{
    public class JobIndexConverter : IJobIndexConverter
    {

        #region Fields

        private readonly IJobIndexDataProvider jobIndexDataProvider;
        private readonly IJobIndexServiceWrapper jobIndexService;
        private readonly IJobIndexInPeriodServiceWrapper jobIndexAssignmentService;
        private readonly IEventPublisher publisher;

        #endregion

        #region Constructors

        public JobIndexConverter(IJobIndexDataProvider jobIndexDataProvider,
            IJobIndexServiceWrapper jobIndexService, IJobIndexInPeriodServiceWrapper jobIndexAssignmentService, IEventPublisher publisher)
        {
            this.jobIndexDataProvider = jobIndexDataProvider;
            this.jobIndexService = jobIndexService;
            this.jobIndexAssignmentService = jobIndexAssignmentService;
            this.publisher = publisher;
        }

        #endregion

        #region Public Methods

        public void ConvertJobIndex(Period period)
        {
            Console.WriteLine("Starting job index convert progress...");

            var jobIndexInperiodList = new List<JobIndexInPeriodDTO>();
            var jobIndexList = new List<JobIndexDTO>();
            var sourceJobIndexListId = jobIndexDataProvider.GetJobIndexListId();
            foreach (var sourceJobIndexId in sourceJobIndexListId)
            {

                var sourceJobIndexDTO = jobIndexDataProvider.GetBy(sourceJobIndexId);
                var jobIndexWithCf = jobIndexService.GetByTransferId(sourceJobIndexDTO.TransferId);
                if (jobIndexWithCf==null)
                {
                    var desJobIndexDTO = createDestinationJobIndex(sourceJobIndexDTO);
                    var jobIndexWithOutCf = jobIndexService.AddJobIndex(desJobIndexDTO);
                    
                    jobIndexWithCf = jobIndexService.GetJobIndex(jobIndexWithOutCf.Id);
                }
                jobIndexList.Add(jobIndexWithCf);
                var periodJobIndexDTO = createPeriodJobIndexDTO(jobIndexWithCf, period, sourceJobIndexDTO);
                periodJobIndexDTO.CustomFields[0].Value = sourceJobIndexDTO.Coefficient.ToString();
                var res = jobIndexAssignmentService.AddJobIndexInPeriod(periodJobIndexDTO);
                jobIndexInperiodList.Add(res);
                Console.WriteLine("Job index convert progress state: " + jobIndexInperiodList.Count + " From " + sourceJobIndexListId.Count);

            }
            publisher.Publish(new JobIndexConverted(jobIndexInperiodList,jobIndexList));
        }

        #endregion

        #region Private Methods

        private JobIndexDTO createDestinationJobIndex(JobIndexIntegrationDTO sourceJobIndex)
        {
            var res = new JobIndexDTO
                      {
                          Name = sourceJobIndex.Title,
                          ParentId = PMSCostantData.JobIndexCategoryId,
                          CustomFields = new List<CustomFieldDTO>
                                         {
                                             new CustomFieldDTO
                                             {
                                                 Id = PMSCostantData.JobIndexFieldId,
                                                 Name = "اهمیت",
                                                 DictionaryName = "JobIndexCustomFieldCoefficient ",
                                                 EntityId = 1,
                                                 MaxValue = 10,
                                                 MinValue = 1,
                                                 TypeId = "string",
                                             }
                                         },
                          DictionaryName = sourceJobIndex.TransferId.ToString(),
                          TransferId = sourceJobIndex.TransferId
                      };
            return res;
        }

        private JobIndexInPeriodDTO createPeriodJobIndexDTO(JobIndexDTO jobIndex, Period period,
            JobIndexIntegrationDTO sourceJobIndexDTO)
        {
            var res = new JobIndexInPeriodDTO
                      {
                          //todo: Kharabe

                          CalculationOrder = 1,
                          IsInquireable = true,
                          Name = jobIndex.Name,
                          DictionaryName = jobIndex.DictionaryName,
                          JobIndexId = jobIndex.Id,
                          PeriodId = period.Id,
                          CustomFields = jobIndex.CustomFields.Select(c => new AbstractCustomFieldDescriptionDTO
                                                                           {
                                                                               Id = c.Id,
                                                                               Name = "fake",
                                                                               Value = "1"
                                                                           }).ToList()

                      };
            if (sourceJobIndexDTO.IndexType == PMSIntegrationCoreConstantData.IntegrationBehaviaralJobIndexId)
            {
                res.ParentId = PMSCostantData.JobIndexGroupBehaviaral;
                res.CalculationLevel = 1;
            }
            else if (sourceJobIndexDTO.IndexType == PMSIntegrationCoreConstantData.IntegrationPerformanceJobIndexId)
            {
                res.ParentId = PMSCostantData.JobIndexGroupPerformance;
                res.CalculationLevel = 2;
            }
            else
            {
                throw new Exception("Invalid index type in source job index (employee management system)");
            }
            return res;
        }

        private void handleException(Exception exception)
        {
            throw new Exception("bad shod", exception);
        }
        #endregion

    }
}
