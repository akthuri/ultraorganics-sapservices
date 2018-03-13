using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HandlebarsDotNet;

namespace UltraorganicsWS.model
{
    public class ElaboracionPartidaVO
    {
        public string itemCode;
        public string itemName;
        public int cantidad;

        public override string ToString()
        {
            string partida = @"{ itemCode: {{itemCode}}, itemName: {{itemName}}, cantidad: {{cantidad}} }";

            var template = Handlebars.Compile(partida);
            var result = template(this);

            return result;
        }
    }
}