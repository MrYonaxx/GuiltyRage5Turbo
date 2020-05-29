using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EnemyBehaviorMoveTo : EnemyBehavior
    {

        [SerializeField]
        [LabelWidth(100)]
        Vector2 sizeMin = Vector2.one;
        [SerializeField]
        [LabelWidth(100)]
        Vector2 sizeMax = Vector2.one;


        //[SerializeField]
        //bool alignHorizontal = false;

        [HorizontalGroup("Setup")]
        [LabelWidth(100)]
        [SerializeField]
        bool nearestSide = false;
        [HorizontalGroup("Setup")]
        [LabelWidth(100)]
        [SerializeField]
        bool lookAtPosition = false;

        [HorizontalGroup("Bleuh")]
        [LabelWidth(100)]
        [SerializeField]
        bool targetIsSelf = false;
        [HorizontalGroup("Bleuh")]
        [LabelWidth(100)]
        [SerializeField]
        bool alwaysUpdatePosition = false;

        /*[SerializeField]
        int behaviourID;*/
        [VerticalGroup(PaddingBottom = 20)]
        [SerializeField]
        [HideLabel]
        EnemyBehavior behaviourDestination;


        Vector3 randomPosition;
        Vector3 movementPosition;

        public override float StartBehavior(EnemyController enemyController, Character character)
        {
            randomPosition = Random.insideUnitCircle * 1f;
            randomPosition.Normalize();
            randomPosition *= new Vector2(Random.Range(sizeMin.x, sizeMax.x), Random.Range(sizeMin.y, sizeMax.y));
            randomPosition = new Vector2(Mathf.Sign(randomPosition.x) * Mathf.Clamp(randomPosition.x, sizeMin.x, sizeMax.x), 
                                         Mathf.Sign(randomPosition.y) * Mathf.Clamp(randomPosition.y, sizeMin.y, sizeMax.y));
            if (nearestSide == true)
            {
                character.LookAt(character.Target.transform);
                randomPosition = new Vector2(Mathf.Abs(randomPosition.x) * -character.GetDirection(), randomPosition.y);
            }
            if (lookAtPosition == true)
                character.LookAtPosition(randomPosition);

            if(targetIsSelf == true)
                movementPosition = new Vector3(character.transform.position.x, character.transform.position.y, 0) + randomPosition;
            else
                movementPosition = new Vector3(character.Target.transform.position.x, character.Target.transform.position.y, 0) + randomPosition;

            return base.StartBehavior(enemyController, character);
        }


        public override void UpdateBehavior(EnemyController enemyController, Character character)
        {
            if(alwaysUpdatePosition == true)
                movementPosition = new Vector3(character.Target.transform.position.x, character.Target.transform.position.y, 0) + randomPosition;
            character.LookAt(character.Target.transform);
            if (character.MoveToPoint(movementPosition, 0, 0) == true)
            {
                if (behaviourDestination != null)
                    enemyController.ForcePattern(behaviourDestination, character);
                /*else if (behaviourID != -1)
                {

                }*/
                else
                    enemyController.StopPattern();
            }
                
        }
    }
}
