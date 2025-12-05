using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Trade
{       
        [Key] // Marks this property as the primary key of the table in the database
        public int Id { get; set; }

        [Required] // Ensures that a value must be provided; cannot be null
        public string Commodity { get; set; } // The name/type of the traded commodity (e.g., Oil, Gold)

        public decimal Quantity { get; set; } // The amount of the commodity being traded

        public decimal Price { get; set; } // Price per unit of the commodity

        public DateTime TradeDate { get; set; } // Date and time when the trade occurred

        public string Counterparty { get; set; } // The entity (person/company) on the other side of the trade
   }
