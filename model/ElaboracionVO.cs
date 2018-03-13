using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UltraorganicsWS.services;

namespace UltraorganicsWS.model
{
    public class ElaboracionVO : DocumentoVO
    {
        public string whsCode;

        public List<ElaboracionPartidaVO> partidas = new List<ElaboracionPartidaVO>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("whsCode = " + whsCode);
            sb.AppendLine("partidas: [");

            foreach (ElaboracionPartidaVO partida in partidas)
            {
                sb.AppendLine(partida.ToString());
            }

            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}