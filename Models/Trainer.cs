using API_Speedforce.Business;
using Speedforce_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Speedforce.Models
{
    public class Trainer
    {
        #region Properties
        public String Username { get; set; }
        public bool Certified { get; set; }
        TB_Entrenadores TrainerEntity { get; set; }
        TB_AtletaEntrenador PairEntity { get; set; }
        #endregion

        #region Constructors
        public Trainer() { }
        #endregion

        #region Main Methods
        public OperationResponse<Trainer> AddTrainer()
        {
            var result = new OperationResponse<Trainer>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    if (!entities.TB_Entrenadores.Any(cred => cred.ID_Usuario == this.Username))
                    {
                        UpdateEntity();
                        entities.Entry(TrainerEntity).State = System.Data.Entity.EntityState.Added;
                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                        return result.Failed("Entrenador ya existe.");

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }


        public void UpdateEntity()
        {
            TrainerEntity = new TB_Entrenadores();
            try
            {
                TrainerEntity.ID_Usuario = this.Username;
                TrainerEntity.bCertificado = this.Certified;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion
    }
}