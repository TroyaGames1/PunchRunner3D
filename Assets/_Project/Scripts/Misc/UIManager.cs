using System;
using UnityEngine;

namespace Miscs
{
    public class UIManager
    {
        UIManager(Settings settings)
        {
            PreGameUI = settings.PreGameUI;
            InGameUI = settings.InGameUI;
            PreGameUI = settings.PreGameUI;
            PreGameUI = settings.PreGameUI;
           
        }

        public GameObject PreGameUI { get; }
        public GameObject InGameUI { get; }
        public GameObject DeadUI { get; }
        public GameObject FinishGameUI { get; }
        [Serializable]
        public class Settings
        {
        
            public GameObject PreGameUI;
            public GameObject InGameUI;
            public GameObject DeadUI;
            public GameObject FinishUI;
        
        }
  
    }

}
