using System.Threading.Tasks;

namespace Application.Sheets
{
    public interface ISaveable
    {
        Task Save();

        Task Refresh();
    }
}
