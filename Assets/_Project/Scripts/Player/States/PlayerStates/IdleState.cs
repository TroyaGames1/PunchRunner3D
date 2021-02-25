using Miscs;


namespace PlayerState
{
    public class IdleState : IState
    { 
        readonly UIManager _uıManager;

        IdleState(UIManager _uı)
        {
            _uıManager = _uı;
        }
        
        public void EnterState()
        {
            _uıManager.preGameUI.SetActive(true);
         
           
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
