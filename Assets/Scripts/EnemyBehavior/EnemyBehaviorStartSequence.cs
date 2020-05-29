using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EnemyBehaviorStartSequence : EnemyBehavior
    {
        [SerializeField]
        [VerticalGroup(PaddingBottom = 20)]
        [LabelWidth(100)]
        string sequenceID;

        public override float StartBehavior(EnemyController enemyController, Character character)
        {
            enemyController.PlaySequence(sequenceID);
            return 0;
        }

        public override void UpdateBehavior(EnemyController enemyController, Character character)
        {

        }
    }
}
