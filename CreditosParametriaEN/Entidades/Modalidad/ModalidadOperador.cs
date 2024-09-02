using System;
using System.Collections.Generic;
using System.Text;

namespace CreditosParametriaEN.Entidades.Modalidad
{
    public class ModalidadOperador
    {
        public short siTipoDatos { get; set; }
        public short siTipoOperador { get; set; }
        public string vTipoOperador { get; set; }

        // Lista estática de operadores
        public static List<ModalidadOperador> ObtenerOperadores()
        {
            return new List<ModalidadOperador>
            {
                new ModalidadOperador { siTipoDatos = 1, siTipoOperador = 1, vTipoOperador = "=" },
                new ModalidadOperador { siTipoDatos = 1, siTipoOperador = 2, vTipoOperador = "<" },
                new ModalidadOperador { siTipoDatos = 1, siTipoOperador = 3, vTipoOperador = ">" },
                new ModalidadOperador { siTipoDatos = 1, siTipoOperador = 4, vTipoOperador = "<=" },
                new ModalidadOperador { siTipoDatos = 1, siTipoOperador = 5, vTipoOperador = ">=" },
                new ModalidadOperador { siTipoDatos = 1, siTipoOperador = 6, vTipoOperador = "!=" },
                new ModalidadOperador { siTipoDatos = 2, siTipoOperador = 1, vTipoOperador = "In" },
                new ModalidadOperador { siTipoDatos = 2, siTipoOperador = 2, vTipoOperador = "Not In" }
            };
        }
    }
}
