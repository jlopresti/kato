using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Kato.vNext.Core;
using Kato.vNext.Messages;

namespace Kato.vNext.Models
{
    public class AddMonitoringModel : ValidatableModel
    {
        private string _name;
        private string _url;
        private bool _isBusy;
        private string _cookies;

        public AddMonitoringModel()
        {
            AddCommand = new RelayCommand(Add, CanAdd);
        }

        protected override void OnPropertyChanged()
        {
            ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
        }

        private bool CanAdd()
        {
            return !HasErrors && !IsBusy;
        }

        private async void Add()
        {
            await ValidateAsync();

            if (HasErrors)
            {
                OnPropertyChanged();
                return;
            }
            Messenger.Default.Send<MonitoringServerAddedMessage>(new MonitoringServerAddedMessage(new MonitoringServer(Name, Url)));
        }

        [Required]
        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        [Url]
        [Required]
        public string Url
        {
            get { return _url; }
            set { Set(() => Url, ref _url, value); }
        }

        public string Cookies
        {
            get { return _cookies; }
            set
            {
                Set(() => Cookies, ref _cookies, value);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(() => IsBusy, ref _isBusy, value); }
        }

        public ICommand AddCommand { get; private set; }
    }
}