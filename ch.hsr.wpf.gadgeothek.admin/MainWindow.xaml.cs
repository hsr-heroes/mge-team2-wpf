using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using ch.hsr.wpf.gadgeothek.service;
using ch.hsr.wpf.gadgeothek.domain;
using System.Collections.ObjectModel;

namespace ch.hsr.wpf.gadgeothek.admin
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LibraryAdminService libraryAdminService;
        public ObservableCollection<Gadget> Gadgets { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            libraryAdminService = new LibraryAdminService(ConfigurationManager.AppSettings.Get("server")?.ToString());

            var gadgets = libraryAdminService.GetAllGadgets();

            Gadgets = new ObservableCollection<Gadget>(gadgets);
        }

        private void addGadget_Click(object sender, RoutedEventArgs e)
        {
            var addNewGadgetWindow = new AddGadget();

            addNewGadgetWindow.Show();
        }

        private void removeGadget_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
