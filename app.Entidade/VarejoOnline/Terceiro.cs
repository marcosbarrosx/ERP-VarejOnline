using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using yamacorp.Entidade.LinxProducao;
using yamacorp.Entidade.YamcolNet;
using static yamacorp.Entidade.Varejonline.Terceiro.Telefone;
using static yamacorp.Entidade.Varejonline.Terceiro;
using static yamacorp.Entidade.Varejonline.Terceiro.MarcaTerceiro;

namespace yamacorp.Entidade.Varejonline
{
    public class Terceiro : VarejOnline
    {
        public long? id { get; set; }               //id do terceiro
        public bool? ativo { get; set; }            //indica se o terceiro está ativo ou não
        public bool? excluido { get; set; }         //indica se o terceiro foi excluído
        public string dataAlteracao { get; set; }   //última data de alteração do terceiro
        public string nome { get; set; }            //nome do terceiro Pessoa Física ou Razão Social da Pessoa Jurídica (max 255 char)
        public string nomeFantasia { get; set; }    //nome fantasia do terceiro. Retornado apenas para terceiros Pessoa Jurídica  (max 255 char)
        public string padraoNome { get; set; }      //indica qual é o campo de nome usando na apresentação do cliente em tela. Opções: "NOME_FANTASIA" ou "RAZAO_SOCIAL"

        public enum PadraoNome { NOME_FANTASIA, RAZAO_SOCIAL }

        public string documento { get; set; }       //cpf ou cnpj do terceiro formatado  (max 255 char)
        public long? entidadeCadastro { get; set; } //Entidade na qual o terceiro foi cadastrado
        public List<string> emails { get; set; }    //emails do terceiro (max 255 char)
        public string rg { get; set; }              //número do RG do terceiro. Retornado apenas para terceiros Pessoa Física, se existir (max 50 char)
        
        public string tipoPessoa { get; set; }      //Define se a Pessoa é Física (PF), Jurídica (PJ), Física Estrangeiro (PF_ESTRANGEIRO) ou Jurídica Estrangeiro (PJ_ESTRANGEIRO)
        
        public enum TipoPessoa { PF, PJ, PF_ESTRANGEIRO, PJ_ESTRANGEIRO }

        public string dataNascimento { get; set; }          //data de nascimento do terceiro. Retornado apenas para terceiros Pessoa Fisica, no formato dd-mm-aaaa hh:mi:ss.
        public string ie { get; set; }                      //número da Inscrição Estadual do terceiro. Retornado apenas para terceiros Pessoa Jurídica, se existir (max 255 char)
        public string inscricaoMunicipal { get; set; }      //número da inscrição municipal do terceiro. Retornado apenas para terceiros Pessoa Jurídica, se existir (max 255 char)
        public string modalidadeTributacao { get; set; }    //indica qual é a modalidade de tributação do terceiro, aplicado apenas para Pessoa Jurídica, se existir

        public List<Endereco> enderecos { get; set; }       //lista de endereços do terceiro, contendo:

        public class Endereco
        {
            public string tipo  { get; set; }               //tipo do logradouro  (obrigatório)
            public string logradouro { get; set; }          //logradouro do endereço (opcional) (max 255 char)
            public string numero { get; set; }              //número do endereço (opcional) (max 400 char)
            public string complemento { get; set; }         //complemento do endereço (opcional) (max 255 char)
            public string bairro { get; set; }              //bairro do endereço (obrigatório caso informado o codigoIBGECidade) (max 255 char)
            public string cep { get; set; }                 //CEP do endereço (obrigatório)
            public string cidade { get; set; }              //cidade do endereço (opcional) (max 255 char)
            public string siglaEstado { get; set; }         //UF do endereço (opcional)
            public string uf { get; set; }                  //sigla em caixa alta da UF do endereço  (opcional)
            public string pais { get; set; }                //País do Endereço  (opcional, padrão: BRASIL)
            public string codigoIBGECidade { get; set; }    //Código da cidade conforme tabela IBGE (opcional)
            public string tipoEndereco { get; set; }        //Tipo do Endereço (opcional, padrão: ENDERECO_SEDE)

            public enum TiposLogradouro { OUTROS, AEROPORTO, ALAMEDA, AREA, AVENIDA, CAMPO, CHACARA, COLONIA, CONDOMINIO, CONJUNTO, DISTRITO, ESPLANADA, ESTACAO, ESTRADA, FAVELA, FAZENDA, FEIRA, JARDIM, LADEIRA, LAGO, LAGOA, LARGO, LOTEAMENTO, MORRO, NUCLEO, PARQUE, PASSARELA, PATIO, PRACA, QUADRA, RECANTO, RESIDENCIAL, RODOVIA, RUA, SETOR, SITIO, TRAVESSA, TRECHO, TREVO, VALE, VEREDA, VIA, VIADUTO, VIELA, VILA }
            public enum TipoEndereco { ENDERECO_CORRESPONDENCIA, ENDERECO_COBRANCA, ENDERECO_ENTREGA, ENDERECO_SEDE }

        }

