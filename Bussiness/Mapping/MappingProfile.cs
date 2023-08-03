using WebApplication1.Bussiness.DTO;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Bussiness.Mapping
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();// lay het tat ca cac thuoc tinh cua ProductDTO gan vao Product va nguoc lai
            CreateMap<Category, CategoryDTO>().ReverseMap();

            //.ForMember(des => des.Category,
            //act => act.MapFrom(src => src.Category));
        }
    }
}
