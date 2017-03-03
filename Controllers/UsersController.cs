using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Speedforce_DataAccess;
using System.Threading;
using System.Security.Principal;

namespace API_Speedforce.Controllers
{
    [BasicAuthentication]
    public class UsersController : ApiController
    {
        
        public List<TB_Usuarios> Get()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;

            using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.TB_Usuarios.ToList();
            }

        }

        public TB_Usuarios Get(string user)
        {
            using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
            {
                return entities.TB_Usuarios.FirstOrDefault(e => e.ID_Usuario == user);
            }
        }

    }
}
