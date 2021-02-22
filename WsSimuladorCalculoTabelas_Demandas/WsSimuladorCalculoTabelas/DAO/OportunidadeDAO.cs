using Dapper;
using Ecoporto.CRM.Business.Enums;
using Ecoporto.CRM.Business.Extensions;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WsSimuladorCalculoTabelas.Configuracao;
using WsSimuladorCalculoTabelas.Enums;
using WsSimuladorCalculoTabelas.Helpers;
using WsSimuladorCalculoTabelas.Models;

namespace WsSimuladorCalculoTabelas.DAO
{
    public class OportunidadeDAO
    {
        private readonly TabelasDAO _tabelasDAO = new TabelasDAO(false);

        public Oportunidade ObterOportunidadePorId(int id)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                    return con.Query<Oportunidade>(@"SELECT * FROM CRM.VW_CRM_OPORTUNIDADES WHERE Id = :Id", parametros).FirstOrDefault();
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                    return con.Query<Oportunidade>(@"SELECT * FROM CRM..VW_CRM_OPORTUNIDADES WHERE Id = @Id", parametros).FirstOrDefault();
                }
            }
        }

        public IEnumerable<LayoutDTO> ObterLayoutProposta(int oportunidadeId)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);

                    return con.Query<LayoutDTO>($@"SELECT * FROM CRM.TB_CRM_OPORTUNIDADE_LAYOUT WHERE OportunidadeId = :OportunidadeId ORDER BY Linha", parametros);
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);

                    return con.Query<LayoutDTO>($@"SELECT * FROM CRM..TB_CRM_OPORTUNIDADE_LAYOUT WHERE OportunidadeId = @OportunidadeId ORDER BY Linha", parametros);
                }
            }
        }

        public IEnumerable<ServicoIPA> ObterServicosIPA(int servicoCrmId)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "ServicoCrmId", value: servicoCrmId, direction: ParameterDirection.Input);

                    return con.Query<ServicoIPA>(@"
                       SELECT 
                            DISTINCT
                                B.AUTONUM As Id,
                                B.DESCR As Descricao
                        FROM
                            CRM.TB_CRM_SERVICO_IPA A
                        INNER JOIN
                            SGIPA.TB_SERVICOS_IPA B ON A.ServicoFaturamentoId = B.AUTONUM
                        WHERE
                            A.ServicoId = :ServicoCrmId", parametros);
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "ServicoCrmId", value: servicoCrmId, direction: ParameterDirection.Input);

                    return con.Query<ServicoIPA>(@"
                       SELECT 
                            DISTINCT
                                B.AUTONUM As Id,
                                B.DESCR As Descricao
                        FROM
                            CRM..TB_CRM_SERVICO_IPA A
                        INNER JOIN
                            SGIPA..TB_SERVICOS_IPA B ON A.ServicoFaturamentoId = B.AUTONUM
                        WHERE
                            A.ServicoId = @ServicoCrmId", parametros);
                }
            }
        }

        public string ObterFreeTime(int oportunidadeId)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);

                    return con.Query<string>(@"SELECT DiasFreeTime FROM CRM.TB_CRM_OPORTUNIDADES WHERE Id = :OportunidadeId", parametros).FirstOrDefault();
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);

                    return con.Query<string>(@"SELECT DiasFreeTime FROM CRM..TB_CRM_OPORTUNIDADES WHERE Id = @OportunidadeId", parametros).FirstOrDefault();
                }
            }
        }

        public string ObterObservacoesModelo(int modeloSimuladorId)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "Id", value: modeloSimuladorId, direction: ParameterDirection.Input);

                    return con.Query<string>(@"SELECT Observacoes FROM CRM.TB_CRM_SIMULADOR_MODELO WHERE Id = :Id", parametros).FirstOrDefault();
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "Id", value: modeloSimuladorId, direction: ParameterDirection.Input);

                    return con.Query<string>(@"SELECT Observacoes FROM CRM..TB_CRM_SIMULADOR_MODELO WHERE Id = @Id", parametros).FirstOrDefault();
                }
            }
        }

        public void AtualizarValoresTicket(Ticket ticket)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();

                    parametros.Add(name: "OportunidadeId", value: ticket.OportunidadeId, direction: ParameterDirection.Input);
                    parametros.Add(name: "TabelaId", value: ticket.TabelaId, direction: ParameterDirection.Input);
                    parametros.Add(name: "LCL", value: ticket.LCL, direction: ParameterDirection.Input);
                    parametros.Add(name: "FCL40", value: ticket.FCL40, direction: ParameterDirection.Input);
                    parametros.Add(name: "FCL20MD", value: ticket.FCL20MD, direction: ParameterDirection.Input);
                    parametros.Add(name: "FCL20ME", value: ticket.FCL20ME, direction: ParameterDirection.Input);
                    parametros.Add(name: "FCL40MD", value: ticket.FCL40MD, direction: ParameterDirection.Input);

                    con.Execute(@"UPDATE TB_CRM_OPORTUNIDADES SET TabelaId = :TabelaId, TicketLCL = :LCL, TicketFCL40 = :FCL40, TicketFCL20MD = :FCL20MD, TicketFCL20ME = :FCL20ME, TicketFCL40MD = :FCL40MD WHERE Id = :OportunidadeId", parametros);
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();

                    parametros.Add(name: "OportunidadeId", value: ticket.OportunidadeId, direction: ParameterDirection.Input);
                    parametros.Add(name: "TabelaId", value: ticket.TabelaId, direction: ParameterDirection.Input);
                    parametros.Add(name: "LCL", value: ticket.LCL, direction: ParameterDirection.Input);
                    parametros.Add(name: "FCL40", value: ticket.FCL40, direction: ParameterDirection.Input);
                    parametros.Add(name: "FCL20MD", value: ticket.FCL20MD, direction: ParameterDirection.Input);
                    parametros.Add(name: "FCL20ME", value: ticket.FCL20ME, direction: ParameterDirection.Input);
                    parametros.Add(name: "FCL40MD", value: ticket.FCL40MD, direction: ParameterDirection.Input);

                    con.Execute(@"UPDATE TB_CRM_OPORTUNIDADES SET TabelaId = @TabelaId, TicketLCL = @LCL, TicketFCL40 = @FCL40, TicketFCL20MD = @FCL20MD, TicketFCL20ME = @FCL20ME, TicketFCL40MD = @FCL40MD WHERE Id = @OportunidadeId", parametros);
                }
            }
        }

        public Ticket ObterValoresTicket(int tabelaId)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "TabelaId", value: tabelaId, direction: ParameterDirection.Input);

                    return con.Query<Ticket>($@"
                       SELECT LCL, FCL40, FCL20MD, FCL20ME, FCL40MD FROM
                        (
                            SELECT VALOR_TABELA, 'LCL' As Tipo FROM VW_VALOR_TABELA_LCL WHERE ID_TABELA = :TabelaId
	                        UNION ALL
	                        SELECT VALOR_TABELA, 'FCL40' As Tipo FROM VW_VALOR_TABELA_FCL20MD WHERE ID_TABELA = :TabelaId
	                        UNION ALL
	                        SELECT VALOR_TABELA, 'FCL20MD' As Tipo FROM VW_VALOR_TABELA_FCL20ME WHERE ID_TABELA = :TabelaId
	                        UNION ALL
	                        SELECT VALOR_TABELA, 'FCL20ME' As Tipo FROM VW_VALOR_TABELA_FCL40MD WHERE ID_TABELA = :TabelaId
	                        UNION ALL
	                        SELECT VALOR_TABELA, 'FCL40MD' As Tipo FROM VW_VALOR_TABELA_FCL40ME WHERE ID_TABELA = :TabelaId
                        ) As Q    
                        PIVOT
                        (
                            MAX(VALOR_TABELA) FOR Tipo IN (LCL,FCL40,FCL20MD,FCL20ME,FCL40MD)
                        )P
                        ORDER BY
                            1", parametros).FirstOrDefault();
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "TabelaId", value: tabelaId, direction: ParameterDirection.Input);

                    return con.Query<Ticket>($@"
                       SELECT LCL, FCL40, FCL20MD, FCL20ME, FCL40MD FROM
                        (
                            SELECT VALOR_TABELA, 'LCL' As Tipo FROM VW_VALOR_TABELA_LCL WHERE ID_TABELA = @TabelaId
	                        UNION ALL
	                        SELECT VALOR_TABELA, 'FCL40' As Tipo FROM VW_VALOR_TABELA_FCL20MD WHERE ID_TABELA = @TabelaId
	                        UNION ALL
	                        SELECT VALOR_TABELA, 'FCL20MD' As Tipo FROM VW_VALOR_TABELA_FCL20ME WHERE ID_TABELA = @TabelaId
	                        UNION ALL
	                        SELECT VALOR_TABELA, 'FCL20ME' As Tipo FROM VW_VALOR_TABELA_FCL40MD WHERE ID_TABELA = @TabelaId
	                        UNION ALL
	                        SELECT VALOR_TABELA, 'FCL40MD' As Tipo FROM VW_VALOR_TABELA_FCL40ME WHERE ID_TABELA = @TabelaId
                        ) As Q    
                        PIVOT
                        (
                            MAX(VALOR_TABELA) FOR Tipo IN (LCL,FCL40,FCL20MD,FCL20ME,FCL40MD)
                        )P
                        ORDER BY
                            1", parametros).FirstOrDefault();
                }
            }
        }

        public OportunidadeFicha ObterFichaFaturamentoPorId(int id)
        {
            using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
            {
                return con.Query<OportunidadeFicha>(@"
                    SELECT
                        A.Id,
                        A.OportunidadeId,
                        A.ContaId,
                        A.FaturadoContraId,
                        A.DiasSemana,
                        A.DiasFaturamento,
                        A.DataCorte,
                        A.CondicaoPagamentoFaturamentoId,
                        A.EmailFaturamento,
                        A.ObservacoesFaturamento,
                        A.StatusFichaFaturamento,
                        A.AnexoFaturamentoId,
                        A.CondicaoPgtoDiaSemana,
                        A.CondicaoPgtoDia,
                        C.Descricao As FaturadoContraDescricao,
                        C.NomeFantasia As FaturadoContraFantasia,
                        C.Documento As FaturadoContraDocumento,
                        D.Descricao As ContaDescricao,
                        D.NomeFantasia As ContaFantasia,
                        D.Documento As ContaDocumento,
                        E.Descricao As FontePagadoraDescricao,
                        E.NomeFantasia As FontePagadoraFantasia,
                        E.Documento As FontePagadoraDocumento,
                        B.TabelaId,
                        A.EntregaEletronica,
                        A.EntregaManual,
                        DECODE(A.CorreiosSedex, 0, 1, 0) As CorrreiosComum,
                        A.CorreiosSedex,
                        A.DiaUtil,
                        A.UltimoDiaDoMes,
                        NVL(F.Segmento,0) As SegmentoSubCliente
                    FROM
                        CRM.TB_CRM_OPORTUNIDADE_FICHA_FAT A
                    INNER JOIN
                        CRM.TB_CRM_OPORTUNIDADES B ON A.OportunidadeId = B.Id
                    LEFT JOIN
                        CRM.TB_CRM_CONTAS C ON A.FaturadoContraId = C.Id
                    LEFT JOIN
                        CRM.TB_CRM_CONTAS D ON A.ContaId = D.Id
                    LEFT JOIN
                        CRM.TB_CRM_CONTAS E ON A.FontePagadoraId = E.Id
                    LEFT JOIN
                        CRM.TB_CRM_OPORTUNIDADE_CLIENTES F ON A.ContaId = F.ContaId AND F.OportunidadeId = B.Id
                    WHERE
                        A.Id = :id", new { id }).FirstOrDefault();
            }
        }

        public IEnumerable<OportunidadeFicha> ObterFichasFaturamento(int oportunidadeId)
        {
            using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
            {
                return con.Query<OportunidadeFicha>(@"
                    SELECT
                        A.Id,
                        A.OportunidadeId,
                        A.ContaId,
                        A.FaturadoContraId,
                        A.DiasSemana,
                        A.DiasFaturamento,
                        A.DataCorte,
                        A.CondicaoPagamentoFaturamentoId,
                        A.EmailFaturamento,
                        A.ObservacoesFaturamento,
                        A.StatusFichaFaturamento,
                        A.AnexoFaturamentoId,
                        C.Descricao As FaturadoContraDescricao,
                        C.NomeFantasia As FaturadoContraFantasia,
                        C.Documento As FaturadoContraDocumento,
                        D.Descricao As ContaDescricao,
                        D.NomeFantasia As ContaFantasia,
                        D.Documento As ContaDocumento,
                        E.Descricao As FontePagadoraDescricao,
                        E.NomeFantasia As FontePagadoraFantasia,
                        E.Documento As FontePagadoraDocumento,
                        B.TabelaId,
                        A.EntregaEletronica,
                        A.EntregaManual,
                        DECODE(A.CorreiosSedex, 0, 1, 0) As CorrreiosComum,
                        A.CorreiosSedex,
                        A.DiaUtil,
                        A.UltimoDiaDoMes
                    FROM
                        CRM.TB_CRM_OPORTUNIDADE_FICHA_FAT A
                    INNER JOIN
                        CRM.TB_CRM_OPORTUNIDADES B ON A.OportunidadeId = B.Id
                    LEFT JOIN
                        CRM.TB_CRM_CONTAS C ON A.FaturadoContraId = C.Id
                    LEFT JOIN
                        CRM.TB_CRM_CONTAS D ON A.ContaId = D.Id
                    LEFT JOIN
                        CRM.TB_CRM_CONTAS E ON A.FontePagadoraId = E.Id
                    WHERE
                        A.OportunidadeId = :oportunidadeId", new { oportunidadeId });
            }
        }

        public OportunidadeAdendo ObterAdendoPorId(int id)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    return con.Query<OportunidadeAdendo>(@"
                        SELECT
                            Id,
                            OportunidadeId,
                            TipoAdendo,
                            CriadoPor,
                            StatusAdendo,
                            DataCadastro                       
                        FROM
                            CRM.TB_CRM_OPORTUNIDADE_ADENDOS
                        WHERE
                            Id = :id", new { id }).FirstOrDefault();
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    return con.Query<OportunidadeAdendo>(@"
                        SELECT
                            Id,
                            OportunidadeId,
                            TipoAdendo,
                            CriadoPor,
                            StatusAdendo,
                            DataCadastro                       
                        FROM
                            CRM.TB_CRM_OPORTUNIDADE_ADENDOS
                        WHERE
                            Id = :id", new { id }).FirstOrDefault();
                }
            }
        }

        public FormaPagamento ObterFormaPagamentoAdendoPorId(int id)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "AdendoId", value: id, direction: ParameterDirection.Input);

                    return con.Query<FormaPagamento>(@"
                       SELECT FormaPagamento FROM CRM.TB_CRM_ADENDO_FORMA_PAGAMENTO WHERE AdendoId = :AdendoId", parametros).FirstOrDefault();
                }
            }
            else
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "AdendoId", value: id, direction: ParameterDirection.Input);

                    return con.Query<FormaPagamento>(@"
                       SELECT FormaPagamento FROM CRM..TB_CRM_ADENDO_FORMA_PAGAMENTO WHERE AdendoId = @AdendoId", parametros).FirstOrDefault();
                }
            }
        }

        public Vendedor ObterVendedorAdendoPorId(int id)
        {
            using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "AdendoId", value: id, direction: ParameterDirection.Input);

                return con.Query<Vendedor>(@"SELECT B.Id, B.Nome, B.CPF FROM TB_CRM_ADENDO_VENDEDOR A INNER JOIN TB_CRM_USUARIOS B ON A.VendedorId = B.Id WHERE AdendoId = :AdendoId", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<ClienteProposta> ObterSubClientesOportunidade(int oportunidadeId)
        {
            using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
            {
                return con.Query<ClienteProposta>(@"
                    SELECT
                        B.Id,
                        DECODE(C.Login, NULL, C.LoginExterno, C.Login) As CriadoPor,
                        b.ContaId,
                        A.Descricao,
                        A.Documento,
                        A.NomeFantasia,
                        B.Segmento,
                        B.DataCriacao,
                        D.Segmento
                    FROM
                        CRM.TB_CRM_CONTAS A 
                    INNER JOIN
                        CRM.TB_CRM_OPORTUNIDADE_CLIENTES B ON A.Id = B.ContaId
                    INNER JOIN
                        CRM.TB_CRM_USUARIOS C ON B.CriadoPor = C.Id
                    INNER JOIN
                        CRM.TB_CRM_OPORTUNIDADE_CLIENTES D ON A.Id = D.ContaId AND D.OportunidadeId = :oportunidadeId
                    WHERE
                        B.OportunidadeId = :oportunidadeId", new { oportunidadeId });
            }
        }

        public IEnumerable<ClienteProposta> ObterSubClientesAdendo(int adendoId, int oportunidadeId, AdendoAcao adendoAcao)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();

                    parametros.Add(name: "AdendoId", value: adendoId, direction: ParameterDirection.Input);
                    parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);
                    parametros.Add(name: "AdendoAcao", value: adendoAcao, direction: ParameterDirection.Input);

                    return con.Query<ClienteProposta>(@"
                        SELECT 
                            C.Id,
	                        C.Documento,
                            A.Segmento,
                            C.Descricao,
                            C.NomeFantasia
                        FROM 
	                        CRM.TB_CRM_ADENDO_SUB_CLIENTE A
                        INNER JOIN
	                        CRM.TB_CRM_OPORTUNIDADE_ADENDOS B ON A.AdendoId = B.Id
                        INNER JOIN
	                        CRM.TB_CRM_CONTAS C ON A.SubClienteId = C.Id
                        WHERE 
	                        A.AdendoId = :AdendoId AND A.Acao = :AdendoAcao
                        AND
	                        B.OportunidadeId = :OportunidadeId", parametros);
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);

                    return con.Query<ClienteProposta>(@"
                        SELECT 
                            C.Id,
	                        C.Documento,
                            C.Segmento,
                            C.Descricao,
                            C.NomeFantasia
                        FROM 
	                        CRM..TB_CRM_ADENDO_SUB_CLIENTE A
                        INNER JOIN
	                        CRM..TB_CRM_OPORTUNIDADE_ADENDOS B ON A.AdendoId = B.Id
                        INNER JOIN
	                        CRM..TB_CRM_CONTAS C ON A.SubClienteId = C.Id
                        WHERE 
	                        A.AdendoId = @AdendoId AND A.Acao = @AdendoId
                        AND
	                        B.OportunidadeId = @OportunidadeId", parametros);
                }
            }
        }

        public IEnumerable<ClienteProposta> ObterClientesGrupoCnpjAdendo(int adendoId, int oportunidadeId, AdendoAcao adendoAcao)
        {
            if (Configuracoes.BancoEmUso() == "ORACLE")
            {
                using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();

                    parametros.Add(name: "AdendoId", value: adendoId, direction: ParameterDirection.Input);
                    parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);
                    parametros.Add(name: "AdendoAcao", value: adendoAcao, direction: ParameterDirection.Input);

                    return con.Query<ClienteProposta>(@"
                        SELECT 
                            C.Id,
	                        C.Documento,
                            C.Segmento As SegmentoConta,
                            C.Descricao,
                            C.NomeFantasia
                        FROM 
	                        CRM.TB_CRM_ADENDO_GRUPO_CNPJ A
                        INNER JOIN
	                        CRM.TB_CRM_OPORTUNIDADE_ADENDOS B ON A.AdendoId = B.Id
                        INNER JOIN
	                        CRM.TB_CRM_CONTAS C ON A.GrupoCnpjId = C.Id
                        WHERE 
	                        A.AdendoId = :AdendoId AND A.Acao = :AdendoAcao
                        AND
	                        B.OportunidadeId = :OportunidadeId", parametros);
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Configuracoes.StringConexao()))
                {
                    var parametros = new DynamicParameters();
                    parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);

                    return con.Query<ClienteProposta>(@"
                        SELECT 
                            C.Id,
	                        C.Documento,
                            C.Segmento,
                            C.Descricao,
                            C.NomeFantasia
                        FROM 
	                        CRM..TB_CRM_ADENDO_GRUPO_CNPJ A
                        INNER JOIN
	                        CRM..TB_CRM_OPORTUNIDADE_ADENDOS B ON A.AdendoId = B.Id
                        INNER JOIN
	                        CRM..TB_CRM_CONTAS C ON A.GrupoCnpjId = C.Id
                        WHERE 
	                        A.AdendoId = @AdendoId AND A.Acao = @AdendoId
                        AND
	                        B.OportunidadeId = @OportunidadeId", parametros);
                }
            }
        }

        public void AtualizarValorTicket(int oportunidadeId, decimal valor, WsSimuladorCalculoTabelas.Enums.Regime regime)
        {
            using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "OportunidadeId", value: oportunidadeId, direction: ParameterDirection.Input);
                parametros.Add(name: "Valor", value: valor, direction: ParameterDirection.Input);

                if (regime == Enums.Regime.LCL)
                {
                    con.Execute("UPDATE CRM.TB_CRM_OPORTUNIDADES SET FaturamentoMensalLCL = :Valor WHERE Id = :OportunidadeId", parametros);
                }
                else
                {
                    con.Execute("UPDATE CRM.TB_CRM_OPORTUNIDADES SET FaturamentoMensalFCL = :Valor WHERE Id = :OportunidadeId", parametros);
                }
            }
        }

        public void IntegrarFontePagadora(OportunidadeFicha ficha)
        {
            using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
            {
                con.Open();

                using (var transaction = con.BeginTransaction())
                {
                    var parametros = new DynamicParameters();

                    var sequencia = con.Query<int>("SELECT SGIPA.SEQ_DADOS_FATURAMENTO_IPA.NEXTVAL FROM DUAL").Single();

                    parametros.Add(name: "FaturadoContraId", value: ficha.FaturadoContraId, direction: ParameterDirection.Input);
                    parametros.Add(name: "FontePagadoraId", value: ficha.FontePagadoraId, direction: ParameterDirection.Input);
                    parametros.Add(name: "ContaId", value: ficha.ContaId, direction: ParameterDirection.Input);
                    parametros.Add(name: "FichaId", value: ficha.Id, direction: ParameterDirection.Input);
                    parametros.Add(name: "Sequencia", value: sequencia, direction: ParameterDirection.Input);
                    parametros.Add(name: "TabelaId", value: ficha.TabelaId, direction: ParameterDirection.Input);
                    parametros.Add(name: "DataCorte", value: ficha.DataCorte, direction: ParameterDirection.Input);
                    parametros.Add(name: "EmailFaturamento", value: ficha.EmailFaturamento, direction: ParameterDirection.Input);
                    parametros.Add(name: "CondicaoPagamentoFaturamentoId", value: ficha.CondicaoPagamentoFaturamentoId, direction: ParameterDirection.Input);
                    parametros.Add(name: "EntregaEletronica", value: ficha.EntregaEletronica.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "EntregaManual", value: ficha.EntregaManual.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "CorrreiosComum", value: ficha.CorrreiosComum.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "CorreiosSedex", value: ficha.CorreiosSedex.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "DiaUtil", value: ficha.DiaUtil.ToInt(), direction: ParameterDirection.Input);
                    parametros.Add(name: "OportunidadeId", value: ficha.OportunidadeId, direction: ParameterDirection.Input);
                    parametros.Add(name: "FichaId", value: ficha.Id, direction: ParameterDirection.Input);

                    try
                    {
                        if (ficha.FichaGeral)
                        {                            
                            con.Execute("DELETE FROM SGIPA.TB_DADOS_FAT_IPA_DIAS WHERE AUTONUM_FONTE_PAGADORA IN (SELECT AUTONUM FROM SGIPA.TB_DADOS_FATURAMENTO_IPA WHERE OPORTUNIDADE_ID = :OportunidadeId)", parametros, transaction);
                            con.Execute("DELETE FROM SGIPA.TB_DADOS_FAT_IPA_COND_PG_DIAS WHERE AUTONUM_FONTE_PAGADORA IN (SELECT AUTONUM FROM SGIPA.TB_DADOS_FATURAMENTO_IPA WHERE OPORTUNIDADE_ID = :OportunidadeId)", parametros, transaction);
                            con.Execute("DELETE FROM SGIPA.TB_DADOS_FAT_IPA_DIAS_SEMANA WHERE AUTONUM_FONTE_PAGADORA IN (SELECT AUTONUM FROM SGIPA.TB_DADOS_FATURAMENTO_IPA WHERE OPORTUNIDADE_ID = :OportunidadeId)", parametros, transaction);
                            con.Execute("DELETE FROM SGIPA.TB_DADOS_FATURAMENTO_IPA WHERE OPORTUNIDADE_ID = :OportunidadeId", parametros, transaction);

                            con.Execute($@"
                                INSERT INTO SGIPA.TB_DADOS_FATURAMENTO_IPA 
                                    (
                                        AUTONUM,  
                         	            AUTONUM_LISTA,  
                         	            AUTONUM_CLIENTE_NOTA,  
                         	            AUTONUM_CLIENTE_ENVIO_NOTA,  
                                        AUTONUM_CLIENTE_PAGAMENTO,
                         	            CORTE,  
                                        EMAIL,
                                        AUTONUM_FORMA_PAGAMENTO,
                                        FLAG_ENTREGA_ELETRONICA,
                                        FLAG_ENTREGA_MANUAL,
                                        FLAG_ENVIO_CORREIO_COMUM,
                                        FLAG_ENVIO_CORREIO_SEDEX,
                                        FLAG_VENCIMENTO_DIA_UTIL,
                                        FLAG_ULTIMO_DIA_DO_MES_VCTO,
                                        OPORTUNIDADE_ID,
                                        FICHA_ID,
                                        DATA_INTEGRACAO
                                    )  
                                    SELECT  
                                        :Sequencia,  
                         	            B.TabelaId,
                         	            :FaturadoContraId,  
                         	            :ContaId,
                                        :FontePagadoraId,
                         	            A.DataCorte,
                                        A.EmailFaturamento,
                         	            A.CondicaoPagamentoFaturamentoId,
                                        A.EntregaEletronica,
                                        A.EntregaManual,
                                        DECODE(A.CorreiosSedex, 0, 1, 0),
                                        A.CorreiosSedex,
                                        A.DiaUtil,
                                        A.UltimoDiaDoMes,
                                        :OportunidadeId,
                                        :FichaId,
                                        SYSDATE
                                    FROM  
                                        CRM.TB_CRM_OPORTUNIDADE_FICHA_FAT A
                                    INNER JOIN
                                        CRM.TB_CRM_OPORTUNIDADES B ON A.OportunidadeId = B.Id
                                    WHERE  
                                        A.Id = :FichaId", parametros, transaction);

                            if (ficha.DiasSemana != null)
                            {
                                foreach (var diaSemana in ficha.DiasSemana.Split(','))
                                {
                                    parametros = new DynamicParameters();

                                    parametros.Add(name: "Sequencia", value: sequencia, direction: ParameterDirection.Input);
                                    parametros.Add(name: "Dia", value: ConverteDiaSemanaIPA.DiaSemana(diaSemana), direction: ParameterDirection.Input);

                                    con.Execute("INSERT INTO SGIPA.TB_DADOS_FAT_IPA_DIAS_SEMANA (AUTONUM, AUTONUM_FONTE_PAGADORA, DIA) VALUES (SGIPA.SEQ_DADOS_FAT_IPA_DIAS_SEMANA.NEXTVAL, :Sequencia, :Dia)", parametros, transaction);
                                }
                            }

                            if (ficha.DiasFaturamento != null)
                            {
                                foreach (var diaFaturamento in ficha.DiasFaturamento.Split(','))
                                {
                                    parametros = new DynamicParameters();

                                    parametros.Add(name: "Sequencia", value: sequencia, direction: ParameterDirection.Input);
                                    parametros.Add(name: "Dia", value: diaFaturamento, direction: ParameterDirection.Input);

                                    con.Execute("INSERT INTO SGIPA.TB_DADOS_FAT_IPA_DIAS (AUTONUM, AUTONUM_FONTE_PAGADORA, DIA) VALUES (SGIPA.SEQ_DADOS_FAT_IPA_DIAS_FAT.NEXTVAL, :Sequencia, :Dia)", parametros, transaction);
                                }
                            }

                            if (ficha.CondicaoPgtoDiaSemana != null)
                            {
                                foreach (var diaFaturamento in ficha.CondicaoPgtoDiaSemana.Split(','))
                                {
                                    parametros = new DynamicParameters();

                                    parametros.Add(name: "Sequencia", value: sequencia, direction: ParameterDirection.Input);
                                    parametros.Add(name: "Dia", value: ConverteDiaSemanaIPA.DiaSemana(diaFaturamento), direction: ParameterDirection.Input);

                                    con.Execute("INSERT INTO SGIPA.TB_DADOS_FAT_IPA_COND_PG_DIAS (AUTONUM, AUTONUM_FONTE_PAGADORA, DIA) VALUES (SGIPA.SEQ_DADOS_FAT_IPA_DIAS_FAT.NEXTVAL, :Sequencia, :Dia)", parametros, transaction);
                                }
                            }

                            if (ficha.CondicaoPgtoDia != null)
                            {
                                foreach (var diaFaturamento in ficha.CondicaoPgtoDia.Split(','))
                                {
                                    parametros = new DynamicParameters();

                                    parametros.Add(name: "Sequencia", value: sequencia, direction: ParameterDirection.Input);
                                    parametros.Add(name: "Dia", value: ConverteDiaSemanaIPA.DiaSemana(diaFaturamento), direction: ParameterDirection.Input);

                                    con.Execute("INSERT INTO SGIPA.TB_DADOS_FAT_IPA_DIAS_PGTO (AUTONUM, AUTONUM_FONTE_PAGADORA, DIA) VALUES (SGIPA.SEQ_DADOS_FAT_IPA_DIAS_PGTO.NEXTVAL, :Sequencia, :Dia)", parametros, transaction);
                                }
                            }

                            transaction.Commit();
                        }
                        else
                        {
                            IntegrarGrupoFontePagadora(ficha);
                        }
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        public void IntegrarGrupoFontePagadora(OportunidadeFicha ficha)
        {
            using (OracleConnection con = new OracleConnection(Configuracoes.StringConexao()))
            {
                con.Open();

                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        var parametrosGrupo = new DynamicParameters();

                        parametrosGrupo.Add(name: "FaturadoContraId", value: ficha.FaturadoContraId, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "FontePagadoraId", value: ficha.FontePagadoraId, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "ContaId", value: ficha.ContaId, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "FichaId", value: ficha.Id, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "TabelaId", value: ficha.TabelaId, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "DataCorte", value: ficha.DataCorte, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "EmailFaturamento", value: ficha.EmailFaturamento, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "CondicaoPagamentoFaturamentoId", value: ficha.CondicaoPagamentoFaturamentoId, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "EntregaEletronica", value: ficha.EntregaEletronica.ToInt(), direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "EntregaManual", value: ficha.EntregaManual.ToInt(), direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "CorreiosComum", value: ficha.CorrreiosComum.ToInt(), direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "CorreiosSedex", value: ficha.CorreiosSedex.ToInt(), direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "DiaUtil", value: ficha.DiaUtil.ToInt(), direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "UltimoDiaDoMes", value: ficha.UltimoDiaDoMes.ToInt(), direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "DiasFaturamento", value: ficha.DiasFaturamento, direction: ParameterDirection.Input);                        
                        parametrosGrupo.Add(name: "OportunidadeId", value: ficha.OportunidadeId, direction: ParameterDirection.Input);
                        parametrosGrupo.Add(name: "FichaId", value: ficha.Id, direction: ParameterDirection.Input);

                        if (!string.IsNullOrEmpty(ficha.ContaDocumento))
                        {
                            var subClienteChronos = _tabelasDAO.ConsultaParceiroFontePagadora(ficha.ContaDocumento, ficha.TabelaId, ficha.SegmentoSubCliente);

                            if (subClienteChronos == null)
                            {
                                var parceiro = new Parceiro
                                {
                                    RazaoSocial = subClienteChronos.RazaoSocial,
                                    NomeFantasia = subClienteChronos.NomeFantasia,
                                    CNPJ = subClienteChronos.CNPJ
                                };

                                ficha.ContaId = _tabelasDAO.CadastrarParceiro(parceiro);
                            }
                            else
                            {
                                ficha.ContaId = subClienteChronos.Id;
                            }
                        }

                        parametrosGrupo.Add(name: "SubClienteId", value: ficha.ContaId, direction: ParameterDirection.Input);

                        con.Execute("DELETE FROM SGIPA.TB_DADOS_FAT_GRU_DIAS WHERE AUTONUM_FONTE_PAGADORA IN (SELECT AUTONUM FROM SGIPA.TB_DADOS_FATURAMENTO_IPA_GRP WHERE OPORTUNIDADE_ID = :OportunidadeId AND AUTONUM_GRUPO_LISTA = :SubClienteId)", parametrosGrupo, transaction);
                        con.Execute("DELETE FROM SGIPA.TB_DADOS_FAT_GRU_DIAS_SEMANA WHERE AUTONUM_FONTE_PAGADORA IN (SELECT AUTONUM FROM SGIPA.TB_DADOS_FATURAMENTO_IPA_GRP WHERE OPORTUNIDADE_ID = :OportunidadeId AND AUTONUM_GRUPO_LISTA = :SubClienteId)", parametrosGrupo, transaction);
                        con.Execute("DELETE FROM SGIPA.TB_DADOS_FAT_GRU_COND_PG_DIAS WHERE AUTONUM_FONTE_PAGADORA IN (SELECT AUTONUM FROM SGIPA.TB_DADOS_FATURAMENTO_IPA_GRP WHERE OPORTUNIDADE_ID = :OportunidadeId AND AUTONUM_GRUPO_LISTA = :SubClienteId)", parametrosGrupo, transaction);
                        con.Execute("DELETE FROM SGIPA.TB_DADOS_FATURAMENTO_IPA_GRP WHERE OPORTUNIDADE_ID = :OportunidadeId AND AUTONUM_GRUPO_LISTA = :SubClienteId", parametrosGrupo, transaction);

                        parametrosGrupo.Add(name: "SequenciaGrupo", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        con.Execute($@"
                                INSERT INTO SGIPA.TB_DADOS_FATURAMENTO_IPA_GRP 
                                    (
                                        AUTONUM,
                                        AUTONUM_LISTA,
                                        AUTONUM_CLIENTE_NOTA,
                                        AUTONUM_CLIENTE_ENVIO_NOTA,
                                        AUTONUM_CLIENTE_PAGAMENTO,
                                        AUTONUM_GRUPO_LISTA,
                                        CORTE,
                                        DIA,
                                        EMAIL,
                                        AUTONUM_FORMA_PAGAMENTO,
                                        FLAG_ENTREGA_ELETRONICA,
                                        FLAG_ENTREGA_MANUAL,
                                        FLAG_ENVIO_CORREIO_COMUM,
                                        FLAG_ENVIO_CORREIO_SEDEX,
                                        FLAG_ULTIMO_DIA_DO_MES_VCTO,
                                        FLAG_VENCIMENTO_DIA_UTIL,
                                        OPORTUNIDADE_ID,
                                        FICHA_ID,
                                        DATA_INTEGRACAO
                                    ) VALUES (
                                        SGIPA.SEQ_DADOS_FATURAMENTO_IPA_GRP.NEXTVAL,  
                                        :TabelaId,
                                        :FaturadoContraId,
                                        :ContaId,
                                        :FontePagadoraId,
                                        :SubClienteId,
                                        :DataCorte,
                                        :DiasFaturamento,
                                        :EmailFaturamento,
                                        :CondicaoPagamentoFaturamentoId,
                                        :EntregaEletronica,
                                        :EntregaManual,
                                        :CorreiosComum,
                                        :CorreiosSedex,
                                        :UltimoDiaDoMes,
                                        :DiaUtil,
                                        :OportunidadeId,
                                        :FichaId,
                                        SYSDATE
                                    ) RETURNING AUTONUM INTO :SequenciaGrupo", parametrosGrupo, transaction);

                        var sequenciaGrupo = parametrosGrupo.Get<int>("SequenciaGrupo");

                        if (ficha.DiasSemana != null)
                        {
                            foreach (var diaSemana in ficha.DiasSemana.Split(','))
                            {
                                var parametrosDiaSemana = new DynamicParameters();

                                parametrosDiaSemana.Add(name: "Dia", value: ConverteDiaSemanaIPA.DiaSemana(diaSemana), direction: ParameterDirection.Input);
                                parametrosDiaSemana.Add(name: "SequenciaFontePagadora", value: sequenciaGrupo, direction: ParameterDirection.Input);

                                con.Execute("INSERT INTO SGIPA.TB_DADOS_FAT_GRU_DIAS_SEMANA (AUTONUM, AUTONUM_FONTE_PAGADORA, DIA) VALUES (SGIPA.SEQ_DADOS_FAT_IPA_DIAS_SEMANA.NEXTVAL, :SequenciaFontePagadora, :Dia)", parametrosDiaSemana, transaction);
                            }
                        }

                        if (ficha.CondicaoPgtoDiaSemana != null)
                        {
                            foreach (var diaFaturamento in ficha.CondicaoPgtoDiaSemana.Split(','))
                            {
                                var parametrosDiaPgto = new DynamicParameters();

                                parametrosDiaPgto.Add(name: "Dia", value: ConverteDiaSemanaIPA.DiaSemana(diaFaturamento), direction: ParameterDirection.Input);
                                parametrosDiaPgto.Add(name: "SequenciaFontePagadora", value: sequenciaGrupo, direction: ParameterDirection.Input);

                                con.Execute("INSERT INTO SGIPA.TB_DADOS_FAT_GRU_COND_PG_DIAS (AUTONUM, AUTONUM_FONTE_PAGADORA, DIA) VALUES (SGIPA.SEQ_DADOS_FAT_IPA_DIAS_FAT.NEXTVAL, :SequenciaFontePagadora, :Dia)", parametrosDiaPgto, transaction);
                            }
                        }

                        if (ficha.DiasFaturamento != null)
                        {
                            foreach (var dia in ficha.DiasFaturamento.Split(','))
                            {
                                var parametrosDiaPgto = new DynamicParameters();

                                parametrosDiaPgto.Add(name: "Dia", value: dia, direction: ParameterDirection.Input);
                                parametrosDiaPgto.Add(name: "SequenciaFontePagadora", value: sequenciaGrupo, direction: ParameterDirection.Input);

                                con.Execute("INSERT INTO SGIPA.TB_DADOS_FAT_GRU_DIAS (AUTONUM, AUTONUM_FONTE_PAGADORA, DIA) VALUES (SGIPA.SEQ_DADOS_FAT_GRU_DIAS_FAT.NEXTVAL, :SequenciaFontePagadora, :Dia)", parametrosDiaPgto, transaction);
                            }
                        }

                        if (ficha.CondicaoPgtoDia != null)
                        {
                            foreach (var dia in ficha.CondicaoPgtoDia.Split(','))
                            {
                                var parametrosDiaPgto = new DynamicParameters();

                                parametrosDiaPgto.Add(name: "Dia", value: dia, direction: ParameterDirection.Input);
                                parametrosDiaPgto.Add(name: "SequenciaFontePagadora", value: sequenciaGrupo, direction: ParameterDirection.Input);

                                con.Execute("INSERT INTO SGIPA.TB_DADOS_FAT_GRU_DIAS_PGTO (AUTONUM, AUTONUM_FONTE_PAGADORA, DIA) VALUES (SGIPA.SEQ_DADOS_FAT_GRU_DIAS_PGTO.NEXTVAL, :SequenciaFontePagadora, :Dia)", parametrosDiaPgto, transaction);
                            }
                        }

                        transaction.Commit();

                    }
                    catch (System.Exception ex)
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }
    }
}