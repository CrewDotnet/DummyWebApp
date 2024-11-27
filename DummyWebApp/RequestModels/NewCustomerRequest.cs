﻿namespace DummyWebApp.RequestModels
{
    public class NewCustomerRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
    }
}