        public List<Telefone> telefones { get; set; }         //lista de telefones do terceiro, contendo:

        public class Telefone
        {
            public string ddi { get; set; }           //Código do DDI (obrigatório) (max 10 char)
            public string ddd { get; set; }           //Código do DDD (obrigatório) (max 10 char)
            public string numero { get; set; }        //Número do telefone (obrigatório) (max 255 char)
            public string ramal { get; set; }         //Número do ramal (obrigatório) (max 10 char)
            public string tipoTelefone { get; set; }  //Opções: CELULAR, RESIDENCIAL, COMERCIAL, RECADO (opcional, padrão: COMERCIAL)

            public enum TipoTelefone { CELULAR, RESIDENCIAL, COMERCIAL, RECADO }
        }

        public List<string> classes { get; set; }       //lista de classes às quais o terceiro pertence.

        public enum Classe { CLIENTE, COOPERADO, ACIONISTA, FORNECEDOR, FUNCIONARIO, OUTROS, PRESTADOR_SERVICO, SOCIO_PROPRIETARIO, TECNICO }

        public string categoria { get; set; }               //Define a categorização do terceiro no Varejonline
        public bool? autorizaReceberEmail { get; set; }     //Opt-in do terceiro autorizando ou não a comunicação por email
        public bool? autorizaReceberSms { get; set; }       //Opt-in do terceiro autorizando ou não a comunicação por sms
        public LimiteCredito limiteCredito { get; set; }    //Define os valores de limite de crédito para o terceiro

        public class LimiteCredito
        {
            public decimal valorTotal { get; set; }         //limite de crédito total
            public decimal valorMensal { get; set; }        //limite de crédito mensal
            public decimal valorRenda { get; set; }         //valor da renda do terceiro
        }
        
        public CamposCustomizados camposCustomizados { get; set; }         //Define os valores da estrutura de campos customizados da base.

        public class CamposCustomizados
        {
            public long id { get; set; }                                    //id do terceiro associado aos valores informados
            public List<Primitivo> valoresPrimitivo { get; set; }           //valores dos campos customizados primitivos
            public List<Composicao> valoresComposicao { get; set; }         //valores dos campos customizados do tipo COMPOSICAO
            public List<Lista> valoresLista { get; set; }                   //valores dos campos customizados do tipo LISTA

            public class Primitivo
            {
                public long id { get; set; }            //id do campo customizado
                public string value { get; set; }       //(object - varia conforme tipagem do campo) (max para string 255 BYTE)
                public Type type { get; set; }          //tipo do campo customizado

                public enum Type
                {
                    TEXTO,      //STRING Primitivo
                    NUMERO,     //INTEGER Primitivo
                    DECIMAL,    //DECIMAL Primitivo
                    DATA,       //CALENDAR Primitivo
                    EMAIL,      //STRING Primitivo
                    OPCIONAL,   //STRING Primitivo
                    COMPOSICAO, //COMPOSITE Complexo
                    LISTA       //LIST Complexo
                }
            }

            public class Composicao
            {
                public long id { get; set; }                 //id do campo customizado
                public List<Primitivo> valores { get; set; } //lista de valores primitivos 
            }

            public class Lista
            {
                public long campoId { get; set; }                           //id do campo customizado retornado pela lista
                public List<Primitivo> valoresPrimitivo { get; set; }       //lista de valores primitivos 
                public List<Composicao> valoresComposicao { get; set; }     //valores dos campos customizados do tipo COMPOSICAO 
            }

        }

        public List<MarcaTerceiro> marcas { get; set; } //lista de marcas do terceiro, contendo:

        public class MarcaTerceiro
        {
            public bool autorizaReceberSms { get; set; }            //limite de crédito total
            public bool autorizaReceberEmail { get; set; }          //limite de crédito total
            public bool autorizaReceberWhatsapp { get; set; }       //limite de crédito total
            public Marca marca { get; set; }                        //informações da marca

            public class Marca 
            {
                public long id      { get; set; }         //id da marca
                public string nome  { get; set; }         //nome da marca
            }

        }

        public override string EndPoint() { return endpoint_terceiros; }

        public enum OrderBy
        {
            id,                 // ordenação crescente pelo id dos registros (Valor padrão caso não informado)
            dataAlteracao,      // ordenação crescente pela data de alteração dos registros
            dataCriacao,        // ordenação crescente pela data de criação dos registros
            documentoTerceiro,  // ordenação crescente pelo documento CPF ou CNPJ do terceiro
            nome                // ordenação crescente pelo nome do terceiro
        }
        public enum Status { ATIVO, INATIVO, EXCLUIDO }

