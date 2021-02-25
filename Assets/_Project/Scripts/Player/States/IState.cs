namespace PlayerState
{
    public interface IState
    {
        void EnterState();
        void ExitState();
        void Update();
        void FixedUpdate();
    }
}
