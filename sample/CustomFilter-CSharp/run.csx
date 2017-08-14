using System.Net;
using System.Web.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

public static void Run(FunctionInvocationContext context, TraceWriter log)
{
    if (context is FunctionExecutingContext executingContext)
    {
        log.Info($"Custom invocation filter called (Executing)");

        // perform validation on query parameters
        var req = context.Arguments.First().Value as HttpRequestMessage;
        if (req != null)
        {
            var queryParams = req.GetQueryNameValuePairs()
                .ToDictionary(p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase);
            string value;
            int age;
            if (queryParams.TryGetValue("age", out value) &&
                int.TryParse(value, out age))
            {
                if (age < 0 || age > 150)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "Invalid age specified."
                    };
                    throw new FunctionResultException(response, response.ReasonPhrase);
                }
            }
        }
    }
    else
    {
        FunctionExecutedContext executedontext = (FunctionExecutedContext)context;
        log.Info($"Custom invocation filter called (Executed)");
    }
}