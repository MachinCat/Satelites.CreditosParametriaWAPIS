using Microsoft.Data.SqlClient;
using Dapper;
using CreditosParametriaEN.DTOs;
using CreditosParametriaEN.Entidades.Modalidad;
using CreditosParametriaEN.Entidades_Request.Get;
using CreditosParametriaEN.Entidades_Request.Put;


namespace CreditosParametriaAD
{
    public class ModalidadRepository
    {
        private readonly string sConnectionString;

        public ModalidadRepository(string psConnectionString)
        {
            sConnectionString = psConnectionString;
        }
        public EnServicesResult ObtenerModalidades()
        {
            EnServicesResult Resultado = new EnServicesResult();
            EnModalidad oEnModalidad = new EnModalidad();

            try
            {
                using (var connection = new SqlConnection(sConnectionString))
                {
                    // Obtiene la listas desde el procedimiento almacenado
                    var multi = connection.QueryMultiple("CRETO_CONFIG_ListarModalidad", commandType: System.Data.CommandType.StoredProcedure);

                    oEnModalidad.loModalidades = multi.Read<Modalidad>().ToList();

                    //  Encapsular Entidad
                    Resultado.AddLista(new EnServices("EnModalidad", oEnModalidad));


                }
            }
            catch (Exception ex)
            {
                Resultado.Error_Exception(ex);

            }

            return Resultado;
        }
        public EnServicesResult ObtenerModalidadDetalle(ModalidadRequest _Parametros)
        {
            EnServicesResult Resultado = new EnServicesResult();
            EnModalidad oEnModalidad = new EnModalidad();

            try
            {
                using (var connection = new SqlConnection(sConnectionString))
                {
                    // Obtiene la listas desde el procedimiento almacenado
                    var multi = connection.QueryMultiple("CRETO_CONFIG_ListarModalidadDetalle", _Parametros, commandType: System.Data.CommandType.StoredProcedure);

                    oEnModalidad.loModalidades = multi.Read<Modalidad>().ToList();
                    oEnModalidad.loCondiciones = multi.Read<ModalidadCondicion>().ToList();
                    oEnModalidad.loCondicionesMultiples = multi.Read<ModalidadCondicionMultiple>().ToList();
                    oEnModalidad.loOperadores = ModalidadOperador.ObtenerOperadores(); //  Es requerido para mostrar los Operador en la parte del Front End
                    
                    //  Encapsular Entidad
                    Resultado.AddLista(new EnServices("EnModalidad", oEnModalidad));


                }
            }
            catch (Exception ex)
            {
                Resultado.Error_Exception(ex);

            }

            return Resultado;
        }

        public EnServicesResult ActualizarModalidad(EnPutModalidad oParametros)
        {
            EnServicesResult Resultado = new EnServicesResult();

            try
            {
                using (var connection = new SqlConnection(sConnectionString))
                {
                    connection.Execute("CRETO_CONFIG_ActualizarModalidad", oParametros, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Resultado.Error_Exception(ex);
            }

            return Resultado;
        }

        public EnServicesResult ActualizarCondicion(EnPutCondicion oParametros)
        {
            EnServicesResult Resultado = new EnServicesResult();

            try
            {
                using (var connection = new SqlConnection(sConnectionString))
                {
                    connection.Execute("CRETO_CONFIG_ActualizarCondicion", oParametros, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Resultado.Error_Exception(ex);
            }

            return Resultado;
        }

    }
}
