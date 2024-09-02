using CreditosParametriaEN.DTOs;
using CreditosParametriaEN.Dtos_BaseDatos;
using CreditosParametriaEN.Entidades.Evaluacion;
using CreditosParametriaEN.Entidades.Evaluacion.Cuota;
using CreditosParametriaEN.Entidades.Modalidad;

namespace CreditosParametriaRN.Generales
{
    public class ModalidadBusinessLogic
    {

        #region Metodos Privados
        private ModalidadOperador? ObtenerOperador(ModalidadCondicion _Parametros)
        {
            return ModalidadOperador.ObtenerOperadores().FirstOrDefault(o => o.siTipoDatos == _Parametros.siTipoDatos && o.siTipoOperador == _Parametros.siTipoOperador);
        }
        private int iCuotaVigente(int _iCantidadTotal, int _iCantidadPagadas)
        {
            int iCuota = 0;
            if (_iCantidadTotal < _iCantidadPagadas)
                iCuota = _iCantidadPagadas + 1;
            else if (_iCantidadTotal == _iCantidadPagadas)
                iCuota = _iCantidadTotal;

            return iCuota;
        }
        private bool bValidanObjecto(decimal _nValorCredito, short _siTipoOperador, decimal _nValorCondicion)
        {
            bool bResultado = false;
            switch (_siTipoOperador)
            {
                case 1: bResultado = _nValorCredito == _nValorCondicion ? true : false; break;
                case 2: bResultado = _nValorCredito < _nValorCondicion ? true : false; break;
                case 3: bResultado = _nValorCredito > _nValorCondicion ? true : false; break;
                case 4: bResultado = _nValorCredito <= _nValorCondicion ? true : false; break;
                case 5: bResultado = _nValorCredito >= _nValorCondicion ? true : false; break;
                case 6: bResultado = _nValorCredito != _nValorCondicion ? true : false; break;
            }

            return bResultado;
        }
        private string sOperadorbjecto(short siTipoDatos)
        {
            string sResultado = string.Empty;
            switch (siTipoDatos)
            {
                case 1: sResultado = " == "; break;
                case 2: sResultado = " < "; break;
                case 3: sResultado = " > "; break;
                case 4: sResultado = " <= "; break;
                case 5: sResultado = " >= "; break;
                case 6: sResultado = " != "; break;
            }

            return sResultado;
        }

        #endregion

        #region Metodos Logica de Negocios

