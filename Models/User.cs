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
        #region Properties
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        TB_Usuarios UserEntity { get; set; }
        #endregion

        #region Constructors
        public User() { }

        public User(string username)
        {
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    Email = entities.TB_Usuarios.Find(username).Email;
                    Username = entities.TB_Usuarios.Find(username).ID_Usuario;
                    Password = entities.TB_Usuarios.Find(username).Contraseña;
                    Role = entities.TB_Usuarios.Find(username).ID_Rol;
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---DB/Entity Problem---");
                Console.WriteLine();
                Console.WriteLine(ex);
            }
            
        }
        #endregion

        #region Main Methods

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
                    return result.Failed("Usuario no encontrado.");

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

        public OperationResponse<User> AddUser()
        {
            var result = new OperationResponse<User>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    if (! entities.TB_Usuarios.Any(cred => cred.ID_Usuario == Username))
                    {
                        UpdateEntity();
                        entities.TB_Usuarios.Add(UserEntity);
                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                        return result.Failed("Usuario ya existe.");
                    
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
            
        }
        #endregion

        #region Utility Methods
        public void UpdateEntity()
        {
            UserEntity = new TB_Usuarios();
            try
            {
                UserEntity.Email = this.Email;
                UserEntity.Contraseña = this.Password;
                UserEntity.ID_Rol = this.Role;
                UserEntity.ID_Usuario = this.Username;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion
    }
}