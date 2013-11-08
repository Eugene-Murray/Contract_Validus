using System;
using System.Reflection;

namespace Validus.Models
{
    public class Version
    {
        public override string ToString()
        {
            return "Version : " + CurrentVersion();
        }

        /// <summary>
        /// Return the Current Version from the AssemblyInfo.cs file.
        /// </summary>
        public static string CurrentVersion()
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version.ToString();
            }
            catch
            {
                return "?.?.?.?";
            }
        }
    }
}