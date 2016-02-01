﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.PMS.Integration.Data.Contract.DataProvider;
using MITD.PMS.Integration.Data.Contract.DTO;
using MITD.PMS.Integration.Data.EF.DBModel;

namespace MITD.PMS.Integration.Data.EF
{
    public class JobIndexDataProvider:IJobIndexDataProvider
    {

        private PersonnelSoft2005Entities DB = new PersonnelSoft2005Entities();


        public List<GeneralJobIndexDto> GetGeneralIndexes()
        {
            IList<long> ids = GetGeneralJobIndexIds();
            List<GeneralJobIndexDto> GeneralIndexesList = new List<GeneralJobIndexDto>();

            foreach (var item in ids)
            {
                try
                {
                    var TempGeneralIndex = (from c in DB.PMS_GeneralIndex where c.ID == item select c).FirstOrDefault();

                    GeneralJobIndexDto GeneralIndex = new GeneralJobIndexDto();
                    GeneralIndex.IndexTitle = TempGeneralIndex.Title;
                    GeneralIndex.IndexTypeID = TempGeneralIndex.ID_IndexType;
                    GeneralIndex.Description = TempGeneralIndex.Description;

                    GeneralIndexesList.Add(GeneralIndex);

                }
                catch (Exception e)
                {                    
                    throw e;
                }
            }

            return GeneralIndexesList;

        }

        private IList<long> GetGeneralJobIndexIds()
        {
            try
            {
                var ids = (from c in DB.PMS_GeneralIndex where c.IsActive == true select c.ID).ToList();
                return ids;
            }
            catch (Exception e)
            {

                throw e;
            }          
            
        }



        
        public List<ExclusiveJobIndexDto> GetExclusiveJobIndexes()
        {
            IJobDataProvider JobService = new JobDataProvider();

            var JobIds = JobService.GetJobIds();

            IList<long> ids = GetExclusiveJobIndexIds();
            List<ExclusiveJobIndexDto> ExclusiveIndexesList = new List<ExclusiveJobIndexDto>();

            foreach (var item in ids)
            {
                try
                {
                    var TempExclusiveIndex = (from c in DB.PMS_JobIndex where c.ID == item && c.IsActive == true select c).FirstOrDefault();

                    ExclusiveJobIndexDto ExclusiveIndex = new ExclusiveJobIndexDto();
                    ExclusiveIndex.IndexTitle = TempExclusiveIndex.Title;
                    ExclusiveIndex.IndexTypeID = TempExclusiveIndex.ID_IndexType;
                    ExclusiveIndex.Description = TempExclusiveIndex.Discription;
                    ExclusiveIndex.JobID = TempExclusiveIndex.ID_Job;



                    ExclusiveIndexesList.Add(ExclusiveIndex);

                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return ExclusiveIndexesList;
        }

        private IList<long> GetExclusiveJobIndexIds()
        {
            try
            {
                var ids = (from c in DB.PMS_JobIndex where c.IsActive == true select c.ID).ToList();
                return ids;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        
    }
}
