using System;

namespace SequenceExecutor.Models
{
    public class Sequence
    {
        public string Name { get; set; } = null!;
        public bool Active { get; set; }
        public Tool[] Tools { get; set; } = [];

        public void Run()
        {
            if (Tools is null)
                throw new InvalidOperationException("В последовательности нет инструментов");

            if (Tools.Length != 1)
                throw new NotSupportedException("Пока поддерживаются только цепочки с одним инструментом");

            foreach (var tool in Tools)
                tool.Execute();
        }

    }
}
