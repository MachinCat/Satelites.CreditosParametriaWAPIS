using CreditosParametriaAD;
using CreditosParametriaEN.DTOs;
using CreditosParametriaEN.Entidades_Request.Get;
using CreditosParametriaEN.Entidades_Request.Put;


namespace CreditosParametriaRN.Services
{
    public class ModalidadServices
    {
        private readonly ModalidadRepository _modalidadRepository;
        public ModalidadServices(ModalidadRepository modalidadRepository)
        {
            _modalidadRepository = modalidadRepository;
        }

        public EnServicesResult ObtenerModalidades()
        {
            return ExecuteWithExceptionHandling(() => _modalidadRepository.ObtenerModalidades());
        }

        public EnServicesResult ObtenerModalidadDetalle(ModalidadRequest _Parametros)
        {
            return ExecuteWithExceptionHandling(() => _modalidadRepository.ObtenerModalidadDetalle(_Parametros));
        }

        public EnServicesResult ActualizarModalidad(EnPutModalidad oParametros)
        {
            return ExecuteWithExceptionHandling(() => _modalidadRepository.ActualizarModalidad(oParametros));
        }
        public EnServicesResult ActualizarCondicion(EnPutCondicion oParametros)
        {
            return ExecuteWithExceptionHandling(() => _modalidadRepository.ActualizarCondicion(oParametros));
        }

        public EnServicesResult ExecuteWithExceptionHandling(Func<EnServicesResult> Metodo)
        {
            EnServicesResult oResultado = new EnServicesResult();

            try
            {
                oResultado = Metodo();
            }
            catch (Exception ex)
            {
                oResultado.Error_Exception(ex);
            }

            return oResultado;
        }
    }
}
