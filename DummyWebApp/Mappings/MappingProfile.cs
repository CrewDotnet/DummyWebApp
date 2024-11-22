using DummyWebApp.ResponseModels;
using PostgreSQL.DataModels;
using AutoMapper;
namespace DummyWebApp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyResponse>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom(src => MapGames(src.Games!)));

            CreateMap<Game, GameResponseForCompany>();

            CreateMap<Game, GameResponseWithCompany>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company.Name));
        }
        private IEnumerable<GameResponseForCompany> MapGames(IEnumerable<Game> games)
        {
            return games.Select(game => new GameResponseForCompany
            {
                Name = game.Name,
                Price = game.Price
            }).ToList();
        }
    }
}
