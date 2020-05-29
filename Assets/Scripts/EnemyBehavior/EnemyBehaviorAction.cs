using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EnemyBehaviorAction : EnemyBehavior
    {


        [SerializeField]
        AttackController attackController;


        public override float StartBehavior(EnemyController enemyController, Character character)
        {
            character.Action(attackController);
            return attackController.AttackBehavior.AttackAnimation2.length + base.StartBehavior(enemyController, character);
            //return base.StartBehavior(enemyController, character);
        }


        public override void UpdateBehavior(EnemyController enemyController, Character character)
        {


        }
    }
}
