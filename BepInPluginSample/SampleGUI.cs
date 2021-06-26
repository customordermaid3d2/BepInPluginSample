using BepInEx;
using BepInEx.Configuration;
using COM3D2.Lilly.Plugin;
using COM3D2API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BepInPluginSample
{
    class SampleGUI : MonoBehaviour
    {
        public static SampleGUI instance;

        private static ConfigFile config;

        private static ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;

        private static int windowId = new System.Random().Next();

        private static Vector2 scrollPosition;

        // 위치 저장용 테스트 json
        public static MyWindowRect myWindowRect;

        public static int all;
        public static int seleted;
        public static int seletedChg;

        static string[] type = new string[] { "one", "all" };


        public static bool IsOpen
        {
            get => myWindowRect.IsOpen;
            set => myWindowRect.IsOpen = value;
        }

        // GUI ON OFF 설정파일로 저장
        private static ConfigEntry<bool> IsGUIOn;

        public static bool isGUIOn
        {
            get => IsGUIOn.Value;
            set => IsGUIOn.Value = value;
        }

        internal static SampleGUI Install(GameObject parent, ConfigFile config)
        {
            SampleGUI.config = config;
            instance = parent.GetComponent<SampleGUI>();
            if (instance == null)
            {
                instance = parent.AddComponent<SampleGUI>();
                MyLog.LogMessage("GameObjectMgr.Install", instance.name);
            }
            return instance;
        }

        public void Awake()
        {
            myWindowRect = new MyWindowRect(config);
            IsGUIOn = config.Bind("GUI", "isGUIOn", false);
            ShowCounter = config.Bind("GUI", "isGUIOnKey", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Minus, KeyCode.LeftControl));
            SystemShortcutAPI.AddButton(MyAttribute.PLAGIN_FULL_NAME, new Action(delegate () { SampleGUI.isGUIOn = !SampleGUI.isGUIOn; }), MyAttribute.PLAGIN_NAME + " : " + ShowCounter.Value.ToString(), MyUtill.ExtractResource(BepInPluginSample.Properties.Resources.icon));
        }

        public void OnEnable()
        {
            MyLog.LogMessage("OnEnable");

            SampleGUI.myWindowRect.load();
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        }

        public void Start()
        {
            MyLog.LogMessage("Start");
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SampleGUI.myWindowRect.save();
        }

        private void Update()
        {
            //if (ShowCounter.Value.IsDown())
            //{
            //    MyLog.LogMessage("IsDown", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
            //if (ShowCounter.Value.IsPressed())
            //{
            //    MyLog.LogMessage("IsPressed", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
            if (ShowCounter.Value.IsUp())
            {
                isGUIOn = !isGUIOn;
                MyLog.LogMessage("IsUp", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            }
        }

        public void OnGUI()
        {
            if (!isGUIOn)
                return;

            //GUI.skin.window = ;

            //myWindowRect.WindowRect = GUILayout.Window(windowId, myWindowRect.WindowRect, WindowFunction, MyAttribute.PLAGIN_NAME + " " + ShowCounter.Value.ToString(), GUI.skin.box);
            //GUI.skin.box.
            myWindowRect.WindowRect = GUILayout.Window(windowId, myWindowRect.WindowRect, WindowFunction, "", GUI.skin.box);
        }

        public void WindowFunction(int id)
        {
            GUI.enabled = true;

            GUILayout.BeginHorizontal();
            GUILayout.Label(MyAttribute.PLAGIN_NAME + " " + ShowCounter.Value.ToString(), GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) { IsOpen = !IsOpen; }
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) { isGUIOn = false; }
            GUILayout.EndHorizontal();

            if (!IsOpen)
            {

            }
            else
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                GUILayout.Label(AnmEditUtill.fileName);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("open")) { AnmEditUtill.ShowDialogLoadRun(); }
                if (GUILayout.Button("save")) { AnmEditUtill.ShowDialogSaveRun(); }

                GUILayout.EndHorizontal();

                GUILayout.Label(AnmEditUtill.anmState.time.ToString());
                time = GUILayout.HorizontalScrollbar(AnmEditUtill.time, 1, 0, AnmEditUtill.length+1);

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("play")) { AnmEditUtill.Play(); }
                if (GUILayout.Button("stop")) { AnmEditUtill.Stop(); }

                GUILayout.EndHorizontal();

                all = GUILayout.SelectionGrid(all, type, 2);

                GUILayout.Label("maid select");
                // 여기는 출력된 메이드들 이름만 가져옴
                // seleted 가 이름 위치 번호만 가져온건데
                seletedChg = GUILayout.SelectionGrid(seleted, SamplePatch.maidNames, 1);

                GUI.enabled = true;

                GUILayout.EndScrollView();
                
                if (GUI.changed)
                {                    
                    Debug.Log("changed");
                    if (seletedChg!= seleted)
                    {
                        seleted = seletedChg;
                        AnmEditUtill.SetMaid();
                    }
                    if (AnmEditUtill.time != time)
                    {
                        AnmEditUtill.time = time;
                    }
                }

            }
            GUI.enabled = true;
            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
        }

        public void OnDisable()
        {
            SampleGUI.isCoroutine = false;
            SampleGUI.myWindowRect.save();
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
        }

        public static bool isCoroutine = false;
        public static int CoroutineCount = 0;
        private float time;

        private IEnumerator MyCoroutine()
        {
            isCoroutine = true;
            while (isCoroutine)
            {
                MyLog.LogMessage("MyCoroutine ", ++CoroutineCount);
                //yield return null;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
