using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UltraorganicsWS.services;

namespace UltraorganicsWS.model
{
    public class ElaboracionVO : DocumentoVO
    {
        public string WhsCode;

        public List<ElaboracionPartidaVO> partidas = new List<ElaboracionPartidaVO>();
    }
}