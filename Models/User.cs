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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_Usuarios.Find(username);

                    Email = obj.Email;
                    Username = obj.ID_Usuario;
                    Password = obj.Contraseña;
                    Role = obj.ID_Rol;
                    
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
        /// <summary>
        /// Method that update an the objects data according to an existing user.
        /// </summary>
        /// <param name="username">The username of the searched User</param>
        public OperationResponse<User> GetUser(string username)
        {
            var result = new OperationResponse<User>();

            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
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

        /// <summary>
        /// Method that adds an user into the DB
        /// </summary>
        public OperationResponse<User> AddUser()
        {
            var result = new OperationResponse<User>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    if (! entities.TB_Usuarios.Any(cred => cred.ID_Usuario == Username))
                    {
                        UpdateEntity();
                        entities.Entry(UserEntity).State = System.Data.Entity.EntityState.Added;
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
        /// <summary>
        /// Method to Update the valuo of the EF's Entity object
        /// </summary>
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