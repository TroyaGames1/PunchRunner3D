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
            Container.DeclareSignal<SignalPunch>().OptionalSubscriber();
            Container.DeclareSignalWithInterfaces<SignalChangeSpeedMovementFactorAndAnimation>().OptionalSubscriber();

            InstallStateSignals();
        }

        private void InstallStateSignals() //TODO
        {
            Container.DeclareSignal<SignalGameStart>().OptionalSubscriber(); 
            Container.DeclareSignal<SignalPlayerFailed>().OptionalSubscriber();
            Container.DeclareSignal<SignalGameFinished>().OptionalSubscriber();
        }

    }
}
