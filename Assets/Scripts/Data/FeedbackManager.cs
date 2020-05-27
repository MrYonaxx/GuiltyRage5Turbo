using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "FeedbackManager", menuName = "FeedbackManager", order = 1)]
    public class FeedbackManager : ScriptableObject
    {


        [SerializeField]
        private BattleFeedbackManager battleFeedbackManager;
        public BattleFeedbackManager BattleFeedbackManager
        {
            set { battleFeedbackManager = value; }
        }



        public void AttackFeedback(AttackBehavior attack)
        {
            battleFeedbackManager.ApplyFeedback(attack);
        }

        public void GuardBreakFeedback(Character character)
        {
            battleFeedbackManager.AnimationCardBreak(character);
        }

        public void GuardFeedback(Character character)
        {
            battleFeedbackManager.Guard(character);
        }

        public void DeadFeedback(Character character)
        {
            battleFeedbackManager.AnimationDeath(character);
        }

        public void WallBounceFeedback(Character character)
        {
            battleFeedbackManager.AnimationWallBounce(character);
        }

    }
}
