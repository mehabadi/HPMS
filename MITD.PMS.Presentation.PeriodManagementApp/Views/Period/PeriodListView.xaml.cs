﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MITD.PMS.Presentation.Logic;
using MITD.Presentation.UI;
using System.Collections.Generic;

namespace MITD.PMS.Presentation.PeriodManagementApp.Views
{
    public partial class PeriodListView : ViewBase,IPeriodListView,IViewWithContextMenu
    {
        public PeriodListView()
        {
            InitializeComponent();
        }
        public PeriodListView(PeriodListVM vm)
        {
            InitializeComponent();
            ViewModel = vm;
        }

        private void drgPeriodList_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition(null), this);
            var row = elements.Where(el => el is DataGridRow).Cast<DataGridRow>().FirstOrDefault();
            if (row != null)
            {
                drgPeriodList.SelectedItem = row.DataContext;
            }
        }

        public void CreateContextMenu(ReadOnlyCollection<DataGridCommandViewModel> periodCommands)
        {
            cmCommands.Items.Clear();
            periodCommands.ToList().ForEach(c =>
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
            });
        }
        public IList<DependencyObject> ItemsWithContextMenu
        {
            get { return new List<DependencyObject> { drgPeriodList }; }
        }
    }
}
