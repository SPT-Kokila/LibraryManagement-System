using Pratice.Model;
using PraticelinqDatabseContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Repository
{
    public class AdditionalRepository : IAdditionalRepository
    {
        //public async Task <string> TotalIssueBook(bookModel bookModel,int Uid)
        public async Task <IEnumerable> TotalIssueBook(ReportModel reportModel ,int Uid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                BookDetail bookDetail = context.BookDetails.SingleOrDefault(x => x.BookName == reportModel.BookName);
                if(bookDetail != null)
                {
                    var query = (from r1 in context.Reports 
                                 join u1 in context.User1s 
                                 on r1.UserID  equals u1.UserID  
                                 where bookDetail.BookID  == r1.BookID && r1.IsIssue == true 
                                 select new
                                 {
                                     u1.UserName,

                                 }).ToList();
                    var count = query.Count();
                    if (query != null && count != 0)
                    {
                        return query;
                    }
                    else
                    {
                        return "Not Issue";
                    }
                }
                else
                {
                    return $"No such book";
                }
            }
        }

        public async Task<IEnumerable> searchBook(ReportModel reportModel, int Uid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                var query = (from a1 in context.Authors
                             join ba1 in context.BookAuthors
                             on a1.AuthorID equals ba1.AuthorID
                             join bd in context.BookDetails
                             on ba1.BookID equals bd.BookID
                             where bd.BookName.Contains(reportModel.BookName)
                             select new
                             {
                                 bd.BookName,
                                 a1.AuthorName,
                                 bd.Publisher,
                                 bd.Edition,
                                 bd.Price
                             }).ToList();
                if (query != null)
                {
                    return query;
                }
                else
                {
                    return "";
                }
                
            }
        }

        public async Task<IEnumerable> GetAllAUthorbookdetail(AuthorModel authorModel)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                var query = (from a1 in context.Authors
                             join ba1 in context.BookAuthors
                             on a1.AuthorID equals ba1.AuthorID
                             join bd in context.BookDetails
                             on ba1.BookID equals bd.BookID
                             where a1.AuthorName == authorModel.Authorname
                             select new
                             {
                                 bd.BookName,
                                 bd.Publisher ,
                                 bd.Price,
                                 bd.Noofcopies 
                             }).ToList();
                var count = query.Count();
                if (query != null && count != 0)
                { 
                    return query;
                }
                else{
                    return "No such Author available";
                }
            }
        }
    }
}
