﻿using MITD.Presentation;

namespace MITD.PMS.Presentation.Contracts
{
    public partial class JobInPeriodJobIndexDTO : ViewModelBase
    {
        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { this.SetField(p => p.IsChecked, ref isChecked, value); }
        }
    }
}
