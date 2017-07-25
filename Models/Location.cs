using API_Speedforce.Business;
using Speedforce_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Speedforce.Models
{
    public class Location
    {
        #region Properties
        public Int32 ID_Location { get; set; }
        public String ID_Route { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public Boolean isMilestone { get; set; }
        public Boolean isStartPoint { get; set; }
        public Boolean isEndPoint { get; set; }
        public TB_Coordenadas LocationEntity { get; set; }
        #endregion

        #region Constructors
        public Location() { }

        public Location(string id)
        {
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    var obj = entities.TB_Coordenadas.Find(id);

                    ID_Location = obj.ID_Coordenada;
                    ID_Route = obj.ID_Ruta;
                    Latitude = obj.Latitud;
                    Longitude = obj.Longitud;
                    isEndPoint = obj.isEndPoint;
                    isMilestone = obj.isMilestone;
                    isStartPoint = obj.isStartPoint;
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
        /// Method to add a location to the DB.
        /// </summary>
        /// <returns></returns>
        public OperationResponse<Location> AddLocation()
        {
            var result = new OperationResponse<Location>();
            try
            {
                using (DB_SpeedforceEntities entities = new DB_SpeedforceEntities())
                {
                    UpdateLocation();
                    entities.Entry(LocationEntity).State = System.Data.Entity.EntityState.Added;
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
        /// Method to update the EF's Entity object
        /// </summary>
        public void UpdateLocation()
        {
            LocationEntity = new TB_Coordenadas();

            try
            {
                LocationEntity.ID_Ruta = this.ID_Route;
                LocationEntity.Latitud = this.Latitude;
                LocationEntity.Longitud = this.Longitude;
                LocationEntity.isStartPoint = this.isStartPoint;
                LocationEntity.isEndPoint = this.isEndPoint;
                LocationEntity.isMilestone = this.isMilestone;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion
    }
}
