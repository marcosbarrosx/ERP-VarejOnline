using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static yamacorp.Entidade.Varejonline.Pedido;
using static yamacorp.Entidade.Varejonline.Pedido.Pagamento;
using static yamacorp.Entidade.Varejonline.Pedido.DescontoDetalhe;
using static yamacorp.Entidade.Varejonline.Terceiro;
using System.Runtime.ConstrainedExecution;
using yamacorp.Entidade.LinxProducao;
using static yamacorp.Entidade.Varejonline.Pedido.Pagamento.Cartao;

namespace yamacorp.Entidade.Varejonline
{
    public class Pedido: VarejOnline
    {
        public long id          { get; set; }         //id do pedido (long)
        public long idSaida     { get; set; }         //id da saída vinculada (long)
        public string numero    { get; set; }         //número do pedido de venda gerado pelo sistema (string)
        public string numeroPedidoCliente { get; set; } //número do referência do pedido de venda utilizado pelo cliente (string)
        public string data      { get; set; }         //data de emissão do pedido (string)
        public string horario   { get; set; }         //horário de criação do pedido, no formato hh:mm:ss (string)
        public string dataAlteracao { get; set; }     //última data de alteração do pedido, no formato dd-mm-aaaa hh:mi:ss (string)
        public bool cancelado   { get; set; }         //informa se o pedido está cancelado (boolean)
        public Entidade entidade { get; set; }         //dados da entidade onde foi realizado o pedido 
        public Representante representante { get; set; }         //dados do vendedor principal da venda

        public Terceiro cliente  { get; set; }         //dados do cliente associado à venda 
        public Terceiro terceiro { get; set; }         //??????????????????????????????
        public long idProvisao   { get; set; }         //id da provisão gerada pelo pedido no contas a receber. Use o parâmetro idProvisao no GET Contas Receber (long)
        public List<ContasReceber> parcelas { get; set; }         //lista das parcelas do pedido, cada uma contendo:
        public class ContasReceber
        {
            public long id               { get; set; }         //id da parcela (long)
            public string dataVencimento { get; set; }         //data de vencimento da parcela (string)
            public decimal valor         { get; set; }         //valor total da parcela (decimal)
        }
        
        public Pagamento composicaoPagamento       //Dados do pagamento aplicado na venda
        { 
            get=> pagamento; 
            set { pagamento = value; }
        }
        public Pagamento pagamento { get; set; }   //dados de pagamento

        public class Pagamento
        {
            public long? saidaId              { get; set; }         //id da saida associada ao pedido (long)
            public decimal? valorChequeVista  { get; set; }         //valor pago em cheque a vista (decimal)
            public decimal? valorChequePrazo  { get; set; }         //valor pago em cheque a prazo (decimal)
            public decimal? valorDinheiro     { get; set; }         //valor pago em dinheiro (decimal)
            public decimal? valorCrediario    { get; set; }         //valor gerado no crediário (decimal)
            public decimal? valorVoucher      { get; set; }         //valor pago em voucher (decimal)
            public decimal? valorValePresente { get; set; }         //valor pago com vale presente (decimal)
            public decimal? valorTroca        { get; set; }         //valor pago com troca (decimal)
            public decimal? valorAdiantamento { get; set; }         //valor pago utilizando adiantamentos (decimal)

            #region Pix
            public List<Pix> pixes            { get; set; }         // lista de pix
            public List<Pix> valoresPix       { get=>pixes; set { pixes = value; } }          //Lista de pagamentos utilizando PIX. Lista de:

            public class Pix
            {
                public string dataPagamento { get; set; }         //data de pagamento do boleto (string dd-MM-yyyy)
                public decimal valor        { get; set; }         //valor do PIX (decimal)
                public string nsu           { get; set; }         //Número sequêncial único do PIX (string)
                public string autorizacao   { get; set; }        //autorização da transação (string)
            }
            public void AddPix(Pix pix)
            { 
                if (this.pixes == null) this.pixes = new List<Pix>();
                pixes.Add(pix);
            }
            #endregion

            #region Boleto
            public List<Boleto> boletos { get; set; }
            public List<Boleto> valoresBoleto { get=>boletos; set { boletos = value; } }         //Lista de pagamentos utilizando BOLETO. Lista de: //dados para geração de crediario
            public class Boleto
            {
                public decimal valor         { get; set; }        //valor do BOLETO(decimal) //valor da parcela (decimal)
                public string identificacao  { get; set; }        //Identificacao do BOLETO (string) identificação interna para obtenção do boleto posteriormente (string)(sugestão: utilizar cód. de barra)
                public string dataVencimento { get; set; }        //data de vencimento do boleto (string dd-MM-yyyy)
                public string dataPagamento  { get; set; }        //data de pagamento do boleto (string dd-MM-yyyy)
                public string codigoConta    { get; set; }        //código da conta na qual o boleto será vínculado (string)
            }

