using Speedforce_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Speedforce
{
    public class UserSecurity
    {
        public static bool Login(string username, string password)
        {
            using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.TB_Usuarios.Any(user => user.ID_Usuario.Equals(username, StringComparison.OrdinalIgnoreCase) && user.Contraseña == password);
            }
        }
    }
}