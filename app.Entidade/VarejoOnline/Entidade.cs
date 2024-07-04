using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yamacorp.Entidade.Varejonline
{
    public class Entidade: VarejOnline
    {
        public long? id          { get; set; }         // id da entidade
        public string nome      { get; set; }         // nome da entidade
        public string documento { get; set; }         // CNPJ da empresa  //CNPJ formatado da loja associada a entidade(string)

        public override string EndPoint()
        {
            throw new NotImplementedException();
        }

        public void SetIdentificacaoID(long? id, string documento, string nome="")
        {
            this.id = Formatar(id);
            this.documento = Formatar(documento);
            this.nome = Formatar(nome);
        }
    }
}