        public enum Parametros
        {
            inicio,
            quantidade,
            status,
            classes,
            alteradoApos,
            documento,
            orderBy,
            integrados,
            integrador,
            carregarCamposCustomizados
        }

        public bool Validar()
        {
            if (string.IsNullOrEmpty(documento) || string.IsNullOrEmpty(nome))
            {
                return false;
            }else
            {
                return true;
            }
        }

        public void SetIdentificacaoID(long? id, string documento, string nome = "")
        {
            this.id = Formatar(id);
            this.documento = Formatar(documento);
            this.nome = Formatar(nome);
        }

        public void SetIdentificacao(string cpf_cnpj, string nome, TipoPessoa tipoPessoa, DateTime ? dataNascimento = null, string rg = "", PadraoNome? padraoNome=null, string nomeFantasia="", string ie="")
        {
            if (string.IsNullOrEmpty(cpf_cnpj?.Trim() ?? ""))
                throw new Exception("CPF ou CNPJ é obrigatório. ");

            documento = cpf_cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", ""); 
            if (string.IsNullOrEmpty(nome?.Trim() ?? ""))
                throw new Exception("Nome é obrigatório. ");

            this.nome = nome.Trim();
            this.tipoPessoa = tipoPessoa.ToString();
            switch (tipoPessoa)
            {
                case TipoPessoa.PF: 
                    { 
                        this.rg = string.IsNullOrEmpty(rg?.Trim()) ? null : rg.Trim();
                        if (dataNascimento != null) this.dataNascimento = ((DateTime)dataNascimento).ToString("dd-MM-yyyy"); 
                        break; 
                    }
                case TipoPessoa.PJ: { this.ie = string.IsNullOrEmpty(ie?.Trim()) ? null : ie.Trim(); break; }
            }

            if (padraoNome != null) 
            {
                this.padraoNome = padraoNome?.ToString();
                this.nomeFantasia = nomeFantasia.Trim();
                if (this.padraoNome.Equals(PadraoNome.NOME_FANTASIA.ToString()) && string.IsNullOrEmpty(this.nomeFantasia))
                    throw new Exception("Parâmetro 'nomeFantasia' é obrigatório quando 'padraoNome' for igual a NOME_FANTASIA. ");
            }

            // OLD - Limitar tamanho dos dados
            //this.documento = LimitarTextoMax255Char(this.documento);
            //this.nome = LimitarTextoMax255Char(this.nome);
            //this.rg = LimitarTextoMax50Char(this.rg);
            //this.nomeFantasia = LimitarTextoMax255Char(this.nomeFantasia);
            //this.ie = LimitarTextoMax255Char(this.ie);

            // NEW - LimitarTamanhoTexto()
            this.documento = LimitarTamanhoTexto(this.documento, 255);
            this.nome = LimitarTamanhoTexto(this.nome, 255);
            this.rg = LimitarTamanhoTexto(this.rg, 50);
            this.nomeFantasia = LimitarTamanhoTexto(this.nomeFantasia, 255);
            this.ie = LimitarTamanhoTexto(this.ie, 255);

        }

        public void SetDadosDeSistema(bool? ativo=null, long? entidadeCadastro=-1, string categoria = "", bool? autorizaReceberEmail = null, bool? autorizaReceberSms = null)
        {
            if (ativo != null) this.ativo = (bool)ativo;
            if (entidadeCadastro >= 0) this.entidadeCadastro = entidadeCadastro;
            if (!string.IsNullOrEmpty(categoria)) this.categoria = categoria;
            if (autorizaReceberEmail != null) this.autorizaReceberEmail = (bool)autorizaReceberEmail;
            if (autorizaReceberSms != null) this.autorizaReceberSms = (bool)autorizaReceberSms;
        }

        public void AddClasse(Classe classe)
        {
            if (this.classes == null) this.classes = new List<string>();
            this.classes.Add(classe.ToString()); 
        }

