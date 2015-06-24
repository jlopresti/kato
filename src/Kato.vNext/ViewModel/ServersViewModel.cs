using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Jenkins.Api.Client;

namespace Kato.vNext.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ServersViewModel : ViewModelBase
    {
        private IList<int> _servers;
        private bool _isModalOpened;

        public IList<int> Servers
        {
            get { return _servers; }
            set { Set(() => Servers, ref _servers, value); }
        }

        public bool IsModalOpened
        {
            get { return _isModalOpened; }
            set { Set(() => IsModalOpened, ref _isModalOpened, value); }
        }

        public ICommand ShowAddServerDialogCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ServersViewModel()
        {
            Servers = new List<int> { 1 };
            ShowAddServerDialogCommand = new RelayCommand(ShowAddServerDialog);
        }

        private async void ShowAddServerDialog()
        {
            Servers = new List<int> { 1,2,4 };
            IsModalOpened = !IsModalOpened;
            try
            {
                JenkinsClient client = new JenkinsClient(new Uri("http://dotnet-ci.cloudapp.net/", UriKind.Absolute));
                Server server = await client.GetJson<Server>(new Uri("http://dotnet-ci.cloudapp.net/"));

                
            }
            catch (Exception)
            {
                
            }
        }
    }
}