// This is content for a test file!  
// Not actually part of the test build. 

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace TestFunction
{
	// Test Functions directly invoking WebJobs. 
    public class DirectLoadFunction
    {
		[FunctionName("DotNetDirectFunction")]
        [TestFilter]
        public static Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req, 
			TraceWriter log)
        {
            log.Info("Function invoked!");

            var res = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello from .NET DirectInvoker!"),
                RequestMessage = req
            };

            return Task.FromResult(res);
        }		
    }

    // Test invocation filter that is applied to the function above
    // to verify invocation filter support in direct load scenarios
    public class TestFilterAttribute : InvocationFilterAttribute
    {
        public override Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            // set a marker to record that the filter was called
            HttpRequestMessage request = (HttpRequestMessage)executingContext.Arguments["req"];
            request.Properties["TestFilter"] = "TestFilter Invoked!";

            return Task.CompletedTask;
        }
    }
}
