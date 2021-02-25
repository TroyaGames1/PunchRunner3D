using System;
using UnityEngine;

namespace Miscs
{
    public class UIManager
    {
        UIManager(Settings settings)
        {
            preGameUI = settings.PreGameUI;
            ınGameUI = settings.InGameUI;
            preGameUI = settings.PreGameUI;
            preGameUI = settings.PreGameUI;
           
        }

        public readonly GameObject preGameUI;
        public readonly GameObject ınGameUI;
        public readonly GameObject deadUI;
        public readonly GameObject finishGameUI;
        
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
