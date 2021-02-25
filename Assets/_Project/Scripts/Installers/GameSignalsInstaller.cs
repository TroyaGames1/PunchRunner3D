using Events;
using Zenject;

namespace Installers
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
    
        public override void InstallBindings()
        {
        
        
            SignalBusInstaller.Install(Container); //Signal birimi extenjecte eklendi
            Container.DeclareSignal<PlayerSignals>().OptionalSubscriber();
         
        }

        public void InstallStateSignals()
        {
            Container.DeclareSignal<StartSignal>().OptionalSubscriber(); 
            Container.DeclareSignal<FailSignal>().OptionalSubscriber();
            Container.DeclareSignal<CompleteSignal>().OptionalSubscriber();
        }

    }
}
