namespace QuotationService.Exceptions;

public class ForbiddenAccessException(string message) : Exception(message);