            public void AddBoleto(Boleto boleto)
            {
                if (this.boletos == null) this.boletos = new List<Boleto>();
                boletos.Add(boleto);
            }
            #endregion

            #region Vouchers
            public List<Vouchers> vouchers   { get; set; }        //lista de vouchers

            public class Vouchers
            {
                public Voucher voucher { get; set; }
                public decimal valor   { get; set; }

                public class Voucher
                {
                    public long id { get; set; }            //id do voucher (long)
                }

                public void SetVoucher(long id)
                {
                    voucher = new Voucher { id = id };
                }
            }

            public void AddVouchers(Vouchers vouchers)
            {
                if (this.vouchers == null) this.vouchers = new List<Vouchers>();
                this.vouchers.Add(vouchers);
            }
            #endregion

            #region Cartao
            public List<Cartao> cartoes { get; set; }         //lista de valores pagos em cartões
            public List<Cartao> valoresCartao { get=>cartoes; set { cartoes = value; } }         //Lista de pagamentos em cartão. Lista de:
            public class Cartao
            {
                public decimal valor            { get; set; }         //valor do cartão (decimal)
                public string bandeiraNome      { get; set; }         //nome da bandeira do cartão (obrigatório se não informado o campo negociacao)
                public string bandeira          { get=> bandeiraNome; set { bandeiraNome = value; } }         //bandeira do cartão (string)
                public string tipo              { get; set; }         //tipo do cartão (string CREDITO, DEBITO ou OUTROS)
                public string tipoCartao        { get=>tipo; set { tipo = value; } }         //tipo do cartão (string CREDITO, DEBITO ou OUTROS)
                public enum TipoCartao { CREDITO, DEBITO }
                public string operadoraNome     { get; set; }         //nome da operadora de cartão (obrigatório se não informado o campo negociacao)
                public string operadora         { get=> operadoraNome; set { operadoraNome = value; } }         //operadora do cartão (string)
                public int? quantidadeParcelas  { get; set; }          //quantidade de parcelas da transação
                public int? numeroParcelas      { get=> quantidadeParcelas; set { quantidadeParcelas = value; } }         //número de parcelas aplicadas no cartão (inteiro)
                public string parcelamento      { get; set; }         //tipo do parcelamento (SEM_PARCELAMENTO, PARCELAMENTO_LOJISTA, PARCELAMENTO_OPERADORA) (obrigatório se não informado o campo negociacao)
                public enum TipoParcelamento { SEM_PARCELAMENTO, PARCELAMENTO_LOJISTA, PARCELAMENTO_OPERADORA }
                public string nsu               { get; set; }         //NSU da transação (string)
                public string autorizacao       { get; set; }         //Autorização da transação (string)
                public long? negociacao         { get; set; }         //id da negociacao (obrigatório se não informados algum dos seguintes campos: operadoraNome, bandeiraNome, tipo e parcelamento)
            }
            public void AddCartao(Cartao cartao)
            {
                if (this.cartoes == null) this.cartoes = new List<Cartao>();
                this.cartoes.Add(cartao);
            }
            #endregion

            #region Cheque
            public List<Cheque> cheques { get; set; }
            public class Cheque
            {
                public long? titular       { get; set; }
                public long? banco         { get; set; }
                public string agencia      { get; set; }
                public long? conta         { get; set; }
                public long? numero        { get; set; }
                public decimal valor       { get; set; }
                public string vencimento   { get; set; }
            }

            public void AddCheque(Cheque cheque)
            {
                if (this.cheques == null) this.cheques = new List<Cheque>();
                this.cheques.Add(cheque);
            }

            #endregion

            #region Vale
            public List<Vale> vales { get; set; }

            public class Vale
            {
                public long numero { get; set; } 
                public decimal valor { get; set; }
            }
            public void AddVale(Vale vale)
            {
                if (this.vales == null) this.vales = new List<Vale>();
                this.vales.Add(vale);
            }
            #endregion

            #region Adiantamento
            public List<Adiantamento> adiantamentos { get; set; }
            public class Adiantamento
            {
                public long id { get; set; }
                public decimal valor { get; set; }
            }
            public void AddAdiantamento(Adiantamento adiantamento)
            {
                if (this.adiantamentos == null) this.adiantamentos = new List<Adiantamento>();
                this.adiantamentos.Add(adiantamento);
            }
            #endregion

            public Crediario crediario { get; set; }
            public class Crediario
            {
                public long? id { get; set; }
                public decimal valor { get; set; }
                public decimal valorAcrescimo { get; set; }
                public List<Parcela> parcelas { get; set; }
                public class Parcela
                {
                    public long numero { get; set; }
                    public decimal valor { get; set; }
                    public string vencimento { get; set; }
                }

