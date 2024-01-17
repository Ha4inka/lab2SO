
using System.Diagnostics;

namespace _2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ProcessManager.exe <processId>");
                return;
            }

            int processId = int.Parse(args[0]);

            // Отримати і вивести інформацію про всі процеси
            Console.WriteLine("Processes information:");
            PrintProcessesInfo();

            // Відобразити дерево процесів
            Console.WriteLine($"\nProcess tree starting from PID {processId}:");
            DisplayProcessTree(processId);

            // Зупинити вказаний процес та його дочірні процеси
            Console.WriteLine($"\nStopping the process tree starting from PID {processId}...");
            StopProcessTree(processId);

            Console.WriteLine("Process tree stopped.");
        }

        static void PrintProcessesInfo()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = "process get ProcessId,ParentProcessId,Name",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                process.WaitForExit();
            }
        }

        static void DisplayProcessTree(int processId)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = $"process where(ProcessId={processId}) get ProcessId,ParentProcessId,Name",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                process.WaitForExit();

                // Отримати інформацію про батьківський процес
                string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 1) // Перевірка, чи є достатньо рядків
                {
                    string[] values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (values.Length >= 3) // Перевірка, чи є достатньо елементів в рядку
                    {
                        int parentProcessId;
                        if (int.TryParse(values[1],
                                out parentProcessId)) // Використовуйте TryParse для безпечного парсингу
                        {
                            // Вивести батьківський процес в останнє
                            Console.WriteLine($"Parent process: {parentProcessId}");

                            // Рекурсивно викликати DisplayProcessTree для батьківського процесу
                            DisplayProcessTree(parentProcessId);
                        }
                        else
                        {
                            Console.WriteLine("Failed to parse parentProcessId.");
                        }
                    }
                }
            }
        }

        static void StopProcessTree(int processId)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/F /PID {processId}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }
    }
}





