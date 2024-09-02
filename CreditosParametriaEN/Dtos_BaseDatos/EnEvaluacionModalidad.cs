using CreditosParametriaEN.DTOs;
using CreditosParametriaEN.Entidades.Evaluacion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace CreditosParametriaEN.Dtos_BaseDatos
{
    public class EnEvaluacionModalidad 
    {
        // Clase contenedora
        public EnServicesResult oResultado { get; set; }

        //  Atribitus para usar en la cpa de AD 
        public EvaluacionCredito oEvaluacionCredito { get; set; }
        public List<EvaluacionCuota> lEvaluacionCuota { get; set; }
        public List<EvaluacionRegistro> lEvaluacionRegistro { get; set; }
        
        

    }
}
