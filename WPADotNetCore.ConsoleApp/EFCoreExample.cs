using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPADotNetCore.ConsoleApp.Models;

namespace WPADotNetCore.ConsoleApp
{
    public class EFCoreExample
    {
        public void Read()
        {
            AppDbContext db = new AppDbContext();
            var lst = db.blogData.Where(b => b.DeleteFlag == false).ToList();
            foreach (var item in lst)
            {
                Console.WriteLine(item.BlogContent);
                Console.WriteLine(item.BlogAuthor);
                Console.WriteLine(item.BlogTitle);

            }
        }
        public void Create(string blogContent,string blogAuthor,string blogTitle)
        {
            BlogDataModel blogDataModel = new BlogDataModel()
            {
                BlogAuthor = blogAuthor,
                BlogContent = blogContent,
                BlogTitle = blogTitle
            };
            AppDbContext db= new AppDbContext();
            db.blogData.Add(blogDataModel);
            int result=db.SaveChanges();
            Console.WriteLine(result > 0 ? "Successfully Created": "Fail Creation");
        }
        public void Edit(int blogId)
        {
            AppDbContext db = new AppDbContext();
            var item = db.blogData.AsNoTracking().Where(b => b.BlogId == blogId).FirstOrDefault();
            if (item is null)
            {
                Console.WriteLine("No Record Found");
                return;
            }
            Console.WriteLine(item.BlogContent);
            Console.WriteLine(item.BlogAuthor);
            Console.WriteLine(item.BlogTitle);
        }
        public void Update(int blogId, string blogTitle, string blogContent, string blogAuthor)
        { 
            AppDbContext db=new AppDbContext();
            var item=db.blogData.Where(b=>b.BlogId==blogId && b.DeleteFlag==false).FirstOrDefault();
            if (item is null) {
                Console.WriteLine("No Record Found");
                return;
            }
            if (!string.IsNullOrEmpty(blogTitle)) 
            {
                item.BlogTitle = blogTitle;
            }
            if (!string.IsNullOrEmpty(blogContent)) 
            {
                item.BlogContent = blogContent; 
            }
            if (!string.IsNullOrEmpty(blogAuthor)) 
            { 
                item.BlogAuthor = blogAuthor;
            }
            db.blogData.Entry(item).State=EntityState.Modified;
            var result=db.SaveChanges();
            Console.WriteLine(result>0?"Successfully Updated":"Failed in Update");
        }
    }
}
