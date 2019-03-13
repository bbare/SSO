using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using ManagerLayer.ApplicationManagement;

namespace KFC_WebAPI.Controllers
{
    public class ApplicationController : ApiController
    {

        [HttpPost]
        [Route("api/applications/create")]
        public HttpResponseMessage Register([FromBody] ApplicationRequest request)
        {
            ApplicationManager manager = new ApplicationManager();
            HttpResponseContent responseContent = manager.ValidateRegistration(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);
            
            
            return  response;
        }

        [HttpPost]
        [Route("api/applications/publish")]
        public HttpResponseMessage Publish([FromBody] ApplicationRequest request)
        {
            ApplicationManager manager = new ApplicationManager();
            HttpResponseContent responseContent = manager.ValidatePublish(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);

            return response;
        }

        [HttpPost]
        [Route("api/applications/generatekey")]
        public HttpResponseMessage GenerateKey([FromBody] ApplicationRequest request)
        {
            ApplicationManager manager = new ApplicationManager();
            HttpResponseContent responseContent = manager.ValidateKeyGeneration(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);

            return response;
        }

        [HttpPost]
        [Route("api/applications/delete")]
        public HttpResponseMessage DeleteApplication([FromBody] ApplicationRequest request)
        {
            ApplicationManager manager = new ApplicationManager();
            HttpResponseContent responseContent = manager.ValidateDeletion(request);
            HttpResponseMessage response = Request.CreateResponse(responseContent.Code, responseContent.Message);

            return response;
        }

    }
}