using System.Web.Http;

namespace MyAPI.Areas.ERP.Controllers
{
    public class DemoController : ApiController
    {
        [HttpGet]
        public string Test(string name)
        {
            return string.Format("Hello:{0}", name);
        }

        [HttpGet]
        public string Test(string name, string sex)
        {
            return string.Format("Hello:{0},You sex is {1}", name, sex);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}