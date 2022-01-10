using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pratice.Model;
using Pratice.Repository;
using PraticelinqDatabseContext;
using System.Linq;
using System.Threading.Tasks;


namespace Pratice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        //Add Book Detail
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> InsertBook([FromBody]bookModel bookModel)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if(rid == 1)
            {
                var result = await _bookRepository.InsertBookDetail(bookModel, uid);
                return Ok(result);
            }
            return Unauthorized ("UnAuthorize User");
            
        }

        //Update Book Detail
        [Authorize]
        [HttpPost("update/{bookid}")]
        public async Task<IActionResult> UpdateBook([FromBody]bookModel bookModel,[FromRoute]int bookid)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if (rid == 1)
            {
                var result = await _bookRepository.UpdateBookDetail(bookModel, uid, bookid);
                return Ok(result);
            }
            return Unauthorized("UnAuthorize User");
        }

        //Delete Book Detail
        [Authorize]
        [HttpDelete("delete/{bookid}")]
        public async Task<IActionResult> DeleteBook([FromRoute]int bookid)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if (rid == 1)
            {
                var result = await _bookRepository.DeleteBookDetail(uid, bookid);
                return Ok(result);
            }
            return Unauthorized("UnAuthorize User");
        }

        //Issue Book Detail
        [Authorize]
        [HttpPost("issue1")]
        public async Task<IActionResult> IssueBook([FromBody]bookModel bookModel)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if(rid == 2)
            {
                var result = await _bookRepository.IssueBookDetail(bookModel, uid);
                return Ok(result);
            }
            return Unauthorized("UnAuthorize User");
        }
    

        //Return Book Detail
        [Authorize]
        [HttpPost("return2")]
        public async Task<IActionResult> ReturnBook([FromBody] bookModel bookModel)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
             if (rid == 2)
             {
                var result = await _bookRepository.ReturnBookDetail(bookModel, uid);
                 return Ok(result);
             }
            return Unauthorized("UnAuthorize User");
        }
       

        [HttpGet ("all")]
        //example
        public IActionResult GetAll([FromBody] bookModel bookModel)
        {
            PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext();
            var name = bookModel.nm;
            var name1 = (from i in context.Authors
                         select i.AuthorName).ToArray();

            var result = name.Except(name1);
            foreach (var item in result)
            {
                context.Authors.InsertOnSubmit(new Author() { AuthorName = item });
            }
            return Ok();
        }

        //Update Patch Book Detail
        [Authorize]
        [HttpPatch("update1/{bookid}")]
        public async Task<IActionResult> Update([FromBody] JsonPatchDocument bookModel, int bookid)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if(rid == 1)
            {
                var result = await _bookRepository.UpdateDetail(bookModel, uid, bookid);
                return Ok(result);
            }
            return Unauthorized("UnAuthorize aacess");
        }

        //Add Book + Author Book Detail
        [Authorize]
        [HttpPost("add1")]
        public async Task<IActionResult> InsertABook([FromBody] bookModel bookModel )
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if (rid == 1 )
            {
                var result = await _bookRepository.InsertAuthorDetail(bookModel, uid);
                return Ok(result);
            }
            return Unauthorized ("UnAuthorize aacess");
        }

        [Authorize]
        [HttpPost("issue")]
        public async Task<IActionResult> IssueBookReport([FromBody] ReportModel reportModel )
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if(rid == 2)
            {
                var result = await _bookRepository.IssueBookSLipDetail(reportModel, uid);
                return Ok(result);
            }
            return Unauthorized("UnAuthorize aacess");
        }

        [HttpPost("return")]
        [Authorize]
        public async Task<IActionResult> ReturnBookReport([FromBody] ReportModel reportModel)
        {
            int uid = (int)HttpContext.Items["UserId"];
            int rid = (int)HttpContext.Items["RoleId"];
            if(rid == 2)
            {
                var result = await _bookRepository.ReturnBookSlipDetail(reportModel, uid);
                return Ok(result);
            }
            return Unauthorized("UnAuthorize aacess");
        }

        [HttpGet("getallbook")]
        public async Task<IActionResult> GetAllBookdetail()
        {
            int uid = (int)HttpContext.Items["UserId"];
            var result = _bookRepository.GetAllBook(uid);
            return Ok(result.Result);
        }
    }
}
