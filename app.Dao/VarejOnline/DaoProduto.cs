using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using yamacorp.Entidade.Varejonline;

namespace yamacorp.Dao.Varejonline
{
    public class DaoProduto
    {


        /*public List<Produto> Produtos(TokenApiVarejonline TokenApi, int inicio = -1, int quantidade = -1, DateTime? alteradoApos = null, long categoria=-1, long produtoBase=-1, string descricao="",
            string codigoBarras = "", string codigoInterno = "", string codigoSistema = "", bool somenteAtivos = false, bool somenteComFotos = false, bool somenteEcommerce = false, 
            bool somenteMarketplace = false, DateTime? alteracaoDesde = null, DateTime? alteracaoAte = null, DateTime? criacaoDesde = null, DateTime? criacaoAte = null, 
            List<string> idsProdutos = null, List<string> idsTabelasPrecos = null)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Produto"); }

            string parametros = "";

            parametros += (inicio >= 0)              ? $"{Produto.Parametros.inicio}={inicio}&{Produto.Parametros.quantidade}={((quantidade < 1) ? 1 : quantidade)}" : "";
            parametros += (alteradoApos != null)     ? $"&{Produto.Parametros.alteradoApos}={Util.FormatarData(alteradoApos)}" : "";
            parametros += (categoria >= 0)           ? $"&{Produto.Parametros.categoria}={categoria}" : "";
            parametros += (produtoBase >= 0)         ? $"&{Produto.Parametros.produtoBase}={produtoBase}" : ""; 
            parametros += (!string.IsNullOrEmpty(descricao))     ? $"&{Produto.Parametros.descricao}={descricao}" : "";
            parametros += (!string.IsNullOrEmpty(codigoBarras))  ? $"&{Produto.Parametros.codigoBarras}={codigoBarras}" : "";
            parametros += (!string.IsNullOrEmpty(codigoInterno)) ? $"&{Produto.Parametros.codigoInterno}={codigoInterno}" : "";
            parametros += (!string.IsNullOrEmpty(codigoSistema)) ? $"&{Produto.Parametros.codigoSistema}={codigoSistema}" : "";
            parametros += (somenteAtivos)            ? $"&{Produto.Parametros.somenteAtivos}={somenteAtivos}" : "";
            parametros += (somenteComFotos)          ? $"&{Produto.Parametros.somenteComFotos}={somenteComFotos}" : "";
            parametros += (somenteEcommerce)         ? $"&{Produto.Parametros.somenteEcommerce}={somenteEcommerce}" : "";
            parametros += (somenteMarketplace)       ? $"&{Produto.Parametros.somenteMarketplace}={somenteMarketplace}" : "";
            parametros += (alteracaoDesde != null)   ? $"&{Produto.Parametros.alteracaoDesde}={Util.FormatarData(alteracaoDesde)}" : "";
            parametros += (alteracaoAte != null)     ? $"&{Produto.Parametros.alteracaoAte}={Util.FormatarData(alteracaoAte)}" : "";
            parametros += (criacaoDesde != null)     ? $"&{Produto.Parametros.criacaoDesde}={Util.FormatarData(criacaoDesde)}" : "";
            parametros += (criacaoAte != null)       ? $"&{Produto.Parametros.criacaoAte}={Util.FormatarData(criacaoAte)}" : "";
            parametros += (idsProdutos != null)      ? $"&{Produto.Parametros.idsProdutos}={string.Join(",", idsProdutos)}" : "";
            parametros += (idsTabelasPrecos != null) ? $"&{Produto.Parametros.idsTabelasPrecos}={string.Join(",", idsTabelasPrecos)}" : "";

            string uri = $"{new Produto().URI_API_EndPoint()}?{parametros}&token={TokenApi.access_token}";

            var listaProduto = DaoVarejonline<Produto>.GET(uri);

            return listaProduto;
        }

        public List<Produto> Produtos(TokenApiVarejonline TokenApi, Dictionary<Produto.Parametros,string> parametros)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Produto"); }

            string parametros_uri = "";
            foreach(var param in parametros)
                parametros_uri += (!string.IsNullOrEmpty(param.Value.Trim())) ? $"&{param.Key}={param.Value}" : "";

            string uri = $"{new Produto().URI_API_EndPoint()}?{parametros_uri}&token={TokenApi.access_token}";
            var lista = DaoVarejonline<Produto>.GET(uri);
            return lista;
        }*/

        public List<Produto> Produtos(TokenApiVarejonline TokenApi, int inicio = -1, int quantidade = -1, DateTime? alteradoApos = null, long categoria = -1, long produtoBase = -1, string descricao = "",
            string codigoBarras = "", string codigoInterno = "", string codigoSistema = "", bool? somenteAtivos = null, bool? somenteComFotos = null, bool? somenteEcommerce = null,
            bool? somenteMarketplace = null, DateTime? alteracaoDesde = null, DateTime? alteracaoAte = null, DateTime? criacaoDesde = null, DateTime? criacaoAte = null,
            List<string> idsProdutos = null, List<string> idsTabelasPrecos = null)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Produto"); }

            ParametroBuilder parametros = new ParametroBuilder();

            parametros.Add(Produto.Parametros.inicio, inicio);
            if (inicio >= 0)
                parametros.Add(Produto.Parametros.quantidade, (quantidade < 1) ? 1 : quantidade);

            parametros.Add(Produto.Parametros.alteradoApos, alteradoApos);
            parametros.Add(Produto.Parametros.categoria, categoria);
            parametros.Add(Produto.Parametros.produtoBase, produtoBase);
            parametros.Add(Produto.Parametros.descricao, descricao);
            parametros.Add(Produto.Parametros.codigoBarras, codigoBarras);
            parametros.Add(Produto.Parametros.codigoInterno, codigoInterno);
            parametros.Add(Produto.Parametros.codigoSistema, codigoSistema);
            parametros.Add(Produto.Parametros.somenteAtivos, somenteAtivos);
            parametros.Add(Produto.Parametros.somenteComFotos, somenteComFotos);
            parametros.Add(Produto.Parametros.somenteEcommerce, somenteEcommerce);
            parametros.Add(Produto.Parametros.somenteMarketplace, somenteMarketplace);
            parametros.Add(Produto.Parametros.alteracaoDesde, alteracaoDesde);
            parametros.Add(Produto.Parametros.alteracaoAte, alteracaoAte);
            parametros.Add(Produto.Parametros.criacaoDesde, criacaoDesde);
            parametros.Add(Produto.Parametros.criacaoAte, criacaoAte);
            parametros.Add(Produto.Parametros.idsProdutos, idsProdutos);
            parametros.Add(Produto.Parametros.idsTabelasPrecos, idsTabelasPrecos);

            return Produtos(TokenApi, parametros);
        }

        public List<Produto> Produtos(TokenApiVarejonline TokenApi, ParametroBuilder parametros)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Produtos"); }
            string uri = $"{new Produto().URI_API_EndPoint()}?{parametros}&token={TokenApi.access_token}";
            return DaoVarejonline<Produto>.GET(uri);
        }

    }
}
