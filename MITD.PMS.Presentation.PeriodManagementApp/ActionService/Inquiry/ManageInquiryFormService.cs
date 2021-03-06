﻿using MITD.PMS.Presentation.Contracts;
using MITD.PMS.Presentation.Logic;
using MITD.Presentation;

namespace MITD.PMS.Presentation.Logic
{
    public class ManageInquiryFormService : IActionService<InquirerInquirySubjectListVM>,IActionService<JobIndexInquiryListVM>
    {
        private readonly IPeriodController periodController;
        private readonly IPMSController pmsController;
        private readonly IInquiryServiceWrapper inquiryService;

        public ManageInquiryFormService(IPeriodController periodController,
            IPMSController pmsController, IInquiryServiceWrapper inquiryService)
        {
            this.periodController = periodController;
            this.pmsController = pmsController;
            this.inquiryService = inquiryService;
        }


        public void DoAction(InquirerInquirySubjectListVM vm)
        {
           
            if (vm.SelectedInquirySubject == null)
                return;
            inquiryService.GetInquiryForm((res, exp) => pmsController.BeginInvokeOnDispatcher(() =>
                {
                    if (exp == null)
                    {
                        res.FullName = vm.SelectedInquirySubject.FullName;
                        periodController.ShowInquiryFormView(res, ActionType.FillInquiryForm);
                    }
                        
                    else
                    {
                        pmsController.HandleException(exp);
                    }
                }), vm.PeriodId, vm.InquirerEmployeeNo,vm.SelectedInquirySubject.InquirerJobPositionId, vm.SelectedInquirySubject.EmployeeNo, vm.SelectedInquirySubject.JobPositionId);


        }


        public void DoAction(JobIndexInquiryListVM vm)
        {
            if (vm.SelectedInquiryIndex == null)
                return;
            inquiryService.GetInquiryFormByJobIndex((res, exp) => pmsController.BeginInvokeOnDispatcher(() =>
            {
                if (exp == null)
                {
                    res.FullName = vm.SelectedInquiryIndex.JobIndexName;
                    periodController.ShowJobIndexInquiryFormView(res, ActionType.FillInquiryForm);
                }

                else
                {
                    pmsController.HandleException(exp);
                }
            }), vm.PeriodId, vm.InquirerEmployeeNo, vm.SelectedInquiryIndex.JobIndexId);

        }
    }
}

