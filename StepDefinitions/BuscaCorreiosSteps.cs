using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using CorreiosAutomation.Pages;
using CorreiosAutomation.Drivers;
using CorreiosAutomation.Utils;

namespace CorreiosAutomation.StepDefinitions
{
    [Binding]
    public class BuscaCorreiosSteps
    {
        private readonly IWebDriver _driver;
        private readonly CorreiosHomePage _homePage;
        private string _resultadoEndereco;

        public BuscaCorreiosSteps()
        {
            _driver = WebDriverManager.GetDriver();
            _homePage = new CorreiosHomePage(_driver);
        }

        [Given(@"que estou na página inicial dos Correios")]
        public void DadoQueEstouNaPaginaInicialDosCorreios()
        {
            _homePage.NavigateTo();
        }

        [When(@"eu pesquisar pelo CEP ""(.*)""")]
        public void QuandoEuPesquisarPeloCep(string cep)
        {
            _homePage.BuscarCep(cep);
            System.Threading.Thread.Sleep(1000); // Aguarda carregamento
        }



        [Then(@"eu volto para a página inicial")]
        public void EntaoEuVoltoParaAPaginaInicial()
        {
            _homePage.VoltarParaPaginaInicial();
        }

        [When(@"eu rastrear o código ""(.*)""")]
        public void QuandoEuRastrearOCodigo(string codigo)
        {
            _homePage.RastrearEncomenda(codigo);
            System.Threading.Thread.Sleep(1000);
        }

        [Then(@"o sistema deve informar que o código não está correto")]
        public void EntaoOSistemaDeveInformarQueOCodigoNaoEstaCorreto()
        {
            var mensagemErro = _homePage.ObterMensagemRastreamentoInvalido();
            Assert.IsTrue(mensagemErro.Contains("não encontrado") || mensagemErro.Contains("inválido"),
                $"Esperava mensagem de código inválido, mas obteve: {mensagemErro}");
        }

        // ========== VALIDAÇÕES DOS SELETORES (ID, XPATH, CSS) ==========

        [Then(@"o elemento do resultado deve ser encontrado por ID")]
        public void EntaoElementoEncontradoPorId()
        {
            var elemento = _homePage.ObterResultadoPorId();
            Assert.IsNotNull(elemento, "Elemento não encontrado pelo seletor ID");
        }

        [Then(@"o elemento do resultado deve ser encontrado por XPATH")]
        public void EntaoElementoEncontradoPorXpath()
        {
            var elemento = _homePage.ObterResultadoPorXPath();
            Assert.IsNotNull(elemento, "Elemento não encontrado pelo seletor XPATH");
        }

        [Then(@"o elemento do resultado deve ser encontrado por CSS")]
        public void EntaoElementoEncontradoPorCss()
        {
            var elemento = _homePage.ObterResultadoPorCss();
            Assert.IsNotNull(elemento, "Elemento não encontrado pelo seletor CSS");
        }
        // Timeout de 30 segundos para este teste específico
        [Then(@"o sistema deve informar que o CEP não existe")]
        [Timeout(30000)]  // NUnit timeout padrão
        public void EntaoOSistemaDeveInformarQueOCepNaoExiste()
        {
            TestTimeoutHelper.ExecuteWithTimeout(() =>
            {
                var mensagemErro = _homePage.ObterMensagemCepNaoEncontrado();
                Assert.IsTrue(mensagemErro.Contains("não encontrado"));
            }, 10000); // 10 segundos para esta operação
        }

        // Com timeout personalizado
        [Then(@"o resultado deve ser ""(.*)""")]
        [CustomTimeout(20000)] // Timeout personalizado de 20 segundos
        public void EntaoOResultadoDeveSer(string enderecoEsperado)
        {
            _resultadoEndereco = _homePage.ObterEnderecoEncontrado();
            Assert.AreEqual(enderecoEsperado, _resultadoEndereco);
        }
    }
}