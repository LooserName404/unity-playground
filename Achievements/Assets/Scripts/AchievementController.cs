using UnityEngine;

namespace TestLab
{
    public class AchievementController : MonoBehaviour
    {
        [SerializeField] private Achievement[] achievements;
        
        private void Start()
        {
            var view = FindObjectOfType<AchievementView>();
            foreach (var achievement in achievements)
            {
                achievement.Register();
                achievement.OnTrigger += view.EnqueueAchievement;
            }
        }
    }
}