﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MITD.PMS.Presentation.Logic;
using MITD.Presentation.UI;

namespace MITD.PMS.Presentation.PeriodManagementApp.Views
{
    public partial class EmployeesInquiryListView : ViewBase, IEmployeesInquiryListView,IViewWithContextMenu
    {
        public EmployeesInquiryListView()
        {
            InitializeComponent();
        }

        public EmployeesInquiryListView(InquirerInquirySubjectListVM vm)
        {
            InitializeComponent();
            ViewModel = vm;

        }

        private void drgList_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition(null), this);
            var row = elements.Where(el => el is DataGridRow).Cast<DataGridRow>().FirstOrDefault();
            if (row != null)
            {
                drgList.SelectedItem = row.DataContext;
            }
        }

        public void CreateContextMenu(ReadOnlyCollection<DataGridCommandViewModel> commands)
        {
            cmCommands.Items.Clear();
            commands.ToList().ForEach(c =>
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

        public IList<DependencyObject> ItemsWithContextMenu
        {
            get { return new List<DependencyObject> { drgList }; }
        }
    }
}
