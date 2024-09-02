using CreditosParametriaEN.Entidades.Modalidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditosParametriaEN.Entidades.Evaluacion
{
    public class EvaluacionCondicion
    {
        public int    iCondicionID { get; set; }
        public string sCondicionNombre { get; set; }
        public string sCondicionInfo { get; set; }
        public bool   bCondicionInfo { get; set; }
        public List<EvaluacionCondicionDetalle> lCondicionInfo { get; set; }
    }
}