        public EvaluacionCondicion Logica_ProgramasGobierno(ModalidadCondicion _ParametrosModalidadCondicion, EnEvaluacionModalidad _ParametrosEvaluacion, EnModalidad _ParametrosModalidad)
        {
            EvaluacionCondicion oEvaluacionCondicion = new EvaluacionCondicion();
            List<EvaluacionCondicionDetalle> lEvaluacionCondicionDetalle = new List<EvaluacionCondicionDetalle>();

            //  Iterara todas las condiciones existentes de la modalidad.
            foreach (var oCondicion in _ParametrosModalidad.loCondicionesMultiples.Where(o => o.siCondicion == _ParametrosModalidadCondicion.siCondicion))
            {
                EvaluacionCondicionDetalle oEvaluacionCondicionDetalle = new EvaluacionCondicionDetalle();
                oEvaluacionCondicionDetalle.iId = oCondicion.siCondicionID;
                oEvaluacionCondicionDetalle.sDetalle = oCondicion.nValor + " - " + oCondicion.vNombre;
                oEvaluacionCondicionDetalle.bDetalle = oCondicion.nValor == _ParametrosEvaluacion.oEvaluacionCredito.nNumeroRiaz ? true : false;

                lEvaluacionCondicionDetalle.Add(oEvaluacionCondicionDetalle);
            }

            oEvaluacionCondicion.iCondicionID = _ParametrosModalidadCondicion.siCondicion;
            oEvaluacionCondicion.sCondicionNombre = _ParametrosModalidadCondicion.vNombreCondicion;
            oEvaluacionCondicion.lCondicionInfo = lEvaluacionCondicionDetalle;


            if (_ParametrosModalidadCondicion.bEstado)
            {
                //  Validamos si el numero de Raiz, se encuentra en la lista de programas de gobierno.
                if (lEvaluacionCondicionDetalle.Exists(o => o.bDetalle == true))
                {
                    oEvaluacionCondicion.bCondicionInfo = false;
                    oEvaluacionCondicion.sCondicionInfo = $"El número producto : [ {_ParametrosEvaluacion.oEvaluacionCredito.nNumeroRiaz} - {_ParametrosEvaluacion.oEvaluacionCredito.nNumeroProducto} ] pertenece a un programa de gobierno.";
                }
                else
                {
                    oEvaluacionCondicion.bCondicionInfo = true;
                    oEvaluacionCondicion.sCondicionInfo = $"El número producto : [ {_ParametrosEvaluacion.oEvaluacionCredito.nNumeroRiaz} - {_ParametrosEvaluacion.oEvaluacionCredito.nNumeroProducto} ] no pertenece a un programa de gobierno.";
                }
            }
            else
            {
                oEvaluacionCondicion.bCondicionInfo = true;
                oEvaluacionCondicion.sCondicionInfo = _ParametrosModalidadCondicion.vNombreCondicion + "deshabilitado.";
            }

            return oEvaluacionCondicion;
        }
        public EvaluacionCondicion Logica_CalificacionesCPP(ModalidadCondicion _ParametrosModalidadCondicion, EnEvaluacionModalidad _ParametrosEvaluacion, EnModalidad _ParametrosModalidad)
        {
            EvaluacionCondicion oEvaluacionCondicion = new EvaluacionCondicion();

            oEvaluacionCondicion.iCondicionID = _ParametrosModalidadCondicion.siCondicion;
            oEvaluacionCondicion.sCondicionNombre = _ParametrosModalidadCondicion.vNombreCondicion;
            oEvaluacionCondicion.lCondicionInfo = null;
            oEvaluacionCondicion.bCondicionInfo = true;
            oEvaluacionCondicion.sCondicionInfo = _ParametrosModalidadCondicion.vNombreCondicion + "Pendiente de implementar.";

            return oEvaluacionCondicion;
        }
        public EvaluacionCondicion Logica_DiasDeAtraaso_Cuo(ModalidadCondicion _ParametrosModalidadCondicion, EnEvaluacionModalidad _ParametrosEvaluacion, EnModalidad _ParametrosModalidad)
        {
            EvaluacionCondicion oEvaluacionCondicion = new EvaluacionCondicion();
            List<EvaluacionCondicionDetalle> lEvaluacionCondicionDetalle = new List<EvaluacionCondicionDetalle>();

            //  Obtiene la cantidad a calulcar para dias de atraso.
            Cuota oCuota = new Cuota(); oCuota.DiasDeAtrasoMaximoEnCuotas(_ParametrosModalidadCondicion.siModalidad, _ParametrosEvaluacion.oEvaluacionCredito.iTotalCuotas);

            //  Cantidade de pagadas Mayor a 0
            if(_ParametrosEvaluacion.oEvaluacionCredito.iCuotasPagadas > 0)
            {
                foreach (var Cuota in _ParametrosEvaluacion.lEvaluacionCuota)
                {
                    if (Cuota.iNumeroCuota >= oCuota.iCuota)
                    {
                        EvaluacionCondicionDetalle oEvaluacionCondicionDetalle = new EvaluacionCondicionDetalle();
                        oEvaluacionCondicionDetalle.iId = Cuota.iNumeroCuota;
                        oEvaluacionCondicionDetalle.sDetalle = Cuota.iDiasAtraso + " dias de atraso. ";
                        oEvaluacionCondicionDetalle.bDetalle = bValidanObjecto(
                            Cuota.iDiasAtraso,                                  //  _nValorCredito
                            _ParametrosModalidadCondicion.siTipoOperador,       //  _siTipoOperador
                            _ParametrosModalidadCondicion.nValorCondicion);     //  _nValorCondicion

                        lEvaluacionCondicionDetalle.Add(oEvaluacionCondicionDetalle);
                    }
                }

                oEvaluacionCondicion.iCondicionID = _ParametrosModalidadCondicion.siCondicion;
                oEvaluacionCondicion.sCondicionNombre = _ParametrosModalidadCondicion.vNombreCondicion;
                oEvaluacionCondicion.lCondicionInfo = lEvaluacionCondicionDetalle;


                if (_ParametrosModalidadCondicion.bEstado)
                {
                    //  Validamos si los alguna de las cuotas no cumple con los dias de atraso.
                    oEvaluacionCondicion.bCondicionInfo = lEvaluacionCondicionDetalle.Exists(o => o.bDetalle == false);
                    string sMensaje = oEvaluacionCondicion.bCondicionInfo ? "cumple" : "no cumple";
                    oEvaluacionCondicion.sCondicionInfo = $"El credito {sMensaje} con la condicion : [ {_ParametrosModalidadCondicion.vNombreCondicion} {sOperadorbjecto(_ParametrosModalidadCondicion.siTipoOperador)} {_ParametrosModalidadCondicion.nValorCondicion}] .";

                }
                else
                {
                    oEvaluacionCondicion.bCondicionInfo = true;
                    oEvaluacionCondicion.sCondicionInfo = _ParametrosModalidadCondicion.vNombreCondicion + "deshabilitado.";
                }
            }
            else
            {
                oEvaluacionCondicion.iCondicionID = _ParametrosModalidadCondicion.siCondicion;
                oEvaluacionCondicion.sCondicionNombre = _ParametrosModalidadCondicion.vNombreCondicion;
                oEvaluacionCondicion.bCondicionInfo = true;
                oEvaluacionCondicion.sCondicionInfo = $"El credito no cuenta con cuotas pagadas : [ {_ParametrosModalidadCondicion.vNombreCondicion} ] ";
            }



            return oEvaluacionCondicion;
        }
        public EvaluacionCondicion Logica_DiasDeAtraaso_Rep(ModalidadCondicion _ParametrosModalidadCondicion, EnEvaluacionModalidad _ParametrosEvaluacion, EnModalidad _ParametrosModalidad)
        {
            EvaluacionCondicion oEvaluacionCondicion = new EvaluacionCondicion();
            List<EvaluacionCondicionDetalle> lEvaluacionCondicionDetalle = new List<EvaluacionCondicionDetalle>();

            //  Obtiene la cantidad a calulcar para dias de atraso.
            Cuota oCuota = new Cuota(); oCuota.DiasDeAtrasoMaximoAlReprogramar(_ParametrosEvaluacion.oEvaluacionCredito.iTotalCuotas, _ParametrosEvaluacion.oEvaluacionCredito.iCuotasPagadas);

            EvaluacionCondicionDetalle oEvaluacionCondicionDetalle = new EvaluacionCondicionDetalle();
            oEvaluacionCondicionDetalle.iId = oCuota.iCuota;
            oEvaluacionCondicionDetalle.sDetalle = _ParametrosEvaluacion.oEvaluacionCredito.iDiasAtraso + " dias de atraso. ";
            oEvaluacionCondicionDetalle.bDetalle = bValidanObjecto(
                _ParametrosEvaluacion.oEvaluacionCredito.iDiasAtraso,   //  _nValorCredito
                _ParametrosModalidadCondicion.siTipoOperador,           //  _siTipoOperador
                _ParametrosModalidadCondicion.nValorCondicion);         //  _nValorCondicion

            lEvaluacionCondicionDetalle.Add(oEvaluacionCondicionDetalle);

            oEvaluacionCondicion.iCondicionID = _ParametrosModalidadCondicion.siCondicion;
            oEvaluacionCondicion.sCondicionNombre = _ParametrosModalidadCondicion.vNombreCondicion;
            oEvaluacionCondicion.lCondicionInfo = lEvaluacionCondicionDetalle.ToList();


            if (_ParametrosModalidadCondicion.bEstado)
            {
                //  Validamos si los alguna de las cuotas no cumple con los dias de atraso.
                oEvaluacionCondicion.bCondicionInfo = lEvaluacionCondicionDetalle.Exists(o => o.bDetalle == true);
                string sMensaje = oEvaluacionCondicion.bCondicionInfo ? "cumple" : "no cumple";
                oEvaluacionCondicion.sCondicionInfo = $"El credito {sMensaje} con la condicion : [ {_ParametrosModalidadCondicion.vNombreCondicion} {sOperadorbjecto(_ParametrosModalidadCondicion.siTipoOperador)} {_ParametrosModalidadCondicion.nValorCondicion}] .";

            }
            else
            {
                oEvaluacionCondicion.bCondicionInfo = true;
                oEvaluacionCondicion.sCondicionInfo = _ParametrosModalidadCondicion.vNombreCondicion + "deshabilitado.";
            }

            return oEvaluacionCondicion;
        }
        public List<EvaluacionFechas> Logica_GenerarCalendarioFecha(ModalidadCondicion _ParametrosModalidadCondicion, EnEvaluacionModalidad _ParametrosEvaluacion, EnModalidad _ParametrosModalidad)
        {
            List<EvaluacionFechas> loFechas = new List<EvaluacionFechas>();

            // Iterar sobre el número de días adicionales
            for (int i = 0; i < _ParametrosModalidadCondicion.nValorCondicion; i++)
            {
                // Crear una nueva fecha sumando días a la fecha base
                DateTime nuevaFecha = _ParametrosEvaluacion.oEvaluacionCredito.dFechaVencimiento.AddDays(i);

                // Determinar el estado de la fecha
                int iEstado = 0;
                string sEstado = string.Empty;
                if (nuevaFecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    iEstado = 3; // Marca como domingo
                    sEstado = "Dia Domingo";
                }
                else if (nuevaFecha.Day >= 24 && nuevaFecha.Day <= 31)
                {
                    iEstado = 2; // Marca como días Deshabilitados.
                    sEstado = "Dias Deshabilitados";
                }
                else
                {
                    iEstado = 1; // Marca como fecha normal
                    sEstado = "Dia Habilitado";
                }

                // Crear un nuevo objeto Fechas
                EvaluacionFechas fecha = new EvaluacionFechas
                {
                    iNumeroFecha = i + 1, // Asignar un número secuencial a cada fecha
                    sFecha = nuevaFecha.ToString("yyyy-MM-dd"), // Formato de la fecha en cadena
                    iEstado = iEstado, // Asignar el estado determinado
                    sEstado = sEstado
                };

                // Agregar la fecha a la lista
                loFechas.Add(fecha);
            }

            return loFechas;
        }

