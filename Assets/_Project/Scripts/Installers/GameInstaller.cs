using Miscs;
using Sirenix.OdinInspector;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {

        /// <summary>
        /// MonoBehavior olarak, sahneden verilerek yüklenecek olan şeyler burada verilecek
        /// </summary>
        
        [TabGroup("UI Settings")]
        public UIManager.Settings _UISettings;
        public override void InstallBindings()
        {
            GameSignalsInstaller.Install(Container); 
            InstallSettingsInstances();

            Container.Bind<UIManager>().AsSingle();
            Container.BindInterfacesTo<LevelLoader>().AsSingle();
        }

        public void InstallSettingsInstances()
        {
            Container.BindInstance(_UISettings).AsSingle().WhenInjectedInto<UIManager>();
        }

   
    
      
    }


}
 