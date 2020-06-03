using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EnemyBehaviorAction : EnemyBehavior
    {
        [HorizontalGroup("BehaviorActionParameter")]
        [SerializeField]
        bool lookAtPlayer;
        [HorizontalGroup("BehaviorActionParameter")]
        [SerializeField]
        bool autoCombo;
        [SerializeField]
        AttackController attackController;


        public override float StartBehavior(EnemyController enemyController, Character character)
        {
            if (lookAtPlayer == true) 
                character.LookAt(character.Target.transform);
            character.SetAutoCombo(autoCombo);
            character.Action(attackController);
            return Mathf.Max(0.01f, base.StartBehavior(enemyController, character));
            //return base.StartBehavior(enemyController, character);
        }


        public override void UpdateBehavior(EnemyController enemyController, Character character)
        {
        }
    }
}
