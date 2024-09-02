using System;
using System.Collections.Generic;
using System.Text;

namespace CreditosParametriaEN.Entidades.Evaluacion.Cuota
{
    public class Cuota
    {
        public int iCuota { get; set; }
        public string sCuota { get; set; }
        public bool bCuota { get; set; }

        public void DiasDeAtrasoMaximoEnCuotas(short _siModalidad,int _iTotalCuotas)
        {
            //  Modalidad 1 (Cambio de Fecha) : Solo debe considerarse las ultimas 6.
            if (_siModalidad == 1)
            {
                iCuota = _iTotalCuotas - 6;
                sCuota = "en sus ultimas 6 cuotas";
            }
            //  Modalidad 2 o 3 : Debe considerarse todas las cuotas. por ende la cuota minima debe ser 0.
            else if (_siModalidad == 2 || _siModalidad == 3) 
            {
                iCuota = 0;
                sCuota = "en todas sus cuotas";
            }
        }

        public void DiasDeAtrasoMaximoAlReprogramar(int _iCantidadTotal, int _iCantidadPagadas)
        {
            if (_iCantidadPagadas < _iCantidadTotal)
                iCuota = _iCantidadPagadas + 1;
            else if (_iCantidadPagadas == _iCantidadTotal)
                iCuota = _iCantidadTotal;
        }
    }
}
