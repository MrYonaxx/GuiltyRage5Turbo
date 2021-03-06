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

        [Title("Attack Controller")]
        [SerializeField]
        BoxCollider2D hitbox;
        [SerializeField]
        BoxCollider2D hitboxY;
        [SerializeField]
        Animator animator;

        [Space]
        [Space]
        [Space]

        [HideLabel]
        [SerializeField]
        private AttackBehavior attackBehavior;
        public AttackBehavior AttackBehavior
        {
            get { return attackBehavior; }
        }



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
            if (attackBehavior.LinkToCharacterAerial == true && hitboxY != null)
                hitboxY.transform.position = user.transform.position;
            // Pattern de l'attaque
        }

        public void CreateAttack(Character cUser, Character lockedTarget)
        {
            user = cUser;
            lockedCharacter = lockedTarget;

            // direction
            //Debug.Log(user.SpriteRenderer.transform.localScale.y);
            // direction
            this.transform.localScale = new Vector3(this.transform.localScale.x * user.SpriteRenderer.transform.localScale.x * cUser.GetDirection(), this.transform.localScale.y * user.SpriteRenderer.transform.localScale.y, user.SpriteRenderer.transform.localScale.z);
            if (attackBehavior.NoDirection == false)
                direction = cUser.GetDirection();

            // hitbox
            hitbox.enabled = attackBehavior.IsActive;

            // Link to user
            if (attackBehavior.LinkToCharacterAerial == true)
            {
                this.transform.SetParent(cUser.SpriteRenderer.transform);
                this.transform.localPosition = Vector3.zero;
                if(hitboxY != null)
                    hitboxY.transform.position = user.transform.position;
            }
            else if (attackBehavior.LinkToCharacter == true)
            {
                this.transform.SetParent(cUser.transform);
            }



            // Tag
            this.tag = cUser.tag;

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
            float lifetime = Mathf.Max(0, (attackBehavior.Lifetime * 60) - currentFrame) / 60f;
            while (lifetime > 0)
            {
                lifetime -= Time.deltaTime * motionSpeed;
                yield return null;
            }
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
                case AttackBehaviorAction.MoveTo:
                    this.transform.position = new Vector3(attackBehaviorData.argument1.x, attackBehaviorData.argument1.y, attackBehaviorData.argument1.z);
                    break;
                case AttackBehaviorAction.Move:
                    this.transform.position += new Vector3(attackBehaviorData.argument1.x * direction, attackBehaviorData.argument1.y, attackBehaviorData.argument1.z);
                    break;
            }
            if (attackBehaviorData.anim != null)
            {

            }
            if (attackBehaviorData.move != null)
            {
                subActions.Add(Instantiate(attackBehaviorData.move, this.transform.position, Quaternion.identity));
                subActions[subActions.Count - 1].CreateAttack(user, lockedCharacter);
                //subActions[subActions.Count - 1].SetDirection(-1);
            }
        }





        public void SetDirection(int direction)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * direction, this.transform.localScale.y, this.transform.localScale.z);
        }



        public bool CheckCollisionY(float posY)
        {
            if (hitboxY == null)
                return true;
            float scaleY = user.SpriteRenderer.transform.localScale.y;
            return ((hitboxY.transform.position.y + ((hitboxY.offset.y - (hitboxY.size.y / 2)) * scaleY)) <= posY && posY <= (hitboxY.transform.position.y + ((hitboxY.offset.y + (hitboxY.size.y / 2)) * scaleY)));
        }


        public void HasHit(Character target)
        {
            target.PlaySound(attackBehavior.OnHitSound);
            //user.SetTarget(target);
            user.HitConfirm();
            if (attackBehavior.OnHitCombo != null)
                user.Action(attackBehavior.OnHitCombo);
            if (attackBehavior.UserKnockbackX != 0 || attackBehavior.UserKnockbackZ != 0)
            {
                user.SetSpeed(attackBehavior.UserKnockbackX * -direction, 0);
                user.Jump(attackBehavior.UserKnockbackZ);
            }

            //Feedback
            if (attackBehavior.OnHitAnimation != null)
                Instantiate(attackBehavior.OnHitAnimation, target.ParticlePoint.position, Quaternion.identity);
            if (attackBehavior.HitStopGlobal == false && attackBehavior.HitStop > 0) 
            {
                target.SetCharacterMotionSpeed(0, attackBehavior.HitStop);
                if(attackBehavior.HitStopUser == true)
                    user.SetCharacterMotionSpeed(0, attackBehavior.HitStop);
                else if (attackBehavior.HitStopProjectile == true)
                {
                    AttackMotionSpeed(0);
                    StartCoroutine(MotionSpeedCoroutine(attackBehavior.HitStop));
                }
            }

            if (attackBehavior.ThrowState == true)
            {
                target.Throw(user.ThrowPoint, direction, user.SpriteRenderer.transform.localScale.x);
                user.SetCharacterToThrow(target);
            }
            if (attackBehavior.IsMultiHit == false)
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

        /*public void SetCharacterMotionSpeed(float newSpeed, float time = 0)
        {
            AttackMotionSpeed(newSpeed);
            if (time > 0)
            {
                StartCoroutine(MotionSpeedCoroutine(time));
            }
        }*/


        private IEnumerator MotionSpeedCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            AttackMotionSpeed(1);
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
            //user.EndActionAttackController();
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