                public void AddParcela(long numero, decimal valor, DateTime vencimento)
                {
                    if(parcelas == null)parcelas = new List<Parcela>();
                    var parcela = new Parcela();
                    parcela.numero = numero;
                    parcela.valor = valor;
                    parcela.vencimento = vencimento.ToString("dd-MM-yyyy");
                    this.parcelas.Add(parcela);
                }
            }

            public Plano plano { get; set; }         //Dados do plano de pagamento utilizado na venda
        }

        public Plano plano { get; set; }         //Dados do plano de pagamento utilizado na venda
        

        public decimal valorTotal      { get; set; }         //valor do pedido (Com abatimentos dos descontos concedidos [bruto - descontos]) (decimal)
        public decimal? valorDesconto  { get; set; }         //valor de desconto total do pedido (decimal)
        public decimal? valorAcrescimo { get; set; }         //valor de acréscimo total do pedido (decimal)
        public List<DescontoDetalhe> descontoDetalhes { get; set; } //lista de detalhes do desconto, contendo:
        public class DescontoDetalhe
        {
            public string observacao     { get; set; }          //observação do desconto (string)
            public decimal valor         { get; set; }          //valor do desconto (decimal)
            public string origemDesconto { get; set; }         //origem do desconto
            public long? idReferencia     { get; set; }          //id da tabela origem do desconto, por exemplo se a origem do desconto for TABELA_PRECO, será o id da tabela (long)
            public string descricao      { get; set; }          //Nome original de referência (Ex, nome da tabela de preço ou nome da ação promocional )
            public enum OrigemDesconto { MANUAL, TABELA_PRECO, DESCONTO_PROGRESSIVO, ACAO_PROMOCIONAL, CLASSE_TERCEIRO, VOUCHER, FIDELIDADE, RATEIO, OUTRO }
        }
        
        public decimal valorLiquido { get; set; }         //valor líquido do pedido (valorTotal - pagamento com vale trocas [valeTrocasUtilizados]) (decimal)
        public decimal? valorFrete   { get; set; }         //valor de frete do pedido (decimal)
        public decimal? valorSeguro  { get; set; }         //valor de seguro do pedido (decimal)
        public decimal? valorOutros  { get; set; }         //outros valores do pedido (decimal)
        public string observacao    { get; set; }         //observações incluídas no pedido (string)
        public bool? vendaConsumidorFinal { get; set; }   //indica se a venda é para consumidor final. Se não informado, será considerado true. (boolean) (opcional)
        public List<Item> itens { get; set; }             //lista de itens do pedido, cada um contendo:
        public class Item
        {
            public Produto produto      { get; set; }         //dados do produto devolvido (objeto complexo)
            public string unidade       { get; set; }         //unidade utilizada na venda (string)
            public decimal quantidade   { get; set; }         //quantidade vendida do produto na unidade informada (decimal)
            public long tabelaPrecoId   { get; set; }         //tabela de preço aplicada na venda do item (long)
            public bool? reservarEstoque { get; set; }        //indica se foi criada uma reserva de estoque para o item (boolean opcional).
            public string dataEntrega       { get; set; }     //indica a data em que será entregue o item ao cliente (Apenas para operação de simples faturamento, formato dd-MM-yyyy)
            public decimal valorTotal       { get; set; }     //valor total do item (decimal)
            public decimal valorDesconto    { get; set; }     //valor de desconto total do item (decimal)
            public decimal valorAcrescimo   { get; set; }     //valor de acréscimo total do item (decimal)
            public List<DescontoDetalhe> descontoDetalhes { get; set; } //lista de detalhes do desconto, contendo:
            public decimal valorUnitario        { get; set; } //valor unitário do item antes de ser aplicado desconto (decimal)
            public decimal valorDescontoItem    { get; set; } //valor de desconto oferecido no item (decimal)
            public decimal valorDescontoRateado { get; set; } //valor de desconto oferecido na venda, rateado no item (decimal)
            public decimal valorPIS    { get; set; }         //valor do PIS aplicado ao item (decimal)
            public decimal valorICMS   { get; set; }         //valor do ICMS aplicado ao item (decimal)
            public decimal valorCOFINS { get; set; }         //valor do CONFINS aplicado ao item (decimal)
            public decimal valorICMSST { get; set; }         //valor do ICMS ST aplicado ao item (decimal)
            public decimal valorIPI    { get; set; }         //valor do IPI aplicado ao item (decimal)
            public decimal valorCusto  { get; set; }         //valor apurado de quanto custou a mercadoria vendida para a empresa. Este é o valor total para toda a quantidade dos itens, o valor individual pode ser obtido dividindo-se este valor pela quantidade de itens (decimal)
            public Lote lote           { get; set; }         //o lote de origem do item, contendo
            public class Lote
            {
                public long id       { get; set; }         //id do lote (long)
                public string codigo { get; set; }         //código do lote (string)
                public string dataFabricacao { get; set; }         //data de fabricação do lote (string)
                public string dataVencimento { get; set; }         //data de vencimento do lote (string)
            }

