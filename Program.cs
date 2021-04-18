using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Management.ManagementObjectCollection;

namespace LinkRedirector
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            try
            {
                if (args.Length > 0)
                {
                    string url = args[0];
                    string parentProcessName = args.Length > 1 ? args[1] : null;
                    Settings settings = LoadSettings();

                    foreach (Redirect redirect in settings.Redirects)
                    {
                        if (redirect.ProcessNamePattern != null)
                        {
                            if (parentProcessName == null)
                            {
                                parentProcessName = GetParentProcessName();
                            }

                            if (!Regex.IsMatch(parentProcessName, redirect.ProcessNamePattern, RegexOptions.IgnoreCase))
                            {
                                continue;
                            }
                        }

                        Regex regex = new Regex(redirect.Pattern, RegexOptions.IgnoreCase);
                        if (regex.IsMatch(url))
                        {
                            if (redirect.RegistryClass != null)
                            {
                                string[] data = GetFromRegistryClass(redirect.RegistryClass);
                                StartTargetProgram(data[0], data[1].Replace("%1", regex.Replace(url, redirect.Replacement)));
                                return;
                            }

                            StartTargetProgram(redirect.TargetProgram, regex.Replace(url, redirect.Replacement));
                            return;
                        }
                    }

                    if (settings.Default.RegistryClass != null)
                    {
                        string[] data = GetFromRegistryClass(settings.Default.RegistryClass);
                        StartTargetProgram(data[0], data[1].Replace("%1", url));
                        return;
                    }

                    StartTargetProgram(settings.Default.TargetProgram, string.Format(settings.Default.Arguments, url));
                }
            }
            catch (Exception e)
            {
                File.WriteAllText("last_error.txt", e.ToString());
            }
        }

        private static string GetParentProcessName()
        {
            using (Process process = Process.GetCurrentProcess())
            {
                string query = string.Format("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {0}", process.Id);
                ManagementObjectSearcher search = new ManagementObjectSearcher("root\\CIMV2", query);

                ManagementObjectEnumerator enumerator = search.Get().GetEnumerator();
                enumerator.MoveNext();
                ManagementBaseObject result = enumerator.Current;

                try
                {
                    using (Process parentProcess = Process.GetProcessById((int)(uint)result["ParentProcessId"]))
                    {
                        return parentProcess.ProcessName;
                    }
                }
                catch (ArgumentException e)
                {
                    return "#unknown#";
                }
            }
        }

        private static Settings LoadSettings()
        {
            string filePath = "settings.json";
            return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filePath));
        }

        private static string[] GetFromRegistryClass(string registryClass)
        {
            using (RegistryKey commandKey = Registry.ClassesRoot.OpenSubKey(registryClass + @"\shell\open\command", false))
            {
                object command = commandKey?.GetValue(string.Empty);
                if (command != null)
                {
                    Regex regex = new Regex("^(\"[^\"]+\"|[^ ]+) (.*)$");
                    Match match = regex.Match((string)command);
                    if (match.Success)
                    {
                        return new string[] { match.Groups[1].Value, match.Groups[2].Value };
                    }
                }
            }
            throw new ArgumentException(@"Can't read default value from: HKEY_CLASSES_ROOT\" + registryClass + @"\shell\open\command");
        }

        private static void StartTargetProgram(string targetProgram, string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = targetProgram;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.EnvironmentVariables["PATH"] = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + ";" + Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.StartInfo.UseShellExecute = false;
            process.Start();
        }
    }

    public class Settings
    {
        public Default Default { get; set; }
        public List<Redirect> Redirects { get; set; }
    }

    public class Default
    {
        public string RegistryClass { get; set; }
        public string TargetProgram { get; set; }
        public string Arguments { get; set; }
    }

    public class Redirect
    {
        public string ProcessNamePattern { get; set; }
        public string Pattern { get; set; }
        public string Replacement { get; set; }
        public string RegistryClass { get; set; }
        public string TargetProgram { get; set; }
    }
}
