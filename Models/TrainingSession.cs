using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;
using API_Speedforce.Business;

namespace API_Speedforce.Models
{
    public class TrainingSession
    {
        #region Properties
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
        #endregion

        #region Constructors
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
        #endregion

        #region Main Methods
        public OperationResponse<List<TrainingSession>> GetTrainingSessions(string userid)
        {
            var result = new OperationResponse<List<TrainingSession>>();

            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    var query = entities.TB_SesionEntrenamiento.Where(cred => cred.ID_Usuario == userid);
                    List<TrainingSession> trainingList = new List<TrainingSession>();

                    foreach (var item in query)
                    {
                        trainingList.Add(new TrainingSession()
                        {
                            SessionID = item.ID_Sesion,
                            RouteID = item.ID_Ruta,
                            UserID = item.ID_Usuario,
                            AverageBPM = (float)item.RitmoCardiacoMedio,
                            BurntCalories = (float)item.CaloriasQuemadas,
                            RelativeHumidity = (float)item.HumedadRelativa,
                            Distance = (float)item.DistanciaRecorrida,
                            StartTime = item.MomentoInicio,
                            EndTime = item.MomentoInicio,
                            Temperature = (float)item.Temperatura,
                            ClimateConditionID = item.ID_Condicion,
                            TrainingTypeID = item.ID_TipoEntrenamiento,
                            SessionStatusID = item.ID_StatusSesion
                        });

                    }

                    return result.Complete(trainingList);
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }

        }
        

        public OperationResponse<TrainingSession> AddSession()
        {
            var result = new OperationResponse<TrainingSession>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    if (!entities.TB_SesionEntrenamiento.Any(cred => cred.ID_Sesion == SessionID))
                    {
                        UpdateEntity();
                        entities.TB_SesionEntrenamiento.Add(TrainingEntity);
                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                        return result.Failed("Sesión de entrenamiento ya existe en el sistema.");
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }
        #endregion

        #region UtilityMethods
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

        public OperationResponse<TrainingSession> FindSavedSession(string userid)
        {
            var result = new OperationResponse<TrainingSession>();
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    
                    var obj = entities.TB_SesionEntrenamiento.SingleOrDefault(cred => cred.ID_Usuario == userid && cred.ID_StatusSesion == 3).ID_Sesion;
                    if (obj == null)
                    {
                        return result.Failed("No hay sesiones pendientes");
                    }
                    else
                    {
                        TrainingSession ts = new TrainingSession(obj);
                        return result.Complete(ts);
                    }
                }

            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        
        public int GetSessionStatusID(string s)
        {
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    int idvalue = entities.TB_StatusSesion.SingleOrDefault(cred => cred.DescripcionStatus == s).ID_StatusSesion;
                    return idvalue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public int GetTrainingTypeID(string s)
        {
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    int idvalue = entities.TB_TipoEntrenamiento.SingleOrDefault(cred => cred.DescripcionEntrenamiento == s).ID_TipoEntrenamiento;
                    return idvalue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public int GetClimateConditionID(string s)
        {
            try
            {
                using (DB_SpeedForceEntities entities = new DB_SpeedForceEntities())
                {
                    int idvalue = entities.TB_CondicionClimatica.SingleOrDefault(cred => cred.Clima == s).ID_Condicion;
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