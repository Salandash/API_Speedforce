using Speedforce_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using API_Speedforce.Business;

namespace API_Speedforce.Models
{
    public class Route
    {
        #region Properties
        public String ID_Route { get; set; }
        public String RouteName { get; set; }
        public List<Location> LocationList { get; set; }
        public Int32 CityID { get; set; }
        public TB_Rutas RouteEntity { get; set; }
        #endregion

        #region Constructores
        public Route() { }

        public Route(string id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    if (entities.TB_Rutas.Any(cred => cred.ID_Ruta == id) ) 
                    {
                        //Select all from TB_Coordenadas where id == ID_Ruta
                        Location location;
                        LocationList = new List<Location>();
                        foreach (var item in (from c in entities.TB_Coordenadas where c.ID_Ruta == id select c)) 
                        {
                            location = new Location();
                            location.ID_Route = item.ID_Ruta;
                            location.Latitude = item.Latitud;
                            location.Longitude = item.Longitud;
                            location.LocationEntity = item;
                            location.ID_Location = item.ID_Coordenada;
                            location.isStartPoint = item.isStartPoint;
                            location.isEndPoint = item.isEndPoint;
                            location.isMilestone = item.isMilestone;


                            LocationList.Add(location);
                        }
                        CityID = entities.TB_Rutas.Find(id).ID_Ciudad;
                        RouteName = entities.TB_Rutas.Find(id).NombreRuta;
                    }
                    Console.WriteLine("No existe ruta registrada");

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

        /// <summary>
        /// Method to add route to DB.
        /// </summary>
        /// <returns></returns>
        public OperationResponse<Route> AddRoute()
        {
            var result = new OperationResponse<Route>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                        UpdateRoute();
                        entities.Entry(RouteEntity).State = System.Data.Entity.EntityState.Added;
                        entities.SaveChanges();
                        return result.Complete(this);

                }
            }
            catch (Exception ex)
            {
                return result.Failed(ex);
            }
        }

        #endregion

        #region Utility
        /// <summary>
        /// Method to update EF's Entity object.
        /// </summary>
        public void UpdateRoute()
        {
            RouteEntity = new TB_Rutas();
            try
            {
                RouteEntity.ID_Ruta = this.ID_Route;
                RouteEntity.ID_Ciudad = this.CityID;
                RouteEntity.NombreRuta = this.RouteName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("---DB/Entity Problem---");
                Console.WriteLine();
                Console.WriteLine(ex);
            }

        }

        /// <summary>
        /// Method to add location items to the LocationList object.
        /// </summary>
        /// <param name="model"> The TrainingSessionModel with the Coordinate list.</param>
        public void FillLocationList(TrainingSessionModel model)
        {
            try
            {
                Location location;
                LocationList = new List<Location>();
                foreach (var item in model.Coordinates)
                {
                    
                    location = new Location();
                    location.ID_Route = this.ID_Route;
                    location.Latitude = item.lat;
                    location.Longitude = item.lng;
                    location.isMilestone = item.isMilestone;
                    location.isStartPoint = false;
                    location.isEndPoint = false;
                    LocationList.Add(location);

                }

                LocationList[0].isStartPoint = true;
                LocationList.Last().isEndPoint = true; 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion


    }
}
