using Miscs;


namespace PlayerState
{
    public class IdleState : IState
    { 
        readonly UIManager.Settings _UISettings;

        IdleState(UIManager.Settings _uı)
        {
            _UISettings = _uı;
        }
        
        public void EnterState()
        {
            _UISettings._gamePreUI.SetActive(true);
         
           
        }

        public void ExitState()
        {
        }

   

        public void FixedUpdate()
        {
        }

        public void Update()
        {
         
        }
    }

}
