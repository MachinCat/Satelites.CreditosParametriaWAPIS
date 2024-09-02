using System;
using System.Collections.Generic;
using System.Text;

namespace CreditosParametriaEN.Entidades_Request.Put
{
    public class EnPutCondicion
    {
        public short siModalidad { get; set; }
        public short siCondicion { get; set; }
        public short siTipoDatos { get; set; }
        public bool bEstado { get; set; }
        public string sJson { get; set; }
    }
}
