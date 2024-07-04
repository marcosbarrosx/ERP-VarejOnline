using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using yamacorp.Entidade.Varejonline;

namespace yamacorp.Dao.Varejonline
{
    public class DaoPedido
    {
        public List<Pedido> Pedidos(TokenApiVarejonline TokenApi, List<long> entidades = null, int inicio = -1, int quantidade = -1, DateTime? alteradoApos = null, DateTime? desde = null, DateTime? ate = null, 
            long terceiro=-1, bool? carregarPagamentos = null, bool? carregarImpostosNota = null, List<long> numeros = null, string numeroPedidoCliente = null, List<long> idsOrcamentos = null,  
            Pedido.OrderBy? orderBy = null, bool? integrados = null, string integrador = null, bool? ignorarSimplesFaturamento = null, List<long> statusPedidoVendaIds = null)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Pedidos"); }

            ParametroBuilder parametros = new ParametroBuilder();
            parametros.Add(Pedido.Parametros.entidades, entidades);
            parametros.Add(Pedido.Parametros.inicio, inicio);
            if (inicio >= 0)
                parametros.Add(Pedido.Parametros.quantidade, (quantidade < 1) ? 1 : quantidade);

            parametros.Add(Pedido.Parametros.alteradoApos, alteradoApos);

            if (desde != null && ate == null)  ate = desde;
            else if (ate != null && desde == null)  desde = ate;
                        
            parametros.AddDataDia(Pedido.Parametros.desde, desde); 
            parametros.AddDataDia(Pedido.Parametros.ate, ate);
            parametros.Add(Pedido.Parametros.terceiro, terceiro);
            parametros.Add(Pedido.Parametros.carregarPagamentos, carregarPagamentos);
            parametros.Add(Pedido.Parametros.carregarImpostosNota, carregarImpostosNota);
            parametros.Add(Pedido.Parametros.numeros, numeros);
            parametros.Add(Pedido.Parametros.numeroPedidoCliente, numeroPedidoCliente);
            parametros.Add(Pedido.Parametros.idsOrcamentos, idsOrcamentos);
            parametros.Add(Pedido.Parametros.terceiro, terceiro);
            parametros.Add(Pedido.Parametros.orderBy, orderBy.ToString().Replace("_", "."));
            parametros.Add(Pedido.Parametros.integrados, integrados);
            parametros.Add(Pedido.Parametros.integrador, integrador);
            parametros.Add(Pedido.Parametros.ignorarSimplesFaturamento, ignorarSimplesFaturamento);
            parametros.Add(Pedido.Parametros.statusPedidoVendaIds, statusPedidoVendaIds);

            return Pedidos(TokenApi, parametros);
        }

        public List<Pedido> Pedidos(TokenApiVarejonline TokenApi, ParametroBuilder parametros)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Pedidos"); }
            string uri = $"{new Pedido().URI_API_EndPoint()}?{parametros}&token={TokenApi.access_token}";
            return DaoVarejonline<Pedido>.GET(uri);
        }

        public Pedido BuscarPedidos(TokenApiVarejonline TokenApi, Pedido pedido)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Pedidos"); }
            string uri = $"{new Pedido().URI_API_EndPoint()}/{pedido.id}?&token={TokenApi.access_token}";
            return DaoVarejonline<Pedido>.GETObjeto(uri);
        }

        public RetornoPOST AdicionarPedido(TokenApiVarejonline TokenApi, Pedido pedido)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Pedidos"); }
            string uri = $"{new Pedido().URI_API_EndPoint()}?&token={TokenApi.access_token}";
            return DaoVarejonline<Pedido>.POST(uri, pedido);
        }

        public void CancelarPedito(TokenApiVarejonline TokenApi, Pedido pedido)
        {
            if (string.IsNullOrEmpty(TokenApi.access_token.Trim())) { throw new Exception("Sem Token de acesso para realizar a consulta de Pedidos"); }
            string uri = $"{new Pedido().URI_API_EndPoint()}/{pedido.id}/cancelar?&token={TokenApi.access_token}";
            DaoVarejonline<Pedido>.POST(uri);
        }
    }
}
