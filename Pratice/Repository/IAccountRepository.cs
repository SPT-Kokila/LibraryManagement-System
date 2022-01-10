
using Pratice.Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratice.Repository
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(userModel u1);
        Task<IEnumerable> GetAllUserDetail();
        Task<IEnumerable> SignUPAsync(userModel u1);
    }    
}