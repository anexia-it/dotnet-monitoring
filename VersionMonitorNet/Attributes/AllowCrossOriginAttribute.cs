using System.Web.Mvc;

namespace VersionMonitorNet.Attributes
{
    /// <summary>
    ///     Attribute for adding CORS headers
    /// </summary>
    public class AllowCrossOriginAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     <inheritdoc/>
        ///     Adds needed headers for allowing cross origin requests.
        /// </summary>
        /// <param name="filterContext">The current action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Add Response Header-Elements
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "GET, OPTIONS");

            base.OnActionExecuting(filterContext);
        }
    }
}