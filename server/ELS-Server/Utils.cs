using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ELS_Server
{
    class Utils
    {
        /// <summary>
        /// Print out a message only if the program is compiled for Debug.
        /// </summary>
        /// <param name="data">Data to print in console</param>
        static internal void DebugWrite(string data)
        {
            if (bool.Parse(API.GetConvar("elsplus_debug", "false")))
            {
                CitizenFX.Core.Debug.Write($"ELS-Plus: {data}");
            }

        }

        /// <summary>
        /// Print out a message only if the program is compiled for Debug.
        /// </summary>
        /// /// <param name="data">Data to print in console</param>
        /// /// <param name="args">Arugments to be formated into data</param>
        static internal void DebugWriteLine(string data, [Optional]object[] args)
        {
            if (bool.Parse(API.GetConvar("elsplus_debug", "false")))
            {
                if (args != null)
                {
                    CitizenFX.Core.Debug.WriteLine($"ELS-Plus: {data}", args);
                }
                else
                {
                    CitizenFX.Core.Debug.WriteLine($"ELS-Plus: {data}");
                }
            }

        }
        /// <summary>
        /// Print out a message only if the program is compiled for all release types.
        /// </summary>
        /// /// <param name="data">Data to print in console</param>
        static internal void ReleaseWrite(string data)
        {
            CitizenFX.Core.Debug.Write($"ELS-Plus: {data}");
        }

        /// <summary>
        /// Print out a message only if the program is compiled for all release types.
        /// </summary>
        /// <param name="data">Data to print in console</param>
        /// <param name="args">Arugments to be formated into data</param>
        static internal void ReleaseWriteLine(string data, [Optional]object[] args)
        {
            if (args != null)
            {
                CitizenFX.Core.Debug.WriteLine($"ELS-Plus: {data}", args);
            }
            else
            {
                CitizenFX.Core.Debug.WriteLine($"ELS-Plus: {data}");
            }
        }

        /// <summary>
        /// Print out a message only if the program is compiled for all release types.
        /// </summary>
        ///<param name = "ex">Message for exception</param>
        static internal void ThrowException(Exception ex)
        {
            ReleaseWriteLine($"Exception thrown:\n" +
                $"{ex.Message}");
            throw (ex);
        }

    }
}
