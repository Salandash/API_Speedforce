using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Speedforce.Models
{
    public class SessionAthlete
    {
        public String UserName { get; set; }

        public DateTime LastTimeOnline { get; set; }

        public String Token { get; set; }
    }
}