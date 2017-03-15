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
        public String Username { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public Int32 Bicicleta { get; set; }
        public Int32 TipoCiclista { get; set; }
        public TB_Atletas AthleteEntity { get; set; }

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
                    Bicicleta = (Int32)entities.TB_Atletas.Find(username).ID_Bicicleta;
                    TipoCiclista = (Int32)entities.TB_Atletas.Find(username).ID_TipoCiclista;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---DB/Entity Problem---");
                Console.WriteLine();
                Console.WriteLine(ex);
            }
            
        }

        public void UpdateAthlete()
        {
            AthleteEntity = new TB_Atletas();
            try
            {
                AthleteEntity.ID_Usuario = this.Username;
                AthleteEntity.ID_Bicicleta = this.Bicicleta;
                AthleteEntity.Peso = this.Weight;
                AthleteEntity.Altura = this.Height;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                        UpdateAthlete();
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
    }
}