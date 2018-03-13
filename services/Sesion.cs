using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPbobsCOM;

namespace UltraorganicsWS.services
{
    public class Sesion
    {
        public int index = -1;
        public Company company = null;

        public void Close()
        {
            SessionPool.DevolverSesion(index);
        }
    }
}