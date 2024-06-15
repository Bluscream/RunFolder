using System.Diagnostics;

namespace RunFolder;

class Program {
    static void Main(string[] args) {
        string assemblyName = GetAssemblyName();
        ExecuteFilesInSubfolder(assemblyName);
    }

    internal static string? GetAssemblyName() => Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);

    static void ExecuteFilesInSubfolder(string subFolderPath) {
        if (!string.IsNullOrEmpty(subFolderPath) && Directory.Exists(subFolderPath)) {
            var files = Directory.GetFiles(subFolderPath);
            Log($"Executing {files.Length} files in {subFolderPath}");
            foreach (var filePath in files) {
                if (filePath.ToLowerInvariant().EndsWith(".disabled")) {
                    Log($"Skipping {filePath}");
                    continue;
                } else {
                    Log($"Executing {filePath}");
                    try {
                        Process.Start(new ProcessStartInfo(filePath) {
                            UseShellExecute = true,
                            CreateNoWindow = true, // Prevents a window from appearing
                            WindowStyle = ProcessWindowStyle.Hidden // Hides the window
                        });
                        Log($"Executed {filePath}");
                    } catch (Exception ex) {
                        Log($"ERROR: Failed to execute {filePath}: {ex.Message}");
                    }
                }
            }
        } else {
            Log($"The specified subfolder {subFolderPath} could not be found or is invalid.");
            Environment.Exit(1); //throw new InvalidOperationException($"Subfolder {assemblyName} does not exist.");
        }
    }
    static void Log(object message) {
        var now = DateTime.Now;
        Console.WriteLine($"[{now}] {message.ToString()}");
    }
}
