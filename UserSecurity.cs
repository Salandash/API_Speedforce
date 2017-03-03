using Speedforce_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Speedforce
{
    public class UserSecurity
    {

        public static bool Login(string usename, string password)
        {
            using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.TB_Usuarios.Any(user => user.ID_Usuario.Equals(usename, StringComparison.OrdinalIgnoreCase) && user.Contraseña == password);
            }
        }
    }
}