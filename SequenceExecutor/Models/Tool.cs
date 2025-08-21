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
                    ExecuteExe(); break;
                default:
                    throw new NotSupportedException("Пока поддерживаются только инструменты типа exe");
            }

            // NOTE: текущая реализация через switch оставлена для простоты.
            // При росте количества типов, выполнение должно быть вынесено из этого класса,
            // чтобы сохранить разделение ответственности (SRP) и открытость к расширению (OCP).
        }

        private void ExecuteExe()
        {
            var target = Options?.Trim();

            if (string.IsNullOrWhiteSpace(target))
                throw new InvalidOperationException("Не указан Options");

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
