﻿/*****************************************************************
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
    public class AttackController: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [HideLabel]
        [SerializeField]
        private AttackBehavior attackBehavior;
        public AttackBehavior AttackBehavior
        {
            get { return attackBehavior; }
        }


        /*[SerializeField]
        AttackData attackData;
        public AttackData AttackData
        {
            get { return attackData; }
        }*/
        [Space]
        [Space]
        [Space]
        [Title("Attack Controller")]
        [SerializeField]
        BoxCollider2D hitbox;
        [SerializeField]
        Animator animator;

        Character user;

        Character lockedCharacter; // si le projectile est à tête chercheuse

        [SerializeField]
        private int direction;
        public int Direction
        {
            get { return direction; }
        }


        Vector3 speed = Vector3.zero;
        float motionSpeed = 1f;
        List<AttackController> subActions = new List<AttackController>();
        List<GameObject> fxParticle;

        IEnumerator attackCoroutine;


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

        public void Update()
        {
            transform.position += (speed * Time.deltaTime) * motionSpeed;
            // Pattern de l'attaque
        }

        public void CreateAttack(Character cUser, Character lockedTarget)
        {
            user = cUser;
            lockedCharacter = lockedTarget;

            // direction
            this.transform.localScale = new Vector3(this.transform.localScale.x * cUser.GetDirection(), this.transform.localScale.y, this.transform.localScale.z);
            if (attackBehavior.NoDirection == false)
                direction = cUser.GetDirection();

            // hitbox
            hitbox.enabled = attackBehavior.IsActive;

            if (attackBehavior.LinkToCharacter == true)
                this.transform.SetParent(cUser.transform);
            this.tag = cUser.tag + "Attack";


            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            attackCoroutine = AttackBehaviorCoroutine();
            StartCoroutine(attackCoroutine);
        }


        private IEnumerator AttackBehaviorCoroutine()
        {
            int currentFrame = 0; 
            for(int i = 0; i < attackBehavior.AttackBehaviorDatas.Length; i++)
            {
                float time = attackBehavior.AttackBehaviorDatas[i].frame - currentFrame;
                currentFrame += (int) time;
                time /= 60f;
                while (time > 0)
                {
                    time -= Time.deltaTime * motionSpeed;
                    yield return null;
                }
                ExecuteActionBehavior(attackBehavior.AttackBehaviorDatas[i]);
            }
            yield return new WaitForSeconds(Mathf.Max(0,(attackBehavior.Lifetime * 60) - currentFrame) / 60f);
            ActionEnd();
        }

        private void ExecuteActionBehavior(AttackBehaviorData attackBehaviorData)
        {
            switch(attackBehaviorData.action)
            {
                case AttackBehaviorAction.Active:
                    ActionActive();
                    break;
                case AttackBehaviorAction.MoveForward:
                    speed = new Vector3(attackBehaviorData.argument1.x * direction, 0, 0);
                    break;
            }
            if (attackBehaviorData.anim != null)
            {

            }
            if (attackBehaviorData.move != null)
            {
                subActions.Add(Instantiate(attackBehaviorData.move, this.transform));
                subActions[subActions.Count - 1].CreateAttack(user, lockedCharacter);
                subActions[subActions.Count - 1].SetDirection(-1);
            }
        }


        public void SetDirection(int direction)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * direction, this.transform.localScale.y, this.transform.localScale.z);
        }






        public void HasHit(Character target)
        {
            if (attackBehavior.TargetCombo != null)
                user.HitConfirm();
            if (attackBehavior.OnHitAnimation != null)
                Instantiate(attackBehavior.OnHitAnimation, target.ParticlePoint.position, Quaternion.identity);
            if (attackBehavior.OnHitCombo != null)
                user.Action(attackBehavior.OnHitCombo);
            if (attackBehavior.ThrowState == true)
                target.Throw(user.ThrowPoint, direction, user.SpriteRenderer.transform.localScale.x);;
            if (attackBehavior.IsPiercing == false)
            {
                ActionEnd();
            }
            else
            {
                ActionUnactive();
            }
        }

        public void AttackMotionSpeed(float newMotionSpeed)
        {
            motionSpeed = newMotionSpeed;
            if (animator != null)
                animator.speed = motionSpeed;
            for (int i = 0; i < subActions.Count; i++)
            {
                subActions[i].AttackMotionSpeed(newMotionSpeed);
            }
        }



        public void ActionMoveToLock()
        {
            if(lockedCharacter != null)
                transform.position = lockedCharacter.transform.position;
        }

        public void ActionActive()
        {
            hitbox.enabled = true;
        }

        public void ActionUnactive()
        {
            hitbox.enabled = false;
        }

        public void ActionEnd()
        {
            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            if(attackBehavior.AnimationDriven == false)
                user.EndActionAttackController();
            CancelAction();
        }


        public void CancelAction()
        {
            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            for (int i = 0; i < subActions.Count; i++)
            {
                if(subActions[i] != null)
                    subActions[i].CancelAction();
            }
            Destroy(this.gameObject);
        }


        public void SetSpeedX(float speedX)
        {
            speed = new Vector3(speedX * direction, 0, 0);
        }

        public void TurnBack()
        {
            direction = -direction;
        }
        #endregion

    } 

} // #PROJECTNAME# namespace