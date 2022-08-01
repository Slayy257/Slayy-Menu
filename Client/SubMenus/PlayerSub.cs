using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace Client.SubMenus
{
    public class PlayerSub : BaseScript
    {
        private static bool godmode;
        private static bool invisible;
        private static bool superjump;
        private static UIMenuItem suicide;

        private void updateFunc()
        {
            Tick += async () =>
            {
                Game.PlayerPed.IsInvincible = godmode;
                Game.PlayerPed.IsVisible = !invisible;

                if (superjump) Game.Player.SetSuperJumpThisFrame();
            };
        }

        public PlayerSub()
        {
            updateFunc();
        }

        public static void Sub(UIMenu menu)
        {
            var sub = MainMenu.menuPool.AddSubMenu(menu, "Player Options");
            sub.MouseControlsEnabled = false;
            sub.MouseEdgeEnabled = false;
            sub.MouseWheelControlEnabled = false;

            var godmodeCheck = new UIMenuCheckboxItem("Godmode", godmode);
            var superjumpCheck = new UIMenuCheckboxItem("Super Jump", superjump);
            var invisibleCheck = new UIMenuCheckboxItem("Invisible", invisible);
            suicide = new UIMenuItem("Suicide");
            sub.AddItem(godmodeCheck);
            sub.AddItem(superjumpCheck);
            sub.AddItem(invisibleCheck);
            sub.AddItem(suicide);


            sub.OnItemSelect += (sender, item, index) =>
            {
                if (item == suicide)
                {
                    Game.PlayerPed.Health = 0;
                };
            };
            sub.OnCheckboxChange += (sender, item, checked_) =>
            {
                if (item == godmodeCheck) godmode = checked_;
                if (item == superjumpCheck) superjump = checked_;
                if (item == invisibleCheck) invisible = checked_;
            };
        }
    }
}
