using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Speedforce
{
    public class UserController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Users> Get()
        {
            return new Users[] {
                new Users {_username = "William", _password="abcd" },
                new Users {_username = "George", _password="wxyz"}
            };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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
        public void Delete(Users user)
        {
            
        }
    }
}