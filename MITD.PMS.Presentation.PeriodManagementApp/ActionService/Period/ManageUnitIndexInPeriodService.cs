﻿using MITD.PMS.Presentation.Contracts;
using MITD.PMS.Presentation.Logic;
using MITD.Presentation;

namespace MITD.PMS.Presentation.PeriodManagementApp
{
    public class ManageUnitIndexInPeriodService : IActionService<PeriodListVM>
    {
        private readonly IPeriodController periodController;
        private readonly IPMSController pmsController;

        public ManageUnitIndexInPeriodService(IPeriodController periodController,
                                         IPMSController pmsController)
        {
            this.periodController = periodController;
            this.pmsController = pmsController;
        }


        public void DoAction(PeriodListVM vm)
        {
            if (vm.SelectedPeriod != null)
            {
                periodController.ShowUnitIndexInPeriodTreeView(vm.SelectedPeriod);
            }
            else
                pmsController.ShowMessage("اطلاعات دوره جهت ارسال به صفحه مدیریت واحد های چارت سازمانی معتبر نمی باشد");

        }


    }
}

