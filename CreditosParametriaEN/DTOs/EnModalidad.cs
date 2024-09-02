using CreditosParametriaEN.Entidades.Modalidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditosParametriaEN.DTOs
{
    public class EnModalidad
    {
        public List<Modalidad> loModalidades { get; set; }
        public List<ModalidadCondicion> loCondiciones { get; set; }
        public List<ModalidadCondicionMultiple> loCondicionesMultiples { get; set; }
        public List<ModalidadOperador> loOperadores { get; set; }
    }
}
