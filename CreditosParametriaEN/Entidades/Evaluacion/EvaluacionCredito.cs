using System;
using System.Collections.Generic;
using System.Text;
using CreditosParametriaEN.Entidades.Modalidad;

namespace CreditosParametriaEN.Entidades.Evaluacion
{
    public class EvaluacionCredito
    {
        public int iTotalCuotas { get; set; }
        public int iCuotasPagadas { get; set; }
        public DateTime dFechaVencimiento { get; set; }
        public int iDiasAtraso { get; set; }
        public decimal nNumeroRiaz { get; set; }
        public decimal nSubFamilia { get; set; }
        public decimal nNumeroOrdinal { get; set; }
        public decimal nNumeroProducto { get; set; }
        public decimal nNumeroPeriodo { get; set; }
        public bool bAgricola { get; set; }
    }
}
