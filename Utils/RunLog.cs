using System;
using System.IO;
using System.Text;

namespace CorreiosAutomation.Utils
{
    public static class RunLog
    {
        private static readonly object _lock = new object();

        // Escreve o log em um local previsível:
        // 1) Tentativa de raiz do projeto (base directory subindo 3 níveis) - útil ao executar via dotnet test
        // 2) Se falhar, usa Directory.GetCurrentDirectory()
        private static string GetLogPath()
        {
            try
            {
                var baseDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
                var projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
                var candidate = Path.Combine(projectRoot, "test_run.log");
                return candidate;
            }
            catch
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "test_run.log");
            }
        }

        public static void Write(string message)
        {
            try
            {
                var path = GetLogPath();
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}{Environment.NewLine}";
                lock (_lock)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path) ?? Directory.GetCurrentDirectory());
                    File.AppendAllText(path, line, Encoding.UTF8);
                }
            }
            catch
            {
                // não falha em caso de erro de log
            }
        }
    }
}
