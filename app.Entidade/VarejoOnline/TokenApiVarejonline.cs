using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace yamacorp.Entidade.Varejonline
{
    public class AuthorizationCode
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string cliente_secret { get; set; }
        public string redirect_uri { get; set; }
        public string code { get; set; }
    }

    public class TokenApiVarejonline: AuthorizationCode
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string cnpj_empresa { get; set; }
        public string id_terceiro { get; set; }
        public string nome_terceiro { get; set; }

        //public HttpStatusCode status { get; set; }
    }
}
