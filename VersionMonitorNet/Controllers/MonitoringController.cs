using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Anexia.Monitoring.Services;

namespace Anexia.Monitoring.Controllers
{
    /// <summary>
    /// APIs for version monitoring
    /// </summary>
    public class MonitoringController : Controller
    {
        // error message for missing authentication token
        private const string TOKEN_ERROR_MESSAGE = "Access token not configured";
        private readonly MonitoringService _service;

        public MonitoringController()
        {
            _service = new MonitoringService();
        }

        /// <summary>
        /// Get info about state of services (database etc.)
        /// </summary>
        /// <param name="access_token">the token to allow access to the monitoring routes - must be send as query-param with each api-call</param>
        /// <returns>plain text with state infos</returns>
        [HttpGet]
        public dynamic GetServiceStates(string access_token)
        {
            var result = CheckAccessToken(access_token);
            if (result != null)
                return result;

            result = new ContentResult();
            result.ContentType = "text/plain";
            result.Content = _service.GetServiceStates();
            return result;
        }

        /// <summary>
        /// Get version-info about runtime and modules
        /// </summary>
        /// <param name="access_token">the token to allow access to the monitoring routes - must be send as query-param with each api-call</param>
        /// <returns>json object with runtime and modules infos</returns>
        [HttpGet]
        public async Task<dynamic> GetModulesInfo(string access_token)
        {
            var result = CheckAccessToken(access_token);
            if (result != null)
                return result;

            result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = await _service.GetModulesInfo();
            return result;
        }

        /// <summary>
        /// Check the access token
        /// </summary>
        /// <param name="token">the token to allow access to the monitoring routes - must be send as query-param with each api-call</param>
        /// <returns></returns>
        private dynamic CheckAccessToken(string token)
        {
            if (!_service.CheckTokenConfiguration())
                return new HttpUnauthorizedResult(TOKEN_ERROR_MESSAGE);

            if (!_service.ValidateToken(token))
                return new HttpUnauthorizedResult();

            return null;
        }
    }
}
