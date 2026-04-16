using System;
using System.Diagnostics;
using System.Threading;
using TechTalk.SpecFlow;
using CorreiosAutomation.Drivers;
using NUnit.Framework;

namespace CorreiosAutomation.Hooks
{
    [Binding]
    public class TimeoutHooks
    {
        private static Timer _timeoutTimer;
        private static Stopwatch _stopwatch;
        private static int _scenarioTimeoutSeconds = 10; // 30 segundos por cenário (reduzido de 60)

        [BeforeScenario]
        public static void BeforeScenarioWithTimeout()
        {
            _stopwatch = Stopwatch.StartNew();

            // Inicia timer que verificará timeout
            _timeoutTimer = new Timer(CheckTimeout, null, _scenarioTimeoutSeconds * 1000, Timeout.Infinite);
        }

        [AfterScenario]
        public static void AfterScenarioWithTimeout()
        {
            _stopwatch?.Stop();
            _timeoutTimer?.Dispose();

            // Garante que o driver seja finalizado após cada cenário para evitar processos pendentes
            try
            {
                Drivers.WebDriverManager.QuitDriver();
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Erro ao finalizar WebDriver após cenário: {ex.Message}");
            }
        }

        private static void CheckTimeout(object state)
        {
            if (_stopwatch != null && _stopwatch.Elapsed.TotalSeconds >= _scenarioTimeoutSeconds)
            {
                TestContext.Progress.WriteLine($"⚠️ Timeout atingido: {_scenarioTimeoutSeconds} segundos");
                CorreiosAutomation.Utils.RunLog.Write($"⚠️ Timeout atingido: {_scenarioTimeoutSeconds} segundos");

                // Tenta fechar o browser graciosamente
                try
                {
                    WebDriverManager.QuitDriver();
                    CorreiosAutomation.Utils.RunLog.Write("WebDriver finalizado no CheckTimeout");
                }
                catch (Exception ex)
                {
                    TestContext.Progress.WriteLine($"Erro ao fechar driver: {ex.Message}");
                    CorreiosAutomation.Utils.RunLog.Write($"Erro ao fechar driver: {ex.Message}");
                }

                // Força o término do processo de teste
                Environment.Exit(1);
            }
        }
    }
}