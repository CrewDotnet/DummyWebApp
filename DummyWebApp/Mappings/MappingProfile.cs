using AutoMapper;
using DummyWebApp.Models.RequestModels.Company;
using DummyWebApp.Models.RequestModels.Customer;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Company;
using DummyWebApp.Models.ResponseModels.Customer;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Models.ResponseModels.Order;
using PostgreSQL.DataModels;

namespace DummyWebApp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyService, CompanyResponse>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom(src => MapGames(src.Games!)));
            CreateMap<NewCompanyRequest, CompanyService>();
            CreateMap<List<CompanyResponse>, CompaniesDTO>()
                .ForMember(dest => dest.Companies, opt => opt.MapFrom(src => src));
            CreateMap<CompanyResponse, CompanyDTO>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src));

            CreateMap<Game, GameResponseForCustomer>();
            CreateMap<Game, GameBaseResponse>();
            CreateMap<Game, GameDTO>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyService.Name))
                .ForMember(dest => dest.OrderIds, opt => opt.MapFrom(src => src.Orders.Select(o => o.Id)));
            CreateMap<Game, CustomerResponse>();
            CreateMap<NewGameRequest, Game>();
            CreateMap<UpdateGameRequest, Game>();

            CreateMap<Customer, CustomerResponse>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom(src => src.Games))
                .ForMember(dest => dest.TotalAmountSpent, opt => opt.MapFrom(src => src.TotalAmountSpent));
            CreateMap<NewCustomerRequest, Customer>();
            CreateMap<CustomerResponse, CustomerDTO>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src));
            CreateMap<List<CustomerResponse>, CustomersDTO>()
                .ForMember(dest => dest.Customers, opt => opt.MapFrom(src => src));

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
