using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WPADotNetCore.Databases.Models;

namespace WPADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext db;
        [HttpGet]
        public IActionResult GetBlog() 
        {
            var lst=db.TblBlogs.ToList();
            return Ok(lst);
        }
        [HttpPost]
        public IActionResult CreateBlog()
        {
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateBlog()
        {
            return Ok();
        }
        [HttpPatch]
        public IActionResult PatchBlog()
        {

            return Ok();
        }
        [HttpDelete]    
        public IActionResult DeleteBlog()
        {
            return Ok();
        }
    }
}
