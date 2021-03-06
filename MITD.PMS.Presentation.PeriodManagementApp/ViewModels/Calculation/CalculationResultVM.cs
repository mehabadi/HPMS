﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MITD.Core;
using MITD.PMS.Presentation.Contracts;
using MITD.PMS.Presentation.PeriodManagementApp;
using MITD.PMS.Presentation.PeriodManagementApp.Views;
using MITD.Presentation;

namespace MITD.PMS.Presentation.Logic
{
    public class CalculationResultVM : PeriodMgtWorkSpaceViewModel, IEventHandler<UpdateCalculationResultListArgs>
    {
        #region Fields

        private readonly IPMSController appController;
        private readonly ICalculationServiceWrapper calculationService;
        private readonly IPeriodServiceWrapper periodService;
        private string employeeNo;

        #endregion

        #region Properties


        private EmployeeResultDTO employeeResultDTO;
        public EmployeeResultDTO EmployeeResultDTO
        {
            get { return employeeResultDTO; }
            set { this.SetField(p => p.EmployeeResultDTO, ref employeeResultDTO, value); }
        }

        private string leveledTotalPoint;
        public string LeveledTotalPoint
        {
            get { return leveledTotalPoint; }
            set { this.SetField(p => p.LeveledTotalPoint, ref leveledTotalPoint, value); }

        }

        private string leveledUnitPoint;
        public string LeveledUnitPoint
        {
            get { return leveledUnitPoint; }
            set { this.SetField(p => p.LeveledUnitPoint, ref leveledUnitPoint, value); }

        }

        private PeriodDescriptionDTO selectedPeriod;
        public PeriodDescriptionDTO SelectedPeriod
        {
            get { return selectedPeriod; }
            set
            {
                this.SetField(p => p.SelectedPeriod, ref selectedPeriod, value);
                getEmployeeResult();
               
            }
        }


        private ObservableCollection<PeriodDescriptionDTO> periodsWithConfirmedResult;
        public ObservableCollection<PeriodDescriptionDTO> PeriodsWithConfirmedResult
        {
            get { return periodsWithConfirmedResult; }
            set { this.SetField(p => p.PeriodsWithConfirmedResult, ref periodsWithConfirmedResult, value); }
        }

        private ObservableCollection<JobIndexValueDTO> strengthEmployeeIndices;
        public ObservableCollection<JobIndexValueDTO> StrengthEmployeeIndices
        {
            get { return strengthEmployeeIndices; }
            set { this.SetField(p => p.StrengthEmployeeIndices, ref strengthEmployeeIndices, value); }
        }

        private ObservableCollection<JobIndexValueDTO> weakEmployeeIndices;
        public ObservableCollection<JobIndexValueDTO> WeakEmployeeIndices
        {
            get { return weakEmployeeIndices; }
            set { this.SetField(p => p.WeakEmployeeIndices, ref weakEmployeeIndices, value); }
        }

        private ObservableCollection<JobIndexValueDTO> trainingEmployeeIndices;
        public ObservableCollection<JobIndexValueDTO> TrainingEmployeeIndices
        {
            get { return trainingEmployeeIndices; }
            set { this.SetField(p => p.TrainingEmployeeIndices, ref trainingEmployeeIndices, value); }
        }

        #endregion

        #region Constructors

        public CalculationResultVM()
        {
            PeriodMgtAppLocalizedResources = new PeriodMgtAppLocalizedResources();
            init();
            // EmployeeCalculationResults.Add(new EmployeeCalculationResultDTO { Id = 4, Name = "Test" });           
        }

        public CalculationResultVM(IPMSController appController,
                            ICalculationServiceWrapper calculationService,
                            IPeriodMgtAppLocalizedResources periodMgtAppLocalizedResources,IPeriodServiceWrapper periodService)
        {

            this.appController = appController;
            this.calculationService = calculationService;
            this.periodService = periodService;
            PeriodMgtAppLocalizedResources = periodMgtAppLocalizedResources;
            init();


        }

        #endregion

        #region Methods

        void init()
        {
            DisplayName = "نتیجه عملکرد من";
        }

        public void Load(string employeeNo)
        {
            
            this.employeeNo = employeeNo;
            preLoad();


        }

        private void preLoad()
        {
           ShowBusyIndicator("در حال دریافت اطلاعات...");
           periodService.GetPeriodsWithConfirmedResult((res, exp) => appController.BeginInvokeOnDispatcher(() =>
                {
                    HideBusyIndicator();
                    if (exp == null)
                    {
                        PeriodsWithConfirmedResult = res;
                        if (PeriodsWithConfirmedResult.Any())
                            SelectedPeriod = PeriodsWithConfirmedResult.First();
                    }
                    else
                        appController.HandleException(exp);

                }));  
        }

        private void getEmployeeResult()
        {
            periodService.GetEmployeeResultInPeriod(
                    (res, exp) => appController.BeginInvokeOnDispatcher(() =>
                    {
                        if (exp != null)
                            appController.HandleException(exp);
                        else
                        {
                            EmployeeResultDTO = res;
                            StrengthEmployeeIndices = new ObservableCollection<JobIndexValueDTO>(EmployeeResultDTO.JobIndexValues.Where(j => Decimal.Parse(j.IndexValue, CultureInfo.InvariantCulture) >= 90));
                            WeakEmployeeIndices = new ObservableCollection<JobIndexValueDTO>(EmployeeResultDTO.JobIndexValues.Where(j => Decimal.Parse(j.IndexValue, CultureInfo.InvariantCulture) < 30));
                            TrainingEmployeeIndices = new ObservableCollection<JobIndexValueDTO>(EmployeeResultDTO.JobIndexValues.Where(j => Decimal.Parse(j.IndexValue, CultureInfo.InvariantCulture) <= 50));
                            LeveledTotalPoint = levelPoint(EmployeeResultDTO.TotalPoint);
                            //LeveledUnitPoint = levelPoint(EmployeeResultDTO.TotalUnitPoint);
                        }
                    }), SelectedPeriod.Id,employeeNo
                    );

        }

        private string levelPoint(string totalPoint)
        {
            var point = Decimal.Parse(totalPoint, CultureInfo.InvariantCulture);
            if (point >= 90)
                return "عالی";
            if (point < 90 && point >= 70)
                return "خوب";
            if (point < 70 && point >= 50)
                return "قابل قبول";
            if (point < 50 && point >= 30)
                return "نیاز به آموزش";
            if (point < 30 && point > 0)
                return "نا مطلوب";
            return "-";

        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        }

        public void Handle(UpdateCalculationResultListArgs eventData)
        {
           
        }

        #endregion
    }

}
