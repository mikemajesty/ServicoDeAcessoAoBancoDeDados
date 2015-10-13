using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost servico = new ServiceHost(typeof(Servico.Servico),
                   new Uri[]
                              { new Uri("net.tcp://localhost:9090")
                              , new Uri("http://localhost:9091") }))
            {
                servico.AddServiceEndpoint(typeof(Interface.IConexao), new NetTcpBinding(), "Conexao");
                servico.AddServiceEndpoint(typeof(Interface.IConexao), new BasicHttpBinding(), "Conexao");

                servico.Description.Behaviors.Add(new System.ServiceModel.Description.ServiceMetadataBehavior() { HttpGetEnabled = true });

                servico.Open();
                Console.WriteLine("Host Esta Rodando, O  Serviço Esta No Ar...");
                Console.Read();
            }
        }
    }
}
