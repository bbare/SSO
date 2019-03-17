using System.Net.Http;
using System.Web;
using System.Web.Http;
using ManagerLayer.ApplicationManagement;

namespace KFC_WebAPI.Controllers
{
    public class ApplicationController : ApiController
    {
        private ApplicationManager manager = new ApplicationManager();

        /// <summary>
        /// Register application into portal
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/applications/create")]
        public HttpResponseMessage Register([FromBody] ApplicationRequest request)
        {
            HttpResponseContent responseContent = manager.ValidateRegistration(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent);
            
            return  response;
        }

        /// <summary>
        /// Publish application to portal
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/applications/publish")]
        public HttpResponseMessage Publish([FromBody] ApplicationRequest request)
        {
            HttpResponseContent responseContent = manager.ValidatePublish(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);

            return response;
        }

        /// <summary>
        /// Generate a new api key
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/applications/generatekey")]
        public HttpResponseMessage GenerateKey([FromBody] ApplicationRequest request)
        {
            HttpResponseContent responseContent = manager.ValidateKeyGeneration(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);

            return response;
        }

        /// <summary>
        /// Delete application from portal
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/applications/delete")]
        public HttpResponseMessage DeleteApplication([FromBody] ApplicationRequest request)
        {
            HttpResponseContent responseContent = manager.ValidateDeletion(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);

            return response;
        }

    }
}