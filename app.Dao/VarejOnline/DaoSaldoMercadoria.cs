using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yamacorp.Entidade.Varejonline;

namespace yamacorp.Dao.Varejonline
{
    public class DaoSaldoMercadoria
    {
        public List<SaldoMercadoria> SaldosMercadorias(TokenApiVarejonline TokenApi, DateTime alteradoApos, List<string> produtos = null, List<string> entidades = null, int inicio = -1, int quantidade = -1, bool somenteEcommerce=false, bool somenteMarketplace=false)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Saldos Mercadorias"); }

            ParametroBuilder parametros = new ParametroBuilder();

            parametros.Add(SaldoMercadoria.Parametros.alteradoApos, alteradoApos);
            parametros.Add(SaldoMercadoria.Parametros.produtos, produtos);
            parametros.Add(SaldoMercadoria.Parametros.entidades, entidades);
            parametros.Add(SaldoMercadoria.Parametros.inicio, inicio);
            parametros.Add(SaldoMercadoria.Parametros.quantidade, quantidade);
            parametros.Add(SaldoMercadoria.Parametros.somenteEcommerce, somenteEcommerce);
            parametros.Add(SaldoMercadoria.Parametros.somenteMarketplace, somenteMarketplace);

            return SaldosMercadorias(TokenApi, parametros);
        }


        public List<SaldoMercadoria> SaldosMercadorias(TokenApiVarejonline TokenApi, ParametroBuilder parametros)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim()))                  { throw new Exception("Sem Token de acesso para realizar a consulta de Saldos Mercadorias"); }
            if (!parametros.ContemParametro(SaldoMercadoria.Parametros.alteradoApos)) { throw new Exception("Sem parametro 'alteradoApos' na consulta de Saldos Mercadorias"); }
            string uri = $"{new SaldoMercadoria().URI_API_EndPoint()}?{parametros}&token={TokenApi.access_token}";
            return DaoVarejonline<SaldoMercadoria>.GET(uri);
        }
    }
}
