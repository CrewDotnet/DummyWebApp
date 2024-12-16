using DummyWebApp.Models.ErrorModel;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Erorr
{
    public class ErrorPresenter
    {
        internal static ActionResult PresentErrorResponse(List<IError> errors)
        {
            var firstError = errors.FirstOrDefault();
            var statusCode = 400; 
            if (firstError?.HasMetadataKey("StatusCode") == true)
            {
                statusCode = (int)firstError.Metadata["StatusCode"];
            }

            var errorResponse = new ErrorResponse
            {
                Errors = errors.Select(MapError).ToList(),
            };

            return new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };
        }

        private static ErrorItem MapError(IError error)
        {
            var errorItem = new ErrorItem(error.Message);

            if (error.HasMetadataKey(ErrorWithReason.ReasonDescription) && error.HasMetadataKey(ErrorWithReason.ReasonName))
            {
                errorItem.Reason = new ErrorItemReason(error.Metadata[ErrorWithReason.ReasonDescription].ToString()!, error.Metadata[ErrorWithReason.ReasonName].ToString()!);
            }

            return errorItem;
        }
    }
}