            public List<Serie> serie { get; set; }         //lista de séries dos produtos vendidos, cada uma contendo:
            public class Serie
            {
                public long id       { get; set; }         //id da série (long)
                public string numero { get; set; }         //número de série do produto (string)
                public string dataFabricacao { get; set; }         //data de fabricação do produto (string)
            }
            public Representante representante { get; set; }         //dados do vendedor do item

            public Object operacao                  //Operação do movimento (lonh | objeto complexo)
            {
                get => _operacao;
                set
                {
                    _operacao = value;
                    carregarDadosOperacao();
                }
            }

            #region operacao  Operação do movimento
            //código Auxiliar
            [JsonIgnore]
            public Object _operacao;

            [JsonIgnore]
            public Operacao operacao_obj = new Operacao();

            public class Operacao
            {
                public long? id          { get; set; }         //id da operação (long)
                public string descricao { get; set; }         //descrição da operação (string)
            }
            public void carregarDadosOperacao()
            {
                string val = operacao.ToString();
                if (val != null && val != "")
                {
                    string[] v_array = val.Split(',');
                    if (val.Contains("{") && v_array.Length == 2)
                    {
                        string[] id_array = v_array[0].Split(':');
                        long id;
                        operacao_obj.id = id_array.Length == 2 && long.TryParse(id_array[1].Trim(), out id) ? (long?)id : null;
                        string[] descricao_array = v_array[1].Split(':');
                        operacao_obj.descricao = descricao_array.Length == 2 ? descricao_array[1].Replace('"', ' ').Replace('\r', ' ').Replace('\n', ' ').Replace('}', ' ').Trim() : "";
                    }
                    else if (!val.Contains("{") && !val.Contains(":"))
                    {
                        string id_string = val.Replace('"', ' ').Trim();
                        long id;
                        operacao_obj.id = id_string != "" && long.TryParse(id_string, out id) ? (long?)id : null;
                    }
                }
            }
            #endregion
            public List<string> numerosValesPresentes { get; set; }         //lista de string contendo os números de vales presentes do item
        }


        public List<Servico> servicos { get; set; }         //lista de servicos vendidos no pedido, cada um contendo:
        public class Servico
        {
            public long? idServico           { get; set; }         //id do serviço(long)
            public string codigoSistema     { get; set; }         //código de sistema do serviço (string)
            public string codigoInterno     { get; set; }         //código interno do serviço (string)
            public string unidade           { get; set; }         //unidade utilizada na venda (string)
            public decimal quantidade       { get; set; }         //quantidade vendida do serviço na unidade informada (decimal)
            public decimal valorTotal       { get; set; }         //valor total do serviço (decimal)
            public decimal valorUnitario    { get; set; }         //valor unitário do serviço antes de ser aplicado desconto (decimal)
            public decimal valorDesconto    { get; set; }         //valor de desconto oferecido no serviço (decimal)
            public decimal valorAcrescimo   { get; set; }         //valor de acréscimo total no serviço (decimal)
            public List<DescontoDetalhe> descontoDetalhes { get; set; }         //lista de detalhes do desconto, contendo:
            public decimal valorPIS     { get; set; }         //valor do PIS aplicado ao serviço (decimal)
            public decimal valorCOFINS  { get; set; }         //valor do CONFINS aplicado ao serviço(decimal)
            public decimal valorISS     { get; set; }         //valor do ISS aplicado ao serviço(decimal)
            public Representante representante { get; set; }         //dados do vendedor do item

            public void SetIdentificacao(long? id, string codigoSistema, string codigoInterno)
            {
                this.idServico = id;
                this.codigoSistema = codigoSistema;
                this.codigoInterno = codigoInterno;
            }
        }

        public string statusConferencia { get; set; }         //status da conferência de caixa: NAO_CONFERIDO', 'CONFERIDO', 'CONFIRMADO', 'PROCESSANDO', 'CONCLUIDO', 'CONFERIDO_AUTOMATICAMENTE' e 'ERRO' (string)
        public enum StatusConferencia { NAO_CONFERIDO, CONFERIDO, CONFIRMADO, PROCESSANDO, CONCLUIDO, CONFERIDO_AUTOMATICAMENTE, ERRO }
        public string nomeTerminal      { get; set; }         //nome do terminal onde foi realizada a venda na loja (string)

