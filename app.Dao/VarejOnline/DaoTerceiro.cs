using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using yamacorp.Entidade.Varejonline;

namespace yamacorp.Dao.Varejonline
{
    public class DaoTerceiro
    {

        /*public List<Terceiro> Terceiros(TokenApiVarejonline TokenApi, int inicio = -1, int quantidade = -1, Status? status = null, List<Terceiro.Classe> classes = null, DateTime? alteradoApos = null, string documento = "", OrderBy? orderBy = null, bool carregarCamposCustomizados = false)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Terceiros"); }
            
            string parametros = "";
            parametros += (inicio >= 0)                      ? $"inicio={inicio}&quantidade={((quantidade < 1) ? 1 : quantidade)}" : "";
            parametros += (status != null)                   ? $"&status={status}" : "";
            parametros += (classes !=null)                   ? $"&classes={string.Join(",", classes)}" : "";
            parametros += (alteradoApos != null)             ? $"&alteradoApos={((DateTime)alteradoApos).ToString("dd-MM-yyyy")}%20{((DateTime)alteradoApos).ToString("HH:mm:ss")}" : "";
            parametros += (!string.IsNullOrEmpty(documento)) ? $"&documento={Util.ReformatarCpfCnpj(documento)}" : "";
            parametros += (orderBy != null)                  ? $"&orderBy={orderBy}" : "";
            parametros += (carregarCamposCustomizados)       ? $"&carregarCamposCustomizados={carregarCamposCustomizados}" : "";

            string uri = $"{VO.uri_api_erp}{VO.endpoints_terceiros}?{parametros}&token={TokenApi.access_token}";
            var listaTerceiros = GetTerceiro(uri);

            return listaTerceiros;
        }

        private List<Terceiro> GetTerceiro(string uri)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            //request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            string resposta = response.Content;
            List<Terceiro> terceiros = JsonConvert.DeserializeObject<List<Terceiro>>(resposta);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta {response.StatusCode} (Terceiros Varejonline)");

            return terceiros;
        }*/

        /// <summary>
        /// Método usado para buscar um Terceiro pela API da Varejo Online, passando como parâmetro um número único de documento.
        /// O documento usado para pesquisa é o CPF.
        /// </summary>
        /// <param name="CPF"></param>
        /// <exception cref="Exception"></exception>
        /// <seealso cref="https://github.com/Varejonline/api/wiki/GET-terceiros"/>
        public Terceiro BuscarTerceiroPorDoc(string CPF)
        {
            string TerceiroUri = VarejOnline.uri_api_erp + VarejOnline.endpoint_terceiros;
            RestRequest request = new RestRequest(TerceiroUri, Method.GET);

            /**************************************
             *      PARÂMETROS DA REQUISIÇÃO      *
             **************************************/

            // Inicialmente iremos usar um token passado como parâmetro para a request, mas não é aconselhado!!!
            request.AddQueryParameter("token", ConfigurationManager.AppSettings["VarejoOnline.Api.Token"]);
            
            // A requisição deve retornar apenas um resultado ou nenhum. Caso haja mais de um resultado, checar a duplicidade do cadastro
            request.AddQueryParameter("inicio", "0");
            request.AddQueryParameter("quantidade", "1");

            // Serão considerados apenas os clientes com status ATIVO e INATIVO
            request.AddQueryParameter("status", "ATIVO, INATIVO");
            request.AddQueryParameter("documento", CPF);
            RestClient client = new RestClient();
            
            /*
                Setar o limite de tempo da requisição para 5 segundos, caso seja indefinido, é possível que haja problemas na request
                e o sistema entrar em deadlock. Necessário ajustar um tempo limite e fazer um tratamento de exceções para o timeout!!!
            */
            client.Timeout = 5000;
            
            try
            {
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    JArray jsonResponse = JArray.Parse(response.Content);
                    if(jsonResponse.Count > 0)
                    {
                        // Cliente encontrado - DESSERIALIZAR A REQUEST
                        return null;
                    }
                    else
                    {
                        // Retornar nulo para caso não haja nenhum resultado.
                        return null;
                    }
                }
                else
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            catch
            {
                throw new Exception();
            }
        }
        
        public List<Terceiro> Terceiros(TokenApiVarejonline TokenApi, int inicio = -1, int quantidade = -1, Terceiro.Status? status = null, List<Terceiro.Classe> classes = null, 
            DateTime? alteradoApos = null, string documento = "", Terceiro.OrderBy? orderBy = null, bool carregarCamposCustomizados = false)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim()))
            {
                throw new Exception("Sem Token de acesso para realizar a consulta de Terceiros");
            }

            ParametroBuilder parametros = new ParametroBuilder();
            parametros.Add(Terceiro.Parametros.inicio, inicio); 
            if (inicio >= 0)
                parametros.Add(Terceiro.Parametros.quantidade, (quantidade < 1) ? 1 : quantidade);

            parametros.Add(Terceiro.Parametros.status, status);
            parametros.Add(Terceiro.Parametros.classes, classes);
            parametros.Add(Terceiro.Parametros.alteradoApos, alteradoApos);
            parametros.Add(Terceiro.Parametros.documento, documento);
            parametros.Add(Terceiro.Parametros.orderBy, orderBy);
            parametros.Add(Terceiro.Parametros.carregarCamposCustomizados, carregarCamposCustomizados);
             
            return Terceiros(TokenApi, parametros);
        }


        public List<Terceiro> Terceiros(TokenApiVarejonline TokenApi, ParametroBuilder parametros)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) 
            { 
                throw new Exception("Sem Token de acesso para realizar a consulta de Terceiros"); 
            }
            string uri = $"{new Terceiro().URI_API_EndPoint()}?{parametros}&token={TokenApi.access_token}";

            return DaoVarejonline<Terceiro>.GET(uri);
        }

        public RetornoPOST AdicionarTerceiro(TokenApiVarejonline TokenApi, Terceiro terceiro)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Terceiros"); }
            string uri = $"{new Terceiro().URI_API_EndPoint()}?&token={TokenApi.access_token}";
            return DaoVarejonline<Terceiro>.POST(uri, terceiro);
        }
    }
}
