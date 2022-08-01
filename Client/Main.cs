using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using NativeUI.PauseMenu;

namespace Client
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["playerSpawned"] += new Action(playerSpawnFunc);
        }

        private static void playerSpawnFunc()
        {
            Screen.DisplayHelpTextThisFrame("Press ~INPUT_SELECT_CHARACTER_MICHAEL~ to open Slayy Menu.");
        }

        private static async Task OnTick()
        {

        }
    }
}
