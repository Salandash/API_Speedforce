using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Speedforce_DataAccess;

namespace API_Speedforce.Models
{
    public class Atleta
    {
        public String Username { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public Int32 Bicicleta { get; set; }
        public Int32 TipoCiclista { get; set; }

        public Atleta() { }

        public Atleta(string username)
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
    }
}