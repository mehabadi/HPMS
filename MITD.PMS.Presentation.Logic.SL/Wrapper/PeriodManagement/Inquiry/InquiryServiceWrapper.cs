﻿using System;
using MITD.PMS.Presentation.Contracts;
using System.Collections.Generic;
using MITD.Presentation;


namespace MITD.PMS.Presentation.Logic.Wrapper
{
    public class InquiryServiceWrapper : IInquiryServiceWrapper
    {
        private readonly IUserProvider userProvider;
        private readonly string baseAddress = PMSClientConfig.BaseApiAddress;

        public InquiryServiceWrapper(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        //By employeee
        private string makeInquirerInquirySubjectApiAdress(long periodId, string inquirerEmployeeNo)
        {
            return "Periods/" + periodId + "/Inquirers/" + inquirerEmployeeNo + "/InquirySubjects";
        }

        //By index
        private string makeInquirerInquiryIndexApiAdress(long periodId, string inquirerEmployeeNo)
        {
            return "Periods/" + periodId + "/Inquirers/" + inquirerEmployeeNo + "/InquiryIndices";
        }

        private string makeInquirySubjectJobIndexPointApiAdress(long periodId, string inquirerEmployeeNo,
            string inquirySubjectEmployeeNo, long jobPositionId)
        {
            return "Periods/" + periodId + "/Inquirers/" + inquirerEmployeeNo + "/InquirySubjects/" +
                   inquirySubjectEmployeeNo + "/JobPositions/" + jobPositionId + "/InquiryJobIndexPoints";
        }

        private string makeJobIndexPointByIndexApiAdress(long periodId, string inquirerEmployeeNo, long jobIndexId)
        {
            return "Periods/" + periodId + "/Inquirers/" + inquirerEmployeeNo + "/InquiryIndices/" +
                   jobIndexId + "/InquiryJobIndexPoints";
        }


        //By employeee
        public void GetInquirerInquirySubjects(Action<List<InquirySubjectDTO>, Exception> action, long periodId, string inquirerEmployeeNo)
        {
            if (periodId == -1)
            {
                action(new List<InquirySubjectDTO>(), null);
            }

            var url = string.Format(baseAddress + makeInquirerInquirySubjectApiAdress(periodId, inquirerEmployeeNo));
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }

        //By index
        public void GetInquirerInquiryIndices(Action<List<InquiryIndexDTO>, Exception> action, long periodId, string inquirerEmployeeNo)
        {
            if (periodId == -1)
            {
                action(new List<InquiryIndexDTO>(), null);
            }

            var url = string.Format(baseAddress + makeInquirerInquiryIndexApiAdress(periodId, inquirerEmployeeNo));
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void GetInquiryForm(Action<InquiryFormDTO, Exception> action, long periodId, string inquirerEmployeeNo, long inquirerJobPositionId, string inquirySubjectEmployeeNo, long jobPositionId)
        {
            var url = string.Format(baseAddress + makeInquirySubjectJobIndexPointApiAdress(periodId, inquirerEmployeeNo, inquirySubjectEmployeeNo, jobPositionId) + "?InquirerJobPositionId=" + inquirerJobPositionId);
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }

        public void GetInquiryFormByJobIndex(Action<InquiryFormByIndexDTO, Exception> action, long periodId, string inquirerEmployeeNo, long jobIndexId)
        {
            var url = string.Format(baseAddress + makeJobIndexPointByIndexApiAdress(periodId, inquirerEmployeeNo, jobIndexId));
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }

        public void UpdateInquirySubjectForm(Action<InquiryFormDTO, Exception> action, InquiryFormDTO inquiryForm)
        {
            var url =
                string.Format(baseAddress +
                              makeInquirySubjectJobIndexPointApiAdress(inquiryForm.PeriodId, inquiryForm.InquirerEmployeeNo,
                                  inquiryForm.InquirySubjectEmployeeNo, inquiryForm.JobPositionId)+"?Batch=1");
            WebClientHelper.Put(new Uri(url, PMSClientConfig.UriKind), action, inquiryForm, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }

        public void UpdateJobIndexInquiryForm(Action<InquiryFormByIndexDTO, Exception> action, InquiryFormByIndexDTO inquiryForm)
        {
            var url =
                string.Format(baseAddress +
                              makeJobIndexPointByIndexApiAdress(inquiryForm.PeriodId,inquiryForm.InquirerEmployeeNo,inquiryForm.JobIndexId) + "?Batch=1");
            WebClientHelper.Put(new Uri(url, PMSClientConfig.UriKind), action, inquiryForm, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }


        public void GetInquirySubjectSubEmployeesInquiryFormList(Action<InquirySubjectInquiryFormListDTO, Exception> action,
                                                          long periodId,string inquirySubjectEmployeeNo,long inquirySubjectJobPositionId,
                                                          string managerInquirerEmployeeNo,long managerInquirerJobPositionId)
        {
            //var url = string.Format(baseAddress + makeInquirySubjectJobIndexPointApiAdress(periodId, inquirerEmployeeNo, inquirySubjectEmployeeNo, jobPositionId));
            //WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat);

            
        }

    }
}
