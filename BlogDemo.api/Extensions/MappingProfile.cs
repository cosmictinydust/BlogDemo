using AutoMapper;
using BlogDemo.Core.Entities;
using BlogDemo.Infrastructure.Resource;

namespace BlogDemo.api.Extensions
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostResource>()
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.LastModified));

            CreateMap<PostResource, Post>()
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.UpdateTime));
        }
    }
}
