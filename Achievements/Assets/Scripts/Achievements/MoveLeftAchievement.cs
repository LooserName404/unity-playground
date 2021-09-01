using UnityEngine;

namespace TestLab.Achievements
{
    [CreateAssetMenu(fileName = "MoveLeftAchievement", menuName = "MoveLeftAchievement", order = 0)]
    public class MoveLeftAchievement : Achievement
    {
        private PlayerController _player;
        public override void Register()
        {
            _player = FindObjectOfType<PlayerController>();
            _player.OnMoveLeftSuccessfully += Check;
        }

        protected override void Check()
        {
            Trigger();
        }

        protected override void Trigger()
        {
            OnTrigger?.Invoke(title, description);
            _player.OnMoveLeftSuccessfully -= Check;
        }
    }
}