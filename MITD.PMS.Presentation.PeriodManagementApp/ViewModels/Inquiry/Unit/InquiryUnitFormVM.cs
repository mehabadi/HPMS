﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MITD.PMS.Presentation.Contracts;
using MITD.PMS.Presentation.PeriodManagementApp;
using MITD.Presentation;

namespace MITD.PMS.Presentation.Logic
{
    public sealed class InquiryUnitFormVM : PeriodMgtWorkSpaceViewModel 
    {
        #region Fields

        private readonly IPMSController appController;
        private readonly IUnitInquiryServiceWrapper inquiryService;
        private ActionType actionType;
        private long periodId;
        
        #endregion

        #region Properties

        public Visibility SubInquirerEmployeesVisibilityMode
        {
            get
            {
                //var visibile = InquirySubjectInquirers.InquirerList.Count>1 ? Visibility.Visible : Visibility.Collapsed;
                //return visibile;
                return Visibility.Collapsed;
            }
        }

        private PeriodDTO period;
        public PeriodDTO Period
        {
            get { return period; }
            set { this.SetField(vm => vm.Period, ref period, value); }
        }

        private InquirySubjectInquiryFormListDTO inquirySubjectInquirers;
        public InquirySubjectInquiryFormListDTO InquirySubjectInquirers
        {
            get { return inquirySubjectInquirers; }
            set { this.SetField(vm => vm.InquirySubjectInquirers, ref inquirySubjectInquirers, value); }
        }

        private InquiryFormInquirerDTO selectedInquirer;
        public InquiryFormInquirerDTO SelectedInquirer
        {
            get { return selectedInquirer; }
            set { this.SetField(vm => vm.SelectedInquirer, ref selectedInquirer, value); }
        }

        private InquiryUnitFormDTO inquiryForm;
        public InquiryUnitFormDTO InquiryForm
        {
            get { return inquiryForm; }
            set { this.SetField(vm => vm.InquiryForm, ref inquiryForm, value); }
        }

        private CommandViewModel saveCommand;
        public CommandViewModel SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new CommandViewModel("ذخیره", new DelegateCommand(save));
                }
                return saveCommand;
            }
        }

        private CommandViewModel cancelCommand;
        public CommandViewModel CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new CommandViewModel("انصراف", new DelegateCommand(OnRequestClose));
                }
                return cancelCommand;
            }
        } 

        #endregion

        #region Constructors

        public InquiryUnitFormVM()
        {
            PeriodMgtAppLocalizedResources=new PeriodMgtAppLocalizedResources();
            init();
            //Period = new Period { Name = "دوره اول", StartDate = DateTime.Now, EndDate = DateTime.Now };
        }
        public InquiryUnitFormVM(IPMSController appController, 
                          IUnitInquiryServiceWrapper inquiryService,
                          IPeriodMgtAppLocalizedResources localizedResources)
        {
            this.appController = appController;
            this.inquiryService = inquiryService;
            PeriodMgtAppLocalizedResources = localizedResources;
            init();
            
        } 

        #endregion

        #region Methods

        private void init()
        {
            inquirySubjectInquirers = new InquirySubjectInquiryFormListDTO();
            
        }

        public void Load(InquiryUnitFormDTO inquiryFormDTOParam, ActionType actionTypeParam)
        {
            periodId = inquiryFormDTOParam.PeriodId;
            actionType = actionTypeParam;
           
            //todo bz
            InquiryForm = inquiryFormDTOParam;
            DisplayName = "فرم ارزیابی" + " "; //+ InquiryForm.FullName;
            //ShowBusyIndicator("در حال دریافت اطلاعات...");
            //inquiryService.GetInquirySubjectSubEmployeesInquiryFormList((res, exp) =>
            //     appController.BeginInvokeOnDispatcher(() =>
            //    {
            //        //HideBusyIndicator();
            //        if (exp == null)
            //            InquirySubjectInquirers = res;
            //        else
            //            appController.HandleException(exp);

            //    }), periodId, InquiryForm.InquirySubjectEmployeeNo, InquiryForm.JobPositionId, inquiryForm.InquirerEmployeeNo, inquiryForm.InquirerJobPositionId);
        }

       
        private void save()
        {
            if(!validate()) return;
            
            ShowBusyIndicator();
            UserStateDTO userState = appController.CurrentUser;
            inquiryService.UpdateInquirySubjectForm((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                    {
                        HideBusyIndicator();
                        if (exp != null)
                            appController.HandleException(exp);
                        else
                        {
                            OnRequestClose();
                            appController.ShowMessage("فرم ارزیابی ثبت شد");
                        }

                    }), inquiryForm);
            
        }

        private bool validate()
        {
            foreach (var value in InquiryForm.UnitIndexValueList)
            {
                if (value.HasErrors) 
                    return false;
            }
            return true;
        }


        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        } 

        #endregion
    }
    
}
