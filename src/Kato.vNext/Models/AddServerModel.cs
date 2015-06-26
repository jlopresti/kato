using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Jenkins.Api.Client;
using Kato.vNext.Messages;

namespace Kato.vNext.Models
{
    public class AddServerModel : ObservableObject, IDisposable
    {
        private string _name;
        private string _url;
        private bool _requireAuthentication;
        private string _login;
        private string _password;

        public AddServerModel()
        {
            AddCommand = new RelayCommand(Add, CanAdd);
            RequireAuthentication = false;
            this.PropertyChanged += AddServerModel_PropertyChanged;
        }

        private void AddServerModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
        }

        private bool CanAdd()
        {
            var baseFields = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Url);
            var optionalFields = !RequireAuthentication || !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
            return baseFields && optionalFields;
        }

        private async void Add()
        {
            try
            {
                JenkinsClient client = new JenkinsClient(new Uri(Url, UriKind.Absolute));
                Server server = await client.GetJson<Server>(new Uri(Url));
                Messenger.Default.Send<ServerAddedMessage>(new ServerAddedMessage(new ServerModel(Name, Url, Login, Password, server.Jobs.Count())));
            }
            catch (Exception ex)
            {

            }
        }

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public string Url
        {
            get { return _url; }
            set { Set(() => Url, ref _url, value); }
        }

        public bool RequireAuthentication
        {
            get { return _requireAuthentication; }
            set { Set(() => RequireAuthentication, ref _requireAuthentication, value); }
        }

        public string Login
        {
            get { return _login; }
            set { Set(() => Login, ref _login, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(() => Password, ref _password, value); }
        }

        public ICommand AddCommand { get; private set; }
        public void Dispose()
        {
            this.PropertyChanged -= AddServerModel_PropertyChanged;
        }
    }
}
