using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using NUnit.Framework;
using System.Threading;

namespace CorreiosAutomation.Components
{
    public class CaptchaComponent
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly By _captchaBy = By.Id("captcha");

        public CaptchaComponent(IWebDriver driver, TimeSpan? timeout = null)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, timeout ?? TimeSpan.FromSeconds(5));
        }

        public bool ValidarCampoCaptcha()
        {
            try
            {
                var captcha = _wait.Until(d => d.FindElement(_captchaBy));
                if (captcha == null || !captcha.Displayed || !captcha.Enabled)
                    return false;

                var tag = captcha.TagName ?? string.Empty;
                var type = captcha.GetAttribute("type") ?? string.Empty;

                return tag.Equals("input", StringComparison.OrdinalIgnoreCase)
                       && type.Equals("text", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public void PreencherCaptcha(string codigo)
        {
            var campo = _wait.Until(d => d.FindElement(_captchaBy));
            campo.Clear();
            campo.SendKeys(codigo);
        }

        public string ObterValorCaptcha()
        {
            try
            {
                var campo = _wait.Until(d => d.FindElement(_captchaBy));
                return campo.GetAttribute("value") ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        // Tenta preencher o captcha e executar a ação de submissão até maxAttempts vezes.
        // submitAction: ação que submete o formulário (ex.: clicar no botão de busca/rastreio)
        // isSuccess: predicado que retorna true quando a submissão foi bem-sucedida
        public bool TrySubmitWithCaptcha(Action submitAction, Func<bool> isSuccess, string codigo, int maxAttempts = 3, int delayMilliseconds = 1000, By refreshButton = null)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                TestContext.Progress.WriteLine($"[Captcha] Tentativa {attempt} de {maxAttempts}");
                CorreiosAutomation.Utils.RunLog.Write($"[Captcha] Tentativa {attempt} de {maxAttempts}");

                PreencherCaptcha(codigo);

                // Confirma que o valor foi inserido
                var valor = ObterValorCaptcha();
                TestContext.Progress.WriteLine($"[Captcha] Valor inserido no campo: '{valor}' (esperado: '{codigo}')");
                CorreiosAutomation.Utils.RunLog.Write($"[Captcha] Valor inserido no campo: '{valor}' (esperado: '{codigo}')");
                if (!string.Equals(valor, codigo, StringComparison.Ordinal))
                {
                    // Se não inseriu corretamente, tenta novamente
                    Thread.Sleep(delayMilliseconds);
                    continue;
                }

                // Submete e aguarda breve intervalo
                submitAction?.Invoke();
                Thread.Sleep(delayMilliseconds);

                // Verifica sucesso
                try
                {
                    var ok = isSuccess();
                    TestContext.Progress.WriteLine($"[Captcha] isSuccess retornou: {ok}");
                    CorreiosAutomation.Utils.RunLog.Write($"[Captcha] isSuccess retornou: {ok}");
                    if (ok)
                        return true;
                }
                catch (Exception ex)
                {
                    TestContext.Progress.WriteLine($"[Captcha] Exceção ao avaliar isSuccess: {ex.Message}");
                    CorreiosAutomation.Utils.RunLog.Write($"[Captcha] Exceção ao avaliar isSuccess: {ex.Message}");
                    // Ignora exceções do predicado e tenta novamente
                }

                // Se não for a última tentativa, tenta renovar o captcha (clicar no botão de refresh) antes da próxima iteração
                if (attempt < maxAttempts && refreshButton != null)
                {
                    try
                    {
                        var btn = _driver.FindElement(refreshButton);
                        if (btn != null && btn.Displayed && btn.Enabled)
                        {
                            TestContext.Progress.WriteLine("[Captcha] Clicando no botão de renovar captcha antes da próxima tentativa");
                            CorreiosAutomation.Utils.RunLog.Write("[Captcha] Clicando no botão de renovar captcha antes da próxima tentativa");
                            btn.Click();
                        }
                    }
                    catch (Exception ex)
                    {
                        TestContext.Progress.WriteLine($"[Captcha] Não foi possível clicar no botão de refresh: {ex.Message}");
                        CorreiosAutomation.Utils.RunLog.Write($"[Captcha] Não foi possível clicar no botão de refresh: {ex.Message}");
                    }

                    // espera breve para nova imagem/campo ser carregado
                    Thread.Sleep(delayMilliseconds);
                }
            }

            TestContext.Progress.WriteLine($"[Captcha] Todas as {maxAttempts} tentativas falharam");
            CorreiosAutomation.Utils.RunLog.Write($"[Captcha] Todas as {maxAttempts} tentativas falharam");
            return false;
        }
    }
}
