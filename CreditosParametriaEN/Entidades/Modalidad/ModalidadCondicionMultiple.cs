using System;
using System.Collections.Generic;
using System.Text;

namespace CreditosParametriaEN.Entidades.Modalidad
{
    public class ModalidadCondicionMultiple
    {
        public short siModalidad { get; set; }
        public short siCondicion { get; set; }
        public short siCondicionID { get; set; }
        public short siTipoValor { get; set; }
        public string vNombre { get; set; }
        public decimal nValor { get; set; }
        public bool bEstado { get; set; }
    }
}
