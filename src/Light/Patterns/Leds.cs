using CitizenFX.Core;
using CitizenFX.Core.Native;
using ELS.configuration;
using ELS.NUI;
using System;
using System.Collections.Generic;

namespace ELS.Light.Patterns
{
    public class Leds
    {
        /// <summary>
        /// List of Patterns More to be added
        /// </summary>
        internal static uint[] Patterns = {
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
        internal static string[] StringPatterns =
        {
            "0101010100000000",
            Reverse("0101010100000000"),
            "0101000001010000",
            Reverse("0101000001010000"),
            "01010101010000000000",
            Reverse("01010101010000000000"),
            "0101010101010101",
            "1010101010101010",
            "0011001100110011",
            "1100110011001100",
            "1110111011101110",
            "0111011101110111",
            "0001000100010001",
            "1000100010001000",
            "0100010001000100",
            "0110011001100110",
            "1011001100110011",
            "1001100110011001",
            "1000000110000001",
            "0111111001111110",
            "0011111100111111",
            "1110001110001110",
            "0001110001110001",
            "1010000010100000",
            "0101000001010000",
            "0000000011111111",
            Reverse("0000000011111111"),
            "0000111111110000"
        };

        internal static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        internal static string SweepPattern()
        {
            return StringPatterns[33] + Reverse(StringPatterns[33]);
        }

        internal static string RandomPattern()
        {
            Random rnd = new Random(StringPatterns.Length);
            return StringPatterns[rnd.Next() - 1];
        }

        internal static void SetDefaultPattern(Extras extras)
        {
            extras.PRML[1].PatternNum = 4;
            
            extras.PRML[2].PatternNum = 5;

            extras.PRML[3].PatternNum = 4;

            extras.PRML[4].PatternNum = 5;
            ElsUiPanel.SetUiPatternNumber(0, "PRML");

            extras.WRNL[5].PatternNum = 3;
            extras.WRNL[5].Delay = 500;
            extras.WRNL[6].PatternNum = 4;
            extras.WRNL[6].Delay = 500;
            ElsUiPanel.SetUiPatternNumber(0, "WRNL");

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

        internal static async void RunLightPattern(Vehicle vehicle, int extra, uint upatt, string color, int delay)
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
