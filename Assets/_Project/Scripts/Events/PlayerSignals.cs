namespace Events
{
  

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
}
