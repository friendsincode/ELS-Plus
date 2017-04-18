using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ELS.panel
{
    class test
    {
        uint[] somestruct = new uint[]
       {
            204,
            204,
            204,
            0,
            0,
            0,
            11,
            11,
            11,
            7,
            7,
            7,
            16,
            16,
            16,
            10,
            10,
            10,
            156,
            161,
            164,
            165,
            23,
            21,
            32,
            33,
            33,
            165,
            23,
            21,
            179,
            150,
            42,
            13,
            84,
            134,
            215,
            135,
            110,
            170,
            75,
            50,
            150,
            218,
            126,
            50,
            170,
            55,
            115,
            180,
            220,
            50,
            130,
            175,
            214,
            115,
            109,
            140,
            30,
            30,
            205,
            205,
            120,
            169,
            122,
            52
        };

        private int infoPanelButtonColor = 19;

        public test()
        {

        }

        public void draw()
        {
            drawScaledRect(160.0f, 1722.5f, 1000.0f, 395.0f, 1, 255);
            // v3 = 2;        
            drawScaledRect(158.0f, 1722.5f, 1000.0f, 393.0f, 2, 255);
            drawScaledRect(150.0f, 1722.5f, 1000.0f, 385.0f, 3, 255);
            drawScaledRect(146.0f, 1722.5f, 1000.0f, 381.0f, 4, 255);
            drawScaledRect(25.0f, 1722.5f, 941.5f, 377.0f, 5, 255);
            //v16 = map_entity_data_index(&g_entityDataMap, &a2a);
            //v155 = 255;
            drawScaledRect(21.0f, 1722.5f, 941.5f, 373.0f, 11, 255);
            drawScaledRect(115.0f, 1722.5f, 1013.5f, 377.0f, 5, 255);
            drawScaledRect(111.0f, 1722.5f, 1013.5f, 373.0f, 4, 255);
            drawScaledRect(42.0f, 1560.0f, 982.0f, 42.0f, 1, 255);
            drawScaledRect(42.0f, 1606.0f, 982.0f, 42.0f, 1, 255);
            drawScaledRect(42.0f, 1652.0f, 982.0f, 42.0f, 1, 255);
            drawScaledRect(42.0f, 1560.0f, 1028.0f, 42.0f, 1, 255);
            drawScaledRect(42.0f, 1606.0f, 1028.0f, 42.0f, 1, 255);
            drawScaledRect(42.0f, 1652.0f, 1028.0f, 42.0f, 1, 255);
            drawScaledRect(74.0f, 1722.5f, 1005.0f, 77.0f, 1, 255);
            drawScaledRect(42.0f, 1793.0f, 982.0f, 42.0f, 1, 255);
            drawScaledRect(42.0f, 1793.0f, 1028.0f, 42.0f, 1, 255);
            drawScaledRect(42.0f, 1839.0f, 1028.0f, 42.0f, 1, 255);
            drawScaledRect(42.0f, 1885.0f, 1028.0f, 42.0f, 1, 255);
            var v154 = 6;
            if (false) //if ltgstage is 3
            {
                drawScaledRect(42.0f, 1862.0f, 982.0f, 88.0f, 1, 255);
                drawScaledRect(38.0f, 1862.0f, 982.0f, 84.0f, 17, 255);
                v154 = 16; //or 16
                drawScaledRect(36.0f, 1862.0f, 982.0f, 82.0f, v154, 255);
            }
            else
            {//try 15 192
                drawScaledRect(36.0f, 1862.0f, 982.0f, 82.0f, v154, 255);
                drawScaledRect(13.0f, 1609.0f, 941.5f, 46.0f, infoPanelButtonColor, 255);
                drawScaledRect(13.0f, 1657.0f, 941.5f, 46.0f, infoPanelButtonColor, 255);
                drawScaledRect(13.0f, 1705.0f, 941.5f, 46.0f, infoPanelButtonColor, 255);
                drawScaledRect(11.0f, 1609.0f, 941.5f, 44.0f, 6, 255);
                drawScaledRect(13.0f, 1657.0f, 941.5f, 46.0f, 15, 255);
                drawScaledRect(13.0f, 1705.0f, 941.5f, 46.0f, infoPanelButtonColor, 255);
                drawScaledRect(11.0f, 1609.0f, 941.5f, 44.0f, 15, 255);
                drawScaledRect(11.0f, 1657.0f, 941.5f, 44.0f, 17, 255);//inner top of labels
                drawScaledRect(11.0f, 1705.0f, 941.5f, 44.0f, 15, 255);
            }
        }

        private void drawScaledRect(float height, float xposf, float yposf, float width, int a5,
            uint alphaChannel)
        {
            var resolution = CitizenFX.Core.UI.Screen.Resolution;
            DRAW_RECT(
                3 * (int)width,
                (xposf * 0.66666669f) * (1.0f / resolution.Width),
                (yposf * 0.66666669f) * (1.0f / resolution.Height),
                (width * 0.66666669f) * (1.0f / resolution.Width),
                (height * 0.66666669f) * (1.0f / resolution.Height),
                somestruct[3 * a5],
                somestruct[3 * a5 + 1],
                somestruct[3 * a5 + 2],
                alphaChannel);
            //throw new NotImplementedException();
        }

        private void DRAW_RECT(int unussed, float xpos, float ypos, float width, float height, uint r, uint g, uint b, uint a)
        {
            //CitizenFX.Core.Native.Function.Call(Hash.DRAW_RECT, 0.5f, 0f, 0.5f, 0.5f, 128, 128, 128, 255);
            CitizenFX.Core.Native.Function.Call(Hash.DRAW_RECT, xpos, ypos, width, height, r, g, b, a);
        }
    }
}
