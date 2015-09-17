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
    public class TaskbarViewModel : ViewModelBase
    {
        private readonly ApplicationDataService _dataService;
        private readonly IWindowService _windowService;
        private readonly NotificationService _notificationService;

        public ICommand OpenApplicationCommand { get; private set; }

        private List<JobModel> _jobs;

        public List<JobModel> Jobs
        {
            get { return _jobs; }
            set { Set(() => Jobs, ref _jobs, value); }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public TaskbarViewModel(ApplicationDataService dataService, IWindowService windowService, NotificationService notificationService)
        {
            _dataService = dataService;
            _windowService = windowService;
            _notificationService = notificationService;
            OpenApplicationCommand = new RelayCommand(OpenApplication, () => _windowService.IsMinimized() || _windowService.IsNormal());    
            Messenger.Default.Register<SubscriptionNotificationMessage>(this, OnSubscriptionUpdated);
        }

        private void OnSubscriptionUpdated(SubscriptionNotificationMessage obj)
        {
            if (Jobs != null)
            {
                RemoveOldEvents(Jobs);
            }

            Jobs = obj.Jobs.ToList();          
            AttachToEvent(Jobs);
        }

        private void RemoveOldEvents(List<JobModel> jobs)
        {
            foreach (var job in jobs)
            {
                job.StatusChanged -= Jobs_StatusChanged;
            }
        }

        private void AttachToEvent(List<JobModel> jobs)
        {
            foreach (var job in jobs)
            {
                job.StatusChanged += Jobs_StatusChanged;
            }
        }

        private void Jobs_StatusChanged(object sender, StatusChangedArgs args)
        {
            JobModel job = (JobModel)sender;
            if (args.OldValue == BuildStatus.Unknown)
                return;

            if (args.NewValue == BuildStatus.Failed)
                _notificationService.ShowError(job.Name, "Build " + args.NewValue);
            else if (args.NewValue == BuildStatus.Success && args.OldValue < BuildStatus.Success)
                _notificationService.ShowSuccess(job.Name, "Build " + args.NewValue);
            else if ((args.NewValue == BuildStatus.SuccessAndBuilding || args.NewValue == BuildStatus.FailedAndBuilding) && args.NewValue != args.OldValue)
                _notificationService.ShowInfo(job.Name, "Build started !");
        }

        private void OpenApplication()
        {
            if (_windowService.IsMinimized())
            {
                _windowService.GoToNormal();
                _windowService.ShowInTaskbar();
            }
            else if (_windowService.IsNormal())
            {
                _windowService.ShowWindow();
            }
        }
    }
}