        #endregion

        public List<EvaluacionCondicion> RegistrosExistentes(EnEvaluacionModalidad _ParametrosEvaluacion)
        {
            List<EvaluacionCondicion> loRegistros = new List<EvaluacionCondicion>();
            int iContandor = 0;
            foreach (var oRegistro in _ParametrosEvaluacion.lEvaluacionRegistro)
            {
                iContandor++;
                EvaluacionCondicion oEvaluacionCondicion = new EvaluacionCondicion();
                oEvaluacionCondicion.iCondicionID = oRegistro.iIdReprogramacion;
                if (oRegistro.siCodigoEstado == 1) oEvaluacionCondicion.sCondicionNombre = "Pendiente";
                if (oRegistro.siCodigoEstado == 2) oEvaluacionCondicion.sCondicionNombre = "Aprobado";
                oEvaluacionCondicion.sCondicionInfo = "No puede continuar con el registro debido que existe un registros en estado : " + oEvaluacionCondicion.sCondicionNombre;

                loRegistros.Add(oEvaluacionCondicion);
            }


            return loRegistros;
        }


        //public EvaluacionCondicion Logica_DiasDeAtraaso_Rep(EvaluacionCondicion _ParametrosEvaluacionCondicion, EnEvaluacionModalidad _ParametrosEvaluacion, EnModalidad _ParametrosModalidad)
        //{

