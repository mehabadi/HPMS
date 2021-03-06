﻿using System;
using MITD.PMS.Presentation.Contracts;
using System.Collections.Generic;
using MITD.Presentation;


namespace MITD.PMS.Presentation.Logic.Wrapper
{
    public class UnitInPeriodServiceWrapper : IUnitInPeriodServiceWrapper
    {
        private readonly IUserProvider userProvider;
        private readonly string baseAddress = PMSClientConfig.BaseApiAddress;

        public UnitInPeriodServiceWrapper(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        private string makeApiAdress(long periodId)
        {
            return "Periods/" + periodId + "/Units";
        }

        public void AddUnitInPeriod(Action<UnitInPeriodAssignmentDTO, Exception> action, UnitInPeriodAssignmentDTO unitInPeriod)
        {
            var url = string.Format(baseAddress + makeApiAdress(unitInPeriod.PeriodId));
            WebClientHelper.Post(new Uri(url, PMSClientConfig.UriKind), action, unitInPeriod, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }

        public void DeleteUnitInPeriod(Action<string, Exception> action, long periodId, long unitId)
        {
            var url = string.Format(baseAddress + makeApiAdress(periodId) + "?UnitId=" + unitId);
            WebClientHelper.Delete(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }

        public void GetAllUnits(Action<List<UnitInPeriodDTO>, Exception> action, long periodId)
        {
            var url = string.Format(baseAddress + makeApiAdress(periodId) + "?Type=UnitInPeriodDTO");
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }

        public void GetUnitsWithActions(Action<List<UnitInPeriodDTOWithActions>, Exception> action, long periodId)
        {
            var url = string.Format(baseAddress + makeApiAdress(periodId) + "?Type=UnitInPeriodDTOWithActions");
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }


        public void GetUnitInPeriod(Action<UnitInPeriodDTO, Exception> action, long periodId, long unitId)
        {
            GetUnitInPeriod(action, periodId, unitId, "");
        }



        public void GetUnitInPeriod(Action<UnitInPeriodDTO, Exception> action, long periodId, long unitId, string columnNames)
        {

            var url = string.Format(baseAddress + makeApiAdress(periodId) + "?unitId=" + unitId);
          //  url += !string.IsNullOrWhiteSpace(columnNames) ? "&SelectedColumns=" + columnNames : "";
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }

        public void GetAllUnitInPeriod(Action<IList<UnitInPeriodDTO>, Exception> action, long periodId, string columnNames)
        {
            var url = string.Format(baseAddress + makeApiAdress(periodId) + "?Type=unitInPeriodDTO");
           // url += !string.IsNullOrWhiteSpace(columnNames) ? "&SelectedColumns=" + columnNames : "";
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }

        public void GetAllUnitInPeriod(Action<IList<UnitInPeriodDTO>, Exception> action, long periodId)
        {
            GetAllUnitInPeriod(action, periodId, "");
        }


        public void AddUnitInPeriod(Action<JobInPeriodDTO, Exception> action, long periodId, UnitInPeriodDTO unitInPeriodDto)
        {
            var url = string.Format(baseAddress + makeApiAdress(periodId));
            WebClientHelper.Post(new Uri(url, PMSClientConfig.UriKind), action, unitInPeriodDto, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }

        public void UpdateUnitInPeriod(Action<UnitInPeriodDTO, Exception> action, long periodId, UnitInPeriodDTO unitInPeriodDto)
        {

            var url = string.Format(baseAddress + makeApiAdress(periodId));
            WebClientHelper.Put(new Uri(url, PMSClientConfig.UriKind), action, unitInPeriodDto, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }


        public void UpdateInquirySubjectInquirers(Action<InquirySubjectWithInquirersDTO, Exception> action, long periodId, long unitId,
            InquirySubjectWithInquirersDTO inquirySubjectWithInquirersDTO)
        {
            throw new NotImplementedException();
        }

        public void GetInquirySubjectWithInquirers(Action<List<InquirySubjectWithInquirersDTO>, Exception> action, long periodId, long unitId)
        {
            var url = string.Format(baseAddress + makeUnitPositionInquirySubjectsApiAdress(periodId, unitId) + "?Include=Inquirers");
            WebClientHelper.Get(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }

        private string makeUnitPositionInquirySubjectsApiAdress(long periodId, long unitId)
        {
            return "Periods/" + periodId + "/Units/" + unitId + "/InquirySubjects";
        }

        public void AddInquirer(Action<string,Exception> action, long periodId, long unitId, string personalNo,long unitIndexInUnitId)
        {
            var url = string.Format(baseAddress + makeUnitPositionInquirySubjectsApiAdress(periodId, unitId) + "?employeeNo=" + personalNo + "&unitIndexInPeiodUnit=" + unitIndexInUnitId);
            WebClientHelper.Put(new Uri(url, PMSClientConfig.UriKind),action,personalNo, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }


        public void DeleteInquirer(Action<string, Exception> action, long periodId, long unitId, string personalNo)
        {
            var url = string.Format(baseAddress + makeUnitPositionInquirySubjectsApiAdress(periodId, unitId) + "?employeeNo=" + personalNo);
            WebClientHelper.Delete(new Uri(url, PMSClientConfig.UriKind), action,  PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }

        private string createUnitVerifierApiAdress(long periodId, long unitId)
        {
            return "Periods/" + periodId + "/Units/" + unitId + "/Verifiers";
        }

        public void AddVerifier(Action<string, Exception> action, long periodId, long unitId, string personnelNo)
        {
            var url = string.Format(baseAddress + createUnitVerifierApiAdress(periodId, unitId) + "?employeeNo=" + personnelNo);
            WebClientHelper.Put(new Uri(url, PMSClientConfig.UriKind), action, personnelNo, PMSClientConfig.MsgFormat, PMSClientConfig.CreateHeaderDic(userProvider.Token));
        }

        public void DeleteVerifier(Action<string, Exception> action, long periodId, long unitId, string personalNo)
        {
            var url = string.Format(baseAddress + createUnitVerifierApiAdress(periodId, unitId) + "?employeeNo=" + personalNo);
            WebClientHelper.Delete(new Uri(url, PMSClientConfig.UriKind), action, PMSClientConfig.CreateHeaderDic(userProvider.Token));

        }


    }
}
