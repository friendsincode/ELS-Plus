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
        ///
        internal static string[] StringPatterns =
        {
            "0101010100000000",
            "0101000001010000",
            "01010101010000000000",
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
            "0000111111110000"
        };
    }
}