        //    EvaluacionCondicion oEvaluacionCondicion = _ParametrosEvaluacionCondicion;

        //    //  Obtiene objeto de operador
        //    var Operador = ObtenerOperador(oEvaluacionCondicion);

        //    oEvaluacionCondicion.nValorCredito = _ParametrosEvaluacion.oEvaluacionCredito.iDiasAtraso;
        //    oEvaluacionCondicion.sTipoDatos = Operador.vTipoOperador;
        //    oEvaluacionCondicion.bCumpleCondicion = bValidaCondicion(Operador, oEvaluacionCondicion);

        //    if (oEvaluacionCondicion.bCumpleCondicion)
        //        oEvaluacionCondicion.vInfo = $"El credito cumple con los dias de atraso al momento de reprogramar.";
        //    else
        //        oEvaluacionCondicion.vInfo = $"El credito no cumple con los dias de atraso al momento de reprogramar.";

        //    //  Calculo de cuota vigente.
        //    int iNumeroCuotaActual = 0;
        //    if (_ParametrosEvaluacion.oEvaluacionCredito.iTotalCuotas < _ParametrosEvaluacion.oEvaluacionCredito.iCuotasPagadas)
        //        iNumeroCuotaActual = _ParametrosEvaluacion.oEvaluacionCredito.iCuotasPagadas + 1;
        //    else
        //        iNumeroCuotaActual = _ParametrosEvaluacion.oEvaluacionCredito.iTotalCuotas;

