using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using yamacorp.Entidade.YamcolNet;
using static System.Net.WebRequestMethods;

namespace yamacorp.Entidade.Varejonline
{
    public abstract class VarejOnline
    {
        //=========================================================
        public const string url_erp = "https://integrador.varejonline.com.br";
        public const string uri_api_erp = url_erp + "/apps/api";
        public const string endpoint_produtos = "/produtos";
        public const string endpoint_terceiros = "/terceiros";
        public const string endpoint_pedidos = "/pedidos";
        public const string endpoint_saldos_mercadorias = "/saldos-mercadorias";
        public const string access_token_teste = "681031b8b3c66a821cce6fb77b36c383a06d6c71bfa832a6eae707000f1a6b1e";
        //=========================================================

        public string UrlErp { get; private set; }
        public string UriApiErp { get; private set; }
        public string EndpointProdutos { get; private set; }
        public string EndpointTerceiros { get; private set; }
        public string EndpointPedidos { get; private set; }
        public string EndpointSaldosMercadorias { get; private set; }
        public string AccessTokenTeste { get; private set; }

        
        // Class Constructor
        public VarejOnline()
        {
            UrlErp = "https://integrador.varejonline.com.br";
            UriApiErp = UrlErp + "/apps/api";
            EndpointProdutos = "/produtos";
            EndpointTerceiros = "/terceiros";
            EndpointPedidos = "/pedidos";
            EndpointSaldosMercadorias = "/saldos-mercadorias";
            AccessTokenTeste = "681031b8b3c66a821cce6fb77b36c383a06d6c71bfa832a6eae707000f1a6b1e";
        }

        public abstract string EndPoint();

        public string URI_API_EndPoint() 
        { 
            return UriApiErp + EndPoint();
        }

        #region Util

        public string LimitarTextoMax50Char(string texto) 
        { 
            return (!string.IsNullOrEmpty(texto) && texto.Length > 50) ? texto.Trim().Substring(0, 50) : texto;
        }
        public string LimitarTextoMax255Char(string texto) 
        { 
            return (!string.IsNullOrEmpty(texto) && texto.Length > 255) ? texto.Trim().Substring(0, 255) : texto;
        } 
        public string LimitarTextoMax400Char(string texto) 
        { 
            return (!string.IsNullOrEmpty(texto) && texto.Length > 400) ? texto.Trim().Substring(0, 400) : texto;
        }

        //LIMITAR QUANTIDADE DE CARACTERES
        //----------------------------------------------------------
        public string LimitarTamanhoTexto(string texto, uint comprimento)
        {
            if (!string.IsNullOrEmpty(texto) && texto.Trim().Length <= comprimento) 
            { 
                return texto.Trim().Substring(0, Convert.ToInt32(comprimento)); 
            }
            else 
            { 
                return texto; 
            }
        }
        //----------------------------------------------------------

        
        // STRING
        public string Formatar(string dados)
        {
            return string.IsNullOrEmpty(dados?.Trim()) ? null : dados.Trim();
        }
        
        // LONG
        public long? Formatar(long? dados) 
        { 
            return (dados != null && dados > 0) ? dados : null; 
        }
        public long? Formatar(long dados)
        { 
            return (dados > 0) ? (long?)dados : null; 
        }

        // DECIMAL
        public decimal? Formatar(decimal? dados)
        { 
            return (dados != null && dados > 0) ? (decimal?)dados : null; 
        }
        public decimal? Formatar(decimal dados) 
        { 
            return (dados > 0) ? (decimal?)dados : null; 
        }


        public string FormatarDataDia(DateTime? data)
        { 
            return (data != null ) ? ((DateTime)data).ToString("dd-MM-yyyy") : null;
        }
        #endregion
    }

