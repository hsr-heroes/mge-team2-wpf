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
        public string InventoryNumber { get; set; }
        private MainWindow mainWindow;

        public AddGadget(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            this.DataContext = this;
            InventoryNumber = Gadget.GenerateInventoryNumber();
            Window window = this.Owner;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void input_LostFocus(object sender, RoutedEventArgs e)
        {
            validateForm();
        }

        private void validateForm()
        {
            if (textName.Text.Length <= 0)
            {
                buttonSave.IsEnabled = false;
                return;
            }

            if (textManufacturer.Text.Length <= 0)
            {
                buttonSave.IsEnabled = false;
                return;
            }

           double result;
           if (!Double.TryParse(textPrice.Text, out result) || result <= 0.0)
           {
                buttonSave.IsEnabled = false;
                return;
           }

            buttonSave.IsEnabled = true;

           Debug.Print("Form is Valid");
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            string name = textName.Text;
            string manufacturer = textManufacturer.Text;
            double price = Double.Parse(textPrice.Text);

            domain.Condition condition = (domain.Condition) comboBoxCondition.SelectedItem;

            Gadget gadget = new Gadget(name, InventoryNumber);
            gadget.Condition = condition;
            gadget.Price = price;
            gadget.Manufacturer = manufacturer;

            if (mainWindow.libraryAdminService.AddGadget(gadget))
            {
                mainWindow.Gadgets.Add(gadget);
                this.Close();
            }
            else
            {
                MessageBox.Show("Fehler beim Speichern des Gadgets. Bitte versuchen Sie es nochmals.", "Speichern fehlgeschlagen", MessageBoxButton.OK);
            }

        }
    }
}
