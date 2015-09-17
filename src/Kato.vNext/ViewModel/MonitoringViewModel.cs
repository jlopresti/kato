using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
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
    public class MonitoringViewModel : ViewModelBase, ILazyLoader
    {
        private readonly ApplicationDataService _dataService;
        private ObservableCollection<MonitoringServer> _websites;

        public ObservableCollection<MonitoringServer> Websites
        {
            get { return _websites; }
            set { Set(() => Websites, ref _websites, value); }
        }
        public ICommand ShowAddMonitoringDialogCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MonitoringViewModel(ApplicationDataService dataService)
        {
            _dataService = dataService;
            Websites=new ObservableCollection<MonitoringServer>();
            ShowAddMonitoringDialogCommand = new RelayCommand(ShowAddMonitoringDialog);
            Messenger.Default.Register<MonitoringServerAddedMessage>(this, OnServerAdded);
        }

        public async Task LoadAsync()
        {
            if (_timer == null)
            {
                RefreshAsync();
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(10);
                _timer.Tick += _timer_Tick;
                _timer.Start();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            RefreshAsync();
        }

        private DispatcherTimer _timer;

        private void RefreshAsync()
        {
            foreach (var wb in Websites)
            {
                wb.RefreshAsync();
            }
        }

        private async void OnServerAdded(MonitoringServerAddedMessage serverMessage)
        {
            Websites.Add(serverMessage.Website);
        }

        private void ShowAddMonitoringDialog()
        {
            Messenger.Default.Send(new OpenAddMonitoringDialogMessage());
        }
    }
}