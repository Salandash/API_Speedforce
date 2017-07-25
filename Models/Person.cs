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
        public Int32 CityID { get; set; }
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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    Email = entities.TB_Personas.Find(email).Email;
                    Name = entities.TB_Personas.Find(email).Nombre;
                    LastName = entities.TB_Personas.Find(email).Apellidos;
                    Sex = entities.TB_Personas.Find(email).ID_Sexo;
                    CityID = entities.TB_Personas.Find(email).ID_Ciudad;
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
        public OperationResponse<Person> GetPerson(string email)
        {
            var result = new OperationResponse<Person>();

            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {

                    var requestedUser = entities.TB_Personas.Find(email);
                    if (requestedUser != null)
                    {
                        Email = requestedUser.Email;
                        Name = requestedUser.Nombre;
                        LastName = requestedUser.Apellidos;
                        BirthDate = requestedUser.FechaNacimiento;
                        Sex = requestedUser.ID_Sexo;
                        CityID = requestedUser.ID_Ciudad;
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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_Personas.SingleOrDefault(cred => cred.Email == Email);
                    if (obj != null)
                    {
                        obj.Email = Email;
                        obj.Apellidos = LastName;
                        obj.ID_Sexo = Sex;
                        obj.Nombre = Name;
                        obj.ID_Ciudad = CityID;
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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    Photo = entities.TB_Personas.SingleOrDefault(cred => cred.Email == Email).Foto;
                    return result.Complete(entities.TB_Personas.SingleOrDefault(cred => cred.Email == Email).Foto);
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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_Personas.SingleOrDefault(cred => cred.Email == Email);
                    if (obj != null)
                    {
                        obj.Foto = s;
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
                PersonEntity.ID_Ciudad = this.CityID;
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