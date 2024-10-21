// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using PetFamily.API.Contracts.Shared;
// using PetFamily.Domain.Shared.Models;
//
// namespace PetFamily.API.Validation;
//
// public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
// {
//     public IActionResult CreateActionResult(
//         ActionExecutingContext context,
//         ValidationProblemDetails? validationProblemDetails)
//     {
//         ArgumentNullException.ThrowIfNull(validationProblemDetails);
//
//         List<ResponseError> errors = [];
//         
//         foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
//         {
//             var responseErrors = from errorMessage in validationErrors
//                 let error = Error.Deserialize(errorMessage)
//                 select new ResponseError(error.Code, error.Message, invalidField);
//
//             errors.AddRange(responseErrors);
//         }
//         
//         var envelope = Envelope.Error(errors);
//
//         return new ObjectResult(envelope)
//         {
//             StatusCode = StatusCodes.Status400BadRequest
//         };
//     }
// }