        //    //  Listado a desplegar.
        //    oEvaluacionCondicion.oInfo = (ObtenerEvaluacionDetalle(
        //        iNumeroCuotaActual,
        //        oEvaluacionCondicion.nValorCredito,
        //        oEvaluacionCondicion.sTipoDatos,
        //        oEvaluacionCondicion.nValorCondicion,
        //        oEvaluacionCondicion.bCumpleCondicion));

        //    return oEvaluacionCondicion;
        //}





        //    return loFechas;
        //}
        //public ItemModalidad Validar_DiasDeAtraso(int iDiasAtraso, Condicion oCondiciones)
        //{
        //    ItemModalidad oItem = new ItemModalidad();

        //    oItem.iItem = (int)oCondiciones.siCondicion;
        //    oItem.bRequerido = oCondiciones.bEstado;
        //    oItem.nValor = oCondiciones.nValor;

        //    if (oItem.bRequerido)
        //    {
        //        // Obtener la lista de operadores
        //        var operadores = Operador.ObtenerOperadores();
        //        var operador = operadores.FirstOrDefault(o => o.siTipoOperador == oCondiciones.siTipoOperador);

        //        if (operador != null)
        //        {
        //            // Realizar la comparación según el tipo de operador
        //            switch (operador.siTipoOperador)
        //            {
        //                case 1: // Igualdad
        //                    oItem.bCumple = iDiasAtraso == (int)oCondiciones.nValor;
        //                    oItem.vDescripcion = oItem.bCumple ?
        //                         $"El crédito si cumple la condición: [ Dias de atraso {iDiasAtraso} ] == [ {(int)oCondiciones.nValor} Dias de atraso como Maximo ] " :
        //                         $"El crédito no cumple la condición: [ Dias de atraso {iDiasAtraso} ] != [ {(int)oCondiciones.nValor} Dias de atraso como Maximo ] ";
        //                    break;

        //                case 2: // Menor que
        //                    oItem.bCumple = iDiasAtraso < (int)oCondiciones.nValor;
        //                    oItem.vDescripcion = oItem.bCumple ?
        //                        $"El crédito si cumple la condición: [ Dias de atraso {iDiasAtraso} < {(int)oCondiciones.nValor}  Dias de atraso como Maximo ] " :
        //                        $"El crédito no cumple la condición: [ Dias de atraso {iDiasAtraso} >= {(int)oCondiciones.nValor} Dias de atraso como Maximo ] ";
        //                    break;

