using System.Collections;
using System.Collections.Generic;
using Events;
using PlayerBehaviors;
using UnityEngine;
using Zenject;

public class SplineTrigger : MonoBehaviour
{
   private SignalBus _signalBus;

   [Inject]
   public void Construct(SignalBus signalBus)
   {
      _signalBus = signalBus;
   }
   public void ChangeAxisToX()
   {
      Debug.Log("VAR");
      _signalBus.Fire(new SignalChangeAxis(PlayerMoveHandler.MoveEnum.MOVEX));
   }  
   public void ChangeAxisToZ()
   {
      _signalBus.Fire(new SignalChangeAxis(PlayerMoveHandler.MoveEnum.MOVEZ));
   }
}
