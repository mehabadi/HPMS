﻿using System.ComponentModel;
using MITD.Core;
using MITD.PMS.Presentation.Contracts;
using MITD.Presentation;
using MITD.PMS.Presentation.BasicInfoApp;
using MITD.PMS.Presentation.BasicInfoApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq; 

namespace MITD.PMS.Presentation.Logic
{
    public class JobListVM : BasicInfoWorkSpaceViewModel, IEventHandler<UpdateJobListArgs>
    {
        #region Fields

        private readonly IBasicInfoController basicInfoController;
        private readonly IPMSController appController;
        private readonly IJobServiceWrapper jobService;

        #endregion

        #region Properties & Back fields

        private PagedSortableCollectionView<JobDTOWithActions> jobs;
        public PagedSortableCollectionView<JobDTOWithActions> Jobs
        {
            get { return jobs; }
            set { this.SetField(p => p.Jobs, ref jobs, value); }
        }

        private JobDTOWithActions selectedJob;
        public JobDTOWithActions SelectedJob
        {
            get { return selectedJob; }
            set
            {
                this.SetField(p => p.SelectedJob, ref selectedJob, value);
                if (selectedJob == null) return;
                JobCommands = createCommands();
                if (View != null)
                    ((IJobListView)View).CreateContextMenu(new ReadOnlyCollection<DataGridCommandViewModel>(JobCommands));
            }
        }

        private List<DataGridCommandViewModel> jobCommands;
        public List<DataGridCommandViewModel> JobCommands
        {
            get { return jobCommands; }
            private set
            {
                this.SetField(p => p.JobCommands, ref jobCommands, value);
                if (JobCommands.Count > 0) SelectedCommand = JobCommands[0];
            }

        }
        
        private DataGridCommandViewModel selectedCommand;
        public DataGridCommandViewModel SelectedCommand
        {
            get { return selectedCommand; }
            set { this.SetField(p => p.SelectedCommand, ref selectedCommand, value); }
        }

        #endregion

        #region Constructors

        public JobListVM()
        {
            BasicInfoAppLocalizedResources=new BasicInfoAppLocalizedResources();
            init();
            Jobs.Add(new JobDTOWithActions { Id = 4, Name = "Test" });
        }

        public JobListVM(IBasicInfoController basicInfoController, IPMSController appController,
            IJobServiceWrapper jobService, IBasicInfoAppLocalizedResources basicInfoAppLocalizedResources)
        {
            
            this.appController = appController;
            this.jobService = jobService;
            BasicInfoAppLocalizedResources = basicInfoAppLocalizedResources;
            this.basicInfoController = basicInfoController;
            init();
            

        }

        #endregion

        #region Methods

        void init()
        {
            DisplayName = BasicInfoAppLocalizedResources.JobListViewTitle;
            Jobs = new PagedSortableCollectionView<JobDTOWithActions> {PageSize = 20};
            Jobs.OnRefresh += (s, args) => Load();
            JobCommands = new List<DataGridCommandViewModel>
            {
                   CommandHelper.GetControlCommands(this, appController, new List<int>{ (int) ActionType.CreateJob}).FirstOrDefault()
            };
        }

        private List<DataGridCommandViewModel> createCommands()
        {
            return CommandHelper.GetControlCommands(this, appController, SelectedJob.ActionCodes);
        }

        public void Load()
        {
            var sortBy = jobs.SortDescriptions.ToDictionary(sortParam => sortParam.PropertyName,
                                                            sortDirect =>
                                                            (sortDirect.Direction == ListSortDirection.Ascending)
                                                                ? "ASC" : "DESC");
            jobService.GetAllJobs(
                  (res, exp) => appController.BeginInvokeOnDispatcher(() =>
                 {
                     HideBusyIndicator();
                     if (exp == null)
                     {
                         Jobs.SourceCollection = res.Result;
                         Jobs.TotalItemCount = res.TotalCount;
                         Jobs.PageIndex = Math.Max(0, res.CurrentPage - 1);
                     }
                     else appController.HandleException(exp);
                 }), jobs.PageSize, jobs.PageIndex + 1,sortBy);
        }


        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            appController.Close(this);
        }

        public void Handle(UpdateJobListArgs eventData)
        {
            Load();
        }

        #endregion

    }
}
