namespace DummyWebApp.Models.ResponseModels.Customer
{
    public class CustomerBaseResponse
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
    }
}
