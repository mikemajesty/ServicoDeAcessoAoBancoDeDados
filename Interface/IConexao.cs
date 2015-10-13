using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    [ServiceContract]
    public interface IConexao
    {
        [OperationContract]
        void LimparParametros();
        [OperationContract]
         void AdicionarParametros(string pNome, object pValor);
        [OperationContract]
         object ExecutaComando(CommandType tipoComando, string comando);
        [OperationContract]
        DataSet ExecutaConsulta(CommandType tipoComando, string comando);
       
    }
}
