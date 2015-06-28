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
    public class JobsViewModel : ViewModelBase, ILazyLoader
    {
        private readonly ApplicationDataService _dataService;
        private List<JobModel> _jobs;
        private List<string> _serversFilter;
        private string _searchFilter;
        private string _selectedServerFilter;
        private List<JobModel> _alljobs;
        public ICommand SubscribeJob { get; private set; }

        public List<JobModel> Jobs
        {
            get { return _jobs; }
            set { Set(() => Jobs, ref _jobs, value); }
        }

        public List<string> ServersFilter
        {
            get { return _serversFilter; }
            set { Set(() => ServersFilter, ref _serversFilter, value); }
        }

        public string SearchFilter
        {
            get { return _searchFilter; }
            set
            {
                Set(() => SearchFilter, ref _searchFilter, value);
                Task.Run(() => InvalidateResult(SearchFilter, SelectedServerFilter));
            }
        }

        public string SelectedServerFilter
        {
            get { return _selectedServerFilter; }
            set
            {
                Set(() => SelectedServerFilter, ref _selectedServerFilter, value);
                Task.Run(() => InvalidateResult(SearchFilter, SelectedServerFilter));
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public JobsViewModel(ApplicationDataService dataService)
        {
            _dataService = dataService;
            Jobs = new List<JobModel>();
            SubscribeJob = new RelayCommand<JobModel>(OnJobSubscribing);
        }

        private async void OnJobSubscribing(JobModel obj)
        {
            if (obj.Subscribe)
            {
                await _dataService.AddSubscribe(obj);
            }
            else
            {
                await _dataService.RemoveSubscribe(obj);
            }
        }

        public async Task LoadAsync()
        {
            _alljobs = await Task.Run(() => _dataService.GetJobsAsync());           
            var serversFilter = _alljobs.Select(x => x.ServerName).Distinct().ToList();
            serversFilter.Insert(0, "All");
            ServersFilter = serversFilter;
            _selectedServerFilter = ServersFilter[0];
            RaisePropertyChanged(() => SelectedServerFilter);
            Jobs = new List<JobModel>(_alljobs);
        }

        private void InvalidateResult(string searchFilter, string serverFilter)
        {
            var result = _alljobs.AsEnumerable();
            if(serverFilter != "All")
                result = result.Where(x => x.ServerName.Equals(serverFilter, StringComparison.InvariantCultureIgnoreCase));
            if(!string.IsNullOrEmpty(searchFilter))
                result = result.Where(x => x.Name.ToLower().Contains(searchFilter.ToLower()));
            Jobs = result.ToList();
        }

        private void RefreshAsync()
        {

        }
    }
}