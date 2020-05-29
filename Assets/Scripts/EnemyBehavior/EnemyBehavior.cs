using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [System.Serializable]
    public class EnemyBehavior
    {
        [SerializeField]
        [LabelWidth(117)]
        protected string note;
        [VerticalGroup("Time", PaddingBottom = 5)]
        [HorizontalGroup("Time/a", LabelWidth = 100)]
        [SerializeField]
        protected float behaviorTime = 0;
        [HideLabel]
        [HorizontalGroup("Time/a", LabelWidth = 100)]
        [SerializeField]
        protected float behaviorMaxTime = 0;


        public virtual float StartBehavior(EnemyController enemyController, Character character)
        {
            return Random.Range(behaviorTime, behaviorMaxTime);
        }

        public virtual void UpdateBehavior(EnemyController enemyController, Character character)
        {

        }
    }
}
