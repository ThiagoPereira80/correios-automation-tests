using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using CorreiosAutomation.Pages;
using CorreiosAutomation.Drivers;

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

        [Then(@"o sistema deve informar que o CEP não existe")]
        public void EntaoOSistemaDeveInformarQueOCepNaoExiste()
        {
            var mensagemErro = _homePage.ObterMensagemCepNaoEncontrado();
            Assert.IsTrue(mensagemErro.Contains("não encontrado") || mensagemErro.Contains("inexistente"),
                $"Esperava mensagem de CEP não encontrado, mas obteve: {mensagemErro}");
        }

        [Then(@"o resultado deve ser ""(.*)""")]
        public void EntaoOResultadoDeveSer(string enderecoEsperado)
        {
            _resultadoEndereco = _homePage.ObterEnderecoEncontrado();
            Assert.AreEqual(enderecoEsperado, _resultadoEndereco,
                $"Endereço encontrado: {_resultadoEndereco} - Esperado: {enderecoEsperado}");
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
    }
}