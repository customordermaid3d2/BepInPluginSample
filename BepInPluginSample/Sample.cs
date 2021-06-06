using BepInEx;
using BepInEx.Configuration;
using COM3D2.Lilly.Plugin;
using COM3D2API;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BepInPluginSample
{
    [BepInPlugin("COM3D2.Sample.Plugin", "COM3D2.Sample.Plugin", "21.6.6")]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    [BepInProcess("COM3D2x64.exe")]
    public class Sample : BaseUnityPlugin
    {

        // 단축키 설정파일로 연동
        private ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;

        // GUI ON OFF 설정파일로 저장
        private ConfigEntry<bool> IsGUIOn;
        private bool isGUIOn
        {
            get => IsGUIOn.Value;
            set => IsGUIOn.Value = value;
        }

        Harmony harmony;

        /// <summary>
        ///  게임 실행시 한번만 실행됨
        /// </summary>
        public void Awake()
        {
            MyLog.LogMessage("Awake");

            // 단축키 기본값 설정
            ShowCounter = Config.Bind("Sample", "KeyboardShortcut0", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Alpha9, KeyCode.LeftControl));

            // 일반 설정값
            IsGUIOn = Config.Bind("Sample", "isGUIOn", false);

            // 기어 메뉴 추가. 이 플러그인 기능 자체를 멈추려면 enabled 를 꺽어야함. 그러면 OnEnable(), OnDisable() 이 작동함
            SystemShortcutAPI.AddButton("Sample", new Action(delegate () { enabled = !enabled; }), "Sample", MyUtill.ExtractResource(Properties.Resources.sample_png));
            SystemShortcutAPI.AddButton("Sample", new Action(delegate () { isGUIOn = !isGUIOn; }), "Sample", MyUtill.ExtractResource(Properties.Resources.sample_png));
        }



        public void OnEnable()
        {
            MyLog.LogMessage("OnEnable");

            SceneManager.sceneLoaded += this.OnSceneLoaded;

            // 하모니 패치
            harmony = Harmony.CreateAndPatchAll(typeof(SamplePatch));
        }

        /// <summary>
        /// 게임 실행시 한번만 실행됨
        /// </summary>
        public void Start()
        {
            MyLog.LogMessage("Start");
        }

        public string scene_name = string.Empty;

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            MyLog.LogMessage("OnSceneLoaded", scene.name, scene.buildIndex);
            //  scene.buildIndex 는 쓰지 말자 제발
            scene_name = scene.name;
        }

        public void FixedUpdate()
        {

        }

        public void Update()
        {
            if (ShowCounter.Value.IsDown())
            {
                MyLog.LogMessage("IsDown", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            }
            if (ShowCounter.Value.IsPressed())
            {
                MyLog.LogMessage("IsPressed", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            }
            if (ShowCounter.Value.IsUp())
            {
                isGUIOn = !isGUIOn;
                MyLog.LogMessage("IsUp", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            }
        }

        public void LateUpdate()
        {

        }

        private Rect windowRect = new Rect(windowSpace, windowSpace, 400f, 400f);
        private int windowId = new System.Random().Next();
        private const float windowSpace = 40.0f;


        public void OnGUI()
        {
            if (!isGUIOn)
            {
                return;
            }
            // 윈도우 리사이즈시 밖으로 나가버리는거 방지
            windowRect.x = Mathf.Clamp(windowRect.x, -windowRect.width + windowSpace, Screen.width - windowSpace);
            windowRect.y = Mathf.Clamp(windowRect.y, -windowRect.height + windowSpace, Screen.height - windowSpace);
            windowRect = GUILayout.Window(windowId, windowRect, WindowFunction, "Sample:" + scene_name);
        }

        private Vector2 scrollPosition;

        public void WindowFunction(int id)
        {
            GUI.enabled = true;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            if (GUILayout.Button("sample1 Start Coroutine"))
            {
                Debug.Log("Button1");
                StartCoroutine(MyCoroutine());
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("sample2 Stop Coroutine"))
            {
                Debug.Log("Button2");
                isCoroutine = false;
            }
            GUILayout.BeginVertical();
            if (GUILayout.Button("sample3"))
            {
                Debug.Log("Button3");
            }
            GUILayout.EndVertical();
            GUI.enabled = false;
            if (GUILayout.Button("sample4"))
            {
                Debug.Log("Button4");
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("sample5"))
            {
                Debug.Log("Button5");
            }
            GUILayout.EndScrollView();

            if (GUI.changed)
            {
                Debug.Log("changed");
            }

            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
            GUI.enabled = true;
        }

        private bool isCoroutine = false;
        public int CoroutineCount = 0;

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

        public void OnDisable()
        {
            MyLog.LogMessage("OnDisable");

            SceneManager.sceneLoaded -= this.OnSceneLoaded;

            harmony.UnpatchSelf();// ==harmony.UnpatchAll(harmony.Id);
            //harmony.UnpatchAll(); // 정대 사용 금지. 다름 플러그인이 패치한것까지 다 풀려버림
        }

        public void Pause()
        {
            MyLog.LogMessage("Pause");
        }

        public void Resume()
        {
            MyLog.LogMessage("Resume");
        }





    }
}
