using AutoMapper;

namespace VideoTranslate.WebAPI.Infrastructure.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Shared.DTO.VideoInfo, WebApiClient.DTO.VideoInfo>()
                .ReverseMap();
        }

        /*
        this.CreateMap<Shared.DTO.AuthenticationTokenResponse, WebApiClient.DTO.AccessToken>()
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.TokenType))
                .ForMember(d => d.Value, opt => opt.MapFrom(src => src.AccessToken))
                .ForMember(d => d.ExpiresIn, opt => opt.MapFrom(src => src.AccessTokenExpiresIn))
                .ForMember(d => d.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
                .ForMember(d => d.RefreshTokenExpiresIn, opt => opt.MapFrom(src => src.RefreshTokenExpiresIn))
                .ForMember(d => d.RefreshTokenExpirationDateTimeUTC, opt => opt.MapFrom(src => src.RefreshTokenExpirationDateTimeUTC.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK")))
                .ForMember(d => d.ExpirationDateTimeUTC, opt => opt.MapFrom(src => src.AccessTokenExpirationDateTimeUTC.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK")))
                .ForMember(d => d.ID, opt => opt.MapFrom(src => src.ID.ToString()))
                .ReverseMap();

            this.CreateMap<DAL.Events.User, WebApiClient.DTO.User>()
                .ReverseMap();
        */
    }
}
