// This project is protected!
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace Utility
{
    internal class Program
    {

        delegate void PassedMethod();

        private static readonly Dictionary<string, PassedMethod> functionsToAccess = new Dictionary<string, PassedMethod>
        {
            {"checkAdmin", CheckAdmin},
            {"getMac", GetMacAddress},
            {"copyright", Copyright},
            {"getProcs", GetProcs},
            {"killProc", KillProc},
            {"getIP", GetExternalIp},
            {"clear", ClearConsole},
            {"bruteForce", delegate
            {
                Console.Write("Please enter a string to bruteforce: ");
                var input = Console.ReadLine();
                BruteForce(input);
            }},
            {"help",delegate {
               Console.Clear();
                Console.WriteLine("Available commands: ");
                foreach (var data in functionsToAccess)
                {
                    Console.WriteLine(data.Key);
                }
                Main(null);
            }}
        };
        public static void Main(string[] args)
        {



            Console.Write("Please enter a command: ");
            var inputUser = Console.ReadLine();

            Console.WriteLine("Trying to execute: " + inputUser);

            foreach (var data in functionsToAccess)
            {
                if (data.Key == inputUser)
                {
                    data.Value();
                }
            }
            Main(null);
        }
        public static void CheckAdmin()
        {
            IsUserAdministrator();
            Console.WriteLine(IsUserAdministrator() ? "Result: True" : "Result: False");
            Main(null);
        }

        public static void GetMacAddress()
        {
            Console.WriteLine("Your mac address: " + MacAddress());
            Main(null);
        }


        private static string MacAddress()
        {
            const int minMacAddrLength = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= minMacAddrLength)
                {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }

        public static void GetProcs()
        {
            Process[] procs = Process.GetProcesses();
            Dictionary<string, Process> processes = new Dictionary<string, Process>();


            procs.ToList().ForEach(delegate (Process process)
            {
                if (!processes.ContainsKey(process.ProcessName))
                    processes.Add(process.ProcessName, process);
            });



            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var data in processes.OrderBy(x => x.Key))
            {
                Console.WriteLine(data.Value.Id.ToString().PadRight(10, ' ') + data.Key);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Main(null);
        }

        public static void KillProc()
        {
            Process[] procs = Process.GetProcesses();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Please enter a process to kill: ");
            Console.ForegroundColor = ConsoleColor.White;
            string inputToKill = Console.ReadLine();
            foreach (var data in procs)
            {
                if (inputToKill != data.ProcessName) continue;
                data.Kill();
            }

        }

        public static void ClearConsole()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Copyright();
            Main(null);

        }


        public static void Copyright()
        {
            Console.Write("Copyright Jens Ahlén 2017, All rights reserved\r\n" +
                          "This project is protected!\r\n" +
                          "Unauthorized copying of this file, via any medium is strictly prohibited\r\n" +
                          "Proprietary and confidential\r\n");
            Main(null);
        }


        public static bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {

                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {
                isAdmin = false;
            }
            catch (Exception)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        public static void GetExternalIp()
        {
            WebClient request = new WebClient();
            try
            {
                Console.WriteLine(request.DownloadString("https://www.tinygreatmob.com/API/Public/GetIP.php"));
            }
            catch
            {
                Console.WriteLine("There was an error connecting to the internet.\r\n Please check your connection!");
            }

            Main(null);
        }

        public static void BruteForce(string pass)
        {
           
            BruteForce brute = new BruteForce(pass);
            brute.MainBrute(null);
            Console.WriteLine(brute.result);
            Main(null);           
        }
    }
}
