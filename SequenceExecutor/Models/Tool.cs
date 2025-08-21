using System;
using System.Diagnostics;

namespace SequenceExecutor.Models
{
    public class Tool
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Options { get; set; } = null!;

        public void Execute()
        {
            switch (Type)
            {
                case "exe":
                    var target = Options?.Trim();
                    if (string.IsNullOrWhiteSpace(target))
                        throw new InvalidOperationException("Не указан Options");
                    LaunchExe(target);
                    break;
                default:
                    throw new NotSupportedException("Пока поддерживаются только инструменты типа exe");
            }
        }

        private static void LaunchExe(string target)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = target,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                throw new Exception($"Не удалось запустить {target}", ex);
            }
        }

    }
}