        public bool? emitirNotaFiscal          { get; set; }     //indica se a emissão da nota será feita automáticamente (boolean opcional)
        public bool? emitirNotaFiscalPresente  { get; set; }     //indica se a emissão da nota de presente será feita automáticamente (boolean opcional, só funciona se emitirNotaFiscal também for true pois a nota de presente depende da nota de venda para referência)
        public bool? enviarEmailNota           { get; set; }     //indica se após autorização da nota, será enviado Email com Danfe e XML ao destinatário (boolean opcional) (se não informado irá seguir padrão configurado pelo cliente)
        public List<NotasFiscais> notasFiscais { get; set; }     //lista de notas fiscais do pedido, cada uma contendo:
        public class NotasFiscais
        {
            public string status         { get; set; }         //status da nota fiscal (string).
            public enum Status { EMITIDO, CANCELADO, ENVIAR, ENVIADO, ERRO_ENVIO, ERRO }
            public long idNotaFiscal     { get; set; }         //id da nota fiscal (long)
            public string tipoNotaFiscal { get; set; }         //tipo da nota fiscal (string)
        }
        public ValeTrocaGerado valeTrocaGerado { get; set; }         //Vale trocas gerados durante a venda (Venda com troca de mercadorias)
        public class ValeTrocaGerado
        {
            public string numeroValeTroca    { get; set; }         //numero do vale troca gerado na troca de mercadorias (string)
            public decimal idDevolucaoGerada { get; set; }         //id da devolução de venda gerada na troca (decimal)
        }
        
        public List<ValeTrocasUtilizados> valeTrocasUtilizados { get; set; }         //lista de valores de vale trocas utilizados no pagamento do pedido
        public class ValeTrocasUtilizados
        {
            public decimal valorUtilizado { get; set; }         //valor utilizado no pagamento (decimal)
        }
        public string origem { get; set; }         //origem da venda (ECOMMERCE, MARKETPLACE, LOJA_FISICA) (string)
        public enum Origem { ECOMMERCE, MARKETPLACE, LOJA_FISICA }
        public string tipo   { get; set; }         //tipo da venda (NORMAL, SHIP_FROM_STORE, CLICK_COLLECT) (string)
        public enum Tipo { NORMAL, SHIP_FROM_STORE, CLICK_COLLECT }
        public StatusPedidoVenda statusPedidoVenda { get; set; }         //Status da venda (string)

        public class StatusPedidoVenda
        {
            public long id              { get; set; }         //identificador do status (long)
            public string nome          { get; set; }         //nome do status (string)
            public string descricao     { get; set; }         //descrição do status (string)
            public string dataCriacao   { get; set; }         //data de criação do status, no formato dd-mm-aaaa (string)
            public string dataAlteracao { get; set; }         //última data de alteração do status, no formato dd-mm-aaaa (string)
            public bool ativo           { get; set; }         //indica se o status se encontra ativo (boolean)
            public bool deletado        { get; set; }         //indica se o status se encontra deletado (boolean)
        }

        public Terceiro intermediador   { get; set; }         //intermediador da venda (informar id ou documento) 
        public Transporte transporte    { get; set; }         //dados de transporte
        public class Transporte
        {
            public string modalidade      { get; set; }         //modalidade do transporte (EMITENTE, DESTINATARIO_REMETENTE, TERCEIRO, PROPRIO_REMETENTE, PROPRIO_DESTINATARIO)
            public enum Modalidade { EMITENTE, DESTINATARIO_REMETENTE, TERCEIRO, PROPRIO_REMETENTE, PROPRIO_DESTINATARIO }
            public Terceiro transportador { get; set; }         //transportador: 
            public string codigoANTT      { get; set; }         //Código ANTT do transporte (string)
            public string placaVeiculo    { get; set; }         //Placa do veiculo de transporte (string)
            public string estadoVeiculo   { get; set; }         //Sigla do estado do veiculo de transporte (string)
            public long quantidade        { get; set; }         //quantidade do volume transportado(long)
            public string especie         { get; set; }         //espécie do volume transportado
            public string marca          { get; set; }         //marca do volume transportado (decimal)
            public decimal numero         { get; set; }         //número do volume transportado (decimal)
            public decimal pesoBruto      { get; set; }         //peso bruto transportado (decimal)
            public decimal pesoLiquido    { get; set; }         //peso líquido transportado (decimal)
        }
        public EnderecoEntrega enderecoEntrega { get; set; }         //dados do endereço de entrega
        public class EnderecoEntrega
        {
            public string logradouro      { get; set; }         //nome do logradouro (string)
            public string numero          { get; set; }         //número do endereço (string)
            public string bairro          { get; set; }         //bairro do endereço (string)
            public string complemento     { get; set; }         //complemento do endereço (string)
            public string cep             { get; set; }         //CEP do endereço sem máscara (string)
            public string cidade          { get; set; }         //Nome da cidade (string)
            public string uf              { get; set; }         //Sigla do estado (string)
            public string receptorEntrega { get; set; }         //Nome da pessoa que receberá a entrega (string)
            public string receptorEntregaDocumento { get; set; }         //Documento da pessoa que receberá a entrega (string)
        }