        //                case 3: // Mayor que
        //                    oItem.bCumple = iDiasAtraso > (int)oCondiciones.nValor;
        //                    oItem.vDescripcion = oItem.bCumple ?
        //                        $"El crédito si cumple la condición: [ Dias de atraso {iDiasAtraso} > {(int)oCondiciones.nValor}  Dias de atraso como Maximo ] " :
        //                        $"El crédito no cumple la condición: [ Dias de atraso {iDiasAtraso} <= {(int)oCondiciones.nValor} Dias de atraso como Maximo ] ";
        //                    break;

        //                case 4: // Menor o igual que
        //                    oItem.bCumple = iDiasAtraso <= (int)oCondiciones.nValor;
        //                    oItem.vDescripcion = oItem.bCumple ?
        //                        $"El crédito si cumple la condición: [ Dias de atraso {iDiasAtraso} <= {(int)oCondiciones.nValor} Dias de atraso como Maximo ] " :
        //                        $"El crédito no cumple la condición: [ Dias de atraso {iDiasAtraso} > {(int)oCondiciones.nValor}  Dias de atraso como Maximo ] ";
        //                    break;

        //                case 5: // Mayor o igual que
        //                    oItem.bCumple = iDiasAtraso >= (int)oCondiciones.nValor;
        //                    oItem.vDescripcion = oItem.bCumple ?
        //                        $"El crédito si cumple la condición: [ Dias de atraso {iDiasAtraso} >= {(int)oCondiciones.nValor} Dias de atraso como Maximo ] " :
        //                        $"El crédito no cumple la condición: [ Dias de atraso {iDiasAtraso} < {(int)oCondiciones.nValor}  Dias de atraso como Maximo ] ";
        //                    break;

        //                case 6: // Diferente de
        //                    oItem.bCumple = iDiasAtraso != (int)oCondiciones.nValor;
        //                    oItem.vDescripcion = oItem.bCumple ?
        //                       $"El crédito si cumple la condición: [ Dias de atraso {iDiasAtraso} != {(int)oCondiciones.nValor} Dias de atraso como Maximo ] " :
        //                       $"El crédito no cumple la condición: [ Dias de atraso {iDiasAtraso} == {(int)oCondiciones.nValor} Dias de atraso como Maximo ] ";
        //                    break;

        //                    // Aquí puedes agregar más casos si es necesario para otros operadores
        //            }
        //        }
        //    }
        //    else
        //    {
        //        oItem.bCumple = true;
        //        oItem.vDescripcion = "Validacion de dias de atraso deshabilitado.";
        //    }

        //    return oItem;
        //}
        //public ItemModalidad Validar_PeriodoGracia(Condicion oCondiciones)
        //{
        //    ItemModalidad oResultado = new ItemModalidad();

        //    oResultado.iItem = (int)oCondiciones.siCondicion;
        //    oResultado.bRequerido = oCondiciones.bEstado;
        //    oResultado.nValor = oCondiciones.nValor;

        //    if (oResultado.bRequerido)
        //    {
        //        oResultado.bCumple = oCondiciones.nValor >= 0 ? true : false;
        //        oResultado.vDescripcion = oResultado.bCumple ?
        //             $"Periodo de gracia se encuentra Habilitado con un maximo de : {oResultado.nValor} cuotas" :
        //             $"Periodo de gracia se deshabilitado";
        //        oResultado.nValor = oCondiciones.nValor >= 0 ? 1 : 0; ;
        //        return oResultado;

        //    }
        //    else
        //    {
        //        oResultado.bCumple = true;
        //        oResultado.vDescripcion = "Periodo de Gracia (no permitira ingreso de cuotas) deshabilitado.";
        //        oResultado.nValor = 0;
        //    }

