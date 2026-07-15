
using Accounts.Application.Dtos;
using Accounts.Application.User;
using Accounts.Models.ViewModels;
using aplication.Infrastructure.Common;
using AutoMapper;

//using Events.Infrastructure.Profile;

namespace Accounts.Mappings
{
    public class AutomapperAccounts : Profile
    {
        public AutomapperAccounts()
        {
            CreateMap<UserViewModel, UserDto>().ReverseMap();

            CreateMap<UserDto, Accounts.Application.User.User>().ReverseMap();


            CreateMap<UserEnabledAViewModel, Accounts.Application.Login.UserEnabledAccount>().ReverseMap();

            CreateMap<UserViewModel, UserDto>().ReverseMap();

            CreateMap<UserDto, User>().ReverseMap();


            CreateMap<ChangePasswordViewModel, ChangePasswordDto>().ReverseMap();


            CreateMap<ApiResponse<UserDto>, ApiResponse<User>>().ReverseMap();

            CreateMap<ApiResponse<UserDto>, ApiResponse<User>>().ReverseMap();


            //CreateMap<ProfilePlatformDto, Events.Infrastructure.Profile.ProfilePlatform>().ReverseMap();

            //CreateMap<ProfileCategoryDto, Events.Infrastructure.Profile.ProfileCategory>().ReverseMap();

            //CreateMap<ProfileExponentDto, Events.Infrastructure.Profile.ProfileExponent>().ReverseMap();


            //CreateMap<PlatformDto, Events.Infrastructure.Platform.Platform>().ReverseMap();

            //CreateMap<EventDto, Events.Infrastructure.Event.Event>().ReverseMap();

            //CreateMap<CategoryDto, Events.Infrastructure.Category.Category>().ReverseMap();

            //CreateMap<ExponentDto, Events.Infrastructure.Exponent.Exponent>().ReverseMap();

            //CreateMap<CountryDto, Events.Infrastructure.Country.Country>().ReverseMap();

            //CreateMap<HourZoneDto, Events.Infrastructure.HourZone.HourZone>().ReverseMap();


            //CreateMap<ProfilePlatformDto, EventPlatformDto>()
            //    .ForMember(dest =>
            //               dest.Name,
            //               opt => opt.MapFrom(src => src.PlatformName));


            //CreateMap<EventRecurrentDto, Events.Infrastructure.Event.EventRecurrent>().ReverseMap();

            //CreateMap<EventPlatformDto, Events.Infrastructure.Event.EventPlatform>().ReverseMap();


            //CreateMap<EventDto, Events.Infrastructure.Event.EventSave>().ReverseMap();


            //CreateMap<PostDto, Post>();

            //CreateMap<Security, SecurityDto>().ReverseMap();

            //CreateMap<User, UserDto>().ReverseMap();

        }


    }
}
