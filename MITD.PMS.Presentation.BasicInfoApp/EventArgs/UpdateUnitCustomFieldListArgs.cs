﻿using MITD.PMS.Presentation.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MITD.PMS.Presentation.BasicInfoApp
{
    public class UpdateUnitCustomFieldListArgs
    {
        public UpdateUnitCustomFieldListArgs(List<AbstractCustomFieldDescriptionDTO> unitCustomFieldDescriptionList)
        {
            UnitCustomFieldDescriptionList = unitCustomFieldDescriptionList;
        }
        public List<AbstractCustomFieldDescriptionDTO> UnitCustomFieldDescriptionList
        {
            get; private set; 
        }
    }
}
