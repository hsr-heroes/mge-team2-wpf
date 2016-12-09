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


            if(gadgets.Count > 0)
            {
                Gadgets = new ObservableCollection<Gadget>(gadgets);

                gadgetsGrid.SelectedIndex = 0;
            } else
            {
                MessageBox.Show("Keine Gadgets vorhanden. Bitte fügen Sie zuerst ein Gadget hinzu.", "Keine Gadgets vorhanden", MessageBoxButton.OK);
            }

        }

        private void addGadget_Click(object sender, RoutedEventArgs e)
        {
            var addNewGadgetWindow = new AddGadget();

            addNewGadgetWindow.Show();
        }

        private void removeGadget_Click(object sender, RoutedEventArgs e)
        {
            var selectedGadget = (Gadget) gadgetsGrid.SelectedItem;
        
            if(selectedGadget != null)
            {
                MessageBoxResult dialogResult = MessageBox.Show($"Sind Sie sicher, dass Sie{Environment.NewLine}{Environment.NewLine}{selectedGadget.FullDescription()}{Environment.NewLine}{Environment.NewLine}löschen möchten?", "Löschen bestätigen", MessageBoxButton.YesNo);

                if(dialogResult == MessageBoxResult.Yes)
                {
                    if(libraryAdminService.DeleteGadget(selectedGadget))
                    {
                        Gadgets.Remove(selectedGadget);
                    } else
                    {
                        MessageBox.Show("Fehler beim Löschen des Gadgets. Bitte versuchen Sie es nochmals.", "Löschen fehlgeschlagen", MessageBoxButton.OK);
                    }
                    
                }
            }
        }
    }
}
