using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Kato.vNext.Core;
using Kato.vNext.Messages;
using Kato.vNext.Models;

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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
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

        public Task LoadAsync()
        {
            return Task.FromResult(1);
        }
    }
}