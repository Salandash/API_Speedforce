using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;
using API_Speedforce.Business;

namespace API_Speedforce.Models
{
    public class User
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public Int32 Role { get; set; }
        public String Email { get; set; }


        public User() { }

        public User(string username)
        {
            using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
            {
                Username = entities.TB_Usuarios.Find(username).ID_Usuario;
                Password = entities.TB_Usuarios.Find(username).Contraseña;
                Role = entities.TB_Usuarios.Find(username).ID_Rol;
                Email = entities.TB_Usuarios.Find(username).Email;
            }
        }

        public OperationResponse<User> GetUser(string username)
        {
            var result = new OperationResponse<User>();

            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {

                    var requestedUser = entities.TB_Usuarios.Find(username);
                    if (requestedUser != null)
                    {
                        Username = requestedUser.ID_Usuario;
                        Password = requestedUser.Contraseña;
                        Role = requestedUser.ID_Rol;
                        Email = requestedUser.Email;

                        return result.Complete(this);
                    }
                    return result.Complete(this);

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        public OperationResponse<User> VerifyLogin()
        {
            var result = new OperationResponse<User>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    if (entities.TB_Usuarios.Any(cred => cred.ID_Usuario
                            == this.Username && cred.Contraseña == this.Password))
                        return result.Complete(this);
                    else
                        return result.Failed("Usuario y contraseña no válidos");
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }
    }
}