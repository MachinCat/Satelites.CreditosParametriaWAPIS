using CreditosParametriaEN.Entidades.Modalidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditosParametriaEN.Entidades.Evaluacion
{
    public class Evaluacion
    {
        public int iCuotaAdicional { get; set; }
        public int iCuotaReduccion { get; set; }
        public short siModalidad { get; set; }
        public List<EvaluacionFechas> lFechas { get; set; }
        public List<EvaluacionCondicion> lCondiciones { get; set; }
        public List<EvaluacionCondicion> lRegistros { get; set; }

    }
}
