using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;

namespace API_Speedforce.Models
{
    public class TrainingSession
    {

        public String SessionID { get; set; }
        public String UserID { get; set; }
        public float AverageBPM { get; set; }
        public float BurntCalories { get; set; }
        public String RouteID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Distance { get; set; }
        public float RelativeHumidity { get; set; }
        public float Temperature { get; set; }
        public int TrainingTypeID { get; set; }
        public int SessionStatusID { get; set; }
        public int ClimateConditionID { get; set; }

        public TB_SesionEntrenamiento TrainingEntity { get; set; }

        public TrainingSession() { }

        public TrainingSession(string sessionID)
        {
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    SessionID = entities.TB_SesionEntrenamiento.Find(sessionID).ID_Sesion;
                    RouteID = entities.TB_SesionEntrenamiento.Find(sessionID).ID_Ruta;
                    UserID = entities.TB_SesionEntrenamiento.Find(sessionID).ID_Sesion;
                    AverageBPM = (float)entities.TB_SesionEntrenamiento.Find(sessionID).RitmoCardiacoMedio;
                    BurntCalories = (float)entities.TB_SesionEntrenamiento.Find(sessionID).CaloriasQuemadas;
                    RelativeHumidity = (float)entities.TB_SesionEntrenamiento.Find(sessionID).HumedadRelativa;
                    Distance = (float)entities.TB_SesionEntrenamiento.Find(sessionID).DistanciaRecorrida;
                    StartTime = entities.TB_SesionEntrenamiento.Find(sessionID).MomentoInicio;
                    EndTime = entities.TB_SesionEntrenamiento.Find(sessionID).MomentoInicio;
                    Temperature = (float)entities.TB_SesionEntrenamiento.Find(sessionID).Temperatura;
                    ClimateConditionID = entities.TB_SesionEntrenamiento.Find(sessionID).ID_Condicion;
                    TrainingTypeID = entities.TB_SesionEntrenamiento.Find(sessionID).ID_TipoEntrenamiento;
                    SessionStatusID = entities.TB_SesionEntrenamiento.Find(sessionID).ID_StatusSesion;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---DB/Entity Problem---");
                Console.WriteLine();
                Console.WriteLine(ex);
            }
        }

        public void UpdateEntity()
        {
            TrainingEntity = new TB_SesionEntrenamiento();

            try
            {
                TrainingEntity.ID_Usuario = this.UserID;
                TrainingEntity.CaloriasQuemadas = this.BurntCalories;
                TrainingEntity.DistanciaRecorrida = this.Distance;
                TrainingEntity.HumedadRelativa = this.RelativeHumidity;
                TrainingEntity.ID_Condicion = this.ClimateConditionID;
                TrainingEntity.ID_Ruta = this.RouteID;
                TrainingEntity.ID_Sesion = this.SessionID;
                TrainingEntity.ID_StatusSesion = this.SessionStatusID;
                TrainingEntity.ID_TipoEntrenamiento = this.TrainingTypeID;
                TrainingEntity.MomentoInicio = this.StartTime;
                TrainingEntity.MomentoTermino = this.EndTime;
                TrainingEntity.RitmoCardiacoMedio = this.AverageBPM;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
    }
}