using Miscs;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {

        /// <summary>
        /// MonoBehavior olarak, sahneden verilerek yüklenecek olan şeyler burada verilecek
        /// </summary>
        
        public UIManager.Settings _UISettings;
        public override void InstallBindings()
        {
            GameSignalsInstaller.Install(Container); //Signal Containerini yükle
            
            Container.BindInstance(_UISettings).AsSingle();
            Container.BindInterfacesTo<LevelLoader>().AsSingle();
        }

   
    
      
    }


}
 