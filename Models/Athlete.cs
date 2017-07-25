using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;
using API_Speedforce.Business;

namespace API_Speedforce.Models
{
    public class Athlete
    {
        #region Properties
        public String Username { get; set; }
        public Double Weight { get; set; }
        public Double Height { get; set; }
        public Int32 Bike { get; set; }
        public Int32 BikerType { get; set; }
        public TB_Atletas AthleteEntity { get; set; }
        #endregion

        #region Constructors
        public Athlete() { }

        public Athlete(string username)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_Atletas.Find(username);

                    Username = obj.ID_Usuario;
                    Weight = Math.Round((double)obj.Peso, 2);
                    Height = Math.Round((double)obj.Altura, 2);
                    Bike = (Int32)obj.ID_Bicicleta;
                    BikerType = (Int32)obj.ID_TipoCiclista;

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
        /// Method to load an Athlete object with the info from the DB
        /// </summary>
        /// <param name="userid">ID to identify which user info should be loaded</param>
        public OperationResponse<AthleteUser> GetAthlete(string userid)
        {
            var result = new OperationResponse<AthleteUser>();


            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    AthleteUser user = new AthleteUser();
                    var requestedUser = entities.TB_Atletas.Find(userid);
                    if (requestedUser != null)
                    {
                        user.Username = requestedUser.ID_Usuario;
                        user.Weight = Math.Round((double)requestedUser.Peso,2);
                        user.Height = Math.Round((double)requestedUser.Altura,2);
                        user.Bike = (Int32)requestedUser.ID_Bicicleta;
                        user.BikerType = Utility.GetBikerTypeString((Int32)requestedUser.ID_TipoCiclista);
                        user.Email = requestedUser.TB_Usuarios.Email;
                        user.CityName = Utility.GetCityString(requestedUser.TB_Usuarios.TB_Personas.ID_Ciudad);
                        user.CountryName = Utility.GetCountryString(requestedUser.TB_Usuarios.TB_Personas.TB_Ciudad.ID_Pais);
                        user.Name = requestedUser.TB_Usuarios.TB_Personas.Nombre;
                        user.LastName = requestedUser.TB_Usuarios.TB_Personas.Apellidos;
                        user.Role = requestedUser.TB_Usuarios.TB_Roles.NombreRol;
                        user.TelephoneNumber = requestedUser.TB_Usuarios.TB_Personas.NumeroTelefono;
                        user.BirthDate = requestedUser.TB_Usuarios.TB_Personas.FechaNacimiento;
                        user.Sex = Utility.GetSexString(requestedUser.TB_Usuarios.TB_Personas.ID_Sexo);

                        return result.Complete(user);
                    }
                    return result.Failed("Atleta no encontrado.");

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        public OperationResponse<Athlete> AddAthlete()
        {
            var result = new OperationResponse<Athlete>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    if (!entities.TB_Atletas.Any(cred => cred.ID_Usuario == this.Username))
                    {
                        UpdateEntity();
                        entities.Entry(AthleteEntity).State = System.Data.Entity.EntityState.Added;
                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                        return result.Failed("Atleta ya existe.");

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        /// <summary>
        /// Method to update information about an athlete on the DB
        /// </summary>
        public OperationResponse<Athlete> UpdateAthlete()
        {
            var result = new OperationResponse<Athlete>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_Atletas.SingleOrDefault(cred => cred.ID_Usuario == Username);
                    if (obj != null)
                    {
                        obj.ID_Usuario = Username;
                        obj.ID_TipoCiclista = BikerType;
                        obj.Peso = Math.Round(Weight,2);
                        obj.Altura = Math.Round(Height,2);
                        obj.ID_Bicicleta = 1;
                        

                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                    {
                        return result.Failed("Atleta no encontrado.");
                    }
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
        /// Method to update the EF's Entity object
        /// </summary>
        public void UpdateEntity()
        {
            AthleteEntity = new TB_Atletas();
            try
            {
                AthleteEntity.ID_Usuario = this.Username;
                AthleteEntity.ID_Bicicleta = this.Bike;
                AthleteEntity.Peso = Math.Round(this.Weight,2);
                AthleteEntity.Altura = Math.Round(this.Height,2);
                AthleteEntity.ID_TipoCiclista = this.BikerType;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        
    }
}