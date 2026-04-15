using TechTalk.SpecFlow;
using CorreiosAutomation.Drivers;

namespace CorreiosAutomation.Hooks
{
    [Binding]
    public class Hooks
    {
        [BeforeScenario]
        public static void BeforeScenario()
        {
            // Garante que o driver seja iniciado antes de cada cenário
            WebDriverManager.GetDriver();
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            // Fecha o browser após cada cenário
            WebDriverManager.QuitDriver();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Garantia de fechamento após todos os testes
            WebDriverManager.QuitDriver();
        }
    }
}