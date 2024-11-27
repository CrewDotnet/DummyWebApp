using DummyWebApp.ResponseModels;
using PostgreSQL.DataModels;
using AutoMapper;
using DummyWebApp.RequestModels;

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
            CreateMap<Game, CustomerResponse>();

            CreateMap<Customer, CustomerResponse>();
            CreateMap<NewCustomerRequest, Customer>();
            CreateMap<Order, OrderResponse>().ForMember(dest => dest.CustomerFullName,
                opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"));
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
