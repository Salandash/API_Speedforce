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
        public float Weight { get; set; }
        public float Height { get; set; }
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
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    Username = entities.TB_Atletas.Find(username).ID_Usuario;
                    Weight = (float)entities.TB_Atletas.Find(username).Peso;
                    Height = (float)entities.TB_Atletas.Find(username).Altura;
                    Bike = (Int32)entities.TB_Atletas.Find(username).ID_Bicicleta;
                    BikerType = (Int32)entities.TB_Atletas.Find(username).ID_TipoCiclista;

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
        public OperationResponse<Athlete> GetAthlete(string userid)
        {
            var result = new OperationResponse<Athlete>();

            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {

                    var requestedUser = entities.TB_Atletas.Find(userid);
                    if (requestedUser != null)
                    {
                        Username = requestedUser.ID_Usuario;
                        Weight = (float)requestedUser.Peso;
                        Height = (float)requestedUser.Altura;
                        Bike = (Int32)requestedUser.ID_Bicicleta;

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

        public OperationResponse<Athlete> AddAthlete()
        {
            var result = new OperationResponse<Athlete>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
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

        public OperationResponse<Athlete> UpdateAthlete()
        {
            var result = new OperationResponse<Athlete>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    var obj = entities.TB_Atletas.SingleOrDefault(cred => cred.ID_Usuario == Username);
                    if (obj != null)
                    {
                        obj.ID_TipoCiclista = BikerType;
                        obj.Peso = Weight;
                        obj.Altura = Height;

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
        public void UpdateEntity()
        {
            AthleteEntity = new TB_Atletas();
            try
            {
                AthleteEntity.ID_Usuario = this.Username;
                AthleteEntity.ID_Bicicleta = this.Bike;
                AthleteEntity.Peso = this.Weight;
                AthleteEntity.Altura = this.Height;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public int GetBikerTypeID(string s)
        {
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    int idvalue = entities.TB_TipoCiclista.SingleOrDefault(cred => cred.DescripcionCiclista == s).ID_TipoCiclista;
                    return idvalue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }
        #endregion

        
    }
}