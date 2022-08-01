using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using NativeUI;
using System;
using System.Collections.Generic;

namespace Client.SubMenus
{
    class WeaponSub : BaseScript
    {
        private static bool admin_gun;
        private static UIMenuItem get_all_weapons;

        private async void PutEntityInCage(Ped ped)
        {
            Model cageModel = "prop_gold_cont_01";
            cageModel.Request();

            while (!API.HasModelLoaded((uint)API.GetHashKey("prop_gold_cont_01"))) await BaseScript.Delay(100);

            Vector3 playerPos = ped.Position;

            API.ClearPedTasksImmediately(API.GetPlayerPedScriptIndex(API.NetworkGetPlayerIndexFromPed(ped.Handle)));
            Prop cageProp = await World.CreatePropNoOffset(cageModel, playerPos, false);
            ped.Position = playerPos;
            Blip cageBlip = cageProp.AttachBlip();
            cageBlip.Name = "Cage: " + API.GetPlayerName(API.NetworkGetPlayerIndexFromPed(ped.Handle));
            cageBlip.Sprite = BlipSprite.Custody;
        }

        private async void PlayEffectOnWeapon(string assetName, string effectName)
        {
            API.RequestNamedPtfxAsset(assetName);
            while (!API.HasNamedPtfxAssetLoaded(effectName)) await BaseScript.Delay(100);

            Vector3 d1 = new Vector3(0, 0, 0);
            Vector3 d2 = new Vector3(0, 0, 0);

            int weaponHash = API.GetSelectedPedWeapon(Game.Player.Handle);
            var weaponHandle = API.GetCurrentPedWeaponEntityIndex(Game.Player.Handle);

            API.GetModelDimensions((uint)weaponHash, ref d1, ref d2);
            API.UseParticleFxAssetNextCall(assetName);
            API.StartParticleFxNonLoopedOnEntity_2(effectName, weaponHandle, (d1.X - d2.X) / 2.0f + 0.7f, 0f, 0f, 0f, 180f, 0, 1, false, false, false);
        }

        private void updateFunc()
        {
            Tick += async () =>
            {
                if (admin_gun)
                {
                    if (Game.PlayerPed.IsShooting)
                    {
                        Weapon pedWeapon = Game.PlayerPed.Weapons.Current;
                        
                        if (pedWeapon.AmmoInClip > 0)
                        {
                            PlayEffectOnWeapon("scr_rcbarry2", "muz_clown");
                            Ped target = (Ped)Game.Player.GetTargetedEntity();
                            PutEntityInCage(target);
                        }
                    }
                }
            };
        }

        public WeaponSub()
        {
            updateFunc();
        }

        public static void Sub(UIMenu menu)
        {
            var sub = MainMenu.menuPool.AddSubMenu(menu, "Weapon Options");
            sub.MouseControlsEnabled = false;
            sub.MouseEdgeEnabled = false;
            sub.MouseWheelControlEnabled = false;

            // items
            get_all_weapons = new UIMenuItem("Get All Weapons");
            var balckhole_check = new UIMenuCheckboxItem("Admin Gun", admin_gun);

            // add items
            sub.AddItem(get_all_weapons);
            sub.AddItem(balckhole_check);

            // change handler
            sub.OnItemSelect += (sender, item, index) =>
            {
                if (item == get_all_weapons)
                {
                    WeaponCollection weaponHandler = Game.PlayerPed.Weapons;
                    weaponHandler.RemoveAll();

                    var values = Enum.GetValues(typeof(WeaponHash));
                    
                    for (int i = 0; i < values.Length; i++)
                    {
                        weaponHandler.Give((WeaponHash)values.GetValue(i), 9999, false, true);
                    }
                }
            };
            sub.OnCheckboxChange += (sender, item, checked_) =>
            {
                if (item == balckhole_check)
                {
                    admin_gun = checked_;
                }
            };
        }
    }
}
