using COM3D2.Lilly.Plugin;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BepInPluginSample
{
    class SamplePatch
    {
        [HarmonyPatch(typeof(CharacterMgr), "SetActive")]
        [HarmonyPostfix]
        public static void SetActive(Maid f_maid, int f_nActiveSlotNo, bool f_bMan)
        {
            MyLog.LogMessage("CharacterMgr.SetActive", f_nActiveSlotNo, f_bMan, f_maid.status.fullNameEnStyle);
        }

        [HarmonyPatch(typeof(CharacterMgr), "Deactivate")]
        [HarmonyPrefix]
        public static void Deactivate(int f_nActiveSlotNo, bool f_bMan)
        {
            MyLog.LogMessage("CharacterMgr.Deactivate", f_nActiveSlotNo, f_bMan);
        }
    }
}
