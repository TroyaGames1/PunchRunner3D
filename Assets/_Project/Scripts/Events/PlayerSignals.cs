using PlayerBehaviors;

namespace Events
{

    #region GameSignals

    #region SignalChangeSpeedAndAnimation

    public readonly struct SignalChangeSpeedMovementFactorAndAnimation: ISignalChangeSpeed,ISignalChangeAnimation, ISignalChangeMovementSpeedFactor
    { 
        public SignalChangeSpeedMovementFactorAndAnimation(string animation, float splineSpeed, float factor)
        {
            Animation = animation;
            SplineSpeed = splineSpeed;
            SpeedFactor = factor;
        }
    
        public string Animation { get; }
        public float SplineSpeed { get; }
        public float SpeedFactor { get; }
    } 
    public interface ISignalChangeSpeed
    {
        float  SplineSpeed { get; }
    }
    public interface ISignalChangeMovementSpeedFactor
    {
        float  SpeedFactor { get; }
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

    

    public struct SignalPunch
    {
        public float xValue;

        public SignalPunch(float xValue)
        {
            this.xValue = xValue;
        }
    }
    
    #endregion
  
}
