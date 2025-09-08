namespace ProcessMonitor.Logic;

using System;
using System.Diagnostics;
using System.Management;

public class ProcessConsoleMonitor
{
    private static ManagementEventWatcher creationWatcher;
    private static ManagementEventWatcher deletionWatcher;

    public static void Start()
    {
        Console.WriteLine("=== Monitor de Processos Iniciado ===");

        creationWatcher = new ManagementEventWatcher(
            "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");
        deletionWatcher = new ManagementEventWatcher(
            "SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'");

        creationWatcher.EventArrived += ProcessStarted;
        deletionWatcher.EventArrived += ProcessEnded;

        creationWatcher.Start();
        deletionWatcher.Start();

        Console.WriteLine("Aperte ENTER para parar o monitor...");
        Console.ReadLine();

        Stop();
    }

    private static void Stop()
    {
        creationWatcher.Stop();
        deletionWatcher.Stop();
        creationWatcher.Dispose();
        deletionWatcher.Dispose();
        Console.WriteLine("=== Monitor Finalizado ===");
    }

    private static void ProcessStarted(object sender, EventArrivedEventArgs e)
    {
        var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
        int pid = Convert.ToInt32(instance["ProcessId"]);
        string name = instance["Name"].ToString();

        string user = GetProcessOwner(pid);
        string runningTime = GetRunningTime(pid);

        Console.WriteLine($"[INICIADO] Nome: {name}, PID: {pid}, Usuário: {user}, Tempo rodando: {runningTime}");
    }

    private static void ProcessEnded(object sender, EventArrivedEventArgs e)
    {
        var instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
        int pid = Convert.ToInt32(instance["ProcessId"]);
        string name = instance["Name"].ToString();

        Console.WriteLine($"[ENCERRADO] Nome: {name}, PID: {pid}");
    }

    private static string GetProcessOwner(int processId)
    {
        try
        {
            string query = $"SELECT * FROM Win32_Process WHERE ProcessId = {processId}";
            using var searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject obj in searcher.Get())
            {
                string[] ownerInfo = new string[2];
                obj.InvokeMethod("GetOwner", ownerInfo);
                return $"{ownerInfo[1]}\\{ownerInfo[0]}"; // DOMAIN\Username
            }
        }
        catch { }
        return "Desconhecido";
    }

    private static string GetRunningTime(int processId)
    {
        try
        {
            var process = Process.GetProcessById(processId);
            var startTime = process.StartTime;
            var duration = DateTime.Now - startTime;
            return $"{duration:hh\\:mm\\:ss}";
        }
        catch
        {
            return "N/A";
        }
    }

    
}
