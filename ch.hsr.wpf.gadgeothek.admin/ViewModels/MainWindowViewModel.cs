using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ch.hsr.wpf.gadgeothek.admin.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public LibraryAdminService libraryAdminService;

        private ICommand _openAddGadgetCommand;
        public ICommand OpenAddGadgetCommand => _openAddGadgetCommand ?? (_openAddGadgetCommand = new RelayCommand(() => OpenAddGadget()));


        private ICommand _removeGadgetCommand;
        public ICommand RemoveGadgetCommand => _removeGadgetCommand ?? (_removeGadgetCommand = new RelayCommand(() => RemoveGadget()));

        public Gadget SelectedGadget { get; set; }

        private ObservableCollection<Gadget> _gadgets;
        public ObservableCollection<Gadget> Gadgets
        {
            get { return _gadgets; }
            set { SetProperty(ref _gadgets, value, nameof(Gadgets));  }
        }

        public Loan SelectedLoan { get; set; }
        private ObservableCollection<Loan> _loans;
        public ObservableCollection<Loan> Loans
        {
            get { return _loans; }
            set { SetProperty(ref _loans, value, nameof(Loans)); }
        }

        public MainWindowViewModel()
        {
            libraryAdminService = new LibraryAdminService(ConfigurationManager.AppSettings.Get("server")?.ToString());

            var gadgets = libraryAdminService.GetAllGadgets();

            if(gadgets == null)
            {
                MessageBox.Show("Konnte Gadgets nicht vom Server laden.", "Serverfehler", MessageBoxButton.OK);
            } else if (gadgets.Count > 0)
            {
                Gadgets = new ObservableCollection<Gadget>(gadgets);

                SelectedGadget = Gadgets.First();
            }
            else
            {
                MessageBox.Show("Keine Gadgets vorhanden. Bitte fügen Sie zuerst ein Gadget hinzu.", "Keine Gadgets vorhanden", MessageBoxButton.OK);
            }


            var loans = libraryAdminService.GetAllLoans();
            if (loans == null)
            {
                MessageBox.Show("Konnte Ausleihen nicht vom Server laden.", "Serverfehler", MessageBoxButton.OK);
                return;
            } else if (loans.Count > 0)
            {
                Loans = new ObservableCollection<Loan>(loans);

                SelectedLoan = Loans.First();
            }

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        loans = libraryAdminService.GetAllLoans();
                        if (loans != null)
                        {
                            Loans.Clear();

                            loans.ForEach(Loans.Add);
                        }
                    });
                }

            });
        }

        public void OpenAddGadget()
        {
            var addNewGadgetWindow = new AddGadget(this);
            addNewGadgetWindow.Show();
        }

        public void RemoveGadget()
        {
            try
            {
                
                if (SelectedGadget != null)
                {
                    MessageBoxResult dialogResult = MessageBox.Show($"Sind Sie sicher, dass Sie{Environment.NewLine}{Environment.NewLine}{SelectedGadget.FullDescription()}{Environment.NewLine}{Environment.NewLine}löschen möchten?", "Löschen bestätigen", MessageBoxButton.YesNo);

                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        if (libraryAdminService.DeleteGadget(SelectedGadget))
                        {
                            Gadgets.Remove(SelectedGadget);
                        }
                        else
                        {
                            MessageBox.Show("Fehler beim Löschen des Gadgets. Bitte versuchen Sie es nochmals.", "Löschen fehlgeschlagen", MessageBoxButton.OK);
                        }
                    }
                }
            }
            catch (InvalidCastException exception)
            {
                MessageBox.Show("Fehler beim Löschen des Gadgets. Bitte versuchen Sie es nochmals.", "Löschen fehlgeschlagen", MessageBoxButton.OK);
                Debug.Print(exception.ToString());
            }
        }


    }
}
