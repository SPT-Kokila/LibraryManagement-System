using Pratice.Model;
using System.Collections;
using System.Threading.Tasks;

namespace Pratice.Repository
{
    public interface IAdditionalRepository
    {
        Task<IEnumerable> TotalIssueBook(ReportModel reportModel, int Uid);
        Task<IEnumerable> searchBook(ReportModel reportModel, int Uid);
        Task<IEnumerable> GetAllAUthorbookdetail(AuthorModel authorModel);

    }
}