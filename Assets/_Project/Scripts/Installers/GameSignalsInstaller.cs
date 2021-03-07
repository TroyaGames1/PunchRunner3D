using Events;
using Zenject;

namespace Installers
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
    
        public override void InstallBindings()
        {
        
        
            SignalBusInstaller.Install(Container); //Signal birimi extenjecte eklendi
            Container.DeclareSignal<SignalStartRaycasting>().OptionalSubscriber();
            Container.DeclareSignal<SignalPlayerHit>().OptionalSubscriber();
            Container.DeclareSignal<SignalDoShake>().OptionalSubscriber();
            Container.DeclareSignal<SignalChangeAxis>().OptionalSubscriber();
            Container.DeclareSignal<SignalPunch>().OptionalSubscriber();
            Container.DeclareSignalWithInterfaces<SignalChangeSpeedAndAnimation>().OptionalSubscriber();

        }

        public void InstallStateSignals() //TODO
        {
            Container.DeclareSignal<StartSignal>().OptionalSubscriber(); 
            Container.DeclareSignal<FailSignal>().OptionalSubscriber();
            Container.DeclareSignal<CompleteSignal>().OptionalSubscriber();
        }

    }
}
