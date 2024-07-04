using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yamacorp.Entidade.Varejonline
{
    public class SaldoMercadoria: VarejOnline
    {
        public decimal estoqueMinimo { get; set; }         // estoque mínimo da mercadoria
        public decimal estoqueMaximo { get; set; }         // estoque máximo da mercadoria
        public decimal saldoAtual { get; set; }         // saldo atual da mercadoria no sistema
        public decimal quantidadeEstoqueTransito { get; set; }         // quantidades em processo de recebimento por transferência
        public decimal quantidadeReservada { get; set; }         // quantidades reservada e pedidos de compras com status diferentes de CANCELADO, ENCERRADO e ATENDIDO
        public Produto produto { get; set; }         // dados do produto envolvido
        public Entidade entidade { get; set; }         // entidade do saldo envolvido

        public override string EndPoint()
        {
            return endpoint_saldos_mercadorias;
        }

        public enum Parametros
        {
            alteradoApos,
            produtos,
            entidades,
            inicio,
            quantidade,
            somenteEcommerce,
            somenteMarketplace
        }

    }

}
