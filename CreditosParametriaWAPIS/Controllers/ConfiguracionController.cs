using Microsoft.AspNetCore.Mvc;
using CreditosParametriaRN.Services;
using CreditosParametriaEN.DTOs;
using CreditosParametriaRN;
using CreditosParametriaEN.Entidades_Request.Get;
using CreditosParametriaEN.Entidades_Request.Put;

[ApiController]
[Route("api/[controller]")]
public class ConfiguracionController : ControllerBase
{
    private readonly ModalidadServices _serviceModalidad;
    private readonly EvaluacionService _serviceEvaluacion;

    public ConfiguracionController(ModalidadServices serviceModalidad, EvaluacionService serviceEvaluacion)
    {
        _serviceModalidad = serviceModalidad;
        _serviceEvaluacion = serviceEvaluacion;
    }

    #region Modalidades

    [HttpGet("Modalidades")]
    public ActionResult<EnServicesResult> Modalidades()
    {
        var response = _serviceModalidad.ObtenerModalidades();
        return Ok(response);
    }

    [HttpGet("ModalidadDetalle")]
    public ActionResult<EnServicesResult> ModalidadDetalle([FromQuery] ModalidadRequest _Parametros)
    {
        var response = _serviceModalidad.ObtenerModalidadDetalle(_Parametros);
        return Ok(response);
    }

    #endregion

    #region Evaluacion

    #region Evaluacion - GET

    private EnModalidad ObtenerModalidadEvaluacion(short siModalidad)
    {
        EnModalidad oModalidad = new EnModalidad();
        var _Parametros = new ModalidadRequest();
        _Parametros.siModalidad = siModalidad;

        EnServicesResult oResultado = _serviceModalidad.ObtenerModalidadDetalle(_Parametros);
        oModalidad = oResultado.GetLista<EnModalidad>("EnModalidad") as EnModalidad;

        return oModalidad;

    }
    [HttpGet("DatosEvaluacion")]
    public IActionResult DatosEvaluacion([FromQuery] EnGetEvaluacion _Parametros)
    {
        EnServicesResult oResultado = new EnServicesResult();
        EnModalidad oModalidad = new EnModalidad();
        try
        {
            //  Obteniendo Datos de la Modalidad ( Requerido : para los datos de evaluacion )
            oModalidad = ObtenerModalidadEvaluacion(_Parametros.siModalidad);

            //  Obteniendo Datos de la Evaluacion
            oResultado = _serviceEvaluacion.DatosEvaluacion(oModalidad,_Parametros);

        }
        catch (Exception ex)
        {
            oResultado.Error_Exception(ex);
        }

        return Ok(oResultado);
    }
   
    #endregion

    #region Evaluacion - PUT

    [HttpPut("Modalidad")]
    public IActionResult Modalidad([FromBody] EnPutModalidad oParametros)
    {
        if (oParametros == null)
        {
            return BadRequest("Los parámetros de la Modalidad no pueden ser nulos.");
        }

        EnServicesResult oResultado = _serviceModalidad.ActualizarModalidad(oParametros);

        if (oResultado.bApiEstado)
        {
            return Ok(oResultado);
        }
        else
        {
            // Aquí puedes personalizar la respuesta en caso de error
            return StatusCode(500, oResultado);
        }
    }

    [HttpPut("Condicion")]
    public IActionResult Condicion([FromBody] EnPutCondicion oParametros)
    {
        if (oParametros == null)
        {
            return BadRequest("Los parámetros de la Condicion no pueden ser nulos.");
        }

        EnServicesResult oResultado = _serviceModalidad.ActualizarCondicion(oParametros);

        if (oResultado.bApiEstado)
        {
            return Ok(oResultado);
        }
        else
        {
            // Aquí puedes personalizar la respuesta en caso de error
            return StatusCode(500, oResultado);
        }
    }

    #endregion


    #endregion



}
