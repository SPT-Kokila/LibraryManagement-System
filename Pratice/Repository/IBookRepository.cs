using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Pratice.Model;
using System.Collections;
using System.Threading.Tasks;

namespace Pratice.Repository
{
    public interface IBookRepository
    {
        Task<string> InsertBookDetail(bookModel bookModel, int Uid);
        Task<string> UpdateBookDetail(bookModel bookModel, int Uid, int bookid);
        Task<string> DeleteBookDetail(int Uid, int bookid);
        Task<string> IssueBookDetail(bookModel bookModel, int Uid);
        Task<string> ReturnBookDetail(bookModel bookModel, int Uid);
        Task<string> UpdateDetail(JsonPatchDocument bookModel, int Uid, int bookid);
        Task<string> InsertAuthorDetail(bookModel bookModel, int Uid);
        Task<string> IssueBookSLipDetail(ReportModel reportModel , int Uid);
        Task<string> ReturnBookSlipDetail(ReportModel reportModel, int Uid);
        Task<IEnumerable> GetAllBook(int Uid);

    }
}