        public List<long> idsOrcamentos { get; set; }         //lista de ids de orçamentos. Retorna as saídas associadas aos orçamentos (lista de longs)
        public string urlEtiqueta       { get; set; }         //url de impressão da etiqueta (string)


        public enum Parametros
        {
            entidades,
            inicio,
            quantidade,
            alteradoApos,
            desde,
            ate,
            terceiro,
            carregarPagamentos,
            carregarImpostosNota,
            numeros,
            numeroPedidoCliente,
            idsOrcamentos,
            orderBy,
            integrados,
            integrador,
            ignorarSimplesFaturamento,
            statusPedidoVendaIds
        }

        public override string EndPoint() { return endpoint_pedidos; }

        public enum OrderBy
        {
            id,         //ordenação crescente pelo id dos registros (Valor padrão caso não informado)
            saida_dataAlteracao,    //ordenação crescente pela data de alteração dos registros
            saida_dataCriacao,  //ordenação crescente pela data de criação dos registros
            saida_dataCancelamento, //ordenação crescente pela data de cancelamento
            saida_numeroDocumento 	//ordenação crescente pelo campo numero
        }

        public void SetIdentificacao(string numeroPedidoCliente, DateTime data_horario, long id_entidade, string documento_entidade, bool vendaConsumidorFinal,
            long id_terceiro, string documento_terceiro, long id_vendedor, string nome_vendedor, long? id_intermediador=null, string documento_intermediador=null, 
            Pedido.Origem? origem = null, Pedido.Tipo? tipo = null,  string observacao=null, string urlEtiqueta=null)
        {
            this.numeroPedidoCliente = numeroPedidoCliente;
            this.data = data_horario.ToString("dd-MM-yyyy");
            this.horario = data_horario.ToString("HH:mm:ss");

            entidade = new Entidade();
            entidade.SetIdentificacaoID(id_entidade, documento_entidade);

            this.vendaConsumidorFinal = vendaConsumidorFinal;

            terceiro = new Terceiro();
            terceiro.SetIdentificacaoID(id_terceiro, documento_terceiro);

            representante = new Representante();
            representante.SetIdentificacaoID(id_vendedor, null, nome_vendedor);

            intermediador = new Terceiro();
            intermediador.SetIdentificacaoID(id_intermediador, documento_intermediador);

            this.origem = origem.ToString();
            this.tipo = tipo.ToString();
            this.observacao = Formatar(observacao);
            this.urlEtiqueta  = Formatar(urlEtiqueta);
        }

        public void AddidOrcamento(long idOrcamento)
        {
            if (this.idsOrcamentos == null) this.idsOrcamentos = new List<long>();
            this.idsOrcamentos.Add(idOrcamento);
        }

        public void SetValores(decimal? valorDesconto=null, decimal? valorFrete = null, decimal? valorOutros = null, decimal? valorSeguro = null)
        {
            this.valorDesconto = valorDesconto;
            this.valorFrete = valorFrete;
            this.valorOutros = valorOutros;
            this.valorSeguro = valorSeguro;
        }

        public void AddDescontoDetalhes(string observacao, decimal valor, OrigemDesconto origemDesconto)
        {
            if (this.descontoDetalhes == null) this.descontoDetalhes = new List<DescontoDetalhe>();
            var descDet = new DescontoDetalhe();
            descDet.observacao = observacao;
            descDet.valor = valor;  
            descDet.origemDesconto = origemDesconto.ToString();
            this.descontoDetalhes.Add(descDet);
        }

        public void AddItem(long? id_produto, string codigoSistema_produto, string codigoInterno_produto, string codigoBarras_produto, 
            decimal quantidade, decimal valorDesconto, decimal valorUnitario, long operacao, DateTime? dataEntrega=null, 
            bool? reservarEstoque = false, List<DescontoDetalhe> descontoDetalhes = null, List<string> numerosValesPresentes=null)
        {
            Item item = new Item();

            Produto produto = new Produto();
            produto.SetIdentificacao(id_produto, codigoSistema_produto, codigoInterno_produto, codigoBarras_produto);

            item.produto = produto;
            item.quantidade = quantidade;
            item.valorDesconto = valorDesconto;
            item.valorUnitario = valorUnitario;
            item.operacao = operacao;
            item.dataEntrega = FormatarDataDia(dataEntrega);
            item.reservarEstoque = reservarEstoque;
            item.descontoDetalhes = descontoDetalhes;
            item.numerosValesPresentes = numerosValesPresentes;

            if (this.itens == null) this.itens = new List<Item>();
            this.itens.Add(item);
        }

