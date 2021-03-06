﻿using MITD.Presentation.UI;
using MITD.PMS.Presentation.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MITD.PMS.Presentation.BasicInfoApp.Views
{
    public partial class JobIndexListView : ViewBase, IJobIndexListView
    {
        public JobIndexListView()
        {
            InitializeComponent();
        }

        public JobIndexListView(JobIndexListVM vm)
        {
            InitializeComponent();
            ViewModel = vm;

        }

        private void drgJobIndexList_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition(null), this);
            var row = elements.Where(el => el is DataGridRow).Cast<DataGridRow>().FirstOrDefault();
            if (row != null)
            {
                drgJobIndexList.SelectedItem = row.DataContext;
            }
        }


        public void CreateContextMenu(ReadOnlyCollection<DataGridCommandViewModel> jobCommands)
        {
            cmCommands.Items.Clear();
            jobCommands.ToList().ForEach(c =>
            {
                var converter = new ImageSourceConverter();
                var img = new Image();
                if (c.Icon != null)
                    img.Source = (ImageSource)converter.ConvertFromString(c.Icon);
                var mi = new MenuItem
                {
                    Header = c.CommandViewModel.DisplayName,
                    FontSize = 11,
                    FontWeight = FontWeights.Medium,
                    Command = c.CommandViewModel.Command,
                    Icon = img,
                    Margin = new Thickness(2),
                    Padding = new Thickness(2)
                };
                cmCommands.Items.Add(mi);
            }
            );
        }
    }
}
