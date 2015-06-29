using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Jenkins.Api.Client;
using Kato.vNext.Core;
using Kato.vNext.Helpers;
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
                JenkinsClient client = JenkinsClientFactory.CreateJenkinsClient(Url, Login, Password);
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

        [CustomValidation(typeof(AddServerModel), "IsReacheableUrl")]
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
                if (string.IsNullOrEmpty(server.Login) || string.IsNullOrEmpty(server.Password))
                    return new ValidationResult("Your credentials is invalid", new List<string> { "Login", "Password" });
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

        [CustomValidation(typeof(AddServerModel), "RequireOnlyWhenAuthRequired")]
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


        private static CancellationTokenSource _token;
        private static Regex _regex = new Regex(@"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        public static ValidationResult IsReacheableUrl(string value, ValidationContext context)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            if (_token != null)
            {
                _token.Cancel();
                _token = null;
            }
            if (_regex.Match(value).Length > 0)
            {
                _token = new CancellationTokenSource();
                Task<ValidationResult> isReachableTask = IsReachableURL(value, context.ObjectInstance as AddServerModel, _token.Token);
                isReachableTask.Wait();
                _token = null;
                return isReachableTask.Result;
            }
            return new ValidationResult("URL Invalid", new List<string> {"Url"});
        }

        private static async Task<ValidationResult> IsReachableURL(string value, AddServerModel model, CancellationToken token)
        {
            ValidationResult isReachableURL = null;

            try
            {
                JenkinsClient client = JenkinsClientFactory.CreateJenkinsClient(value, model.Login, model.Password);
                Server server = await client.GetJson<Server>(new Uri(value.ToString()), token);
                isReachableURL = ValidationResult.Success;
            }
            catch (TaskCanceledException ex)
            {
                isReachableURL = new ValidationResult("URL Invalid", new List<string> { "Url" });
            }
            catch (Exception e)
            {
                isReachableURL = new ValidationResult("Url is unreacheable", new List<string> { "Url" });
            }
            return isReachableURL;
        }
    }
}
