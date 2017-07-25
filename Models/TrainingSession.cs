using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;
using API_Speedforce.Business;
using System.Text;

namespace API_Speedforce.Models
{
    public class TrainingSession
    {
        #region Properties
        public String SessionID { get; set; }
        public String UserID { get; set; }
        public Double AverageBPM { get; set; }
        public Double BurntCalories { get; set; }
        public String RouteID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Double Distance { get; set; }
        public Double RelativeHumidity { get; set; }
        public Double Temperature { get; set; }
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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_SesionEntrenamiento.Find(sessionID);

                    SessionID = obj.ID_Sesion;
                    RouteID = obj.ID_Ruta;
                    UserID = obj.ID_Usuario;
                    AverageBPM = Math.Round(obj.RitmoCardiacoMedio,2);
                    BurntCalories = Math.Round(obj.CaloriasQuemadas,2);
                    RelativeHumidity = Math.Round(obj.HumedadRelativa,2);
                    Distance = Math.Round(obj.DistanciaRecorrida,2);
                    StartTime = obj.MomentoInicio;
                    EndTime = obj.MomentoInicio;
                    Temperature = Math.Round(obj.Temperatura,2);
                    ClimateConditionID = obj.ID_Condicion;
                    TrainingTypeID = obj.ID_TipoEntrenamiento;
                    SessionStatusID = obj.ID_StatusSesion;

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
        public OperationResponse<List<TrainingSessionModel>> GetTrainingSessions(string userid)
        {
            var result = new OperationResponse<List<TrainingSessionModel>>();

            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var query = entities.TB_SesionEntrenamiento.Where(cred => cred.ID_Usuario == userid);
                    List<TrainingSessionModel> trainingList = new List<TrainingSessionModel>();

                    foreach (var item in query)
                    {
                        trainingList.Add(new TrainingSessionModel()
                        {
                            SessionID = item.ID_Sesion,
                            RouteID = item.ID_Ruta,
                            UserID = item.ID_Usuario,
                            AverageBPM = Math.Round((double)item.RitmoCardiacoMedio, 2),
                            BurntCalories = Math.Round((double)item.CaloriasQuemadas, 2),
                            RelativeHumidity = Math.Round((double)item.HumedadRelativa, 2),
                            Distance = Math.Round((double)item.DistanciaRecorrida, 2),
                            StartTime = item.MomentoInicio,
                            EndTime = item.MomentoTermino,
                            Temperature = Math.Round((double)item.Temperatura, 2),
                            ClimateConditionID = Utility.GetClimateString(item.ID_Condicion),
                            TrainingTypeID = Utility.GetTrainingTyeString(item.ID_TipoEntrenamiento),
                            SessionStatusID = Utility.GetSessionStatusString(item.ID_StatusSesion),
                            Duration = Utility.GetDuration(StartTime, EndTime),
                            RouteName = item.TB_Rutas.NombreRuta
                            
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
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    if (!entities.TB_SesionEntrenamiento.Any(cred => cred.ID_Sesion == SessionID))
                    {
                        UpdateEntity();
                        entities.TB_SesionEntrenamiento.Add(TrainingEntity);
                        entities.SaveChanges();
                        return result.Complete(this);
                    }
                    else
                    {
                        UpdateSession();
                        return result.Complete(this);
                    }
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }
        #endregion

        #region UtilityMethods
        /// <summary>
        /// Add values to the Entity Framework Training Session Entity.
        /// </summary>
        public void UpdateEntity()
        {
            TrainingEntity = new TB_SesionEntrenamiento();

            try
            {
                if (this.SessionStatusID == 3)
                    this.SessionStatusID = 2;

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
                TrainingEntity.Temperatura = this.Temperature;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public OperationResponse<TrainingSession> AddSession(BIEntryModel model)
        {
            var result = new OperationResponse<TrainingSession>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {

                    this.SessionID = System.Guid.NewGuid().ToString();
                    this.UserID = model.UserID;
                    this.RouteID = model.RouteID;
                    this.AverageBPM = 0;
                    this.BurntCalories = 0;
                    this.ClimateConditionID = 1;
                    this.Distance = 0;
                    this.EndTime = DateTime.Now;
                    this.StartTime = DateTime.Now;
                    this.Temperature = 0;
                    this.TrainingTypeID = 1;
                    this.SessionStatusID = 1;
                    this.RelativeHumidity = 0;


                    UpdateEntity();
                    entities.Entry(TrainingEntity).State = System.Data.Entity.EntityState.Added;
                    entities.SaveChanges();
                    return result.Complete(this);
                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        /// <summary>
        /// Method to find a Training Session with the "Pending" state.
        /// </summary>
        /// <param name="userid"></param>
        public TrainingSession FindSavedSession(string userid)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_SesionEntrenamiento.SingleOrDefault(cred => cred.ID_Usuario == userid && cred.ID_StatusSesion == 1);

                    SessionID = obj.ID_Sesion;
                    RouteID = obj.ID_Ruta;
                    
                    UserID = obj.ID_Usuario;
                    AverageBPM = obj.RitmoCardiacoMedio;
                    BurntCalories = obj.CaloriasQuemadas;
                    RelativeHumidity = obj.HumedadRelativa;
                    Distance = obj.DistanciaRecorrida;
                    StartTime = obj.MomentoInicio;
                    EndTime = obj.MomentoInicio;
                    Temperature = obj.Temperatura;
                    ClimateConditionID = obj.ID_Condicion;
                    TrainingTypeID = obj.ID_TipoEntrenamiento;
                    SessionStatusID = 2;

                    UpdateSession();

                    return this;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("---DB/Entity Problem---");
                Console.WriteLine();
                Console.WriteLine(ex);
                return this;
            }

        }

        public void UpdateSession()
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_SesionEntrenamiento.SingleOrDefault(cred => cred.ID_Sesion == SessionID);
                    if (obj != null)
                    {
                        obj.CaloriasQuemadas = this.BurntCalories;
                        obj.HumedadRelativa = this.RelativeHumidity;
                        obj.ID_Ruta = this.RouteID;
                        obj.ID_Sesion = this.SessionID;
                        obj.ID_Condicion = this.ClimateConditionID;
                        obj.ID_StatusSesion = this.SessionStatusID;
                        obj.ID_TipoEntrenamiento = this.TrainingTypeID;
                        obj.ID_Usuario = this.UserID;
                        obj.MomentoInicio = this.StartTime;
                        obj.MomentoTermino = this.EndTime;
                        obj.RitmoCardiacoMedio = this.AverageBPM;
                        obj.DistanciaRecorrida = this.Distance;
                        obj.Temperatura = this.Temperature;

                        entities.SaveChanges();
                        UpdateEntity();
                        entities.TB_SesionEntrenamiento.Add(TrainingEntity);
                        entities.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}