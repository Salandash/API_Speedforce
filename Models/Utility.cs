using API_Speedforce.Business;
using Speedforce_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Speedforce.Models
{
    public class Utility
    {
        #region Utility Methods
        /// <summary>
        /// Method to look for a City's ID from the DB
        /// </summary>
        /// <param name="city">The name of the city.</param>
        /// <param name="country">The ID of the country the city is in</param>
        /// <returns>The ID of the searched City</returns>
        public static int GetCityID(string city, int country)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    int idvalue = entities.TB_Ciudad.SingleOrDefault(cred => cred.NombreCiudad == city && cred.ID_Pais == country).ID_Ciudad;
                    return idvalue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public static OperationResponse<List<string>> GetCountryList()
        {
            var result = new OperationResponse<List<string>>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    List<string> list = new List<string>();
                    foreach (var item in entities.TB_Pais)
                    {
                        list.Add(item.NombrePais);
                    }
                    return result.Complete(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return result.Failed(ex);
            }
        }

        public static OperationResponse<List<string>> GetCityList(int idcountry)
        {
            var result = new OperationResponse<List<string>>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    List<string> list = new List<string>();
                    foreach (var item in entities.TB_Ciudad.Where(cred => cred.ID_Pais == idcountry))
                    {
                        list.Add(item.NombreCiudad);
                    }
                    return result.Complete(list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return result.Failed(ex);
            }
        }

        public static string GetCityString(int city)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    string value = entities.TB_Ciudad.SingleOrDefault(cred => cred.ID_Ciudad == city).NombreCiudad;
                    return value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }


        /// <summary>
        /// Method to retrieve the ID of a Country from a Database
        /// </summary>
        /// <param name="s">The string containing the name of the Country</param>
        /// <returns>The ID of the country</returns>
        public static int GetCountryID(string s)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    int idvalue = entities.TB_Pais.SingleOrDefault(cred => cred.NombrePais == s).ID_Pais;
                    return idvalue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public static int GetCountryFromCity(int cityid)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    int idvalue = entities.TB_Ciudad.SingleOrDefault(cred => cred.ID_Ciudad == cityid).ID_Pais;
                    return idvalue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        public static string GetCountryString(int id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    string value = entities.TB_Ciudad.SingleOrDefault(cred => cred.ID_Ciudad == id).TB_Pais.NombrePais;
                    return value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }

        
        public static string GetEmail(string id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    string value = entities.TB_Usuarios.SingleOrDefault(cred => cred.ID_Usuario == id).Email;
                    return value;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Method to retrieve the a training Session's StatusID
        /// </summary>
        /// <param name="s">The ID of the Trainign Session</param>
        /// <returns>The ID of the Training Session's status</returns>
        public static int GetSessionStatusID(string s)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
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

        /// <summary>
        /// Method to retrieve the Session's status description.
        /// </summary>
        /// <param name="id">The ID of the session status</param>
        /// <returns>A string with the Description of the session.</returns>
        public static string GetSessionStatusString(int id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    string value = entities.TB_StatusSesion.SingleOrDefault(cred => cred.ID_StatusSesion == id).DescripcionStatus;
                    return value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Failed";
            }
        }

        /// <summary>
        /// Method to retrieve the training type's ID
        /// </summary>
        /// <param name="s">The training type's name</param>
        /// <returns>The training type's ID</returns>
        public static int GetTrainingTypeID(string s)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
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

        public static string GetTrainingTyeString(int id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    string value = entities.TB_TipoEntrenamiento.SingleOrDefault(cred => cred.ID_TipoEntrenamiento == id).DescripcionEntrenamiento;
                    return value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// Method to retrieve the training type's ID
        /// </summary>
        /// <param name="s">The climate condition's name</param>
        /// <returns>The cimate condition's ID</returns>
        public static int GetClimateConditionID(string s)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
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

        public static string GetClimateString(int id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    string value = entities.TB_CondicionClimatica.SingleOrDefault(cred => cred.ID_Condicion == id).Clima;
                    return value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// Method to retrieve the sex ID.
        /// </summary>
        /// <param name="s">The string representing with the sex description</param>
        /// <returns>The ID of the corresponding sex</returns>
        public static int GetSexID(string s)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
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

        /// <summary>
        /// Method to retrieve the sex's description
        /// </summary>
        /// <param name="id">The ID of the description</param>
        /// <returns>A string witht he description of the sex</returns>
        public static string GetSexString(int id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    string value = entities.TB_Sexo.SingleOrDefault(cred => cred.ID_Sexo == id).Descripcion;
                    return value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// Method to retrieve a Role's ID.
        /// </summary>
        /// <param name="s">A string with the Role's description</param>
        /// <returns>The ID of the role.</returns>
        public static int GetRoleID(string s)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    int idvalue = entities.TB_Roles.SingleOrDefault(cred => cred.NombreRol == s).ID_Rol;
                    return idvalue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        /// <summary>
        /// Method to retrieve the BikerType's ID
        /// </summary>
        /// <param name="s">A string with the description fo the BikerType</param>
        /// <returns>The ID of the Biker Type</returns>
        public static int GetBikerTypeID(string s)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
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

        /// <summary>
        /// Method to retrieve the BikerType's descripcion.
        /// </summary>
        /// <param name="id">The ID of the BikerType</param>
        public static string GetBikerTypeString(int id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    string value = entities.TB_TipoCiclista.SingleOrDefault(cred => cred.ID_TipoCiclista == id).DescripcionCiclista;
                    return value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }

        public static TimeSpan GetDuration(DateTime start, DateTime end)
        {
                var duration = end.Subtract(start);
                return duration;
        }

        public static double GetRadians(double d)
        {
            return (d * Math.PI) / 180;
        }

        public static double GetDistance(double lon1, double lon2, double lat1, double lat2)
        {
            double radius = 6371000; //Constante para adquirir la medida en metros

            double sinLat1 = Math.Sin(GetRadians(lat1));
            double sinLat2 = Math.Sin(GetRadians(lat2));
            double cosLat1 = Math.Cos(GetRadians(lat1));
            double cosLat2 = Math.Cos(GetRadians(lat2));
            double cLon = Math.Cos(GetRadians(lon1) - GetRadians(lon2));

            double cosD = sinLat1 * sinLat2 + cosLat2 * cLon * cosLat1;

            double d = Math.Acos(cosD);

            double distance = radius * d;

            return distance;
        }

        public static List<Route> isRouteSimilar(ValidationModel route)
        {
            List<Route> routeList = new List<Route>();
            var response = new OperationResponse<List<Route>>();

            using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
            {

                var startList = entities.sp_StartList(route.StartPoint.lng, route.StartPoint.lat);
                var endList =  entities.sp_EndList(route.EndPoint.lng, route.EndPoint.lat);



                List<Location> locList1 = new List<Location>();

                foreach (var item in startList)
                {
                    Location loc = new Location();

                    loc.ID_Route = item.ID;

                    locList1.Add(loc);
                }

                List<Location> locList2 = new List<Location>();

                foreach (var item2 in endList)
                {
                    Location loc2 = new Location();

                    loc2.ID_Route = item2.ID;

                    locList2.Add(loc2);
                }

                locList1.Intersect(locList2);

                foreach(var item3 in locList1)
                {
                    Route r = new Route(item3.ID_Route);

                    routeList.Add(r);
                }


                return routeList;

            }
        }

        public static OperationResponse<List<Route>> FilteredRoutes(List<Route> list, double dist)
        {
            List<Route> listR = new List<Route>();
            var response = new OperationResponse<List<Route>>();


            using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
            {
                var ts = entities.TB_SesionEntrenamiento.Where(cred => cred.DistanciaRecorrida - dist <= 0.01 && cred.DistanciaRecorrida - dist >= -0.01);

                foreach (var item in ts)
                {
                    Route r = new Route(item.ID_Ruta);
                    listR.Add(r);
                }

                return response.Complete(listR);

            }
              
        }

        #endregion

        #region Transport Utility Methods
        public static OperationResponse<TrainingSessionModel> SessionToTransport(TrainingSession model)
        {
            TrainingSessionModel tsM = new TrainingSessionModel();
            LocationModel locM = new LocationModel();
            Route route = new Route(model.RouteID);
            tsM.Coordinates = new List<LocationModel>();
            
            var response = new OperationResponse<TrainingSessionModel>();

            tsM.Distance = model.Distance;
            tsM.SessionID = model.SessionID;
            tsM.SessionStatusID = Utility.GetSessionStatusString(model.SessionStatusID);
            tsM.RouteID = model.RouteID;
            tsM.RouteName = route.RouteName;
            tsM.RelativeHumidity = model.RelativeHumidity;
            tsM.Temperature = model.Temperature;
            tsM.AverageBPM = model.AverageBPM;
            tsM.StartTime = model.StartTime;
            tsM.EndTime = model.EndTime;
            tsM.BurntCalories = model.BurntCalories;
            tsM.ClimateConditionID = Utility.GetClimateString(model.ClimateConditionID);
            tsM.UserID = model.UserID;
            tsM.TrainingTypeID = Utility.GetTrainingTyeString(model.TrainingTypeID);
            tsM.CityName = Utility.GetCityString(route.CityID);
            tsM.CountryName = Utility.GetCountryString(Utility.GetCountryFromCity(route.CityID));

            foreach (var item in route.LocationList)
            {
                locM.lat = item.Latitude;
                locM.lng = item.Longitude;
                locM.isMilestone = item.isMilestone;

                tsM.Coordinates.Add(locM);
            }

            return response.Complete(tsM);


        }

        

        public static OperationResponse<List<AthleteUser>> GetAthletes(string userid)
        {
            List<AthleteUser> listModel = new List<AthleteUser>();
            var response = new OperationResponse<List<AthleteUser>>();
            
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var listofUsers = (from c in entities.TB_AtletaEntrenador where c.ID_Entrenador == userid select c);
                    
                    foreach (var item in listofUsers)
                    {
                        var athleteEntity = entities.TB_Atletas.SingleOrDefault(cred => cred.ID_Usuario == item.ID_Atleta);

                        AthleteUser athlete = new AthleteUser();
                        athlete.Username = athleteEntity.ID_Usuario;
                        athlete.Email = athleteEntity.TB_Usuarios.Email;
                        athlete.Bike = (int)athleteEntity.ID_Bicicleta;
                        athlete.BikerType = GetBikerTypeString((int)athleteEntity.ID_TipoCiclista);
                        athlete.BirthDate = athleteEntity.TB_Usuarios.TB_Personas.FechaNacimiento;
                        athlete.CityName = athleteEntity.TB_Usuarios.TB_Personas.TB_Ciudad.NombreCiudad;
                        athlete.CountryName = athleteEntity.TB_Usuarios.TB_Personas.TB_Ciudad.TB_Pais.NombrePais;
                        athlete.Height = (double)athleteEntity.Altura;
                        athlete.LastName = athleteEntity.TB_Usuarios.TB_Personas.Apellidos;
                        athlete.Name = athleteEntity.TB_Usuarios.TB_Personas.Nombre;
                        athlete.Weight = (double)athleteEntity.Peso;
                        athlete.Sex = athleteEntity.TB_Usuarios.TB_Personas.TB_Sexo.Descripcion;
                        athlete.Password = " ";
                        athlete.TelephoneNumber = athleteEntity.TB_Usuarios.TB_Personas.NumeroTelefono;
                        athlete.Role = athleteEntity.TB_Usuarios.TB_Roles.NombreRol;

                        //Add object to list
                        listModel.Add(athlete);

                    }
                    return response.Complete(listModel);
                }
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return response.Failed(ex);
            }
        }
        #endregion
    }
}