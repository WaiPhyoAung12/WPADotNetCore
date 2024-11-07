namespace WPADotNetCore.MinimalApi.EndPoints.Blog
{
    public static class BlogEndPoint
    {
        public static void MapBlogEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/getBlogs", () =>
            {
                AppDbContext appDbContext = new AppDbContext();
                var response = appDbContext.TblBlogs.AsNoTracking().ToList();
                if (response.Count <= 0)
                    return Results.BadRequest("No Record Found");
                return Results.Ok(response);
            });

            app.MapGet("/getBlogById/{id}", (int id) =>
            {
                AppDbContext dbContext = new AppDbContext();
                var response = dbContext.TblBlogs.AsNoTracking().Where(x => x.BlogId == id).FirstOrDefault();
                if (response is null)
                    return Results.BadRequest("No Record Found");
                return Results.Ok(response);
            });

            app.MapPost("/CreateBlog", (TblBlog blog) =>
            {
                AppDbContext dbContext = new AppDbContext();
                dbContext.TblBlogs.Add(blog);
                int result = dbContext.SaveChanges();
                if (result <= 0)
                    return Results.BadRequest("Fail To Insert");
                return Results.Ok(blog);
            });

            app.MapPut("/updateBlog/{id}", (int id, TblBlog tblBlog) =>
            {
                AppDbContext appDbContext = new AppDbContext();
                var response = appDbContext.TblBlogs.AsNoTracking().Where(x => x.BlogId == id).FirstOrDefault();
                if (response is null)
                    return Results.BadRequest("No Record Found");

                response.BlogTitle = tblBlog.BlogTitle;
                response.BlogAuthor = tblBlog.BlogAuthor;
                response.BlogContent = tblBlog.BlogContent;

                appDbContext.Entry(response).State = EntityState.Modified;
                int result = appDbContext.SaveChanges();
                if (result <= 0)
                    return Results.BadRequest("Failed To Update");
                return Results.Ok(response);
            });

            app.MapDelete("/deleteBlog/{id}", (int id) =>
            {
                AppDbContext appDbContext = new AppDbContext();
                var response = appDbContext.TblBlogs.AsNoTracking().FirstOrDefault(x => x.BlogId == id);
                if (response is null)
                    return Results.BadRequest("No Record Found");

                appDbContext.Entry(response).State = EntityState.Deleted;
                int result = appDbContext.SaveChanges();
                if (result <= 0)
                    return Results.BadRequest("Failed To Delete");
                return Results.Ok(response);
            });
        }
    }
}
