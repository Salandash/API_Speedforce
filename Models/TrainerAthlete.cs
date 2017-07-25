using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;
using API_Speedforce.Business;

namespace API_Speedforce.Models
{
    public class TrainerAthlete
    {
        #region Propeties
        public String AthleteID { get; set; }
        public String TrainerID { get; set; }
        public TB_AtletaEntrenador PairEntity {get; set;}
        #endregion

        #region Constructors
        public TrainerAthlete() { }
        #endregion

        #region Main Methods

        public OperationResponse<TrainerAthlete> AddPair()
        {
            var result = new OperationResponse<TrainerAthlete>();

            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    if (!entities.TB_AtletaEntrenador.Any(cred => cred.ID_Atleta == this.AthleteID && cred.ID_Entrenador == this.TrainerID))
                    {
                        UpdateEntity();
                        entities.Entry(PairEntity).State = System.Data.Entity.EntityState.Added;
                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                        return result.Failed("Entrenador y Atleta ya trabajan juntos.");

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        public OperationResponse<bool> DeletePair()
        {
            var result = new OperationResponse<bool>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    UpdateEntity();
                    entities.TB_AtletaEntrenador.Attach(PairEntity);
                    entities.TB_AtletaEntrenador.Remove(PairEntity);
                    entities.SaveChanges();
                    return result.Complete(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result.Failed(false);
            }
        }

        public void UpdateEntity()
        {
            PairEntity = new TB_AtletaEntrenador();
            try
            {
                PairEntity.ID_Atleta = this.AthleteID;
                PairEntity.ID_Entrenador = this.TrainerID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion
    }
}