        public void AddServico(long? id_servico, string codigoSistema, string codigoInterno,  long quantidade, decimal valorDesconto, decimal valorUnitario, List<DescontoDetalhe> descontoDetalhes = null) 
        {
            Servico servico = new Servico();
            servico.SetIdentificacao(id_servico, codigoSistema, codigoInterno);
            servico.quantidade = quantidade;
            servico.valorDesconto = valorDesconto;
            servico.valorUnitario = valorUnitario;
            servico.descontoDetalhes = descontoDetalhes;

            if (this.servicos == null) this.servicos = new List<Servico>();
            this.servicos.Add(servico);
        }

        public void SetEmissaoNota(bool? emitirNotaFiscal = false, bool? emitirNotaFiscalPresente = null, bool? enviarEmailNota=null)
        {
            this.emitirNotaFiscal = emitirNotaFiscal;
            if (emitirNotaFiscal??false)
            {
                this.emitirNotaFiscalPresente = emitirNotaFiscalPresente;
                this.enviarEmailNota = enviarEmailNota;
            }
        }

        public void SetTranporte(Transporte.Modalidade modalidade, long id_transportador, string documento_transportador, string codigoANTT, 
            string placaVeiculo, string estadoVeiculo, long quantidade, string especie, string marca, decimal numero_volume, decimal pesoBruto, decimal pesoLiquido)
        {
            Transporte transporte = new Transporte();
            transporte.modalidade = modalidade.ToString();
            Terceiro transportador = new Terceiro();
            transportador.SetIdentificacaoID(id_transportador, documento_transportador);
            transporte.transportador = transportador;
            transporte.codigoANTT = codigoANTT;
            transporte.placaVeiculo = placaVeiculo;
            transporte.estadoVeiculo = estadoVeiculo;
            transporte.quantidade = quantidade;
            transporte.especie = especie;
            transporte.marca = marca;
            transporte.numero = numero_volume;
            transporte.pesoBruto = pesoBruto;
            transporte.pesoLiquido = pesoLiquido;
            this.transporte = transporte; 
        }

        public void SetEnderecoEntrega(string logradouro, string numero_endereco, string bairro, string complemento, string cep, 
            string cidade, string uf, string receptorEntrega, string receptorEntregaDocumento)
        {
            EnderecoEntrega enderecoEntrega = new EnderecoEntrega();

            enderecoEntrega.logradouro = logradouro;
            enderecoEntrega.numero = numero_endereco;
            enderecoEntrega.bairro = bairro;
            enderecoEntrega.complemento = complemento;
            enderecoEntrega.cep = cep;
            enderecoEntrega.cidade = cidade;
            enderecoEntrega.uf = uf;
            enderecoEntrega.receptorEntrega = receptorEntrega;
            enderecoEntrega.receptorEntregaDocumento = receptorEntregaDocumento;

            this.enderecoEntrega = enderecoEntrega;
        }

        public void SetPlano(long? id, string descricao)
        {
            if (id !=null || !string.IsNullOrEmpty(descricao))
            {
                Plano plano = new Plano();
                plano.id = id;
                plano.descricao = descricao;    
                this.plano = plano;
            }
        }

        private void VerificarPagamento() { if (this.pagamento == null) this.pagamento = new Pagamento(); }
        public void SetPagamentoValorDinheiro(decimal valorDinheiro) 
        {
            VerificarPagamento();
            this.pagamento.valorDinheiro = Formatar(valorDinheiro);
        }

        public void AddPagamentoCartao(decimal valor, string autorizacao, int quantidadeParcelas, string nsu,  
            string operadoraNome, string bandeiraNome, Cartao.TipoCartao? tipo=null, Cartao.TipoParcelamento? parcelamento = null, long? negociacao=null) 
        {
            Cartao cartao = new Cartao();
            cartao.valor = valor;
            cartao.autorizacao = autorizacao;
            cartao.quantidadeParcelas = quantidadeParcelas;
            cartao.nsu = nsu;
            cartao.operadoraNome = operadoraNome;
            cartao.bandeiraNome = bandeiraNome;
            cartao.tipo = tipo.ToString();
            cartao.parcelamento = parcelamento.ToString();
            cartao.negociacao = negociacao;

            bool temDadosFaltado = string.IsNullOrEmpty(operadoraNome) || string.IsNullOrEmpty(bandeiraNome);
            if (negociacao == null && temDadosFaltado)
                throw new Exception("Pagamento de Cartão está com dados incompleto!");
            
            VerificarPagamento();
            this.pagamento.AddCartao(cartao);
        }

