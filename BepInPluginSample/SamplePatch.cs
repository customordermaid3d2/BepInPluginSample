using COM3D2.Lilly.Plugin;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.TBodyAddItemExceptionSkip.Plugin
{
    class SamplePatch
    {
        /*
[Warning:TBodyAddItemExceptionSkip] TBody.AddItem , skirt , skirt , adly_skirt.model ,  ,  , False , 100
[Error  :  HarmonyX] Error while running static void BepInPluginSample.SamplePatch::AddItem(Exception& __exception, MPN mpn, string slotname, string filename, string AttachSlot, string AttachName, bool f_bTemp, int version). Error: System.NullReferenceException: Object reference not set to an instance of an object
at BepInPluginSample.SamplePatch.AddItem (System.Exception&,MPN,string,string,string,string,bool,int) <0x00280>
at (wrapper dynamic-method) TBody.DMD<TBody..AddItem> (TBody,MPN,string,string,string,string,bool,int) <0x0089f>
         */

        [HarmonyPatch(typeof(TBody), "AddItem",typeof(MPN),typeof(string),typeof(string),typeof(string),typeof(string),typeof(bool),typeof(int))]
        [HarmonyFinalizer] // CharacterMgr의 SetActive가 실행 전에 아래 메소드 작동
        public static void AddItem(ref Exception __exception,MPN mpn, string slotname, string filename, string AttachSlot, string AttachName, bool f_bTemp, int version)
        {
            if (__exception == null)
                return; // already handled

            MyLog.LogWarning("TBody.AddItem"
                , mpn
                , slotname
                , filename
                , AttachSlot
                , AttachName
                , f_bTemp
                , version
                );
            MyLog.LogWarning("TBody.AddItem", __exception.ToString());
            __exception = null;

        }

        


    }
}
