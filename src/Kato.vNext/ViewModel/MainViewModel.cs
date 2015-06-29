using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Kato.vNext.Core;
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
    public class MainViewModel : ViewModelBase, ILazyLoader
    {
        private readonly ApplicationDataService _dataService;
        private bool _isAddServerModalOpened;

        public bool IsAddServerModalOpened
        {
            get { return _isAddServerModalOpened; }
            set { Set(() => IsAddServerModalOpened, ref _isAddServerModalOpened, value); }
        }

        private AddServerModel _addServerModel;

        public AddServerModel AddServerModel
        {
            get { return _addServerModel; }
            set { Set(() => AddServerModel, ref _addServerModel, value); }
        }
        private List<JobModel> _jobs;

        public List<JobModel> Jobs
        {
            get { return _jobs; }
            set { Set(() => Jobs, ref _jobs, value); }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ApplicationDataService dataService)
        {
            _dataService = dataService;
            Messenger.Default.Register<OpenAddServerDialogMessage>(this, OpenAddServerDialogRequested);
            Messenger.Default.Register<ServerAddedMessage>(this, OnServerAdded);
        }

        private void OnServerAdded(ServerAddedMessage obj)
        {
            IsAddServerModalOpened = false;
        }

        private void OpenAddServerDialogRequested(OpenAddServerDialogMessage message)
        {
            AddServerModel = new AddServerModel();
            IsAddServerModalOpened = true;
        }

        public async Task LoadAsync()
        {
            Jobs = await Task.Run(() => _dataService.GetSubscribedJobs());
            Messenger.Default.Send(new SubscriptionNotificationMessage(Jobs));
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
            foreach (var jobs in Jobs)
            {
                jobs.RefreshAsync();
            }
        }
    }
}