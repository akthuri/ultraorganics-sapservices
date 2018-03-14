using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HandlebarsDotNet;

namespace UltraorganicsWS.model
{
    public class ResultadoVO
    {
        public bool Success;
        public string Mensaje;
        public int DocEntry;
        public int DocNum;

        public override string ToString()
        {
            string resultado = @"{ Success: {{ Success }}, Mensaje: {{ Mensaje }}, DocEntry: {{ DocEntry }}, DocNum: {{ DocNum }} }";
            var template = Handlebars.Compile(resultado);
            var texto = template(this);

            return texto;
        }
    }
}