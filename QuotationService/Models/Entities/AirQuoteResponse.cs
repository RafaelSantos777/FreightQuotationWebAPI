namespace QuotationService.Models.Entities;

public class AirQuoteResponse { 

    public long Id { get; set; }

    public required AirQuoteRequest AirQuoteRequest { get; set; }

    public required ICollection<AirQuote> AirQuotes { get; set; }

}
