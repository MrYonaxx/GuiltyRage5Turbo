/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class EnemyController: Character
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Enemy")]
        [SerializeField]
        float timeEnemyAppear = 0.5f;
        [SerializeField]
        float timeEnemyDisappear = 2f;


        [SerializeField]
        Transform debugPoint;




        int enemyPatternID = 0;
        float enemyActionTime = 1;
        Vector3 randomPosition;
        Vector3 movementPosition;

        int cardID = 0;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        public override void SetCharacter(PlayerData data, CharacterStatController stat)
        {
            base.SetCharacter(data, stat);
            EnemyAppear(true);
        }

        protected override void Update()
        {
            if (canEndAction == false)
                canEndAction = true;
            if (knockbackTime > 0)
                UpdateKnockback();
            else
            {
                if (state == CharacterState.Idle || state == CharacterState.Moving || state == CharacterState.Acting)
                {
                    //EnemyDecision();
                }
            }
            if (state != CharacterState.Throw)
                ApplyGravity();
            else
                UpdateThrow();
            UpdateCollision();
            SetAnimation();
            EndActionState();
            // Les animations events sont joué après l'Update
        }

        public void EnemyAppear(bool b)
        {
            if(b == true)
            {
                StartCoroutine(EnemyAppearCoroutine());
            }
            else
            {
                StartCoroutine(EnemyDisappearCoroutine());
            }
        }

        private IEnumerator EnemyAppearCoroutine()
        {
            Vector3 startPosition = new Vector3(1, 0, 1);
            Vector3 finalPosition = new Vector3(1, 1, 1);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / timeEnemyAppear;
                this.transform.localScale = Vector3.Lerp(startPosition, finalPosition, t);
                yield return null;
            }
        }

        private IEnumerator EnemyDisappearCoroutine()
        {
            characterCollider.enabled = false;
            Vector3 startPosition = new Vector3(1, 1, 1);
            Vector3 finalPosition = new Vector3(1, 0, 1);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / timeEnemyDisappear;
                this.transform.localScale = Vector3.Lerp(startPosition, finalPosition, t);
                yield return null;
            }
            Destroy(this.gameObject);
        }




        public void EnemyDecision()
        {
            if (enemyActionTime <= 0) 
            {
                state = CharacterState.Idle;
                enemyPatternID = Random.Range(1, 4 + 1);
                switch (enemyPatternID) 
                {
                    case 1: // Move random
                        randomPosition = Random.insideUnitCircle * 1f;
                        randomPosition.Normalize();
                        randomPosition *= 1.5f;
                        enemyActionTime = Random.Range(180, 360) / 60f;
                        break;
                    case 2: // Wait
                        SetSpeed(0, 0);
                        enemyActionTime = Random.Range(60, 240) / 60f;
                        break;
                    case 3: // Move in front of player
                        LookAt(target.transform);
                        randomPosition = Vector3.left * direction;
                        enemyActionTime = Random.Range(180, 360) / 60f;
                        break;
                    case 4: // Move in front of player
                        Guard();
                        Debug.Log("Guard");
                        LookAt(target.transform);
                        enemyActionTime = Random.Range(180, 360) / 60f;
                        break;
                }
            }


            if(state != CharacterState.Acting)
                enemyActionTime -= Time.deltaTime * characterMotionSpeed;

            switch (enemyPatternID)
            {
                case 1:
                    movementPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0) + randomPosition;
                    if (MoveToPoint(movementPosition, 0, 0) == true)
                        enemyActionTime = 0;
                    LookAt(target.transform);
                    break;

                case 2:
                    LookAt(target.transform);
                    break;

                case 3:
                    movementPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0) + randomPosition;
                    if (MoveToPoint(movementPosition, 0, 0) == true)
                    {
                        enemyPatternID = 4;
                        enemyActionTime = 0;
                    }
                    LookAt(target.transform);
                    break;

                case 4:
                    LookAt(target.transform);
                    break;


            }

        }



        private void LookAt(Transform targetPos)
        {
            if (targetPos.position.x < this.transform.position.x)
                direction = -1;
            if (targetPos.position.x > this.transform.position.x)
                direction = 1;
        }
        #endregion

    } 

} // #PROJECTNAME# namespace