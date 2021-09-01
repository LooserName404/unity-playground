using UnityEngine;

namespace TestLab.Achievements
{
    [CreateAssetMenu(fileName = "JumpAchievement", menuName = "JumpAchievement", order = 0)]
    public class JumpAchievement : Achievement
    {
        [SerializeField] private int targetValue;

        private int _jumpCounter;
        private PlayerController _player;

        public override void Register()
        {
            _player = FindObjectOfType<PlayerController>();
            _player.OnJumpSuccesfully += Check;
        } 
        
        protected override void Check()
        {
            _jumpCounter += 1;
            if (_jumpCounter >= targetValue)
            {
                Trigger();
            }
        }

        protected override void Trigger()
        {
            OnTrigger?.Invoke(title, description);
            _player.OnJumpSuccesfully -= Check;
        }

        private void OnDisable()
        {
            _jumpCounter = 0;
        }
    }
}