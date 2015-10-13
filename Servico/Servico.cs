using Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;


namespace Servico
{
    public class Servico : IConexao
    {

        private readonly SqlConnection conexao = new SqlConnection(ConfigurationManager.AppSettings["Con"]);

        private SqlCommand comando = new SqlCommand();

        private static SqlParameterCollection listaParametros = new SqlCommand().Parameters;

        void IConexao.LimparParametros()
        {
            listaParametros.Clear();
        }

        private bool parteDoComando(CommandType tipoComando, string comandoValor)
        {
            try
            {
                if (conexao.State != ConnectionState.Open)
                {
                    this.conexao.Open();
                }
              //  conexao.Open();

                using (this.comando = this.conexao.CreateCommand())
                {
                    comando.CommandText = comandoValor;
                    comando.CommandType = tipoComando;
                    comando.CommandTimeout = 1800;

                    foreach (SqlParameter parametro in listaParametros)
                    {
                        this.comando.Parameters.Add(new SqlParameter(parametro.ParameterName,parametro.Value));
                    }
                    return true;
                }
            }
            catch (SqlException Erro)
            {
                if (this.conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                    return false;

                }
                throw new Exception(Erro.Message);

            }

        }

    
        
        public void AdicionarParametros(string pNome, object pValor)
        {
            listaParametros.Add(new SqlParameter(pNome, pValor));
        }

        object IConexao.ExecutaComando(CommandType tipoComando, string comando)
        {
            try
            {
                if (this.parteDoComando(tipoComando, comando))
                {
                    return this.comando.ExecuteScalar();
                }
                return null;
            }
            catch (Exception Erro)
            {
                throw new Exception(Erro.Message);
            }

        }

        DataSet IConexao.ExecutaConsulta(CommandType tipoComando, string comando)
        {

            if (this.parteDoComando(tipoComando, comando))
            {
                try
                {

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(this.comando))
                    {
                        using (DataSet tabela = new DataSet())
                        {
                            adaptador.Fill(tabela);
                            return tabela;
                        }
                    }
                    
                }
                catch (SqlException Erro)
                {
                    if (this.conexao.State != ConnectionState.Closed)
                    {
                        conexao.Close();
                        return null;
                    }
                    throw new Exception(Erro.Message);
                }



            }
            return null;
        }



      
    }
}
