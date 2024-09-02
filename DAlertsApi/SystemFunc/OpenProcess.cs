using DAlertsApi.Logger;
using System;
using System.Diagnostics;

namespace DAlertsApi.SystemFunc
{
    public static class OpenProcess
    {
        private static readonly Dictionary<int, Process> processes = new();

        public static int Open(string url, ILogger? logger = null)
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(url) { UseShellExecute = true }; 
                Process process = Process.Start(processInfo); 
                processes.Add(process.Id, process);
                logger?.Log($"Process was starder. ID:{process.Id}, SessionID:{process.SessionId}", LogLevel.Info);
                return process.Id;
            }
            catch (Exception ex)
            {
                logger?.Log("Ошибка при открытии URL: " + ex.Message, LogLevel.Error);
                return -1;
            }
        }

        public static void Close(int processId, ILogger? logger = null)
        {
            if (processes.ContainsKey(processId))
            {
                processes[processId].Close();
                processes.Remove(processId);
                logger?.Log($"Process was killed. ID:{processId}", LogLevel.Info);
            }
            else
            {
                logger?.Log($"Process with ID:{processId} not found", LogLevel.Warning);
            }
        }
    }
}
