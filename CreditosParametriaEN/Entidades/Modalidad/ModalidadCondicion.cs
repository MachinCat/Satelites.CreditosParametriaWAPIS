using CreditosParametriaEN.Entidades.Evaluacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditosParametriaEN.Entidades.Modalidad
{
    public class ModalidadCondicion
    {
        public short siModalidad { get; set; }
        public short siCondicion { get; set; }
        public short siTipoDatos { get; set; }
        public short siTipoOperador { get; set; }
        public string vNombreCondicion { get; set; }
        public decimal nValorCondicion { get; set; }
        public bool bEstado { get; set; }
        public string vInfo { get; set; }
    }
}
