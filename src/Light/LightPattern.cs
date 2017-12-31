using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using ELS.NUI;
using System;


namespace ELS.Light
{
    public class LightPattern
    {
        /// <summary>
        /// List of Patterns More to be added
        /// </summary>
        public static uint[] Patterns = {
            858993459,
            859002155,
            2863311530,
            1431655765,
            3435965013,
            3435973836,
            3817748650,
            477218645,
            2694881440,
            168430090,
            84215045,
            1347440720
        };

        /// <summary>
        /// List of patterns in binary string format
        /// </summary>
        public static string[] StringPatterns =
        {
            "0101010101010101",//0
            "1010101010101010",//1
            "0011001100110011",//2
            "1100110011001100",//3
            "1110111011101110",//4
            "0111011101110111",//5
            "0001000100010001",//6
            "1000100010001000",//7
            "0100010001000100",//8
            "0110011001100110",//9
            "1011001100110011",//10
            "1001100110011001",//11
            "1000000110000001",//12
            "0111111001111110",//13
            "0011111100111111",//14
            "1110001110001110",//15
            "0001110001110001",//16
            "1010000010100000",//17
            "0101000001010000",//18
            "1000011111111111",//19
            "1100001111111111",//20
            "1111000011111111",//21
            "1111100001111111",//22
            "1111110000111111",//23
            "1111111000011111",//24
            "1111111100001111",//25
            "1111111110000111",//26
            "1111111111000011",//27
            "1111111111100001",//28
            "1111111111110000",//29
            "0111111111111000",//30
            "0011111111111100",//31
            "0001111111111110",//32
            "0000111111111111",//33
        };

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string SweepPattern()
        {
            return StringPatterns[33] + Reverse(StringPatterns[33]);
        }

        public static string RandomPattern()
        {
            Random rnd = new Random(StringPatterns.Length);
            return StringPatterns[rnd.Next() - 1];
        }

        internal static async void RunLightPattern(Vehicle vehicle, int extra, string patt, string color, int delay)
        {
            string light = $"#extra{extra}";
            do
            {
                foreach (char c in patt.ToCharArray())
                {
                    if (!ElsUiPanel._runPattern)
                    {
                        break;
                    }
                    if (c.Equals('0'))
                    {
                        ElsUiPanel.SendLightData(false, light, "");
                        vehicle.ToggleExtra(extra, false);
                    }
                    else
                    {
                        ElsUiPanel.SendLightData(true, light, color);
                        vehicle.ToggleExtra(extra, true);
                    }
                    await ELS.Delay(delay);
                    //ElsUiPanel.SendLightData(false, light, "");
                }
            } while (ElsUiPanel._runPattern);
            ElsUiPanel.SendLightData(false, light, "");
            vehicle.ToggleExtra(extra, false);
        }

        public static async void RunLightPattern(Vehicle vehicle, int extra, uint upatt, string color, int delay)
        {
            string patt = Convert.ToString(upatt, 2);
            string light = $"#extra{extra}";
            do
            {
                foreach (char c in patt.ToCharArray())
                {
                    if (!ElsUiPanel._runPattern)
                    {
                        break;
                    }
                    if (c.Equals('0'))
                    {
                        ElsUiPanel.SendLightData(false, light, "");
                        vehicle.ToggleExtra(extra, false);
                    }
                    else
                    {
                        ElsUiPanel.SendLightData(true, light, color);
                        vehicle.ToggleExtra(extra, true);
                    }
                    await ELS.Delay(delay);
                }
            } while (ElsUiPanel._runPattern);
            ElsUiPanel.SendLightData(false, light, "");
            vehicle.ToggleExtra(extra, false);
        }
    }
}
