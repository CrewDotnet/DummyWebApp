﻿using DummyWebApp.ResponseModels.Game;

namespace DummyWebApp.ResponseModels.Customer
{
    public class CustomerResponse : CustomerBaseResponse
    {
        public int LoyaltyPoints { get; set; } = 0;
        public decimal TotalAmountSpent { get; set; }
        public List<GameBaseResponse>? Games { get; set; }
    }
}
