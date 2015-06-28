using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Kato.vNext.Core;
using Kato.vNext.Messages;
using Kato.vNext.Models;
using Kato.vNext.Repositories;
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
    public class ServersViewModel : ViewModelBase, ILazyLoader
    {
        private readonly ApplicationDataService _dataService;
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
        public ServersViewModel(ApplicationDataService dataService)
        {
            _dataService = dataService;
            Servers = new ObservableCollection<ServerModel>();
            ShowAddServerDialogCommand = new RelayCommand(ShowAddServerDialog);
            Messenger.Default.Register<ServerAddedMessage>(this, OnServerAdded);
        }

        public async Task LoadAsync()
        {
            var servers = await Task.Run(() => _dataService.GetServersAsync());
            Servers = new ObservableCollection<ServerModel>(servers);
            RefreshAsync();
        }

        private void RefreshAsync()
        {
            foreach (var serverModel in Servers)
            {
                serverModel.RefreshAsync();
            }
        }

        private async void OnServerAdded(ServerAddedMessage serverMessage)
        {
            Servers.Add(serverMessage.Server);
            await _dataService.AddServerAsync(serverMessage.Server);
        }

        private void ShowAddServerDialog()
        {
            Messenger.Default.Send(new OpenAddServerDialogMessage());
        }
    }
}