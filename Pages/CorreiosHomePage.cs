using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace CorreiosAutomation.Pages
{
    public class CorreiosHomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // URLs
        private const string UrlCorreios = "https://www.correios.com.br/";

        // Seletores dos elementos (ID, XPATH, CSS)
        private By CampoBuscaCepId => By.Id("endereco"); // Exemplo - ajustar conforme site real
        private By BotaoBuscarCepId => By.Id("btnBuscar");
        private By ResultadoEnderecoXpath => By.XPath("//div[@class='resultado']//span[@class='logradouro']");
        private By ResultadoEnderecoCss => By.CssSelector("div.resultado span.logradouro");
        private By MensagemErroCep => By.ClassName("erro");
        private By LinkVoltar => By.LinkText("Página inicial");
        private By CampoTracking => By.Name("objeto");
        private By BotaoTracking => By.XPath("//button[contains(text(),'Buscar')]");

        public CorreiosHomePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateTo()
        {
            _driver.Navigate().GoToUrl(UrlCorreios);
            _driver.Manage().Window.Maximize();
        }

        public void BuscarCep(string cep)
        {
            var campo = _wait.Until(d => d.FindElement(CampoBuscaCepId));
            campo.Clear();
            campo.SendKeys(cep);
            _driver.FindElement(BotaoBuscarCepId).Click();
        }

        public string ObterMensagemCepNaoEncontrado()
        {
            try
            {
                var msg = _wait.Until(d => d.FindElement(MensagemErroCep));
                return msg.Text;
            }
            catch
            {
                return "Mensagem não encontrada";
            }
        }

        public string ObterEnderecoEncontrado()
        {
            var elemento = _wait.Until(d => d.FindElement(ResultadoEnderecoXpath));
            return elemento.Text;
        }

        public void VoltarParaPaginaInicial()
        {
            _driver.Navigate().Back();
            // Ou clicar no link de voltar
            // _driver.FindElement(LinkVoltar).Click();
        }

        public void RastrearEncomenda(string codigo)
        {
            var campo = _wait.Until(d => d.FindElement(CampoTracking));
            campo.Clear();
            campo.SendKeys(codigo);
            _driver.FindElement(BotaoTracking).Click();
        }

        public string ObterMensagemRastreamentoInvalido()
        {
            // Aguarda elemento de erro no rastreamento
            var msg = _wait.Until(d => d.FindElement(By.ClassName("erro-tracking")));
            return msg.Text;
        }

        // Métodos para os seletores específicos do teste

        public IWebElement ObterResultadoPorId()
        {
            return _wait.Until(d => d.FindElement(By.Id("resultado-endereco"))); // Ajustar ID real
        }

        public IWebElement ObterResultadoPorXPath()
        {
            return _wait.Until(d => d.FindElement(By.XPath("//div[@class='resultado-busca']")));
        }

        public IWebElement ObterResultadoPorCss()
        {
            return _wait.Until(d => d.FindElement(By.CssSelector("div.resultado-busca")));
        }
    }
}