﻿using MITD.Core;
using MITD.PMS.Domain.Service;

namespace MITD.PMS.Domain.Model.Periods
{
    public class PeriodConfirmedState : PeriodState
    {
        public PeriodConfirmedState()
            : base("8", "PeriodConfirmedState")
        {

        }

        internal override void RollBack(Period period, IPeriodManagerService periodManagerService)
        {
            period.State=new PeriodConfirmationState();
        }

        internal override InquiryInitializingProgress GetInitializeInquiryProgress(Period period, IPeriodManagerService periodManagerService)
        {
            return periodManagerService.GetCompletedInitializeInquiryProgress(period);
        }

        internal override void Close(Period period, IPeriodManagerService periodManagerService)
        {
            if (!periodManagerService.HasDeterministicCalculation(period))
                throw new PeriodException((int)ApiExceptionCode.CouldNotClosePeriodWithoutAnyDeterministicCalculation
                    , ApiExceptionCode.CouldNotClosePeriodWithoutAnyDeterministicCalculation.DisplayName);

            //if (periodManagerService.HasOpenClaim(period))
            //    throw new PeriodException((int)ApiExceptionCode.CouldNotClosePeriodWithOpenClaims
            //        , ApiExceptionCode.CouldNotClosePeriodWithOpenClaims.DisplayName);


            period.State = new PeriodClosedState();
            period.DeActive();
        }



        //internal override void CheckAssigningUnit()
        //{
        //}
        //internal override void CheckRemovingUnit()
        //{
        //}
        //internal override void CheckAssigningJobIndex()
        //{
        //}
        //internal override void CheckModifingJobIndex()
        //{
        //}
        //internal override void CheckModifingUnitIndex()
        //{
        //}
        //internal override void CheckAssigningJob()
        //{
        //}
        //internal override void CheckModifyingJobIndices(Job job)
        //{
        //    throw new PMSOperationException("ویرایش شاخص های شغل باعث تغییر در وضعیت نظر سنجی می شود");
        //}
        //internal override void CheckModifyingJobCustomFields()
        //{
        //}
        //internal override void CheckAssigningJobPosition()
        //{
        //}

        //internal override void CheckCreatingEmployee()
        //{
        //}
        //internal override void CheckModifyingJobPositionInquirers()
        //{
        //    throw new PMSOperationException("ویرایش نظردهنده ها باعث تغییر در وضعیت نظر سنجی می شود");
        //}

        //internal override void CheckModifyingEmployeeJobPositions()
        //{
        //    throw new PMSOperationException("خطا،  ویرایش پست های سازمانی کارمند در این وضعیت دوره امکان پذیر نمی باشد زیرا تغییر در پست سازمانی کارمند باعث تغییر در وضعیت نظر سنجی می شود");
        //} 
         
        //internal override void CheckModifyingEmployeeCustomFieldsAndValues()
        //{
        //}

        //internal override void CheckShowingInquirySubject()
        //{
        //}
        //internal override void CheckSettingInquiryJobIndexPointValue()
        //{
        //    throw new PMSOperationException("زمان نظر سنجی پایان یافته است");
        //}
        //internal override void CheckCreatingCalculation()
        //{
        //}

        //internal override void CheckChangeCalculationDeterministicStatus()
        //{
        //}
    }


}