    public class Util
    {
        public static string ReformatarCpfCnpj(string documento_cpf_cnpj)
        {
            const string formatacao_cpf = @"000\.000\.000\-00";
            const string formatacao_cnpj = @"00\.000\.000\/0000\-00";

            string erro = "";
            string formatacao = "";

            documento_cpf_cnpj = documento_cpf_cnpj.Trim();

            if (Regex.IsMatch(documento_cpf_cnpj, "^[0-9]{2}\\.?[0-9]{3}\\.?[0-9]{3}\\/[0-9]{4}\\-?[0-9]{2}"))
            {
                formatacao = formatacao_cnpj;
            }
            else if (Regex.IsMatch(documento_cpf_cnpj, "^[0-9]{3}\\.[0-9]{3}\\.[0-9]{3}\\-[0-9]{2}"))
            {
                formatacao = formatacao_cpf;
            }
            else if (Regex.IsMatch(documento_cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", ""), @"^\d+$"))
            {
                documento_cpf_cnpj = documento_cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                if (documento_cpf_cnpj.Length <= 11)
                {
                    formatacao = formatacao_cpf;
                }
                else if (documento_cpf_cnpj.Length <= 14)
                {
                    formatacao = formatacao_cnpj;
                }
                else
                {
                    erro = "O documento CPF ou CNPJ não é válido, o número é maior que 14 dígitos. Nº: " + documento_cpf_cnpj;
                }
            }
            else
            {
                erro = $"O documento CPF ou CNPJ não é válido, {(string.IsNullOrEmpty(documento_cpf_cnpj) ? "está vazio" : "pode tem letra ou caractere especial não separador")}. Nº: {documento_cpf_cnpj}";
            }
            if (string.IsNullOrEmpty(erro) && !string.IsNullOrEmpty(formatacao))
            {
                documento_cpf_cnpj = Convert.ToUInt64(documento_cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "")).ToString(formatacao);
            }
            else
            {
                throw new Exception("Erro na validação CPF ou CNPJ. " + erro);
            }
            return documento_cpf_cnpj;
        }

    }

    public class ParametroBuilder
    {
        
        Dictionary<string, string> parametroMap = new Dictionary<string, string>();

        public void Add(Enum parametro, string valor) { if (!string.IsNullOrEmpty(valor)) parametroMap.Add(parametro.ToString(), valor); }
        public void Add(Enum parametro, int valor)    { if (valor >= 0) parametroMap.Add(parametro.ToString(), valor.ToString()); }
        public void Add(Enum parametro, long valor)   { if (valor >= 0) parametroMap.Add(parametro.ToString(), valor.ToString()); }
        public void Add(Enum parametro, bool? valor)  { if (valor != null) parametroMap.Add(parametro.ToString(), valor.ToString().ToLower()); } 
        public void Add(Enum parametro, List<string> valor) { if (valor != null && valor.Count > 0) parametroMap.Add(parametro.ToString(), string.Join(",", valor)); }
        public void Add(Enum parametro, Enum valor) { if (valor != null) parametroMap.Add(parametro.ToString(), valor.ToString()); }
        public void Add<T>(Enum parametro, List<T> valor) { if (valor != null && valor.Count > 0) parametroMap.Add(parametro.ToString(), string.Join(",", valor)); }

        public void Add(Enum parametro, DateTime? valor) { if (valor != null) parametroMap.Add(parametro.ToString(), ((DateTime)valor).ToString("dd-MM-yyyy") + "%20" + ((DateTime)valor).ToString("HH:mm:ss"));  }
        public void AddDataDia(Enum parametro, DateTime? valor) { if (valor != null) parametroMap.Add(parametro.ToString(), ((DateTime)valor).ToString("dd-MM-yyyy")); }

        public override string ToString() 
        {
            var parametroList = new List<string>();
            foreach (var param in parametroMap)
                parametroList.Add($"{param.Key}={param.Value}");
            return (parametroList.Count > 0) ? string.Join("&", parametroList) : ""; 
        }

        public bool ContemParametro(Enum parametro)
        {
            return (parametro!=null && !string.IsNullOrEmpty(parametro.ToString())) ? parametroMap.ContainsKey(parametro.ToString()) : false;
        }
    }

    public class RetornoPOST
    {
        public string idRecurso { get; set; }
        public int codigoMensagem { get; set; }
        public string mensagem { get; set; }
    }


}
