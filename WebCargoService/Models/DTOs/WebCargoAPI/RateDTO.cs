using Shared;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using WebCargoService.Models.Entities;

namespace WebCargoService.Models.DTOs.WebCargoAPI;

[XmlType("rate")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public record RateDTO {

    [XmlElement("unique_code")]
    public required long UniqueCode { get; init; }

    [XmlElement("airline")]
    public required string Airline { get; init; }

    [XmlElement("origin")]
    public required string OriginAirportIATACode { get; init; }

    [XmlElement("origin_country")]
    public required string OriginCountryCode { get; init; }

    [XmlElement("destination")]
    public required string DestinationAirportIATACode { get; init; }

    [XmlElement("destination_country")]
    public required string DestinationCountryCode { get; init; }

    [XmlElement("valid_from")]
    public required string ValidFromString { get; init; }

    [XmlElement("valid_to")]
    public required string? ValidToString { get; init; }

    [XmlElement("volumetric_factor")]
    public required double VolumetricFactorImperial { get; init; }

    [XmlElement("product_name")]
    public required string ProductName { get; init; }

    [XmlElement("currency")]
    public required string CurrencyCode { get; init; }

    [XmlElement("service")]
    public required SpecialHandlingCodeDTO ServiceDetails { get; init; }

    [XmlArray("surcharges")]
    public required List<RateSurchargeDTO> Surcharges { get; init; }

    [XmlElement("breakpoints")]
    public required RateBreakpointsDTO Breakpoints { get; init; }

    [XmlIgnore]
    private DateOnly ValidFrom => DateOnly.Parse(ValidFromString);

    [XmlIgnore]
    private DateOnly? ValidTo => string.IsNullOrWhiteSpace(ValidToString) ? null : DateOnly.Parse(ValidToString);

    [XmlIgnore]
    public bool IsValid => Breakpoints.IsValid() && Surcharges.All(surcharge => surcharge.IsValid);

    [XmlIgnore]
    private double VolumetricFactorMetric => VolumetricFactorImperial * Math.Pow(MeasurementConstants.InchesToCentimeters, 3) / MeasurementConstants.PoundsToKilograms;


    public static Rate ToRate(RateDTO rateDTO) {
        return new Rate {
            UniqueCode = rateDTO.UniqueCode,
            Airline = rateDTO.Airline,
            OriginAirportIATACode = rateDTO.OriginAirportIATACode,
            DestinationAirportIATACode = rateDTO.DestinationAirportIATACode,
            VolumetricFactorMetric = (decimal)rateDTO.VolumetricFactorMetric,
            ProductName = rateDTO.ProductName,
            CurrencyCode = "EUR", // TODO Use currency in DTO
            ValidFrom = rateDTO.ValidFrom,
            ValidTo = rateDTO.ValidTo,
            SpecialHandlingCodeString = rateDTO.ServiceDetails.Code,
            Surcharges = rateDTO.Surcharges.Select(RateSurchargeDTO.ToRateSurcharge).ToList(),
            MinimumBreakpointCost = rateDTO.Breakpoints.Find(breakpoint => breakpoint.IsMinimumCost)!.Cost,
            Breakpoints = RateBreakpointsDTO.ToRateBreakpointCollection(rateDTO.Breakpoints)
        };

    }

}