        public void AddPagamentoCheque(long? titular, long? banco, string agencia, long? conta, long? numero, decimal valor, DateTime vencimento) 
        {
            Cheque cheque = new Cheque();
            cheque.titular = titular;
            cheque.banco = banco;
            cheque.agencia = agencia;
            cheque.conta = conta;
            cheque.numero = numero;
            cheque.valor = valor;
            cheque.vencimento = FormatarDataDia(vencimento);

            VerificarPagamento(); 
            this.pagamento.AddCheque(cheque);
        }

        public void AddPagamentoVale(long numero, decimal valor)
        {
            Vale vale = new Vale();
            vale.numero = numero;
            vale.valor = valor; 
            VerificarPagamento();
            this.pagamento.AddVale(vale);
        }

        public void AddPagamentoAdiantamento(long id, decimal valor)
        {
            Adiantamento adiantamento = new Adiantamento();
            adiantamento.id = id;
            adiantamento.valor = valor;
            VerificarPagamento();
            this.pagamento.AddAdiantamento(adiantamento);
        }

        public void SetPagamentoCrediario(Crediario crediario)
        {
            VerificarPagamento();
            this.pagamento.crediario = crediario;
        }

        public void AddPagamentoBoleto(decimal valor, string identificacao, DateTime dataVencimento, DateTime dataPagamento, string codigoConta)
        {
            Boleto boleto = new Boleto();
            boleto.valor = valor;
            boleto.identificacao = identificacao;
            boleto.dataVencimento = FormatarDataDia(dataVencimento);
            boleto.dataPagamento = FormatarDataDia(dataPagamento);
            boleto.codigoConta = codigoConta;
            VerificarPagamento();
            this.pagamento.AddBoleto(boleto);
        }

        public void AddPagamentoVoucher(long id, decimal valor)
        {
            Vouchers Vouchers = new Vouchers();
            Vouchers.SetVoucher(id) ;
            Vouchers.valor = valor;
            VerificarPagamento();
            this.pagamento.AddVouchers(Vouchers);
        }

        public void AddPagamentoPixe(DateTime dataPagamento , decimal valor, string nsu, string autorizacao)
        {
            Pix pix = new Pix();
            pix.dataPagamento = FormatarDataDia(dataPagamento);
            pix.valor = valor;
            pix.nsu = nsu;  
            pix.autorizacao = autorizacao;  
            VerificarPagamento();
            this.pagamento.AddPix(pix);
        }

    }

    public class Plano: VarejOnline
    {
        public long? id { get; set; }         //id do plano de pagamento(long)
        public string descricao { get; set; }         //nome do plano de pagamento(long)

        public override string EndPoint()
        {
            throw new NotImplementedException();
        }

        public void SetIdentificacaoID(long? id, string descricao)
        {
            this.id = Formatar(id);
            this.descricao = Formatar(descricao);
        }
    }

    /*public class Teste
    {
        
        public Object operacao 
        { 
            get=>  _operacao; 
            set 
            { 
                _operacao = value;
                carregarDadosOperacao();
            } 
        }

        [JsonIgnore]
        public Object _operacao;

        //[JsonIgnore]
        //public long? operacao_id;

        [JsonIgnore]
        public Operacao operacao_obj = new Operacao();


        public class Operacao
        {
            public long? id { get; set; }         //id da operação (long)
            public string descricao { get; set; }         //descrição da operação (string)
        }

        public void carregarDadosOperacao()
        {
            string val =  operacao.ToString();
            if (val != null && val != "")
            {
                string[] v_array = val.Split(',');
                if (val.Contains("{") && v_array.Length == 2)
                {
                    string[] id_array = v_array[0].Split(':');
                    long id;
                    operacao_obj.id = id_array.Length == 2 && long.TryParse(id_array[1].Trim(), out id) ? (long?)id : null;
                    string[] descricao_array = v_array[1].Split(':');
                    operacao_obj.descricao = descricao_array.Length == 2 ? descricao_array[1].Replace('"', ' ').Replace('\r',' ').Replace('\n',' ').Replace('}', ' ').Trim() : "";
                    //operacao_id = operacao_obj.id;
                }
                else if (!val.Contains("{") && !val.Contains(":"))
                {
                    string id_string = val.Replace('"', ' ').Trim();
                    long id;
                    operacao_obj.id = id_string != "" && long.TryParse(id_string, out id) ? (long?)id : null;
                    //operacao_id = operacao_obj.id;
                }
            }
        }
    }*/
}
