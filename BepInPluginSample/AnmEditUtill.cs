using COM3D2.Lilly.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BepInPluginSample
{
    public class AnmEditUtill
    {
        static System.Windows.Forms.OpenFileDialog openDialog;
        static System.Windows.Forms.SaveFileDialog saveDialog;

        public static string fileName = string.Empty;

        //public static float value=0;// 직접 사용 금지
        //internal static float max=0;//=> (float)(anm[tag]?.length);

        internal static string tag = string.Empty;

        internal static Maid maid = null;//=> SamplePatch.maids[SampleGUI.seleted];

        internal static Animation anm => maid?.body0.m_Animation;        
        internal static AnimationState anmState  => anm?[tag];

        internal static float time
        {
            get
            {
                if (anmState != null)
                {
                    return anmState.time;
                }
                return 0;
            }
            set
            {
                if (anmState != null)
                {
                    anmState.time=value;
                }
            }
        }
        internal static float length
        {
            get
            {
                if (anmState != null)
                {
                    return anmState.length;
                }
                return 0;
            }
        }

        //=null; //=> SamplePatch.maids[SampleGUI.seleted].body0.m_Animation;            

        public static void init()
        {
            MyLog.LogMessage("AnmEditUtill.init");

            openDialog = new System.Windows.Forms.OpenFileDialog()
            {
                // 기본 확장자
                DefaultExt = "anm",
                // 기본 디렉토리
                InitialDirectory = Path.Combine(GameMain.Instance.SerializeStorageManager.StoreDirectoryPath, @"PhotoModeData\MyPose"),
                // 선택 가능 확장자
                Filter = "Xml files (*.anm)|*.anm|All files (*.*)|*.*"
            };

            saveDialog = new System.Windows.Forms.SaveFileDialog()
            {
                DefaultExt = "anm",
                InitialDirectory = Path.Combine(GameMain.Instance.SerializeStorageManager.StoreDirectoryPath, @"PhotoModeData\MyPose"),
                Filter = "Xml files (*.anm)|*.anm|All files (*.*)|*.*"
            };

        }

        public static void SetMaid()
        {
            maid = SamplePatch.maids[SampleGUI.seleted];
        }


        public static void ShowDialogLoadRun()
        {

            if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)// 오픈했을때
            {
                fileName = openDialog.FileName;
                SetAnimation(maid);
                //if (SampleGUI.all==0)
                //{
                //    if (maid != null)
                //    {
                //        maid.CrossFade(fileName);
                //    }
                //}
                //else
                //{
                //    foreach (var maid in SamplePatch.maids)
                //    {
                //        if (maid != null)
                //        {
                //            maid.CrossFade(fileName);
                //        }
                //    }
                //}
            }
        }

        public static void SetAnimation(Maid maid)
        {
            if (maid != null)
            {
                byte[] byte_data = File.ReadAllBytes(fileName);
                tag = Path.GetFileName(fileName).GetHashCode().ToString();
                maid.body0.CrossFade(tag, byte_data, false, true, false, 0f, 1f);
            }
        }

        public static void ShowDialogSaveRun()
        {
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        internal static void Play()
        {
            anm?.Play(tag);            
            /*
            if (SampleGUI.all == 0)
            {
                if (anm != null)
                {
                    anm.Play();
                }
            }
            else
            {
                foreach (var anm in SamplePatch.maidAnms)
                {
                    if (anm != null)
                    {
                        anm.Play();
                    }
                }
            }*/
        }

        internal static void Stop()
        {
            anm?.Stop(tag);
            
            /*
            if (SampleGUI.all == 0)
            {
                if (anm != null)
                {
                    anm.Stop();
                }
            }
            else
            {
                foreach (var anm in SamplePatch.maidAnms)
                {
                    if (anm != null)
                    {
                        anm.Stop();
                    }
                }
            }*/
        }

        static void test()
        {
            MotionWindow mw = GameObject.FindObjectOfType<MotionWindow>();
            if (mw != null)
            {
                PopupAndTabList patl = mw.PopupAndTabList;
                try
                {
                    mw.AddMyPose(fileName);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }


        /// <summary>
        /// 분석용
        /// </summary>
        /// <param name="target_maid"></param>
        /// 
        /*
        public void UpdateAnimationData(Maid target_maid)
        {
            if (target_maid != null && target_maid.body0 != null && target_maid.body0 != null && target_maid.body0.m_Bones.GetComponent<Animation>() != null)
            {
                this.animation_ = target_maid.body0.m_Bones.GetComponent<Animation>();
                this.anime_state_ = this.animation_[target_maid.body0.LastAnimeFN.ToLower()];
                if (this.anime_state_.length == 0f)
                {
                    this.CopyAndPasteBtn.enabled = false;
                    WFCheckBox checkbtnStop = this.CheckbtnStop;
                    bool flag = false;
                    this.CheckbtnStop.check = flag;
                    checkbtnStop.enabled = flag;
                    this.Slider.enabled = false;
                    this.animation_ = null;
                    this.anime_state_ = null;
                    this.Slider.MaxNum = 1f;
                    this.Slider.value = 0f;
                }
                else
                {
                    Dictionary<string, string> maidStoreData = base.GetMaidStoreData(target_maid);
                    this.Slider.enabled = true;
                    this.Slider.MaxNum = this.anime_state_.length;
                    if (this.anime_state_.wrapMode == WrapMode.Once)
                    {
                        this.anime_state_.wrapMode = WrapMode.ClampForever;
                    }
                    this.CopyAndPasteBtn.enabled = true;
                    this.CheckbtnStop.enabled = true;
                    this.CheckbtnStop.check = bool.Parse(maidStoreData["is_stop"]);
                    if (this.CheckbtnStop.check)
                    {
                        this.OnClickStopCheck(this.CheckbtnStop);
                        this.Slider.value = float.Parse(maidStoreData["time"]);
                        this.anime_state_.enabled = true;
                        this.animation_.Sample();
                        this.anime_state_.enabled = false;
                    }
                }
            }
            else
            {
                this.CopyAndPasteBtn.enabled = false;
                WFCheckBox checkbtnStop2 = this.CheckbtnStop;
                bool flag = false;
                this.CheckbtnStop.check = flag;
                checkbtnStop2.enabled = flag;
                this.Slider.enabled = false;
                this.animation_ = null;
                this.anime_state_ = null;
                this.Slider.MaxNum = 1f;
                this.Slider.value = 0f;
            }
        }
        */
    }
}
