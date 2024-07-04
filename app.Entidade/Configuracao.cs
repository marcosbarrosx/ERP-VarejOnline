using System;
using System.Collections.Generic;
using yamacorp.Entidade.LinxProducao;
using yamacorp.Entidade.YamcolNet;

namespace yamacorp.Entidade
{
    public class Configuracao
    {
        public int id_empresa { get; set; }
        public int id_empresa_microvix { get; set; }
        public int id_loja { get; set; }
        public string loja { get; set; }
        public string codigo_filial { get; set; }
        public string codigo_saas { get; set; }
        public string codigo_tab_preco { get; set; }
        public string id_rede_lojas { get; set; }
        public string url_api { get; set; }
        public string token { get; set; }
        public string banco { get; set; }
        public string conexao { get; set; }
        public string versao { get; set; }
        public DateTime ultima_atualizacao { get; set; }
        public List<EmailSuporte> emails { get; set; }
        public List<ClienteBase> bases { get; set; }
        public List<ClienteBase> basesGlobais { get; set; }
        public ConexaoLocal conexaoLocal { get; set; }
        public Estacao estacao { get; set; }
        public string stringConexao { get; set; }


        public Configuracao()
        {
            bases = new List<ClienteBase>();
            basesGlobais = new List<ClienteBase>();
            emails = new List<EmailSuporte>();
            estacao = new Estacao();

        }
    }
    public class EmailSuporte
    {
        public string email { get; set; }
    }
    public class Estacao
    {
        public string terminal { get; set; }
        public string serialHd { get; set; }
        public string Impressora { get; set; }
        public bool tef { get; set; }
        public string data { get; set; }
        public string terminal_autenticador { get; set; }
    }
    public class ConexaoLocal
    {
        public string instancia { get; set; }
        public string enderecoLocal { get; set; }
        public string enderecoBanco { get; set; }
    }
    public class PacoteInicial
    {
        public List<Loja> lojas { get; set; }
        public List<LojaGrupo> lojasGrupo { get; set; }
        public List<ParametroLoja> parametrosLoja { get; set; }
        public List<UsuarioGrupo> usuariosGrupo { get; set; }
        public List<UsuarioDepartamento> usuariosDepartamento { get; set; }
        public List<Usuario> usuarios { get; set; }
        public List<UsuarioLoja> usuariosLoja { get; set; }
        public List<Cidade> cidades { get; set; }
        public List<Estado> estados { get; set; }
        public List<Menu> menus { get; set; }
        public List<UsuariosPermissao> usuariosPermissao { get; set; }
        public List<UsuariosGruposPermissao> usuariosGruposPermissao { get; set; }
        public List<CatalagoConfiguracaoGeral> catalagoConfiguracaoGerals { get; set; }
        public List<CatalagoConfiguracaoGetnet> catalagoConfiguracaoGetnet { get; set; }
        public List<Advertencia> advertencias { get; set; }
        public List<LojasVendedor> lojasVendedor { get; set; }
        public List<LojaRede> lojasRede { get; set; }
        public List<RegiaoVenda> regioesVenda { get; set; }
        public List<CadastroCliFor> cadastroCliFor { get; set; }
        public List<Empresa> empresas { get; set; }
        public List<Filial> filiais { get; set; }
        public List<LojasVarejo> lojasVarejo { get; set; }
        public List<LojaTerminal> lojasTerminal { get; set; }
        public List<ClienteVarejo> clientesVarejos { get; set; }
        public List<Produto> produtos { get; set; }
        public List<ProdutoCor> produtosCor { get; set; }
        public List<ProdutoBarra> produtosBarra { get; set; }
        public List<ProdutoPreco> produtosPreco { get; set; }
        public List<ProdutoPrecoCor> produtosPrecoCor { get; set; }
        public List<ClassifFiscal> classifFiscal { get; set; }
        public List<EstadoCivil> estadoCivil { get; set; }
        public List<ClienteVarTipo> clienteVarTipos { get; set; }
        public List<LojaOperacaoPgto> lojaOperacaoPgto { get; set; }
        public List<LojaOperacaoParcela> lojaOperacaoParcela { get; set; }
        public List<LojaFormaPgto> lojaFormaPgto { get; set; }
        public List<AdministradoraCartao> administradoraCartao { get; set; }
        public List<Banco> banco { get; set; }

        public List<LojasVendedor> lojasVendedorTaco { get; set; }
    }
    public class RegistroLog
    {
        public int ultimo_registro { get; set; }
        public List<Erro> erros { get; set; }
        public List<Atualizacao> atualizacao { get; set; }

        public RegistroLog()
        {
            erros = new List<Erro>();
            atualizacao = new List<Atualizacao>();
        }
    }

    public static class LogSistema
    {
        public static RegistroLog RegistroLog { get; set; }
    }
    public static class LogTabelas
    {
        public static RegistroLog RegistroTabelas { get; set; }
    }


    public class VersaoSistema
    {
        public int id_programa { get; set; }
        public string programa { get; set; }
        public string versao { get; set; }
        public DateTime atualizacao { get; set; }
        public string implementado { get; set; }
        public string arquivos_atualizar { get; set; }
        public string ftp_endereco { get; set; }
        public string ftp_login { get; set; }
        public string ftp_senha { get; set; }
    }
    public class Erro
    {
        public int id { get; set; }
        public string processo { get; set; }
        public string erro { get; set; }
        public string usuario { get; set; }
        public string loja { get; set; }
        public string data { get; set; }
    }
    public class Atualizacao
    {
        public int id { get; set; }
        public string processo { get; set; }
        public string usuario { get; set; }
        public string data { get; set; }
    }
    public class Tabelas
    {
        public string nome { get; set; }
        public string ultimo_id { get; set; }
        public string ultima_registro { get; set; }
    }
}
