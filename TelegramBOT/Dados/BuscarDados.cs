using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TelegramBOT.Dados
{
    internal class BuscarDados
    {
        private SqlConnection conexao;

        private SqlConnection GetConexao()
        {
            string connectionString = @"Data Source=DESKTOP-4CT40MG\SQLEXPRESS; Initial Catalog=standby_org; User Id=sa; Password=123adr;";
            conexao = new SqlConnection(connectionString);
            conexao.Open();

            return conexao;
        }

        public Decimal LucrosMes(string _mes)
        {
            string dataInicialMes = "";
            using (SqlConnection con = GetConexao())
            {
                switch (_mes)
                {
                    case "jan":
                        dataInicialMes = "1-1-2022";
                        break;

                    case "fev":
                        dataInicialMes = "1-2-2022";
                        break;

                    case "mar":
                        dataInicialMes = "1-3-2022";
                        break;

                    case "abr":
                        dataInicialMes = "1-4-2022";
                        break;

                    case "mai":
                        dataInicialMes = "1-5-2022";
                        break;

                    case "jun":
                        dataInicialMes = "1-6-2022";
                        break;

                    case "jul":
                        dataInicialMes = "1-7-2022";
                        break;

                    case "ago":
                        dataInicialMes = "1-8-2022";
                        break;

                    case "set":
                        dataInicialMes = "1-9-2022";
                        break;

                    case "out":
                        dataInicialMes = "1-10-2022";
                        break;

                    case "nov":
                        dataInicialMes = "1-11-2022";
                        break;

                    case "dez":
                        dataInicialMes = "1-12-2022";
                        break;
                }
                string query = "SELECT sum(sv_lucro) " +
                               "FROM tb_servicos " +
                               "INNER JOIN tb_clientes " +
                               "ON sv_cl_idcliente = cl_id " +
                               $"WHERE month(sv_data) = MONTH(EOMONTH('{dataInicialMes}')) and year(sv_data) = YEAR(GETDATE())";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();

                Decimal lucroTotal = dr.GetDecimal(0);

                return lucroTotal;
            }
        }

        public DataTable BuscarServicosEmAndamento()
        {
            DataTable datatable = new DataTable();
            try
            {
                using (SqlConnection conexao = GetConexao())
                {
                    //0 = id
                    //1 = sv_cl_idcliente
                    //2 = sv_data
                    //3 = cl_nome
                    //4 = sv_aparelho
                    //5 = sv_defeito
                    //6 = sv_situacao
                    //7 = sv_senha
                    //8 = sv_valorservico
                    //9 = sv_valorpeca
                    //10 = sv_lucro
                    //11 = sv_servico
                    //12 = sv_previsao_entrega
                    //13 = sv_existe_um_prazo
                    //14 = sv_acessorios
                    //15 = sv_cor_tempo
                    //16 = sv_tempo_para_entregar
                    string query = "select sv_id, sv_cl_idcliente, sv_data, cl_nome, sv_aparelho, sv_defeito, sv_situacao, sv_senha, " +
                                   "sv_valorservico, sv_valorpeca, sv_lucro, sv_servico, sv_previsao_entrega, sv_existe_um_prazo, sv_acessorios, sv_cor_tempo, sv_tempo_para_entregar " +
                                   "FROM tb_servicos " +
                                   "INNER JOIN tb_clientes ON tb_servicos.sv_cl_idcliente = tb_clientes.cl_id " +
                                   "WHERE sv_status = 1 and sv_ativo = 1 order by sv_cor_tempo asc, sv_data desc, sv_id desc";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexao);
                    adapter.Fill(datatable);
                }
            }
            catch (Exception ex)
            {
                //
            }
            return datatable;
        }

        public List<object> BuscarServicoPorID(int _idServico)
        {
            try
            {
                using (SqlConnection con = GetConexao())
                {
                    string query = "select sv_id, sv_cl_idcliente, sv_data, cl_nome, sv_aparelho, sv_defeito, sv_situacao, sv_senha, " +
                    "sv_valorservico, sv_valorpeca, sv_lucro, sv_servico, sv_previsao_entrega, sv_existe_um_prazo, sv_acessorios, sv_cor_tempo " +
                    "FROM tb_servicos " +
                    "INNER JOIN tb_clientes ON tb_servicos.sv_cl_idcliente = tb_clientes.cl_id " +
                    "WHERE sv_id = @IdServico";

                    List<object> dados = new List<object>();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@IdServico", _idServico);

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        dados.Add(dr.GetValue(0));//id
                        dados.Add(dr.GetValue(1));//id cliente
                        dados.Add(dr.GetValue(2));//sv_data
                        dados.Add(dr.GetValue(3));//cl_nome
                        dados.Add(dr.GetValue(4));//sv_aparelho
                        dados.Add(dr.GetValue(5));//sv_defeito
                        dados.Add(dr.GetValue(6));//sv_situacao
                        dados.Add(dr.GetValue(7));//sv_senha
                        dados.Add(dr.GetValue(8));//sv_valorservico
                        dados.Add(dr.GetValue(9));//sv_valorpeca
                        dados.Add(dr.GetValue(10));//sv_lucro
                        dados.Add(dr.GetValue(11));//sv_servico
                        dados.Add(dr.GetValue(12));//sv_previsao_entrega
                        dados.Add(dr.GetValue(13));//sv_existe_um_prazo
                        dados.Add(dr.GetValue(14));//sv_acessorios
                        dados.Add(dr.GetValue(15));//sv_cor_tempo

                        return dados;
                    }
                }
            }
            catch (Exception)
            {
                // throw;
            }

            return null;
        }
    }
}