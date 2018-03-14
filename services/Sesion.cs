using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPbobsCOM;

namespace UltraorganicsWS.services
{
    public class Sesion
    {
        public Guid uui = Guid.NewGuid();
        public DateTime born = DateTime.Now;
        public Company company = null;

        public void Close()
        {
            SessionPool.DevolverSesion(uui);
        }
    }
}