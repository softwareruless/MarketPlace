using Microsoft.AspNetCore.Identity;
using MarketPlace.Data.Entities;
using MarketPlace.Data;
using MarketPlace.Data.Model;
using AutoMapper;
using MarketPlace.Data.Model.ReturnModel;
using MarketPlace.Utilities;
using MarketPlace.Service.Interfaces;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace MarketPlace.Service
{
    public class BlogService : IBlogService
    {

        private readonly MarketPlaceDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public BlogService(MarketPlaceDbContext context, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ResponseModel> Create(BlogAddModel blogAddModel)
        {
            var resultPhotoUpload = await UploadPhotoHelper.UploadBlog(blogAddModel);

            if (!resultPhotoUpload.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = resultPhotoUpload.Message
                };
            }

            try
            {
                var blog = _mapper.Map<Blog>(blogAddModel);
                blog.CreatedDate = DateTime.Now;
                blog.PhotoUrl = resultPhotoUpload.Url;

                _context.Blog.Add(blog);

                var result = _context.SaveChanges();

                if (!(result > 0))
                {
                    return new ResponseModel()
                    {
                        Success = false,
                        Message = _configuration["BlogService.CreateBlog"]
                    };
                }

                if (blog.IsFeatured)
                {
                    UpdateFeature(blog.Id);
                }

                var tags = blogAddModel.TagRelations.Select(x => new TagRelation()
                {
                    BlogId = blog.Id,
                    TagId = x
                }).ToList();

                if (tags == null || !tags.Any())
                {
                    return new ResponseModel()
                    {
                        Message = _configuration["BlogService.NullTag"],
                        Success = false
                    };
                }

                _context.TagRelation.AddRange(tags);

                result = _context.SaveChanges();

                if (result > tags.Count() - 1)
                {
                    return new ResponseModel()
                    {
                        Success = true,
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        Success = false,
                        Message = _configuration["BlogService.CreateTag"]
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.CreateBlog"]
                };
            }

        }

        public async Task<ResponseModel> Update(BlogUpdateModel blogUpdateModel)
        {
            var resultPhotoUpload = await UploadPhotoHelper.UpdateBlog(blogUpdateModel);

            if (!resultPhotoUpload.Success)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = resultPhotoUpload.Message
                };
            }

            var blog = _context.Blog.Include(x => x.Tags).FirstOrDefault(x => x.Id == blogUpdateModel.Id);
            if (blog == null)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullBlog"]
                };
            }

            blog.UpdatedDate = DateTime.Now;
            blog.Title = blogUpdateModel.Title;
            blog.Description = blogUpdateModel.Description;
            blog.IsPremium = blogUpdateModel.IsPremium;
            blog.IsFeatured = blogUpdateModel.IsFeatured;
            blog.PhotoUrl = resultPhotoUpload.Url;

            _context.Blog.Update(blog);

            var result = _context.SaveChanges();

            if (!(result > 0))
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.UpdateBlog"]
                };
            }

            if (blog.IsFeatured)
            {
                UpdateFeature(blog.Id);
            }

            var tags = blogUpdateModel.TagRelations.Select(x => new TagRelation()
            {
                BlogId = blog.Id,
                TagId = x
            }).ToList();

            if (tags == null || !tags.Any())
            {
                return new ResponseModel()
                {
                    Message = _configuration["BlogService.NullTag"],
                    Success = false
                };
            }

            _context.TagRelation.RemoveRange(blog.Tags);
            _context.TagRelation.AddRange(tags);

            result = _context.SaveChanges();

            if (result > tags.Count() - 1)
            {
                return new ResponseModel()
                {
                    Success = true,
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.CreateTag"]
                };
            }

        }

        public ResponseModel Delete(int id)
        {
            var blog = _context.Blog.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (blog == null)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullBlog"]
                };
            }

            blog.UpdatedDate = DateTime.Now;
            blog.IsDeleted = true;

            _context.Blog.Update(blog);
            var result = _context.SaveChanges();

            if (result > 0)
            {
                return new ResponseModel()
                {
                    Success = true,
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.DeleteBlog"]
                };
            }

        }

        public BlogDetailReturnModel GetBlog(int Id)
        {
            var blog = _context.Blog.Include(x => x.Tags).ThenInclude(x => x.Tag).Where(x => x.Id == Id && !x.IsDeleted)
                .Select(x => new BlogDetail()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsPremium = x.IsPremium,
                    IsFeatured = x.IsFeatured,
                    PhotoUrl = x.PhotoUrl,
                    Date = x.CreatedDateStr,
                    Tags = x.Tags.ToList()
                }).FirstOrDefault();

            if (blog != null)
            {
                return new BlogDetailReturnModel()
                {
                    Blog = blog,
                    Success = true
                };
            }
            else
            {
                return new BlogDetailReturnModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullBlog"]
                };
            }
        }

        public DatatableModel<BlogDetail> GetBlogsAdmin(string sortColumn, string sortColumnDirection, string searchValue, int recordsTotal, int pageSize, int skip, string draw)
        {
            var blogs = _context.Blog.Where(x => !x.IsDeleted)
                .Select(x => new BlogDetail()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsPremium = x.IsPremium,
                    IsFeatured = x.IsFeatured,
                    PhotoUrl = x.PhotoUrl,
                    Date = x.CreatedDateStr,
                    Tags = x.Tags.ToList()
                }).AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                blogs = blogs.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                blogs = blogs.Where(m => (m.Title.ToLower() ?? string.Empty).Contains(searchValue.ToLower()));
            }

            recordsTotal = (blogs != null) ? blogs.Count() : 0;
            var data = blogs.Skip(skip).Take(pageSize).ToList();
            var jsonData = new DatatableModel<BlogDetail> { draw = draw, recordsFiltered = data.Count(), recordsTotal = recordsTotal, data = data };

            return jsonData;
        }

        public DatatableModel<BlogDetail> GetBlogs(PagingParams model)
        {
            var blogs = _context.Blog.Include(x=>x.Tags).ThenInclude(x=>x.Tag).Where(x => !x.IsDeleted)
                .Select(x => new BlogDetail()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsPremium = x.IsPremium,
                    IsFeatured = x.IsFeatured,
                    PhotoUrl = x.PhotoUrl,
                    Date = x.CreatedDateStr,
                    Tags = x.Tags.ToList()
                }).AsQueryable();

            if (!string.IsNullOrEmpty(model.searchValue))
            {
                blogs = blogs.Where(m => (m.Title.ToLower() ?? string.Empty).Contains(model.searchValue.ToLower()));
            }

            model.recordsTotal = (blogs != null) ? blogs.Count() : 0;
            var data = blogs.Skip(model.skip.GetValueOrDefault()).Take(model.pageSize.GetValueOrDefault()).ToList();
            var jsonData = new DatatableModel<BlogDetail> { draw = model.draw, recordsFiltered = data.Count(), recordsTotal = model.recordsTotal.GetValueOrDefault(), data = data };

            return jsonData;
        }

        public ResponseModel<List<BlogDetail>> GetFeaturedBlogs()
        {
            var blogs = _context.Blog.Where(x => !x.IsDeleted && x.IsFeatured).Include(x=>x.Tags).ThenInclude(x=>x.Tag)
                .Select(x => new BlogDetail()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsPremium = x.IsPremium,
                    IsFeatured = x.IsFeatured,
                    PhotoUrl = x.PhotoUrl,
                    Date = x.CreatedDateStr,
                    Tags = x.Tags.ToList()
                }).OrderBy(t => Guid.NewGuid()).Take(4).ToList();

            if (blogs.Any())
            {
                return new ResponseModel<List<BlogDetail>>()
                {
                    Data = blogs,
                    Success = true,
                };
            }
            else
            {
                return new ResponseModel<List<BlogDetail>>()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullBlog"]
                };
            }

        }

        public ResponseModel CreateTag(TagModel model)
        {

            var tag = _mapper.Map<Tag>(model);

            if (tag == null)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullTag"],
                };
            }

            _context.Tag.Add(tag);

            var result = _context.SaveChanges();

            if (result > 0)
            {
                return new ResponseModel()
                {
                    Success = true
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.CreateTag"]
                };
            }

        }

        public ResponseModel DeleteTag(int id)
        {
            var tag = _context.Tag.FirstOrDefault(x => x.Id == id);

            if (tag == null)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullTag"],
                };
            }

            _context.Tag.Remove(tag);

            var result = _context.SaveChanges();

            if (result > 0)
            {
                return new ResponseModel()
                {
                    Success = true
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.DeleteTag"]
                };
            }

        }

        public ResponseModel UpdateTag(TagModel model)
        {

            var tag = _context.Tag.FirstOrDefault(x => x.Id == model.Id);

            if (tag == null)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullTag"],
                };
            }

            tag.Name = model.Name;
            tag.Color = model.Color;

            _context.Tag.Update(tag);

            var result = _context.SaveChanges();

            if (result > 0)
            {
                return new ResponseModel()
                {
                    Success = true
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.DeleteTag"]
                };
            }

        }

        public TagDetailReturnModel GetTag(int Id)
        {
            var tag = _context.Tag.Where(x => x.Id == Id && !x.IsDeleted)
                .Select(x => new TagModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color,
                }).FirstOrDefault();

            if (tag != null)
            {
                return new TagDetailReturnModel()
                {
                    Tag = tag,
                    Success = true
                };
            }
            else
            {
                return new TagDetailReturnModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullBlog"]
                };
            }
        }

        public DatatableModel<TagModel> GetTagsAdmin(string sortColumn, string sortColumnDirection, string searchValue, int recordsTotal, int pageSize, int skip, string draw)
        {
            var tags = _context.Tag.Where(x => !x.IsDeleted)
                .Select(x => new TagModel()
                {
                    Id = x.Id,
                    Color = x.Color,
                    Name = x.Name,
                }).AsQueryable();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                tags = tags.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                tags = tags.Where(m => (m.Name.ToLower() ?? string.Empty).Contains(searchValue.ToLower()));
            }

            recordsTotal = (tags != null) ? tags.Count() : 0;
            var data = tags.Skip(skip).Take(pageSize).ToList();
            var jsonData = new DatatableModel<TagModel> { draw = draw, recordsFiltered = data.Count(), recordsTotal = recordsTotal, data = data };

            return jsonData;
        }

        public TagsDetailReturnModel GetTags()
        {
            var tags = _context.Tag.Where(x => !x.IsDeleted)
            .Select(x => new TagModel()
            {
                Id = x.Id,
                Color = x.Color,
                Name = x.Name,
            }).ToList();

            if (tags != null)
            {
                return new TagsDetailReturnModel()
                {
                    Tags = tags,
                    Success = true
                };
            }
            else
            {
                return new TagsDetailReturnModel()
                {
                    Success = false,
                    Message = _configuration["BlogService.NullBlog"]
                };
            }
        }

        public ResponseModel UpdateFeature(int id)
        {
            var featuredBlogs = _context.Blog.Where(x => x.IsFeatured && !x.IsDeleted).OrderByDescending(x => x.UpdatedDate).ToList();

            if (featuredBlogs.Count() == 4)
            {
                var lastBlog = featuredBlogs.LastOrDefault();

                lastBlog.UpdatedDate = DateTime.Now;
                lastBlog.IsFeatured = false;

                _context.Blog.Update(lastBlog);

                _context.SaveChanges();
            }

            return new ResponseModel()
            {
                Success = true
            };

        }
    }
}
