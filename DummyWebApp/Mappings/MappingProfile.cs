using AutoMapper;
using DummyWebApp.RequestModels.Customer;
using DummyWebApp.ResponseModels.Company;
using DummyWebApp.ResponseModels.Customer;
using DummyWebApp.ResponseModels.Game;
using DummyWebApp.ResponseModels.Order;
using PostgreSQL.DataModels;

namespace DummyWebApp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyResponse>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom(src => MapGames(src.Games!)));

            CreateMap<Game, GameResponseForCustomer>();

            CreateMap<Game, GameBaseResponse>();

            CreateMap<Game, GameResponseWithCompany>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.OrderIds, opt => opt.MapFrom(src => src.Orders.Select(o => o.Id)));
            //CreateMap<Game, CustomerResponse>();

            CreateMap<Customer, CustomerResponse>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom(src => src.Games))
                .ForMember(dest => dest.TotalAmountSpent, opt => opt.MapFrom(src => src.TotalAmountSpent));
            CreateMap<Customer, CustomerBaseResponse>();
            CreateMap<NewCustomerRequest, Customer>();
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.CustomerFullName,
                    opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"));
        }
        private IEnumerable<GameBaseResponse> MapGames(IEnumerable<Game> games)
        {
            return games.Select(game => new GameBaseResponse
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price
            }).ToList();
        }
    }
}
