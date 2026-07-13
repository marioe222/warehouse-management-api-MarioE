namespace Warehouse.Presentation.Contracts;

public record ApiErrorResponse(
    string ErrorCode,
    string Message,
    string TraceId
);