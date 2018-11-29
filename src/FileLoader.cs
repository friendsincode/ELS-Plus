/*
    ELS FiveM - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;

namespace ELS
{
    class FileLoader
    {
        ELS _baseScript;
        internal delegate void SettingsLoadedHandler(SettingsType.Type type, String data);
        internal static event SettingsLoadedHandler OnSettingsLoaded;
        public FileLoader(ELS h)
        {
            _baseScript = h;
        }
        internal void RunLoader(String scriptName)
        {
            //LoadFilesPromScript(scriptName);
            OnSettingsLoaded?.Invoke(SettingsType.Type.GLOBAL, API.LoadResourceFile(scriptName, "ELS.ini"));
        }

        private static void LoadFilesPromScript(string name)
        {
            int num = Function.Call<int>(Hash.GET_NUM_RESOURCE_METADATA, name, "file");
            Utils.DebugWriteLine($"{num} files for {name}");
            for (int i = 0; i < num; i++)
            {
                var filename = Function.Call<string>(Hash.GET_RESOURCE_METADATA, name, "file", i);

                var data = Function.Call<string>(Hash.LOAD_RESOURCE_FILE, name, filename);


                Utils.DebugWriteLine($"Checking {filename}");
                if (filename.Equals("ELS.ini"))
                {
                    if (configuration.ElsConfiguration.isValidData(data))
                    {
                        OnSettingsLoaded?.Invoke(SettingsType.Type.GLOBAL, data);
                    }
                }
            }
        }
        internal void UnLoadFilesFromScript(string name)
        {
            VCF.unload(name);
        }
    }
}
