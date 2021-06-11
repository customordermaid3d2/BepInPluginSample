using BepInEx.Configuration;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BepInPluginSample
{
    public class MyWindowRect
    {
        private float windowSpace;
        private Rect windowRect;
        private Size windowRectOpen;
        private Size windowRectClose;
        private Position position;
        private string jsonPath;

        private static Harmony harmony;
        private static event Action actionSave;

        private ConfigEntry<bool> isOpen ;

        public bool IsOpen
        {
            get => isOpen.Value;
            set
            {
                if (isOpen.Value != value)
                    if (isOpen.Value = value)
                    {
                        windowRect.width = windowRectOpen.w;
                        windowRect.height = windowRectOpen.h;
                    }
                    else
                    {
                        windowRect.width = windowRectClose.w;
                        windowRect.height = windowRectClose.h;
                    }
            }
        }

        struct Position
        {
            public float x;
            public float y;

            public Position(float x, float y) : this()
            {
                this.x = x;
                this.y = y;
            }
        }

        struct Size
        {
            public float w;
            public float h;

            public Size(float w, float h) : this()
            {
                this.w = w;
                this.h = h;
            }
        }

        public Rect WindowRect
        {
            get
            {
                // 윈도우 리사이즈시 밖으로 나가버리는거 방지
                windowRect.x = Mathf.Clamp(windowRect.x, -windowRect.width + windowSpace, Screen.width - windowSpace);
                windowRect.y = Mathf.Clamp(windowRect.y, -windowRect.height + windowSpace, Screen.height - windowSpace);
                return windowRect;
            }
            set => windowRect = value;
        }

        public float Height { get => windowRect.height; set => windowRect.height = value; }
        public float Width { get => windowRect.width; set => windowRect.width = value; }
        public float X { get => windowRect.x; set => windowRect.x = value; }
        public float Y { get => windowRect.y; set => windowRect.y = value; }

        public MyWindowRect(ConfigFile config,string fileName= MyAttribute.PLAGIN_FULL_NAME, float ho = 600f, float hc = 40f, float wo = 300f, float wc = 300f,  float x = 50f, float y = 40f, float windowSpace = 40f)
        {
            jsonPath = Path.GetDirectoryName(config.ConfigFilePath) + $@"\{fileName}-windowRect.json";
            isOpen = config.Bind("GUI", "isOpen", true);

            this.windowSpace = windowSpace;
            windowRect = new Rect(x, y, wo, ho);
            windowRectOpen = new Size( wo, ho);
            windowRectClose = new Size( wc, hc);

            if (harmony==null)
            {
                harmony = Harmony.CreateAndPatchAll(typeof(MyWindowRect));
            }
            actionSave += save;
        }

        public void load()
        {
            if (File.Exists(jsonPath))
            {
                position = JsonConvert.DeserializeObject<Position>(File.ReadAllText(jsonPath));
            }
            else
            {
                position = new Position(windowSpace, windowSpace);
                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(position, Formatting.Indented)); // 자동 들여쓰기
            }
            windowRect.x = position.x;
            windowRect.y = position.y;
        }

        public void save()
        {
            position.x = windowRect.x;
            position.y = windowRect.y;
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(position, Formatting.Indented)); // 자동 들여쓰기
        }

        [HarmonyPatch(typeof(GameMain), "LoadScene")]
        [HarmonyPostfix]
        public static void LoadScene()
        {
            actionSave();
        }
    }
}
