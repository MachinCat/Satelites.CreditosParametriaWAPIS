using CreditosParametriaEN.DTOs;
using CreditosParametriaEN.Dtos_BaseDatos;
using CreditosParametriaEN.Entidades.Evaluacion;
using CreditosParametriaEN.Entidades_Request.Get;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditosParametriaAD
{
    public class EvaluacionRepository
    {
        private readonly string sConnectionString;

        public EvaluacionRepository(string psConnectionString)
        {
            sConnectionString = psConnectionString;
        }

        public EnEvaluacionModalidad ObtenerDatosEvaluacion(EnGetEvaluacion _Parametros)
        {
            EnEvaluacionModalidad oEvaluacion = new EnEvaluacionModalidad();
            oEvaluacion.oResultado = new EnServicesResult();

            try
            {
                var Parametros = new
                {
                    nNumeroCredito = _Parametros.nNumeroCredito
                };

                using (var connection = new SqlConnection(sConnectionString))
                {
                    var multi = connection.QueryMultiple("CRETO_CONFIG_ObtenerDatosEvaluacion", Parametros, commandType: System.Data.CommandType.StoredProcedure);

                    oEvaluacion.oEvaluacionCredito  = multi.ReadFirstOrDefault<EvaluacionCredito>();
                    oEvaluacion.lEvaluacionCuota    = multi.Read <EvaluacionCuota>().ToList();
                    oEvaluacion.lEvaluacionRegistro = multi.Read <EvaluacionRegistro>().ToList();
                    return oEvaluacion;
                }
            }
            catch (Exception ex)
            {
                oEvaluacion.oResultado.Error_Exception(ex);
                return oEvaluacion;
            }
        }

    }
}
