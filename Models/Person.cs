using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;
using API_Speedforce.Business;
using System.Text;

namespace API_Speedforce.Models
{
    public class Person
    {
        #region Properties
        public String Email { get; set; }
        public Int32 Sex { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public String CityName { get; set; }
        public String CountryName { get; set; }
        public String TelephoneNumber { get; set; }
        public String Photo { get; set; }
        public TB_Personas PersonEntity { get; set; }
        #endregion

        #region Constructors
        public Person() { }

        public Person(string email)
        {
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    Email = entities.TB_Personas.Find(email).Email;
                    Name = entities.TB_Personas.Find(email).Nombre;
                    LastName = entities.TB_Personas.Find(email).Apellidos;
                    Sex = entities.TB_Personas.Find(email).ID_Sexo;
                    CityName = entities.TB_Personas.Find(email).NombreCiudad;
                    CountryName = entities.TB_Personas.Find(email).NombrePais;
                    BirthDate = entities.TB_Personas.Find(email).FechaNacimiento;
                    TelephoneNumber = entities.TB_Personas.Find(email).NumeroTelefono;

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
        public OperationResponse<Person> GetPerson(string userid)
        {
            var result = new OperationResponse<Person>();

            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {

                    var requestedUser = entities.TB_Personas.Find(userid);
                    if (requestedUser != null)
                    {
                        Email = requestedUser.Email;
                        Name = requestedUser.Nombre;
                        LastName = requestedUser.Apellidos;
                        BirthDate = requestedUser.FechaNacimiento;
                        Sex = requestedUser.ID_Sexo;
                        CityName = requestedUser.NombreCiudad;
                        CountryName = requestedUser.NombrePais;
                        TelephoneNumber = requestedUser.NumeroTelefono;

                        return result.Complete(this);
                    }
                    return result.Failed("Atleta no encontrado.");

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        public OperationResponse<Person> AddPerson()
        {
            var result = new OperationResponse<Person>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    if (!entities.TB_Personas.Any(cred => cred.Email == this.Email))
                    {
                        UpdateEntity();
                        entities.Entry(PersonEntity).State = System.Data.Entity.EntityState.Added;
                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                        return result.Failed("Email ya está siendo usado.");

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }

        }

        public OperationResponse<Person> UpdatePerson()
        {
            var result = new OperationResponse<Person>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    var obj = entities.TB_Personas.SingleOrDefault(cred => cred.Email == Email);
                    if (obj != null)
                    {
                        obj.Apellidos = LastName;
                        obj.ID_Sexo = Sex;
                        obj.Nombre = Name;
                        obj.NombreCiudad = CityName;
                        obj.NombrePais = CountryName;
                        obj.NumeroTelefono = TelephoneNumber;
                        obj.FechaNacimiento = BirthDate;

                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                    {
                        return result.Failed("Usuario no encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        public OperationResponse<String> GetPhoto()
        {
            var result = new OperationResponse<String>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    Photo = Encoding.ASCII.GetString(entities.TB_Personas.SingleOrDefault(cred => cred.Email == Email).Foto);
                    return result.Complete(System.Convert.ToBase64String
                        (entities.TB_Personas.SingleOrDefault(cred => cred.Email == Email).Foto));
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        public OperationResponse<bool> UpdatePhoto(string s)
        {
            var result = new OperationResponse<bool>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    var obj = entities.TB_Personas.SingleOrDefault(cred => cred.Email == Email);
                    if (obj != null)
                    {
                        obj.Foto = System.Convert.FromBase64String(s);
                        return result.Complete(true);
                    }
                    return result.Failed("No se encontró usuario");
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }

        }
        #endregion

        #region Utility Methods
        public int GetSexID(string s)
        {
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    int idvalue = entities.TB_Sexo.SingleOrDefault(cred => cred.Descripcion == s).ID_Sexo;
                    return idvalue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public void UpdateEntity()
        {
            PersonEntity = new TB_Personas();

            try
            {
                PersonEntity.Nombre = this.Name;
                PersonEntity.Apellidos = this.LastName;
                PersonEntity.Email = this.Email;
                PersonEntity.FechaNacimiento = this.BirthDate.Date;
                PersonEntity.ID_Sexo = this.Sex;
                PersonEntity.NombreCiudad = this.CityName;
                PersonEntity.NombrePais = this.CountryName;
                PersonEntity.NumeroTelefono = this.TelephoneNumber;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        
    }
}