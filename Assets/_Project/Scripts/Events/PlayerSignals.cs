namespace Events
{
  

    #region SignalChangeSpeedAndAnimation

    public readonly struct SignalChangeSpeedAndAnimation: ISignalChangeSpeed,ISignalChangeAnimation
    { 
        public SignalChangeSpeedAndAnimation(string animation, int speed)
        {
            Animation = animation;
            Speed = speed;
        }
    
        public string Animation { get; }
        public int Speed { get; }
    } 
    public interface ISignalChangeSpeed
    {
        int  Speed { get; }
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
