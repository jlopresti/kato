using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Kato.vNext.Messages;
using Kato.vNext.Models;
using Kato.vNext.Services;

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
        private ObservableCollection<ServerModel> _servers;

        public ObservableCollection<ServerModel> Servers
        {
            get { return _servers; }
            set { Set(() => Servers, ref _servers, value); }
        }

        public ICommand ShowAddServerDialogCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ServersViewModel()
        {
            Servers = new ObservableCollection<ServerModel>();
            ShowAddServerDialogCommand = new RelayCommand(ShowAddServerDialog);
            Messenger.Default.Register<ServerAddedMessage>(this, OnServerAdded);
            var data = new UserDataService().RetrieveServers();
            foreach (var serverModel in data)
            {
                Servers.Add(serverModel);
            }
        }

        private void OnServerAdded(ServerAddedMessage serverMessage)
        {
            Servers.Add(serverMessage.Server);
            new UserDataService().SaveData(Servers.ToList());
        }

        private void ShowAddServerDialog()
        {
            Messenger.Default.Send(new OpenAddServerDialogMessage());
        }
    }
}