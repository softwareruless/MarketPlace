using MarketPlace.Data.Model;
using MarketPlace.Data.Model.ReturnModel;

namespace MarketPlace.Service.Interfaces
{
    public interface IBlogService
    {
        Task<ResponseModel> Create(BlogAddModel blogAddModel);
        Task<ResponseModel> Update(BlogUpdateModel blogUpdateModel);
        ResponseModel Delete(int id);
        BlogDetailReturnModel GetBlog(int Id);
        DatatableModel<BlogDetail> GetBlogsAdmin(string sortColumn, string sortColumnDirection, string searchValue, int recordsTotal, int pageSize, int skip, string draw);
        DatatableModel<BlogDetail> GetBlogs(PagingParams model);
        ResponseModel<List<BlogDetail>> GetFeaturedBlogs();
        ResponseModel CreateTag(TagModel model);
        ResponseModel DeleteTag(int id);
        ResponseModel UpdateTag(TagModel model);
        TagDetailReturnModel GetTag(int Id);
        DatatableModel<TagModel> GetTagsAdmin(string sortColumn, string sortColumnDirection, string searchValue, int recordsTotal, int pageSize, int skip, string draw);
        TagsDetailReturnModel GetTags();



    }
}
