using System.Diagnostics;
using System.Reflection;

namespace RunFolder;

class Program {
    static void Main(string[] args) {
        string assemblyName = GetAssemblyName();
        if (Directory.Exists(assemblyName)) {
            ExecuteFilesInSubfolder(assemblyName);
        } else {
            Console.WriteLine($"Subfolder {assemblyName} does not exist.");
            Environment.Exit(1); //throw new InvalidOperationException($"Subfolder {assemblyName} does not exist.");
        }
    }

    internal static string? GetAssemblyName() => Assembly.GetExecutingAssembly().GetName().Name;

    static void ExecuteFilesInSubfolder(string subFolderPath) {
        if (!string.IsNullOrEmpty(subFolderPath) && Directory.Exists(subFolderPath)) {
            foreach (var filePath in Directory.GetFiles(subFolderPath)) {
                try {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                } catch (Exception ex) {
                    Console.WriteLine($"Failed to execute {filePath}: {ex.Message}");
                }
            }
        } else {
            Console.WriteLine("The specified subfolder path could not be found or is invalid.");
        }
    }
}