        //    return oResultado;
        //}
        //public ItemModalidad Validar_CantidadDeReprogramacones(Condicion oCondiciones, decimal nNumeroOrdinal)
        //{
        //    ItemModalidad oResultado = new ItemModalidad();

        //    oResultado.iItem = (int)oCondiciones.siCondicion;
        //    oResultado.bRequerido = oCondiciones.bEstado;
        //    oResultado.nValor = oCondiciones.nValor;

        //    // Obtener la lista de operadores
        //    var operadores = Operador.ObtenerOperadores();
        //    var operador = operadores.FirstOrDefault(o => o.siTipoOperador == oCondiciones.siTipoOperador);

        //    if (operador != null)
        //    {
        //        // Realizar la comparación según el tipo de operador
        //        switch (operador.siTipoOperador)
        //        {
        //            case 1: // Igualdad
        //                oResultado.bCumple = nNumeroOrdinal == oCondiciones.nValor;
        //                oResultado.vDescripcion = oResultado.bCumple ?
        //                     $"El numero total de reprogramaciones si cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} == {oCondiciones.nValor} Total reprogramaciones como Maximo ] " :
        //                     $"El numero total de reprogramaciones no cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} != {oCondiciones.nValor} Total reprogramaciones como Maximo ] ";
        //                break;

        //            case 2: // Menor que
        //                oResultado.bCumple = nNumeroOrdinal < oCondiciones.nValor;
        //                oResultado.vDescripcion = oResultado.bCumple ?
        //                    $"El numero total de reprogramaciones si cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} < {oCondiciones.nValor}  Total reprogramaciones como Maximo ] " :
        //                    $"El numero total de reprogramaciones no cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} >= {oCondiciones.nValor} Total reprogramaciones como Maximo ] ";
        //                break;

        //            case 3: // Mayor que
        //                oResultado.bCumple = nNumeroOrdinal > oCondiciones.nValor;
        //                oResultado.vDescripcion = oResultado.bCumple ?
        //                    $"El numero total de reprogramaciones si cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} > {oCondiciones.nValor}  Total reprogramaciones como Maximo ] " :
        //                    $"El numero total de reprogramaciones no cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} <= {oCondiciones.nValor} Total reprogramaciones como Maximo ] ";
        //                break;

        //            case 4: // Menor o igual que
        //                oResultado.bCumple = nNumeroOrdinal <= oCondiciones.nValor;
        //                oResultado.vDescripcion = oResultado.bCumple ?
        //                    $"El numero total de reprogramaciones si cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} <= {oCondiciones.nValor} Total reprogramaciones como Maximo ] " :
        //                    $"El numero total de reprogramaciones no cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} > {oCondiciones.nValor}  Total reprogramaciones como Maximo ] ";
        //                break;

        //            case 5: // Mayor o igual que
        //                oResultado.bCumple = nNumeroOrdinal >= oCondiciones.nValor;
        //                oResultado.vDescripcion = oResultado.bCumple ?
        //                    $"El numero total de reprogramaciones si cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} >= {oCondiciones.nValor} Total reprogramaciones como Maximo ] " :
        //                    $"El numero total de reprogramaciones no cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} < {oCondiciones.nValor}  Total reprogramaciones como Maximo ] ";
        //                break;

        //            case 6: // Diferente de
        //                oResultado.bCumple = nNumeroOrdinal != oCondiciones.nValor;
        //                oResultado.vDescripcion = oResultado.bCumple ?
        //                    $"El numero total de reprogramaciones si cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} != {oCondiciones.nValor} Total reprogramaciones como Maximo ] " :
        //                    $"El numero total de reprogramaciones no cumple la condición: [ Nro de reprogramaciones {nNumeroOrdinal} == {oCondiciones.nValor} Total reprogramaciones como Maximo ] ";
        //                break;

        //                // Aquí puedes agregar más casos si es necesario para otros operadores
        //        }
        //    }

        //    return oResultado;
        //}

    }
}
