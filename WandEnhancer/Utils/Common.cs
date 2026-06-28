using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace WandEnhancer.Utils
{
    public static class Common
    {
        public static void TryKillProcess(string processName)
        {
            const int maxAttempts = 5;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                var processes = Process.GetProcessesByName(processName);
                try
                {
                    // Nothing (left) to kill - we're done immediately.
                    if (processes.Length == 0)
                    {
                        return;
                    }

                    foreach (var process in processes)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                finally
                {
                    foreach (var process in processes)
                    {
                        process.Dispose();
                    }
                }

                Thread.Sleep(250);
            }

            var remaining = Process.GetProcessesByName(processName);
            try
            {
                if (remaining.Length > 0)
                {
                    throw new Exception("Failed to kill WeMod");
                }
            }
            finally
            {
                foreach (var process in remaining)
                {
                    process.Dispose();
                }
            }
        }

        public static string GetCurrentDir()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(assemblyLocation) ?? throw new InvalidOperationException();
        }
        
        public static string ComputeSha256Hash(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}