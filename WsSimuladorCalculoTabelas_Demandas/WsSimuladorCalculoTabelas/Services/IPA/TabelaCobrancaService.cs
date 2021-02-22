using WsSimuladorCalculoTabelas.DAO;
using WsSimuladorCalculoTabelas.Responses;

namespace WsSimuladorCalculoTabelas.Services.IPA
{
    public class TabelaCobrancaService
    {
        private readonly TabelasDAO _tabelasDAO = new TabelasDAO(false);
        private readonly OportunidadeDAO _oportunidadeDAO = new OportunidadeDAO();
        private readonly ExportaTabelasService _exportaTabelasService;
        private readonly int _oportunidadeId;

        public TabelaCobrancaService(int oportunidadeId)
        {
            _oportunidadeDAO = new OportunidadeDAO();
            _tabelasDAO = new TabelasDAO(false);
            _exportaTabelasService = new ExportaTabelasService(oportunidadeId, false);
            _oportunidadeId = oportunidadeId;
        }

        public Response ExportarTabela(int usuarioIntegracao)
        {
            var oportunidade = _oportunidadeDAO.ObterOportunidadePorId(_oportunidadeId);

            if (oportunidade == null)
                return new Response
                {
                    Sucesso = false,
                    Mensagem = $"Oportunidade {_oportunidadeId} não encontrada"
                };

            _tabelasDAO.ExcluirTabelaCobrancaIPA(oportunidade.Id);

            oportunidade.UsuarioIntegracaoId = usuarioIntegracao;

            var tabelaCobranca = _tabelasDAO.CadastrarTabelaCobrancaSGIPA(oportunidade);

            //_tabelasDAO.ExportarLotes(tabelaCobranca, oportunidade.Id);

            if (oportunidade.TabelaReferencia == 0)
            {
                _exportaTabelasService.ExportarServicos(tabelaCobranca);
                _tabelasDAO.ImportarImpostos(0, tabelaCobranca);

            }
            else
            {
                //_tabelasDAO.ImportarServicosFixos(oportunidade.TabelaReferencia, tabelaCobranca);
                //_tabelasDAO.ImportarServicosVariaveis(oportunidade.TabelaReferencia, tabelaCobranca);
                //_tabelasDAO.ImportarImpostos(oportunidade.TabelaReferencia, tabelaCobranca);
                //_tabelasDAO.ImportarFontePagadora(oportunidade.TabelaReferencia, tabelaCobranca);
             }

            _tabelasDAO.AtualizaDataVendedor(tabelaCobranca);

            return new Response
            {
                Sucesso = true,
                TabelaId = tabelaCobranca,
                Mensagem = $"Tabela {tabelaCobranca} criada com sucesso"
            };
        }
    }
}