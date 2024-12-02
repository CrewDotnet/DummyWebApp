﻿using PostgreSQL.DataModels;
namespace DummyWebApp.RequestModels.Game
{
    public class NewGameRequest
    {
        public required string? Name { get; set; }
        public required decimal Price { get; set; }
        public required int CompanyId { get; set; }
    }
}
