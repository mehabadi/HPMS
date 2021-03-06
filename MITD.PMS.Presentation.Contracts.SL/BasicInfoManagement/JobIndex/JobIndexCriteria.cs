﻿using MITD.Presentation;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MITD.PMS.Presentation.Contracts
{
    public class JobIndexCriteria : ViewModelBase
    {
        private long categoryId;
        public long CategoryId
        {
            get { return categoryId; }
            set
            {
                this.SetField(p => p.CategoryId, ref categoryId, value);
            }
        }

    }
}
