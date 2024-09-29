using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPADotNetCore.Databases.Models;

namespace WPADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _db=new AppDbContext();
        [HttpGet]
        public IActionResult GetBlog() 
        {
            var lst=_db.TblBlogs.Where(b=>b.DeleteFlag==false).ToList();
            return lst.Count > 0 ? Ok(lst) : NotFound();
        }
        [HttpPost]
        public IActionResult CreateBlog(TblBlog blog)
        {
            _db.TblBlogs.Add(blog);
            int result = _db.SaveChanges();

            return result>0?Ok(blog):NotFound();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id,TblBlog blog)
        {
            var item=_db.TblBlogs.AsNoTracking().FirstOrDefault(b=>b.BlogId==id&&b.DeleteFlag==false);
            if (item == null)
            {
                return NotFound();
            }
            item.BlogTitle = blog.BlogTitle;
            item.BlogContent = blog.BlogContent;
            item.BlogAuthor = blog.BlogAuthor;
            _db.TblBlogs.Entry(item).State=EntityState.Modified;
            int result = _db.SaveChanges(); 
            return result>0?Ok(item) : NotFound();
        }
        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id,TblBlog blog)
        {
            var item=_db.TblBlogs.AsNoTracking().FirstOrDefault(b=>b.BlogId==id&&b.DeleteFlag==false);
            if (item == null)
            {
                return NotFound();
            }
            item.BlogTitle = blog.BlogTitle;
            item.BlogContent = blog.BlogContent;    
            item.BlogAuthor = blog.BlogAuthor;
            _db.TblBlogs.Entry(item).State = EntityState.Modified;
            int result= _db.SaveChanges();

            return result>0?Ok(item):NotFound();
        }
        [HttpDelete("{id}")]    
        public IActionResult DeleteBlog(int id)
        {
            var item=_db.TblBlogs.AsNoTracking().Where(b=>b.BlogId==id && b.DeleteFlag==false).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            item.DeleteFlag = true;
            _db.TblBlogs.Entry(item).State= EntityState.Modified;
            int result= _db.SaveChanges();

            return result>0?Ok(item):NotFound();
        }
    }
}
