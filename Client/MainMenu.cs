using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;

namespace Client
{
    public class MainMenu : BaseScript
    {
        public static MenuPool menuPool;
        public static UIMenu mainMenu;
        public static NativeUI.Sprite m_banner_sprite;

        public MainMenu()
        {
            // init
            menuPool = new MenuPool();
            mainMenu = new UIMenu("Slayy Menu", "Da best menu", true);
            menuPool.Add(mainMenu);

            // subs
            SubMenus.PlayerSub.Sub(mainMenu);
            SubMenus.WeaponSub.Sub(mainMenu);

            // params
            mainMenu.MouseControlsEnabled = false;
            mainMenu.MouseEdgeEnabled = false;
            mainMenu.MouseWheelControlEnabled = false;
            menuPool.RefreshIndex();

            // menu view update
            Tick += async () =>
            {
                menuPool.ProcessMenus();
                if (API.IsControlJustPressed(0, (int)Control.SelectCharacterMichael) && !menuPool.IsAnyMenuOpen())
                    mainMenu.Visible = !mainMenu.Visible;
            };
        }
    }
}
