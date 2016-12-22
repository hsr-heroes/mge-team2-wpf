﻿using ch.hsr.wpf.gadgeothek.admin.ViewModels;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ch.hsr.wpf.gadgeothek.admin
{
    /// <summary>
    /// Interaktionslogik für AddGadget.xaml
    /// </summary>
    public partial class AddGadget : Window
    {
        public AddGadget(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();

            DataContext = new AddGadgetViewModel(mainWindowViewModel);
        }
    }
}
