using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using yamacorp.Entidade.YamcolNet;

namespace yamacorp.Entidade
{
    public class Sessao
    {
        public Usuario usuario { get; set; }
        public LogsSistema logsSistema { get; set; }
        public Sessao()
        {
            usuario = new Usuario();
            logsSistema = new LogsSistema();

        }
    }
    public class TokenAcesso
    {
        public string token { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public string arquivo_conexao { get; set; }
        public HttpStatusCode status { get; set; }
    }

    public class SessaoApi
    {
        public UsuarioGlobal usuario_global { get; set; }
        public Usuario usuario { get; set; }
        public List<ClienteBase> basesGlobais { get; set; }
        public string token { get; set; }
        public string data { get; set; }
        public HttpStatusCode status { get; set; }
    }

    public class Autenticador
    {
        public int id_usuario { get; set; }
        public string token { get; set; }
        public string usuario { get; set; }
        public string oldSenha { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public HttpStatusCode status { get; set; }
    }
    public class ErroSistema
    {
        public string erro { get; set; } = string.Empty;
    }
    
    public class RootVendedorExterno
    {
        public VendedorExternoApi vendedor_externo { get; set; }
        public string status { get; set; }
        public string erro { get; set; }
    }

    public class VendedorExternoApi
    {
        public int id_vendedor { get; set; }
        public string nome_vendedor { get; set; }
        public string cpf_vendedor { get; set; }
        public bool ativo { get; set; }
        public string token { get; set; }
    }

    //public class Autenticador
    //{
    //    public int id_usuario { get; set; }
    //    public string token { get; set; }
    //    public string usuario { get; set; }
    //    public string oldSenha { get; set; }
    //    public string password { get; set; }
    //    public string confirmPassword { get; set; }

    //    public string error { get; set; }
    //    public HttpStatusCode status { get; set; }


    //    public Autenticador VerificaSenha(Autenticador autenticador)
    //    {
    //        var client = new RestClient(Url.base_url + "/autenticacao");

    //        client.Timeout = -1;
    //        var request = new RestRequest(Method.POST);
    //        request.AddHeader("Authorization", "Bearer " + autenticador.token);
    //        request.AddJsonBody(new { usuario = autenticador.usuario, password = autenticador.password });
    //        IRestResponse response = client.Execute(request);


    //        autenticador = JsonConvert.DeserializeObject<Autenticador>(response.Content);
    //        autenticador.status = response.StatusCode;


    //        return autenticador;
    //    }
    //    public Autenticador AlteraSenha(Autenticador autenticador)
    //    {
    //        var client = new RestClient(Url.base_url + "/usuariosglobais");

    //        client.Timeout = -1;
    //        var request = new RestRequest(Method.PUT);
    //        request.AddHeader("Authorization", "Bearer " + autenticador.token);
    //        request.AddJsonBody(new { id_usuario = autenticador.id_usuario, oldSenha = autenticador.oldSenha, password = autenticador.password, confirmPassword = autenticador.confirmPassword });
    //        IRestResponse response = client.Execute(request);


    //        autenticador = JsonConvert.DeserializeObject<Autenticador>(response.Content);
    //        autenticador.status = response.StatusCode;

    //        return autenticador;
    //    }
    //    public Autenticador ResetSenha(Autenticador autenticador)
    //    {
    //        var client = new RestClient(Url.base_url + "/sessions/reset");

    //        client.Timeout = -1;
    //        var request = new RestRequest(Method.PUT);
    //        request.AddJsonBody(new { id_usuario_global = autenticador.id_usuario, password = autenticador.password, confirmPassword = autenticador.confirmPassword, oldSenha = autenticador.oldSenha });
    //        IRestResponse response = client.Execute(request);


    //        autenticador = JsonConvert.DeserializeObject<Autenticador>(response.Content);
    //        autenticador.status = response.StatusCode;

    //        return autenticador;
    //    }
    //}
    //public class Session
    //{
    //    public UsuarioGlobal usuario_global { get; set; }
    //    public UsuarioSistema usuario { get; set; }
    //    public List<ClienteBase> basesGlobais { get; set; }
    //    public string token { get; set; }
    //    public HttpStatusCode status { get; set; }
    //    public string error { get; set; }

    //    public UsuarioGlobal SolicitarAlteracaoSenha(UsuarioGlobal usuarioGlobal)
    //    {
    //        var client = new RestClient(Url.base_url + "/sessions/recuperar");
    //        client.Timeout = -1;
    //        var request = new RestRequest(Method.PUT);
    //        request.AddJsonBody(new { id_usuario_global = usuarioGlobal.id_usuario_global, codigo_recuperar_senha = usuarioGlobal.senha });
    //        IRestResponse response = client.Execute(request);
    //        usuarioGlobal.status = response.StatusCode;

    //        return usuarioGlobal;
    //    }



    //}
    //public class EncryptaDescrypta
    //{
    //    public bool encriptar { get; set; }
    //    public string key { get; set; }
    //    public string msq { get; set; }
    //    public HttpStatusCode status { get; set; }

    //    public EncryptaDescrypta Encripatar(EncryptaDescrypta encryptaDescrypta)
    //    {
    //        var client = new RestClient(Url.base_url + "/ferramentas");

    //        client.Timeout = -1;
    //        var request = new RestRequest(Method.POST);

    //        request.AddJsonBody(new { key = encryptaDescrypta.key, encriptar = encryptaDescrypta.encriptar });

    //        IRestResponse response = client.Execute(request);


    //        encryptaDescrypta = JsonConvert.DeserializeObject<EncryptaDescrypta>(response.Content);

    //        return encryptaDescrypta;
    //    }
    //}
    //public static class Url
    //{
    //    public static List<string> listUrl = ConfigurationManager.AppSettings["api"] == null ? "http://localhost:3337,http://yamacorp.net:3337,http://localhost:3337".Split(',').ToList() : ConfigurationManager.AppSettings["api"].Split(',').ToList();
    //    public static string base_url = ConfigurationManager.AppSettings["api"] == null ? "http://yamacorp.com.br:3336" : ConfigurationManager.AppSettings["api"];
    //    public static string servidor_sql = ConfigurationManager.AppSettings["servidor_sql"];
    //    public static string link_principal = ConfigurationManager.AppSettings["link_principal"];
    //}
}
