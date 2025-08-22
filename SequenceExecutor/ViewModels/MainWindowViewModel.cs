using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SequenceExecutor.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Text.Json;
using System.Threading.Tasks;

namespace SequenceExecutor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        [Reactive] public bool IsTopMost { get; set; }

        public ObservableCollection<Sequence> ActiveSequences { get; } = [];

        public ReactiveCommand<Sequence, Unit> RunSequenceCommand { get; }

        public MainWindowViewModel()
        {
            RunSequenceCommand = ReactiveCommand.CreateFromTask<Sequence>(RunSequenceAsync);
            LoadSequences();
        }

        private void LoadSequences()
        {
            Root? root;

            try
            {
                var path = Path.Combine(AppContext.BaseDirectory, "sequences.json");
                var json = File.ReadAllText(path);
                root = JsonSerializer.Deserialize<Root>(json, jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка чтения JSON: {ex.Message}");
                return;
            }

            ActiveSequences.Clear();

            if (root != null)
                foreach (var s in root.Sequences)
                    if (s.Active)
                        ActiveSequences.Add(s);
        }

        private async Task RunSequenceAsync(Sequence seq)
        {
            try
            {
                seq.Run();
            }
            catch (Exception ex)
            {
                await ShowInfoAsync(ex.Message);
            }
        }

        private static async Task ShowInfoAsync(string text)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Информация", text, ButtonEnum.Ok, Icon.Info);
            await box.ShowAsync();
        }

    }
}
