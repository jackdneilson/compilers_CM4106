using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace lab_2 {
    public class QuoteReader {
        public QuoteReader() {
            RunTeleprompter().Wait();
        }

        private async Task ShowTeleprompter(TeleprompterConfig config) {
            IEnumerable<string> words = ReadFrom("sampleQuotes.txt");
            foreach (var line in words) {
                Console.Write(line);
                if (!string.IsNullOrWhiteSpace(line)) {
                    await Task.Delay(config.DelayInMilliseconds);
                }
            }
            config.SetDone();
        }

        private async Task GetInput(TeleprompterConfig config) {
            Action work = () => {
                do {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.KeyChar == '>') {
                        config.UpdateDelay(-10);
                    }
                    else if (key.KeyChar == '<') {
                        config.UpdateDelay(10);
                    }
                } while (!config.Done);
            };
            await Task.Run(work);
        }
        
        IEnumerable<string> ReadFrom(string file) {
            string line;
            using (StreamReader reader = File.OpenText(file)) {
                while ((line = reader.ReadLine()) != null) {
                    String[] words = line.Split(' ');
                    foreach (string word in words) {
                        yield return word + " ";
                        int lineLength = word.Length + 1;
                        if (lineLength > 70) {
                            yield return Environment.NewLine;
                            lineLength = 0;
                        }
                    }
                    yield return Environment.NewLine;
                }
            }
        }
        
        private async Task RunTeleprompter() {
            var config = new TeleprompterConfig();
            var displayTask = ShowTeleprompter(config);
            var speedTask = GetInput(config);
            await Task.WhenAny(displayTask, speedTask);
        }
    }    
}