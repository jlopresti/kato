using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Jenkins.Api.Client;
using Kato.vNext.Core;
using Kato.vNext.Messages;

namespace Kato.vNext.Models
{
    public class AddServerModel : ValidatableModel
    {
        private string _name;
        private string _url;
        private bool _requireAuthentication;
        private string _login;
        private string _password;
        private bool _isBusy;

        public AddServerModel()
        {
            AddCommand = new RelayCommand(Add, CanAdd);
            RequireAuthentication = false;
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

            try
            {
                IsBusy = true;
                JenkinsClient client = new JenkinsClient(new Uri(Url, UriKind.Absolute));
                Server server = await client.GetJson<Server>(new Uri(Url));
                Messenger.Default.Send<ServerAddedMessage>(new ServerAddedMessage(new ServerModel(Name, Url, Login, Password, server.Jobs.Count())));
            }
            catch (Exception ex)
            {
                AddError("Url", "Server is unreachable");
            }
            finally
            {
                IsBusy = false;
            }
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

        [ReachableURL]        
        [Required]
        public string Url
        {
            get { return _url; }
            set { Set(() => Url, ref _url, value); }
        }

        public static ValidationResult RequireOnlyWhenAuthRequired(object obj, ValidationContext context)
        {
            var server = (AddServerModel)context.ObjectInstance;
            if (server.RequireAuthentication)
            {
                if(string.IsNullOrEmpty(server.Login) || string.IsNullOrEmpty(server.Password))
                    return new ValidationResult("Your credentials is invalid", new List<string> {"Login", "Password"});
            }

            return ValidationResult.Success;
        }


        public bool RequireAuthentication
        {
            get { return _requireAuthentication; }
            set { Set(() => RequireAuthentication, ref _requireAuthentication, value); }
        }

        [CustomValidation(typeof(AddServerModel), "RequireOnlyWhenAuthRequired")]
        public string Login
        {
            get { return _login; }
            set { Set(() => Login, ref _login, value); }
        }

        [CustomValidation(typeof (AddServerModel), "RequireOnlyWhenAuthRequired")]
        public string Password
        {
            get { return _password; }
            set { Set(() => Password, ref _password, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(() => IsBusy, ref _isBusy, value); }
        }


        public ICommand AddCommand { get; private set; }        
    }
}
