using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreditosParametriaEN.DTOs
{
    public class EnServicesResult
    {
        public bool bApiEstado { get; set; } = true;
        public int iApiCodigo { get; set; } = 200;
        public string sApiMensaje { get; set; } = "Ok";
        public object oData { get; set; } = new List<EnServices>();


        public void AddLista(EnServices Parametros)
        {
            List<EnServices> oListaInicial = new List<EnServices>();
            List<EnServices> oListaAgregar = new List<EnServices>();

            oListaInicial = oData == null ? oListaInicial : (List<EnServices>)oData;
            oListaInicial.Add(Parametros);

            oData = oListaInicial;
        }

        // Método para deserializar y obtener un objeto específico de la lista oData
        public T GetLista<T>(string objectNombre) where T : class
        {
            // Busca el primer objeto en la lista oData que tenga el nombre especificado
            var serviceObject = ((List<EnServices>)oData).FirstOrDefault(x => x.ObjectNombre == objectNombre);

            if (serviceObject != null)
            {
                // Deserializa el contenido del objeto a un tipo específico (T)
                return serviceObject.ObjectContent as T;
            }

            // Retorna null si no encuentra el objeto
            return null;
        }


        public void AddObjecto(EnServices Parametros)
        {

            oData = Parametros;
        }
        public void Error_Exception(Exception ex)
        {
            bApiEstado = false;
            iApiCodigo = -1;
            sApiMensaje = ex.Message;
        }

        public void Error_NoEncontrado(string sMensaje)
        {
            bApiEstado = false;
            iApiCodigo = -1;
            sApiMensaje = sMensaje;
        }

    }
    public class EnServices
    {
        public string ObjectNombre { get; set; }
        public object ObjectContent { get; set; }
        public EnServices(string psNombre, object poContent)
        {
            ObjectNombre = psNombre;
            ObjectContent = poContent;
        }
    }
}
