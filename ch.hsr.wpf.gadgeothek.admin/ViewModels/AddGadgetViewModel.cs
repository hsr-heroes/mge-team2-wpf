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

        private bool _formIsValid;
        public bool FormIsValid
        {
            get
            {
                return _formIsValid;
            }
            set
            {
                SetProperty(ref _formIsValid, value, nameof(FormIsValid));
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetProperty(ref _name, value, nameof(Name));
                validateForm();
            }
        }

        private string _manufacturer;
        public string Manufacturer {
            get {
                return _manufacturer;
            }
            set {
                SetProperty(ref _manufacturer, value, nameof(Manufacturer));
                validateForm();
            }
        }

        private double _price;
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                SetProperty(ref _price, value, nameof(Price));
                validateForm();
            }
        }


        private domain.Condition _condition;
        public domain.Condition Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                SetProperty(ref _condition, value, nameof(Condition));
                validateForm();
            }
        }

        private MainWindowViewModel _mainWindowViewModel;

        public AddGadgetViewModel(MainWindowViewModel mainWindowViewModel)
        {
           _mainWindowViewModel = mainWindowViewModel;
        }

        public void CloseDialog(Window window)
        {
            window.Close();
        }

        private void validateForm()
        {
            if (_name == null || _name.Length <= 0)
            {
                FormIsValid = false;
                return;
            }

            if (_manufacturer == null || _manufacturer.Length <= 0)
            {
                FormIsValid = false;
                return;
            }

            if (_price <= 0.0)
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
            var newGadget = new Gadget(_name)
            {
                Condition = _condition,
                Manufacturer = _manufacturer,
                Price = _price
            };

            if (_mainWindowViewModel.libraryAdminService.AddGadget(newGadget)) {
                _mainWindowViewModel.Gadgets.Add(newGadget);
                 window.Close();
             }
             else
             {
                 MessageBox.Show("Fehler beim Speichern des Gadgets. Bitte versuchen Sie es nochmals.", "Speichern fehlgeschlagen", MessageBoxButton.OK);
             }

        }
    }
}
