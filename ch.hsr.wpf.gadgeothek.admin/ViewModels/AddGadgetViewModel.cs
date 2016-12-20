using ch.hsr.wpf.gadgeothek.domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ch.hsr.wpf.gadgeothek.admin.ViewModels
{
    class AddGadgetViewModel : BindableBase
    {
        private ICommand _closeDialogCommand;
        public ICommand CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new RelayCommand<Window>((x) => CloseDialog(x)));

        private ICommand _saveGadgetCommand;
        public ICommand SaveGadgetCommand => _saveGadgetCommand ?? (_saveGadgetCommand = new RelayCommand<Window>((x) => SaveGadget(x)));

        public bool FormIsValid { get; set; } = false;

        public Gadget Gadget { get; set; } = new Gadget("");
     
        public string InventoryNumber {
            get
            {
                return Gadget.InventoryNumber;
            }
            set
            {
                Gadget.InventoryNumber = value;

                validateForm();                
            }
        }
        
        public string Name
        {
            get
            {
                return Gadget.Name;
            }
            set
            {
                Gadget.Name = value;

                validateForm();
            }
        }

        public string Manufacturer {
            get {
                return Gadget.Manufacturer;
            }
            set {
                Gadget.Manufacturer = value;

                validateForm();

                OnPropertyChanged();
            }
        }

        public double Price
        {
            get
            {
                return Gadget.Price;
            }
            set
            {
                Gadget.Price = value;

                validateForm();

                OnPropertyChanged();
            }
        }



        public domain.Condition Condition
        {
            get
            {
                return Gadget.Condition;
            }
            set
            {
                Gadget.Condition = value;

                validateForm();

                OnPropertyChanged();
            }
        }

        private MainWindowViewModel _mainWindowViewModel;

        public AddGadgetViewModel(MainWindowViewModel mainWindowViewModel)
        {
           _mainWindowViewModel = mainWindowViewModel;

            this.Gadget = new Gadget("");
        }

        public void CloseDialog(Window window)
        {
            window.Close();
        }

        private void validateForm()
        {
            if (Gadget.Name == null || Gadget.Name.Length <= 0)
            {
                FormIsValid = false;
                return;
            }

            if (Gadget.Manufacturer == null || Gadget.Manufacturer.Length <= 0)
            {
                FormIsValid = false;
                return;
            }

            if (Gadget.Price <= 0.0)
            {
                FormIsValid = false;
                return;
            }

            FormIsValid = true;
            CommandManager.InvalidateRequerySuggested();

            Debug.Print("Form is Valid");
        }

        public void SaveGadget(Window window)
        {
            if (_mainWindowViewModel.libraryAdminService.AddGadget(Gadget))
             {
                _mainWindowViewModel.Gadgets.Add(Gadget);
                 window.Close();
             }
             else
             {
                 MessageBox.Show("Fehler beim Speichern des Gadgets. Bitte versuchen Sie es nochmals.", "Speichern fehlgeschlagen", MessageBoxButton.OK);
             }

        }
    }
}
