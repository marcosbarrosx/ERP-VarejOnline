using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using yamacorp.Entidade.LinxProducao;

namespace yamacorp.Entidade.Varejonline
{
    public class Produto: VarejOnline
    {
        public long? id      { get; set; }           //id do produto
        public bool ativo   { get; set; }           //indica se o produto está ativo ou não
        public string dataAlteracao { get; set; }   //última data de alteração do produto, no formato dd-mm-aaaa hh:mi:ss
        public string dataCriacao   { get; set; }   //data em que o produto foi criado no sistema, no formato dd-mm-aaaa hh:mi:ss
        public List<string> cnpjFornecedores { get; set; }  //Lista com o CNPJ dos fornecedores associados ao produto.
        public long idFabricante    { get; set; }   //lista com os id dos terceiros que fabrica o produto, se existir
        public string descricao     { get; set; }   //descrição do produto
        public string descricaoSimplificada { get; set; } //descrição simplificada do produto
        public string especificacao { get; set; }   //especificação do produto
        public decimal peso         { get; set; }   //peso do produto
        public decimal altura       { get; set; }   //altura do produto
        public decimal comprimento  { get; set; }   //comprimento do produto
        public decimal largura      { get; set; }   //largura do produto
        public string codigoBarras  { get; set; }   //código de barras do produto
        public List<string> codigosBarraAdicionais { get; set; } //lista de códigos de barras adicionais do produto
        public string codigoInterno { get; set; }   //código interno do produto na empresa
        public string codigoSistema { get; set; }   //código do sistema gerado para o produto
        public string tags      { get; set; }       //Strings concatenadas por vírgula que permitem consulta de produtos no sistema
        public string unidade   { get; set; }       //sigla da unidade do produto
        public List<UnidadeProporcao> unidadesProporcao { get; set; } //Lista de Unidades Proporcionais da Mercadoria
        public Classificacao classificacao { get; set; }   //Classificação do produto, que indica a finalidade para a qual o produto será utilizado.Pode assumir um dos seguintes valores: PRODUCAO_PROPRIA, REVENDA, ATIVO_IMOBILIZADO, CONSUMO, INSUMO

        public enum Classificacao { PRODUCAO_PROPRIA, REVENDA, ATIVO_IMOBILIZADO, CONSUMO, INSUMO }

        public long origem       { get; set; }      //número de 0 a 7 que representa a origem do produto segundo a receita federal
        public string fci        { get; set; }      //número de controle da Ficha de Conteúdo de Importação.
        public string codigoCest { get; set; }      //Código Especificador da Substituição Tributária.
        public string codigoNcm  { get; set; }      //código do NCM( Nomenclatura Comum Mercosul) que identifica a natureza do produto
        public MetodoControle metodoControle    { get; set; }   //método de controle de estoque do produto, podendo assumir um dos seguintes valores:  ESTOCAVEL, LOTE, SERIE, NAO_ESTOCAVEL

        public enum MetodoControle { ESTOCAVEL, LOTE, SERIE, NAO_ESTOCAVEL }

        public bool permiteVenda        { get; set; }   //indica se o produto pode ser vendido através da API de pedidos. Esta regra está relacionada à classificação do produto
        public decimal custoReferencial { get; set; }   //Preço de custo do Produto
        public List<CustoReferencial> listCustoReferencial { get; set; } //Lista de preço de Custo por entidade.
        public decimal preco          { get; set; }     //Preço do Produto na tabela Padrão
        public decimal descontoMaximo { get; set; }     //Porcentagem máxima de desconto que pode ser oferecida na venda do produto
        public decimal comissao       { get; set; }     //Comissão percentual recebida pelo vendedor na venda do produto
        public decimal margemLucro    { get; set; }     //Margem percentual de lucro obtida na venda do produto
        public decimal estoqueMaximo  { get; set; }     //Quantidade referencial máxima de estoque do produto
        public decimal estoqueMinimo  { get; set; }     //Quantidade referencial mínima de estoque do produto
        public List<DadosPorEntidade> dadosPorEntidade { get; set; }   //Lista de Configurações por Entidade do Produto
        public List<ValorAtributo> valorAtributos      { get; set; }   //Valor dos atributos de grade configurados no SKU.
        public List<Categorias> categorias    { get; set; }   //Lista com os dados das categorias que definem a estrutura mercadológica do produto.
        public ProdutoBase produtoBase        { get; set; }   //Dados do produto pai do SKU listado.
        public List<string> urlsFotosProduto  { get; set; }   //Lista com os endereços das imagens associadas a mercadoria
        public bool disponivelEcommerce       { get; set; }   //Define se o produto pode ser disponibilizado no ecommerce
        public bool disponivelMarketplace     { get; set; }   //Define se o produto pode ser disponibilizado no marketplace
        public decimal custoUnitarioRoyalties { get; set; }   //custo unitário em royalties do produto
        public bool precoVariavel             { get; set; }   //Define se o preço de venda do produto é definido somente no momento da venda
        public Componentes componentes        { get; set; }   //Dados dos itens que compõem a ficha técnica do produto
        public List<DescontoProgressivo> descontoProgressivo  { get; set; }   //Valores da configuração do desconto progressivo do produto
        public List<AtributoProduto> atributosProduto         { get; set; }   //Lista com os dados dos atributos que o produto faz parte.
        public List<TabelaPreco> precosPorTabelas             { get; set; }   //Lista com os preços solicitados no parâmetro idsTabelasPrecos

