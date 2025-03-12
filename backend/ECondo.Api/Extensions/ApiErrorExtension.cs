using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using ECondo.Domain.Shared;

namespace ECondo.Api.Extensions;
public static class ApiErrorExtension
{
    public static ValidationProblem ToValidationProblem(this Error? identityError)
    {
        Debug.Assert(identityError is not null);
        var errorDictionary = new Dictionary<string, string[]>(1)
        {
            [identityError.Code] = [identityError.Description]
        };

        return TypedResults.ValidationProblem(errorDictionary);
    }

    public static ValidationProblem ToValidationProblem(this Error[]? errors)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(errors is not null);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
}
