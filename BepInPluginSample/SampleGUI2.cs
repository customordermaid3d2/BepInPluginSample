using COM3D2.LillyUtill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

namespace BepInPluginSample
{
    class SampleGUI2 : MyGUI
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }

        public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            base.OnSceneLoaded(scene, mode);
        }

        public override void Start()
        {
            base.Start();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void WindowFunction(int id)
        {
            base.WindowFunction(id);
        }

        public override void WindowFunctionBody(int id)
        {
            base.WindowFunctionBody(id);
        }
    }
}
