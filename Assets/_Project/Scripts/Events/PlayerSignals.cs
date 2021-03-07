using PlayerBehaviors;

namespace Events
{

    #region GameSignals

    #region SignalChangeSpeedAndAnimation

    public readonly struct SignalChangeSpeedAndAnimation: ISignalChangeSpeed,ISignalChangeAnimation
    { 
        public SignalChangeSpeedAndAnimation(string animation, float speed)
        {
            Animation = animation;
            Speed = speed;
        }
    
        public string Animation { get; }
        public float Speed { get; }
    } 
    public interface ISignalChangeSpeed
    {
        float  Speed { get; }
    } 
    public interface ISignalChangeAnimation
    {
        string Animation { get; }
    }
    #endregion

    public struct SignalStartRaycasting
    {
    }

    public struct SignalPlayerHit
    {
        public SignalPlayerHit(float value)
        {
            Value = value;
        }

        public float Value { get; }
   }
    
    public struct SignalDoShake{}

    public struct SignalChangeAxis
    {
        public PlayerMoveHandler.MoveEnum _moveEnum;

     public SignalChangeAxis(PlayerMoveHandler.MoveEnum moveEnum)
        {
            _moveEnum = moveEnum;
        }

    }

    public struct SignalPunch
    {
        
    }
    
    #endregion
  
}
