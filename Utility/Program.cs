// This project is protected!
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Utility
{
    class Program
    {

        delegate void passedMethod();
        static Dictionary<string, passedMethod> functionsToAccess = new Dictionary<string, passedMethod>
        {
            {"checkAdmin", () => { CheckAdmin();}},
            {"getMac", () => { GetMacAddress();}},
            {"copyright", () => { Copyright();}},
            {"getProcs", () => { GetProcs();}},
            {"killProc", () => { KillProc();}},
            {"help",delegate {
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
            Console.Clear();
            Main(null);
        }
        public static void CheckAdmin()
        {
            IsUserAdministrator();
            if (IsUserAdministrator() == true)
                Console.WriteLine("Result: True");
            else Console.WriteLine("Result: False");
            Main(null);
        }

        public static void GetMacAddress()
        {
            Console.WriteLine("Your mac address: " + MacAddress());
            Main(null);
        }


        private static string MacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
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
            foreach (var data in procs)
            {
                Console.WriteLine(data.Id + "   " + data.ProcessName);
            }
            Main(null);
        }

        public static void KillProc()
        {
            Process[] procs = Process.GetProcesses();
            Console.Write("Please enter a process to kill: ");
            string inputToKill = Console.ReadLine();
            foreach (var data in procs)
            {
                if (inputToKill != data.ProcessName) continue;
                data.Kill();
            }

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
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }
    }
}