        public override string EndPoint() { return endpoint_produtos; }

        public enum Parametros 
        {
            inicio,
            quantidade,
            alteradoApos,
            categoria,
            produtoBase,
            descricao,
            codigoBarras,
            codigoInterno,
            codigoSistema,
            somenteAtivos,
            somenteComFotos,
            somenteEcommerce,
            somenteMarketplace,
            alteracaoDesde,
            alteracaoAte,
            criacaoDesde,
            criacaoAte,
            idsProdutos,
            idsTabelasPrecos
        }

        public void SetIdentificacao(long? id, string codigoSistema, string codigoInterno, string codigoBarras)
        {
            this.id = Formatar(id);
            this.codigoSistema = Formatar(codigoSistema);
            this.codigoInterno = Formatar(codigoInterno);
            this.codigoBarras = Formatar(codigoBarras);
        }
    }
    
    public class UnidadeProporcao
    {
        public string unidade    { get; set; }   //Sigla da unidade proporcional
        public decimal proporcao { get; set; }   //Proporção da unidade sobre a unidade principal
    }
    public class CustoReferencial
    {
        public long entidade      { get; set; }   //Id da entidade
        public decimal precoCusto { get; set; }   //Custo do produto na entidade
    }
    public class DadosPorEntidade
    {
        public long    entidade      { get; set; } //Id da entidade
        public decimal estoqueMinimo { get; set; } //Estoque mínimo referencial do produto na entidade
        public decimal estoqueMaximo { get; set; } //Estoque máximo referencial do produto na entidade
        public string  codBeneficioFiscal { get; set; } //Código do benefício fiscal do produto na entidade
    }

    public class ValorAtributo
    {
        public string nome   { get; set; }  //nome do atributo da grade
        public string valor  { get; set; }  //valor do atributo para este produto
        public string codigo { get; set; }  //código do valor do atributo
    }

    public class Categorias
    {
        public long   id    { get; set; }   //Id da Categoria
        public string nome  { get; set; }   //Nome da Categoria
        public string nivel { get; set; }   //Nome do nível onde está localizada a categoria
    }

    public class ProdutoBase
    {
        public long   id            { get; set; }   //id do produto pai
        public string codigoSistema { get; set; }   //Código do sistema para o produto pai
        public string nome          { get; set; }   //Nome do produto pai
    }

    public class Componentes
    {
        public Produto  produto         { get; set; }   //Produto presente na ficha técnica
        public decimal  quantidade      { get; set; }   //Quantidade de peças do item na ficha técnica
        public string   unidade         { get; set; }   //Sigla da unidade de medida que representa a quantidade

        public class Produto
        {
            public long   id            { get; set; }   //id do produto
            public string descricao     { get; set; }   //Descricao do produto
            public string codigoBarras  { get; set; }   //Código de barras do produto
            public string codigoInterno { get; set; }   //Código interno do produto
            public string codigoSistema { get; set; }   //Código sistema do produto
        }

    }

    public class DescontoProgressivo
    {
        public decimal qtde      { get; set; }   //Quantidade de peças que ativa o desconto unitário
        public Tipo    tipo      { get; set; }   //Tipo do desconto. Pode ser PERCENTUAL ou VALOR_UNITARIO
        public string  desconto  { get; set; }   //Valor do desconto a ser aplicado
        public string  ativo     { get; set; }   //Indica se o desconto progressivo está ou não ativo

        public enum Tipo { PERCENTUAL, VALOR_UNITARIO }
    }

    public class AtributoProduto
    {
        public long   id      { get; set; }   //id do atributo
        public string nome    { get; set; }   //nome do atributo
        public long   campo   { get; set; }   //id do campo que o atributo pertence
    }

    public class TabelaPreco
    {
        public decimal preco        { get; set; }     //preço do produto na tabela
        public long   idTabelaPreco { get; set; }     //id da tabela
    }
    

    
}
