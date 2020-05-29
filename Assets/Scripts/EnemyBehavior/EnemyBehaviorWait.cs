using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    //[System.Serializable]
    public class EnemyBehaviorWait : EnemyBehavior
    {
        [SerializeField]
        [VerticalGroup(PaddingBottom = 20)]
        [LabelWidth(100)]
        bool lookAtPlayer = false;

        public override float StartBehavior(EnemyController enemyController, Character character)
        {
            character.SetSpeed(0, 0);
            return base.StartBehavior(enemyController, character);
        }

        public override void UpdateBehavior(EnemyController enemyController, Character character)
        {
            if(lookAtPlayer == true)
                character.LookAt(character.Target.transform);
        }
    }
}
