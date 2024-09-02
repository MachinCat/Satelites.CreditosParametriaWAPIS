using CreditosParametriaAD;
using CreditosParametriaEN.DTOs;
using CreditosParametriaEN.Dtos_BaseDatos;
using CreditosParametriaEN.Entidades.Evaluacion;
using CreditosParametriaEN.Entidades_Request.Get;
using CreditosParametriaRN.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CreditosParametriaRN
{
    public class EvaluacionService
    {
        private readonly EvaluacionRepository _EvaluacionModalidadRepository;

        //  Inicializando clase de Logica de negocio.
        private ModalidadBusinessLogic _Reglas = new ModalidadBusinessLogic();

        //  ##  Importante referenciar las clases para ser utilizadas
        public EvaluacionService(EvaluacionRepository repository)
        {
            _EvaluacionModalidadRepository = repository;
        }

        public EnEvaluacionModalidad ObtenerDatosEvaluacion(EnGetEvaluacion oParametros)
        {
            EnEvaluacionModalidad oEvaluacion = new EnEvaluacionModalidad();

            try
            {
                oEvaluacion = _EvaluacionModalidadRepository.ObtenerDatosEvaluacion(oParametros);
            }
            catch (Exception ex)
            {
                oEvaluacion.oResultado.Error_Exception(ex);
            }

            return oEvaluacion;

        }


        #region Logica de Modalidades.
        public EnServicesResult DatosEvaluacion(EnModalidad _ParametrosModalidad, EnGetEvaluacion _Parametros)
        {
            EnServicesResult oResultado = new EnServicesResult();
            EnServicesResult oModalidad = new EnServicesResult();

            EnEvaluacionModalidad oEnEvaluacionModalidad = ObtenerDatosEvaluacion(_Parametros);
            if (oEnEvaluacionModalidad.oResultado.bApiEstado)
            {
                //  =========================================
                //          Evaluacion de Modalidades
                //  =========================================
                //  
                //          1	=> Cambio Fecha
                //          2	=> Individual
                //          3	=> Normalizacion

                switch (_Parametros.siModalidad)
                {
                    case 1: oResultado = Modalidad_1(_ParametrosModalidad, oEnEvaluacionModalidad); break;
                    case 2: oResultado = Modalidad_2(_ParametrosModalidad, oEnEvaluacionModalidad); break;
                    case 3: oResultado = Modalidad_3(_ParametrosModalidad, oEnEvaluacionModalidad); break;
                    default: oResultado.Error_NoEncontrado("${ Parametros.siModalidad} : Modalidad no reconocida"); break;
                }
            }
            else
            {
                oResultado = oEnEvaluacionModalidad.oResultado;
            }

            return oResultado;

        }

        public EnServicesResult Modalidad_1(EnModalidad _ParametrosModalidad, EnEvaluacionModalidad _ParametrosEvaluacion)
        {
            //  Entidad padre de retorno.
            EnServicesResult oResultado = new EnServicesResult();

            //  Inicializando variables de entorno a encapsular las Condiciones.
            Evaluacion oEvaluacion   = new Evaluacion();
            oEvaluacion.lCondiciones = new List<EvaluacionCondicion>();
            oEvaluacion.lRegistros   = new List<EvaluacionCondicion>(); oEvaluacion.lRegistros= (_Reglas.RegistrosExistentes(_ParametrosEvaluacion));
            foreach (var oCondicion in _ParametrosModalidad.loCondiciones)
            {
                oEvaluacion.siModalidad = oCondicion.siModalidad;
                //  ##  Programas de gobierno
                if (oCondicion.siCondicion == 1)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_ProgramasGobierno(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Calificacion Normal o CPP	(Pendiente)
                if (oCondicion.siCondicion == 2)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_CalificacionesCPP(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Dias de atraso maximo en ultimas 6 cuotas ( 6 es configurable )
                if (oCondicion.siCondicion == 3)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_DiasDeAtraaso_Cuo(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                ////  ##  Dias de atraso al momento de reprogramar	
                if (oCondicion.siCondicion == 4)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_DiasDeAtraaso_Rep(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Dias de reprogramacion a otorgar	
                if (oCondicion.siCondicion == 5)
                {
                    oEvaluacion.lFechas = (_Reglas.Logica_GenerarCalendarioFecha(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));
                }

                //  ##  Cuotas adicionales		
                if (oCondicion.siCondicion == 6)
                {
                    oEvaluacion.iCuotaAdicional = (int)oCondicion.nValorCondicion;
                }
            }
      
            //  Encapsulando clases.
            oResultado.AddLista(new EnServices("oEvaluacion", oEvaluacion));

            return oResultado;
        }

        public EnServicesResult Modalidad_2(EnModalidad _ParametrosModalidad, EnEvaluacionModalidad _ParametrosEvaluacion)
        {
            //  Entidad padre de retorno.
            EnServicesResult oResultado = new EnServicesResult();

            //  Inicializando variables de entorno a encapsular las Condiciones.
            Evaluacion oEvaluacion = new Evaluacion();
            oEvaluacion.lCondiciones = new List<EvaluacionCondicion>();
            oEvaluacion.lRegistros = new List<EvaluacionCondicion>(); oEvaluacion.lRegistros = (_Reglas.RegistrosExistentes(_ParametrosEvaluacion));
            foreach (var oCondicion in _ParametrosModalidad.loCondiciones)
            {
                oEvaluacion.siModalidad = oCondicion.siModalidad;
                //  ##  Programas de gobierno
                if (oCondicion.siCondicion == 1)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_ProgramasGobierno(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Calificacion Normal o CPP	(Pendiente)
                if (oCondicion.siCondicion == 2)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_CalificacionesCPP(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Dias de atraso maximo en ultimas 6 cuotas ( 6 es configurable )
                if (oCondicion.siCondicion == 3)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_DiasDeAtraaso_Cuo(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                ////  ##  Dias de atraso al momento de reprogramar	
                if (oCondicion.siCondicion == 4)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_DiasDeAtraaso_Rep(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Dias de reprogramacion a otorgar	
                if (oCondicion.siCondicion == 5)
                {
                    oEvaluacion.lFechas = (_Reglas.Logica_GenerarCalendarioFecha(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));
                }

                //  ##  Cuotas adicionales	
                if (oCondicion.siCondicion == 6)
                {
                    oEvaluacion.iCuotaAdicional = (int)oCondicion.nValorCondicion;
                }

                //  ##  	Reduccion del monto de cuota
                if (oCondicion.siCondicion == 7)
                {
                    oEvaluacion.iCuotaReduccion = (int)oCondicion.nValorCondicion;
                }
            }

            //  Encapsulando clases.
            oResultado.AddLista(new EnServices("oEvaluacion", oEvaluacion));

            return oResultado;
        }

        public EnServicesResult Modalidad_3(EnModalidad _ParametrosModalidad, EnEvaluacionModalidad _ParametrosEvaluacion)
        {
            //  Entidad padre de retorno.
            EnServicesResult oResultado = new EnServicesResult();

            //  Inicializando variables de entorno a encapsular las Condiciones.
            Evaluacion oEvaluacion = new Evaluacion();
            oEvaluacion.lCondiciones = new List<EvaluacionCondicion>();
            oEvaluacion.lRegistros = new List<EvaluacionCondicion>(); oEvaluacion.lRegistros = (_Reglas.RegistrosExistentes(_ParametrosEvaluacion));
            foreach (var oCondicion in _ParametrosModalidad.loCondiciones)
            {
                oEvaluacion.siModalidad = oCondicion.siModalidad;
                //  ##  Programas de gobierno
                if (oCondicion.siCondicion == 1)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_ProgramasGobierno(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Calificacion Normal o CPP	(Pendiente)
                if (oCondicion.siCondicion == 2)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_CalificacionesCPP(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Dias de atraso maximo en ultimas 6 cuotas ( 6 es configurable )
                if (oCondicion.siCondicion == 3)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_DiasDeAtraaso_Cuo(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Dias de atraso al momento de reprogramar	
                if (oCondicion.siCondicion == 4)
                    oEvaluacion.lCondiciones.Add(_Reglas.Logica_DiasDeAtraaso_Rep(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));

                //  ##  Dias de reprogramacion a otorgar [Productos Normales]	
                if (oCondicion.siCondicion == 5 && _ParametrosEvaluacion.oEvaluacionCredito.bAgricola == false)
                {
                    oEvaluacion.lFechas = (_Reglas.Logica_GenerarCalendarioFecha(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));
                }
                //  ##  	Dias de reprogramacion a otorgar [Productos Agricola]
                if (oCondicion.siCondicion == 5 && _ParametrosEvaluacion.oEvaluacionCredito.bAgricola == true)
                {
                    oEvaluacion.lFechas = (_Reglas.Logica_GenerarCalendarioFecha(oCondicion, _ParametrosEvaluacion, _ParametrosModalidad));
                }

                //  ##  Cuotas adicionales	
                if (oCondicion.siCondicion == 7)
                {
                    oEvaluacion.iCuotaAdicional = (int)oCondicion.nValorCondicion;
                }

                //  ##  	Reduccion del monto de cuota
                if (oCondicion.siCondicion == 8)
                {
                    oEvaluacion.iCuotaReduccion = (int)oCondicion.nValorCondicion;
                }
            }

            //  Encapsulando clases.
            oResultado.AddLista(new EnServices("oEvaluacion", oEvaluacion));

            return oResultado;
        }

        #endregion







    }
}
