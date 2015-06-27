using System.Threading.Tasks;

namespace Kato.vNext.Core
{
    internal interface ILazyLoader
    {
        Task LoadAsync();
    }
}