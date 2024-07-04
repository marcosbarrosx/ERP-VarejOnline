using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using yamacorp.Entidade.Varejonline;

namespace yamacorp.Dao.Varejonline
{
    public class DaoVarejonline<T>
    {
        private TokenApiVarejonline TokenApi;

        public DaoVarejonline()
        {
            TokenApi = new TokenApiVarejonline();
            TokenApi.access_token = "681031b8b3c66a821cce6fb77b36c383a06d6c71bfa832a6eae707000f1a6b1e";
        }

        public TokenApiVarejonline GerarToken(AuthorizationCode authorization_code)
        {
            //var client = new RestClient("https://erp.varejonline.com.br");
            var client = new RestClient("https://localhost");

            client.Timeout = -1;
            var request = new RestRequest("/apps/oauth/token?", Method.POST);
            request.AddHeader("Content-Type", "application/json"); 

            var jsonAuthorization = JsonConvert.SerializeObject(authorization_code);
            request.AddParameter("application/json", jsonAuthorization, ParameterType.RequestBody);

            var r = request.ToString();

            IRestResponse response = client.Execute(request);

            var token = JsonConvert.DeserializeObject<TokenApiVarejonline>(response.Content);

            return token;
        }


        public string Produtos(uint inicio, uint quantidade)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token))
                throw new Exception("Dados imcompleto para busca produto (Produtos Varejonline)");

            string uri = $"{VO.uri_api_erp}{VO.endpoints_produtos}/?inicio={inicio}&quantidade={quantidade}&token={TokenApi.access_token}";

            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            //request.AddHeader("Authorization", "Bearer " + token);
            /**/
            IRestResponse response = client.Execute(request);

            string resposta = response.Content;
            
            var vendas = JsonConvert.DeserializeObject<List<Produto>>(resposta);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta {response.StatusCode} (Produtos Varejonline)");
            /**/

            /** /string resposta = "[{\"id\":241,\"ativo\":false,\"dataAlteracao\":\"27-01-201315:28:05\",\"dataCriacao\":\"21-01-201315:28:05\",\"cnpjFornecedores\":[\"25.348.796/0001-07\"],\"idFabricante\":155,\"descricao\":\"GRANOLA20G\",\"especificacao\":\"GRANOLA20GSACHE\",\"peso\":65.00,\"altura\":65.00,\"comprimento\":65.00,\"largura\":65.00,\"codigoBarras\":\"6445662344598\",\"codigosBarraAdicionais\":[\"6445662344599\",\"6445662344580\"],\"codigoInterno\":\"12322\",\"codigoSistema\":\"0001\",\"tags\":\"000192,camisalarga,pontadeestoque\",\"unidade\":\"UN\",\"unidadesProporcao\":[{\"unidade\":\"L\",\"proporcao\":0.02}],\"classificacao\":\"CONSUMO\",\"origem\":3,\"fci\":\"B01F70AF-10BF-4B1F-848C-65FF57F616FE\",\"codigoCest\":\"22.001.00\",\"metodoControle\":\"ESTOCAVEL\",\"codigoNCM\":\"9999.99.99\",\"permiteVenda\":false,\"preco\":65,\"precoVariavel\":false,\"descontoMaximo\":10.0,\"comissao\":0,\"margemLucro\":30.0,\"estoqueMaximo\":700,\"estoqueMinimo\":100,\"custoUnitarioRoyalties\":10.00,\"dadosPorEntidade\":[{\"entidade\":1,\"estoqueMinimo\":450,\"estoqueMaximo\":550,\"codBeneficioFiscal\":\"\"},{\"entidade\":2,\"estoqueMinimo\":250,\"estoqueMaximo\":400,\"codBeneficioFiscal\":\"\"}],\"categorias\":[{\"id\":\"1\",\"nome\":\"Natural\",\"nivel\":\"DEPARTAMENTO\",},{\"id\":\"10\",\"nome\":\"Graos\",\"nivel\":\"SETOR\",}],\"listCustoReferencial\":[{\"entidade\":1,\"precoCusto\":29.85},{\"entidade\":2,\"precoCusto\":32.02}],\"urlsFotosProduto\":[\"https://uploader-vpsa-store.s3.amazonaws.com/df9b08f3-d441-44af-8684-a37813f18ee0.png\",\"https://uploader-vpsa-store.s3.amazonaws.com/b5afee84-ba5f-44d1-be4f-48856d7a65a7.png\"],\"disponivelEcommerce\":true,\"disponivelMarketplace\":true,\"atributosProduto\":[{\"nome\":\"Relógio\",\t\"campo\":101,\t\"id\":103},{\"nome\":\"Anonovo\",\t\"campo\":121,\t\"id\":122},{\"nome\":\"Smart\",\t\"campo\":124,\t\"id\":125}]},{\"id\":242,\"ativo\":true,\"dataAlteracao\":\"22-01-201317:21:05\",\"dataCriacao\":\"21-01-201315:28:05\",\"cnpjFornecedores\":[\"25.348.796/0001-07\",\"90.837.241/0001-82\"],\"descricao\":\"CAMISETAAZULP\",\"especificacao\":\"CAMISETAPOLOAZULP\",\"peso\":45.00,\"altura\":65.00,\"comprimento\":65.00,\"largura\":65.00,\"codigoBarras\":\"6465464654567\",\"codigoInterno\":\"12002\",\"codigoSistema\":\"0002.0001\",\"tags\":\"000192,camisalarga,pontadeestoque\",\"unidade\":\"UN\",\"classificacao\":\"REVENDA\",\"origem\":0,\"metodoControle\":\"ESTOCAVEL\",\"codigoNCM\":\"9999.99.99\",\"permiteVenda\":true,\"preco\":0,\"precoVariavel\":true,\"descontoMaximo\":0,\"comissao\":5.0,\"codigoCest\":\"22.001.00\",\"margemLucro\":60.0,\"estoqueMaximo\":100,\"estoqueMinimo\":10,\"dadosPorEntidade\":[{\"entidade\":1,\"estoqueMinimo\":120,\"estoqueMaximo\":150},{\"entidade\":2,\"estoqueMinimo\":30,\"estoqueMaximo\":40}],\"produtoBase\":{\"id\":239,\"codigoSistema\":\"0002\",\"nome\":\"CAMISETAPOLO\"},\"categorias\":[{\"id\":\"5\",\"nome\":\"Vestuario\",\"nivel\":\"DEPARTAMENTO\",},{\"id\":\"35\",\"nome\":\"EsporteFino\",\"nivel\":\"SETOR\",}],\"valorAtributos\":[{\"nome\":\"COR\",\"valor\":\"AZUL\",\"codigo\":\"123\"},{\"nome\":\"TAMANHO\",\"valor\":\"P\",\"codigo\":\"456\"}],\"listCustoReferencial\":[{\"entidade\":1,\"precoCusto\":31.85},{\"entidade\":2,\"precoCusto\":22.02}],\"urlsFotosProduto\":[\"https://uploader-vpsa-store.s3.amazonaws.com/df9b08f3-d441-44af-8684-a37813f18ee0.png\",\"https://uploader-vpsa-store.s3.amazonaws.com/b5afee84-ba5f-44d1-be4f-48856d7a65a7.png\"],\"precosPorTabelas\":[{\"preco\":30.45,\"idTabelaPreco\":1},{\"preco\":27.405000,\"idTabelaPreco\":2}],\"disponivelEcommerce\":false,\"disponivelMarketplace\":true,\"atributosProduto\":[{\"nome\":\"Relógio\",\t\"campo\":101,\t\"id\":103},{\"nome\":\"Anonovo\",\t\"campo\":121,\t\"id\":122},{\"nome\":\"Smart\",\t\"campo\":124,\t\"id\":125}]}]";
            var vendas = JsonConvert.DeserializeObject<List<ListObjectRetorno>>(resposta);/**/

            return resposta;
        }

        public string Terceiro(uint inicio, uint quantidade)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token))
                throw new Exception("Dados imcompleto para busca terceiro (Terceiros Varejonline)");

            string uri = $"{VO.uri_api_erp}{VO.endpoints_terceiros}/?inicio={inicio}&quantidade={quantidade}&token={TokenApi.access_token}";

            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            //request.AddHeader("Authorization", "Bearer " + token);
            /**/
            IRestResponse response = client.Execute(request);

            string resposta = response.Content;

            var vendas = JsonConvert.DeserializeObject<List<Terceiro>>(resposta);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta {response.StatusCode} (Terceiros Varejonline)");
            /**/

            /** /string resposta = "[{\"id\":241,\"ativo\":false,\"dataAlteracao\":\"27-01-201315:28:05\",\"dataCriacao\":\"21-01-201315:28:05\",\"cnpjFornecedores\":[\"25.348.796/0001-07\"],\"idFabricante\":155,\"descricao\":\"GRANOLA20G\",\"especificacao\":\"GRANOLA20GSACHE\",\"peso\":65.00,\"altura\":65.00,\"comprimento\":65.00,\"largura\":65.00,\"codigoBarras\":\"6445662344598\",\"codigosBarraAdicionais\":[\"6445662344599\",\"6445662344580\"],\"codigoInterno\":\"12322\",\"codigoSistema\":\"0001\",\"tags\":\"000192,camisalarga,pontadeestoque\",\"unidade\":\"UN\",\"unidadesProporcao\":[{\"unidade\":\"L\",\"proporcao\":0.02}],\"classificacao\":\"CONSUMO\",\"origem\":3,\"fci\":\"B01F70AF-10BF-4B1F-848C-65FF57F616FE\",\"codigoCest\":\"22.001.00\",\"metodoControle\":\"ESTOCAVEL\",\"codigoNCM\":\"9999.99.99\",\"permiteVenda\":false,\"preco\":65,\"precoVariavel\":false,\"descontoMaximo\":10.0,\"comissao\":0,\"margemLucro\":30.0,\"estoqueMaximo\":700,\"estoqueMinimo\":100,\"custoUnitarioRoyalties\":10.00,\"dadosPorEntidade\":[{\"entidade\":1,\"estoqueMinimo\":450,\"estoqueMaximo\":550,\"codBeneficioFiscal\":\"\"},{\"entidade\":2,\"estoqueMinimo\":250,\"estoqueMaximo\":400,\"codBeneficioFiscal\":\"\"}],\"categorias\":[{\"id\":\"1\",\"nome\":\"Natural\",\"nivel\":\"DEPARTAMENTO\",},{\"id\":\"10\",\"nome\":\"Graos\",\"nivel\":\"SETOR\",}],\"listCustoReferencial\":[{\"entidade\":1,\"precoCusto\":29.85},{\"entidade\":2,\"precoCusto\":32.02}],\"urlsFotosProduto\":[\"https://uploader-vpsa-store.s3.amazonaws.com/df9b08f3-d441-44af-8684-a37813f18ee0.png\",\"https://uploader-vpsa-store.s3.amazonaws.com/b5afee84-ba5f-44d1-be4f-48856d7a65a7.png\"],\"disponivelEcommerce\":true,\"disponivelMarketplace\":true,\"atributosProduto\":[{\"nome\":\"Relógio\",\t\"campo\":101,\t\"id\":103},{\"nome\":\"Anonovo\",\t\"campo\":121,\t\"id\":122},{\"nome\":\"Smart\",\t\"campo\":124,\t\"id\":125}]},{\"id\":242,\"ativo\":true,\"dataAlteracao\":\"22-01-201317:21:05\",\"dataCriacao\":\"21-01-201315:28:05\",\"cnpjFornecedores\":[\"25.348.796/0001-07\",\"90.837.241/0001-82\"],\"descricao\":\"CAMISETAAZULP\",\"especificacao\":\"CAMISETAPOLOAZULP\",\"peso\":45.00,\"altura\":65.00,\"comprimento\":65.00,\"largura\":65.00,\"codigoBarras\":\"6465464654567\",\"codigoInterno\":\"12002\",\"codigoSistema\":\"0002.0001\",\"tags\":\"000192,camisalarga,pontadeestoque\",\"unidade\":\"UN\",\"classificacao\":\"REVENDA\",\"origem\":0,\"metodoControle\":\"ESTOCAVEL\",\"codigoNCM\":\"9999.99.99\",\"permiteVenda\":true,\"preco\":0,\"precoVariavel\":true,\"descontoMaximo\":0,\"comissao\":5.0,\"codigoCest\":\"22.001.00\",\"margemLucro\":60.0,\"estoqueMaximo\":100,\"estoqueMinimo\":10,\"dadosPorEntidade\":[{\"entidade\":1,\"estoqueMinimo\":120,\"estoqueMaximo\":150},{\"entidade\":2,\"estoqueMinimo\":30,\"estoqueMaximo\":40}],\"produtoBase\":{\"id\":239,\"codigoSistema\":\"0002\",\"nome\":\"CAMISETAPOLO\"},\"categorias\":[{\"id\":\"5\",\"nome\":\"Vestuario\",\"nivel\":\"DEPARTAMENTO\",},{\"id\":\"35\",\"nome\":\"EsporteFino\",\"nivel\":\"SETOR\",}],\"valorAtributos\":[{\"nome\":\"COR\",\"valor\":\"AZUL\",\"codigo\":\"123\"},{\"nome\":\"TAMANHO\",\"valor\":\"P\",\"codigo\":\"456\"}],\"listCustoReferencial\":[{\"entidade\":1,\"precoCusto\":31.85},{\"entidade\":2,\"precoCusto\":22.02}],\"urlsFotosProduto\":[\"https://uploader-vpsa-store.s3.amazonaws.com/df9b08f3-d441-44af-8684-a37813f18ee0.png\",\"https://uploader-vpsa-store.s3.amazonaws.com/b5afee84-ba5f-44d1-be4f-48856d7a65a7.png\"],\"precosPorTabelas\":[{\"preco\":30.45,\"idTabelaPreco\":1},{\"preco\":27.405000,\"idTabelaPreco\":2}],\"disponivelEcommerce\":false,\"disponivelMarketplace\":true,\"atributosProduto\":[{\"nome\":\"Relógio\",\t\"campo\":101,\t\"id\":103},{\"nome\":\"Anonovo\",\t\"campo\":121,\t\"id\":122},{\"nome\":\"Smart\",\t\"campo\":124,\t\"id\":125}]}]";
            var vendas = JsonConvert.DeserializeObject<List<ListObjectRetorno>>(resposta);/**/

            return resposta;
        }

        public static List<T> GET(string uri)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string resposta = response.Content;
            List<T> ListObjectRetorno = JsonConvert.DeserializeObject<List<T>>(resposta);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta GET Varejonline, Codigo Status: {response.StatusCode} | {response.ErrorMessage}");

            return ListObjectRetorno;
        }

        public static T GETObjeto(string uri)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string resposta = response.Content;
            T ObjectRetorno = JsonConvert.DeserializeObject<T>(resposta);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta GET Varejonline, Codigo Status: {response.StatusCode} | {response.ErrorMessage}");

            return ObjectRetorno;
        }

        public static T GET_1(string uri)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string resposta = response.Content;
            T ListObjectRetorno = JsonConvert.DeserializeObject<T>(resposta);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta GET Varejonline, Codigo Status: {response.StatusCode} | {response.ErrorMessage}");

            return ListObjectRetorno;
        }

        /*public static RetornoPOST POST(string uri, T objetoBory)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            string jsonObjetoBory = JsonConvert.SerializeObject(objetoBory, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            //request.AddBody(jsonObjetoBory);

            request.AddParameter("application/json", jsonObjetoBory, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            RetornoPOST retorno = JsonConvert.DeserializeObject<RetornoPOST>(response.Content);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta POST Varejonline, Codigo Status: {response.StatusCode} | {response.ErrorMessage}");

            return retorno;
        }*/

        public static RetornoPOST POST(string uri, T objetoBory)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST); 

            string jsonObjetoBory = JsonConvert.SerializeObject(objetoBory, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            request.AddJsonBody(jsonObjetoBory);
            IRestResponse response = client.Execute(request);
            RetornoPOST retorno = JsonConvert.DeserializeObject<RetornoPOST>(response.Content);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception($"Erro na consulta POST Varejonline, Requisição inválida: {retorno.codigoMensagem} | {retorno.mensagem}");

            if (response.StatusCode != HttpStatusCode.Created)
                throw new Exception($"Erro na consulta POST Varejonline, Codigo Status: {response.StatusCode} | {response.ErrorMessage}");

            return retorno;
        }

        public static void POST(string uri)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
             
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception($"Erro na consulta POST Varejonline, Requisição inválida: {response.StatusCode} | {response.ErrorMessage} | {response.Content}");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta POST Varejonline, Codigo Status: {response.StatusCode} | {response.ErrorMessage}");

        }

        /*public static RetornoPOST POST2(string uri, T objetoBory)
        {
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(objetoBory);  
            IRestResponse response = client.Execute(request);
            RetornoPOST retorno = JsonConvert.DeserializeObject<RetornoPOST>(response.Content);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Erro na consulta POST Varejonline, Codigo Status: {response.StatusCode} | {response.ErrorMessage}");

            return retorno;
        }*/

    }
}
