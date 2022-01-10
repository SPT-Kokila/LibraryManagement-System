using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Pratice.Model;
using PraticelinqDatabseContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Pratice.Repository
{
    public class BookRepository : IBookRepository
    {
        //Insert book deatail
        public async Task<string> InsertBookDetail(bookModel bookModel, int Uid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                BookDetail bookDetail = new BookDetail();
                bookDetail.BookName = bookModel.BookName;
                bookDetail.Publisher = bookModel.Publisher;
                bookDetail.Edition = bookModel.Edition;
                bookDetail.Noofcopies = bookModel.noofcopies;
                bookDetail.Price = bookModel.price;
                bookDetail.UserID = Uid;
                context.BookDetails.InsertOnSubmit(bookDetail);
                context.SubmitChanges();

            }
            return $"{bookModel.BookName } Book Added Successfully";

        }
        //Update book using post deatail
        public async Task<string> UpdateBookDetail(bookModel bookModel, int Uid, int bookid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                BookDetail bookDetail = new BookDetail();
                bookDetail = context.BookDetails.SingleOrDefault(x => x.BookID == bookid && x.UserID == Uid);
                if (bookDetail != null)
                {
                    bookDetail.BookName = bookModel.BookName;
                    bookDetail.Publisher = bookModel.Publisher;
                    bookDetail.Edition = bookModel.Edition;
                    bookDetail.Noofcopies = bookModel.noofcopies;
                    bookDetail.Price = bookModel.price;
                    context.SubmitChanges();
                    return $"{bookDetail.BookName }Book Update successfully";
                }
                return "Not Allow to update this data";
            }
        }
        //Delete book deatail
        public async Task<string> DeleteBookDetail(int Uid, int bookid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                BookDetail bookDetail = new BookDetail();
                bookDetail = context.BookDetails.SingleOrDefault(x => x.BookID == bookid && x.UserID == Uid);
                if (bookDetail != null)
                {
                    var delete = (from d1 in context.BookDetails
                                  where d1.BookID == bookid
                                  select d1).FirstOrDefault();
                    context.BookDetails.DeleteOnSubmit(delete);
                    context.SubmitChanges();
                    return $"Book delete";
                }
                return "Not Allow to delete this data";
            }
        }
        //Issue book deatail
        public async Task<string> IssueBookDetail(bookModel bookModel, int Uid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                BookIssue bookIssue = new BookIssue();
                BookDetail bd = context.BookDetails.SingleOrDefault(x => x.BookName == bookModel.BookName);
                bookIssue.BName = bookModel.BookName;
                bookIssue.UserID = Uid;
                bookIssue.Issuedate = DateTime.Now;
                bookIssue.Returndate = DateTime.Now.AddDays(15);
                bookIssue.BookID = bd.BookID;
                bd.Noofcopies = bd.Noofcopies - 1;
                context.BookIssues.InsertOnSubmit(bookIssue);
                context.SubmitChanges();
                return $"{bookIssue.BName } Issue ";

            }
            return "not issue";
        }
        //Return book deatail
        public async Task<string> ReturnBookDetail(bookModel bookModel, int Uid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                BookReturn bookReturn = new BookReturn();

                BookIssue bookIssue = context.BookIssues.SingleOrDefault(x => x.BName == bookModel.BookName && x.UserID == Uid);
                bookReturn.BName = bookModel.BookName;
                bookReturn.UserID = Uid;
                bookReturn.Returndate = DateTime.Now;
                BookDetail copy = context.BookDetails.SingleOrDefault(x => x.BookName == bookModel.BookName);
                copy.Noofcopies = copy.Noofcopies + 1;
                int Days = Convert.ToInt32((DateTime.Now - ((DateTime)bookIssue.Returndate)).Days);
                if (Days >= 0)
                {
                    bookReturn.Extraday = Days;
                }
                else
                {
                    bookReturn.Extraday = 0;
                }
                var delete = (from d1 in context.BookIssues
                              where d1.BName == bookModel.BookName && d1.UserID == Uid
                              select d1).FirstOrDefault();
                context.BookIssues.DeleteOnSubmit(delete);
                context.BookReturns.InsertOnSubmit(bookReturn);
                context.SubmitChanges();
                return "Book Return successfully";
            }
            //return "not issue";
        }
        //Update book using patch deatail
        public async Task<string> UpdateDetail(JsonPatchDocument bookModel, int Uid, int bookid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                BookDetail bookDetail = new BookDetail();
                bookDetail = context.BookDetails.SingleOrDefault(x => x.BookID == bookid && x.UserID == Uid);
                if (bookDetail != null)
                {
                    bookModel.ApplyTo(bookDetail);
                    context.SubmitChanges();
                    return "Book Update successfully";
                }
                return "Not Allow to update this data";
            }
        }
        //Insert book + author + bookauthor deatail
        public async Task<string> InsertAuthorDetail(bookModel bookModel, int Uid)
        {
             using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
             {
                    BookDetail bookDetail = new BookDetail();
                    bookDetail.BookName = bookModel.BookName;
                    bookDetail.Publisher = bookModel.Publisher;
                    bookDetail.Edition = bookModel.Edition;
                    bookDetail.Noofcopies = bookModel.noofcopies;
                    bookDetail.Price = bookModel.price;
                    bookDetail.UserID = Uid;
                     context.BookDetails.InsertOnSubmit(bookDetail);
                        context.SubmitChanges();

                        Author author = new Author();
                        var name = bookModel.nm;
                        var name1 = (from i in context.Authors
                                     select i.AuthorName).ToArray();

                        var result = name.Except(name1);
                        foreach (var item in result)
                        {
                            context.Authors.InsertOnSubmit(new Author() { AuthorName = item });
                            context.SubmitChanges();
                        }
                        BookAuthor bookAuthor = new BookAuthor();
                        if (name != null)
                        {
                            foreach (var item in name)
                            {
                                var id = (from r in context.Authors
                                          where r.AuthorName == item
                                          select r.AuthorID).SingleOrDefault();

                                var bid1 = (from r in context.BookDetails
                                            where r.BookName == bookModel.BookName
                                            select r.BookID).SingleOrDefault();
                                context.BookAuthors.InsertOnSubmit(new BookAuthor() { AuthorID = id, BookID = bid1 });
                            }
                        }
                        context.SubmitChanges();
                        return " Book Added Successfully";
                   
             }
        }

        //Issue report table deatail
        public async Task<string> IssueBookSLipDetail( ReportModel reportModel , int Uid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                try
                {
                    Report report = new Report();
                    BookDetail bd = context.BookDetails.SingleOrDefault(x => x.BookName == reportModel.BookName);
                    
                    if(bd != null) 
                    {
                        var check = context.Reports.SingleOrDefault(x => x.BookID == bd.BookID && x.UserID == Uid && x.IsIssue == true);
                        if (check == null)
                        {
                            report.UserID = Uid;
                            User1 user1 = context.User1s.SingleOrDefault(x => x.UserID == Uid);
                            report.BookID = bd.BookID;
                            report.Issuedate = DateTime.Now;
                            report.Returndate = DateTime.Now.AddDays(15);
                            report.IsIssue = true;
                            if (bd.Noofcopies <= 0)
                            {
                                return "Book Not Available";
                            }
                            else
                            {
                                bd.Noofcopies = bd.Noofcopies - 1;
                            }
                            context.Reports.InsertOnSubmit(report);
                            context.SubmitChanges();
                            return $"{reportModel.BookName} Book Issue Successfully \n IssuerName : {user1.UserName } \n Issuedate : {report.Issuedate.ToString("dd-m-yy")} \n Returndate : {report.Returndate.ToString("dd-m-yy")} \n ";
                        }
                        else
                        {
                            throw new MethodAccessException();
                        }
                    }
                    else
                    {
                        return "No such book";
                    }
                       
                }
                catch (MethodAccessException e)
                {
                    return "Already Issue";
                }
            }
        }

        //Return report table deatail
        public async Task<string> ReturnBookSlipDetail(ReportModel reportModel , int Uid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                BookReturn bookReturn = new BookReturn();
                BookDetail bookDetail = context.BookDetails.SingleOrDefault(x => x.BookName == reportModel.BookName);
               if(bookDetail != null)
                {
                    Report report = context.Reports.SingleOrDefault(x => x.BookID == bookDetail.BookID && x.UserID == Uid && x.IsIssue == true);
                    if (report != null)
                    {
                        int Days = Convert.ToInt32((DateTime.Now - ((DateTime)report.Returndate)).Days);
                        int day;
                        if (Days >= 0)
                        {
                            day = Days;
                        }
                        else
                        {
                            day = 0;
                        }
                        report.IsIssue = false;
                        report.Returndate = DateTime.Now;
                        bookDetail.Noofcopies = bookDetail.Noofcopies + 1;
                        context.SubmitChanges();
                        return $"Book Return successfully \n Extra day : {day}  ";
                    }
                    else
                    {
                        return "Book Already Return";
                    }
                }
                else
                {
                    return "No Such Book";
                }
               
            }
        }

        public async Task<IEnumerable> GetAllBook(int Uid)
        {
            using (PraticelinqDatabseDataContext context = new PraticelinqDatabseDataContext())
            {
                var query = (from  bd in context.BookDetails
                             select new
                             {
                                 bd.BookName,
                                 bd.Publisher,
                                 bd.Edition,
                                 bd.Price
                             }).ToList();
                    return query;
            }
        }
    }
}

