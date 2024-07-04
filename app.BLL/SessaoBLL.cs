using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using yamacorp.Dao;
using yamacorp.Entidade;
using yamacorp.Entidade.YamcolNet;
using static yamacorp.Dao.BasesClientes;

namespace yamacorp.BLL
{
    public class SessaoBLL : DaoConexao
    {

        private List<DaoConexao> conexao;
        private DaoSessao daoSessao;

        public SessaoBLL()
        {

        }

        public SessaoBLL(List<DaoConexao> conexao)
        {
            daoSessao = new DaoSessao(conexao);
        }

        #region Token Api 

        public SessaoApi CarregarSessaoToken(TokenAcesso tokenAcesso)
        {
            return daoSessao.CarregaSessaoApi(tokenAcesso);
        }

        public Autenticador VerificaSenha(Autenticador autenticador)
        {
            return daoSessao.VerificaSenha(autenticador);
        }

        public bool VerificaSenha(Usuario usuario)
        {
            return daoSessao.VerificaSenha(usuario);
        }
        public string CarregarDataServidor()
        {
            string dataServidor = null;

            var client = new RestClient(Url.base_url + "/sessions/data");

            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var retorno = JsonConvert.DeserializeObject<SessaoApi>(response.Content);
                dataServidor = retorno.data;

            }

            return dataServidor;
        }
       



        #endregion
    }
}