        public void AddEmail(string email)
        {
            if(!string.IsNullOrEmpty(email?.Trim()) && email.Split('@').Length == 2)
            {
                if(this.emails == null) this.emails = new List<string>();
                this.emails.Add(LimitarTextoMax255Char(email.Trim()));
            }
        }
        public void AddEndereco(string cep, Endereco.TiposLogradouro tipo, string logradouro="", string complemento = "", string numero = "", string bairro = "", long codigoIBGECidade=-1, Endereco.TipoEndereco? tipoEndereco=null, string cidade = "", string uf = "", string pais = "")
        {
            if (string.IsNullOrEmpty(cep?.Trim()))
                throw new Exception("O cep e obrigatório!");

            if (string.IsNullOrEmpty(bairro?.Trim()) && codigoIBGECidade > 0)
                throw new Exception("O bairro e obrigatório quando é informado codigoIBGECidade!");

            Endereco endereco = new Endereco();

            endereco.cep = cep.Trim().Replace("-", "");
            endereco.tipo = tipo.ToString();
            if (!string.IsNullOrEmpty(logradouro?.Trim())) endereco.logradouro = LimitarTextoMax255Char(logradouro);
            if (!string.IsNullOrEmpty(numero?.Trim())) endereco.numero = LimitarTextoMax400Char(numero);
            if (!string.IsNullOrEmpty(bairro?.Trim())) endereco.bairro = LimitarTextoMax255Char(bairro);
            if (!string.IsNullOrEmpty(complemento?.Trim())) endereco.complemento = LimitarTextoMax255Char(complemento);
            if (codigoIBGECidade>0) endereco.codigoIBGECidade = codigoIBGECidade.ToString();
            if (tipoEndereco != null) endereco.tipoEndereco = tipoEndereco.ToString();
            if (!string.IsNullOrEmpty(pais?.Trim())) endereco.pais = LimitarTextoMax255Char(pais.ToUpper());
            if (!string.IsNullOrEmpty(uf?.Trim())) endereco.uf = LimitarTextoMax255Char(uf.ToUpper());
            if (!string.IsNullOrEmpty(cidade?.Trim())) endereco.cidade = LimitarTextoMax255Char(cidade);

            if (this.enderecos == null) this.enderecos = new List<Endereco>();
            this.enderecos.Add(endereco);
        }

        public void AddTelefone(int ddi, int ddd, int numero, int? ramal=null, TipoTelefone? tipoTelefone=null)
        {
            var erro = new List<string>();
            if (ddi < 0) erro.Add($"ddi ({ddi})");
            if (ddd <= 0) erro.Add($"ddd ({ddd})");
            if (ddd <= 0) erro.Add($"número ({numero})");
            if (ramal!=null && ramal <= 0) erro.Add($"ramal ({ramal})");

            if (erro.Count > 0) throw new Exception($"Dados do telefone está incompleto: {string.Join(",", erro)}. ");

            Telefone telefone = new Telefone()
            {
                ddi = ddi.ToString(),
                ddd = ddd.ToString(),
                numero = LimitarTextoMax255Char(numero.ToString()),
                ramal = ramal?.ToString()
            };

            if (tipoTelefone != null) telefone.tipoTelefone = tipoTelefone.ToString();
            if (this.telefones == null) this.telefones = new List<Telefone>();
            this.telefones.Add(telefone);
        }

        public void AddTelefoneCelular(int ddi, int ddd, int numero, int? ramal = null) { AddTelefone(ddi, ddd, numero, ramal, TipoTelefone.CELULAR); }
        public void AddTelefoneResidencial(int ddi, int ddd, int numero, int? ramal = null) { AddTelefone(ddi, ddd, numero, ramal, TipoTelefone.RESIDENCIAL); }

        public void SetLimiteCredito(decimal valorTotal, decimal valorMensal, decimal valorRenda)
        {
            this.limiteCredito = new LimiteCredito()
            {
                valorTotal = valorTotal,
                valorMensal = valorMensal,
                valorRenda = valorRenda
            };
        }

        public void SetCamposCustomizados(CamposCustomizados camposCustomizados)
        {
            this.camposCustomizados = camposCustomizados;
        }

        public void AddMarca(long id, string nome, bool autorizaReceberSms=false, bool autorizaReceberEmail = false, bool autorizaReceberWhatsapp = false)
        {
            var marcaTerceiro = new MarcaTerceiro()
            {
                marca = new MarcaTerceiro.Marca()
                {
                    id = id,
                    nome = nome
                },
                autorizaReceberSms = autorizaReceberSms,
                autorizaReceberEmail = autorizaReceberEmail, 
                autorizaReceberWhatsapp = autorizaReceberWhatsapp,
            };
            if (this.marcas == null) this.marcas = new List<MarcaTerceiro>();
            marcas.Add(marcaTerceiro);
        }



    }

  

   

    public class Representante: VarejOnline
    {
        public long? id { get; set; }         //id do vendedor(long)
        public string nome { get; set; }         //nome do vendedor(string)
        public string documento { get; set; }         //CPF formato do vendedor(string)

        public override string EndPoint()
        {
            throw new NotImplementedException();
        }

        public void SetIdentificacaoID(long? id, string documento, string nome = "")
        {
            this.id = Formatar(id);
            this.documento = Formatar(documento);
            this.nome = Formatar(nome);
        }

    }




}
