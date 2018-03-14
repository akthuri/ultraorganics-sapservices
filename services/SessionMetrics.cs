using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UltraorganicsWS.model;

namespace UltraorganicsWS.services
{
    public class SessionMetrics
    {
        public List<MetricVO> SesionesDisponibles = new List<MetricVO>();
        public List<MetricVO> SesionesEnUso = new List<MetricVO>();
    }
}