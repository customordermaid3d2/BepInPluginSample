using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using COM3D2API;
using HarmonyLib;
using LillyUtill.MyWindowRect;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BepInPluginSample
{
    class MyAttribute
    {
        public const string PLAGIN_NAME = "Sample";
        public const string PLAGIN_VERSION = "22.3.4";
        public const string PLAGIN_FULL_NAME = "COM3D2.Sample.Plugin";
    }

    [BepInPlugin(MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, MyAttribute.PLAGIN_VERSION)]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    //[BepInPlugin("COM3D2.Sample.Plugin", "COM3D2.Sample.Plugin", "21.6.6")]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    [BepInProcess("COM3D2x64.exe")]
    public class Sample : BaseUnityPlugin
    {
        // 단축키 설정파일로 연동
        //private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;

        Harmony harmony;

        public static Sample sample;

        public static ManualLogSource log;


        private static ConfigFile config;

        private static ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;



        private static Vector2 scrollPosition;

        // 위치 저장용 테스트 json
        public static WindowRectUtill myWindowRect;

        public string windowName = MyAttribute.PLAGIN_NAME;
        public string FullName = MyAttribute.PLAGIN_NAME;
        public string ShortName = "SP";


        public Sample()
        {
            log = Logger;
            sample = this;
        }

        /// <summary>
        ///  게임 실행시 한번만 실행됨
        /// </summary>
        private void Awake()
        {
            Sample.log.LogMessage("Awake");

            // 단축키 기본값 설정
            //ShowCounter = Config.Bind("KeyboardShortcut", "KeyboardShortcut0", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Alpha9, KeyCode.LeftControl));

            myWindowRect = new WindowRectUtill(config, MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, "SP");
            ShowCounter = config.Bind("GUI", "isGUIOnKey", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Alpha9, KeyCode.LeftControl));

        }

        private void OnEnable()
        {
            Sample.log.LogMessage("OnEnable");

            SceneManager.sceneLoaded += this.OnSceneLoaded;

            // 하모니 패치
            harmony = Harmony.CreateAndPatchAll(typeof(SamplePatch));
        }

        /// <summary>
        /// 게임 실행시 한번만 실행됨
        /// </summary>
        private void Start()
        {
            Sample.log.LogMessage("Start");

            // 기어 메뉴 추가. 이 플러그인 기능 자체를 멈추려면 enabled 를 꺽어야함. 그러면 OnEnable(), OnDisable() 이 작동함
            SystemShortcutAPI.AddButton(MyAttribute.PLAGIN_FULL_NAME, new Action(delegate () { myWindowRect.IsGUIOn = !myWindowRect.IsGUIOn; }), MyAttribute.PLAGIN_NAME + " : " + ShowCounter.Value.ToString(), BepInPluginSample.Properties.Resources.icon);
            //SystemShortcutAPI.AddButton(MyAttribute.PLAGIN_FULL_NAME, new Action(delegate () { enabled = !enabled; }), MyAttribute.PLAGIN_NAME, MyUtill.ExtractResource(BepInPluginSample.Properties.Resources.icon));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //  scene.buildIndex 는 쓰지 말자 제발
            Sample.log.LogMessage($"OnSceneLoaded, {scene.name}, {scene.buildIndex}");
            // SceneManager.GetActiveScene().name
        }

        /*
        public void FixedUpdate()
        {

        }
        */

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
                myWindowRect.IsGUIOn = !myWindowRect.IsGUIOn;
                Sample.log.LogMessage($"IsUp {ShowCounter.Value.Modifiers} {ShowCounter.Value.MainKey}");
            }
        }

        /*
        public void LateUpdate()
        {

        }
        */

        private void OnGUI()
        {
            if (!myWindowRect.IsGUIOn)
                return;

            myWindowRect.WindowRect = GUILayout.Window(myWindowRect.winNum, myWindowRect.WindowRect, WindowFunction, "", GUI.skin.box);
        }

        private void WindowFunction(int id)
        {
            GUI.enabled = true;

            GUILayout.BeginHorizontal();
            GUILayout.Label(myWindowRect.windowName, GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) { myWindowRect.IsOpen = !myWindowRect.IsOpen; }
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) { myWindowRect.IsGUIOn = false; }
            GUILayout.EndHorizontal();

            if (!myWindowRect.IsOpen)
            {

            }
            else
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                if (GUILayout.Button("sample1 Start Coroutine"))
                {
                    Debug.Log("Button1");
                    Sample.sample.StartCoroutine(MyCoroutine());
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("sample5"))
                {
                    Debug.Log("Button5");
                }
                GUILayout.BeginVertical();
                if (GUILayout.Button("sample7"))
                {
                    Debug.Log("Button7");
                }
                GUI.enabled = false;
                if (GUILayout.Button("sample3"))
                {
                    Debug.Log("Button3");
                }
                GUILayout.EndVertical();

                if (GUILayout.Button("sample4"))
                {
                    Debug.Log("Button4");
                }
                GUILayout.EndHorizontal();
                GUI.enabled = true;
                if (GUILayout.Button("sample2 Stop Coroutine"))
                {
                    Debug.Log("Button2");
                    isCoroutine = false;
                }
                GUILayout.EndScrollView();

                if (GUI.changed)
                {
                    Debug.Log("changed");
                }

            }
            GUI.enabled = true;
            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
        }

        public static bool isCoroutine = false;
        public static int CoroutineCount = 0;

        private IEnumerator MyCoroutine()
        {
            isCoroutine = true;
            while (isCoroutine)
            {
                Sample.log.LogMessage($"MyCoroutine {++CoroutineCount}");
                //yield return null;
                yield return new WaitForSeconds(1f);
            }
        }
        /*
        /// <summary>
        /// 게임 X 버튼 눌렀을때 반응
        /// </summary>
        public void OnApplicationQuit()
        {
            Sample.myLog.LogMessage("OnApplicationQuit");
        }
        */

        private void OnDisable()
        {
            Sample.log.LogMessage("OnDisable");
            isCoroutine = false;
            SceneManager.sceneLoaded -= this.OnSceneLoaded;

            harmony.UnpatchSelf();// ==harmony.UnpatchAll(harmony.Id);
            //harmony.UnpatchAll(); // 정대 사용 금지. 다름 플러그인이 패치한것까지 다 풀려버림
        }
        /*
        public void Pause()
        {
            Sample.myLog.LogMessage("Pause");
        }

        public void Resume()
        {
            Sample.myLog.LogMessage("Resume");
        }
        */


    }
}
