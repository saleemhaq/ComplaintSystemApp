using System;
using System.Threading.Tasks;
using ComplaintApp.Application.Complaint;
using ComplaintApp.Core.AuditTracking;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ComplaintApp.Application.Shared
{
    public class LogUserActivity : IAsyncActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">This is used if we want to do something while the action is executed</param>
        /// <param name="next">This allows us to run some code after the action has been executed. </param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            // Stores the Request in an Accessible object  
            var request = context.HttpContext.Request;

            var audit = new AuditTrail()
            {
                // Your Audit Identifier   
                AuditID = Guid.NewGuid(),
                // Our Username (if available)  
                UserName = (context.HttpContext.User.Identity.IsAuthenticated) ? context.HttpContext.User.Identity.Name : "Anonymous",
                // The IP Address of the Request  
                IPAddress = request.HttpContext.Connection.RemoteIpAddress.ToString(),
                // The URL that was accessed  
                AreaAccessed = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(request),
                // Creates our Timestamp  
                Timestamp = DateTime.UtcNow
            };
              
            var repo = resultContext.HttpContext.RequestServices
                .GetService<IComplaintRepository>();
            repo.Add(audit);
            // Stores the Audit in the Database
            await repo.SaveAll();

        }
    }
}
