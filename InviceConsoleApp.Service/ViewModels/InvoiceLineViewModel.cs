﻿namespace InviceConsoleApp.Service.ViewModels
{
    public class InvoiceLineViewModel
    {
        public int LineId { get; set; }

        public required string InvoiceNumber { get; set; }

        public string? Description { get; set; }

        public double Quantity { get; set; }

        public double UnitSellingPriceExVAT { get; set; }

        public required InvoiceHeaderViewModel InvoiceHeader { get; set; }
    }
}
