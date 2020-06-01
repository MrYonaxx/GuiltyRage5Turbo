﻿/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public enum CharacterState
    {
        Idle,
        Moving,
        Guard,
        Acting,
        Hit,
        Down,
        Dead,
        Throw
    }


    public class Character: SerializedMonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Title("CharacterController")]
        [SerializeField]
        protected Rigidbody2D characterRigidbody;
        [SerializeField]
        protected BoxCollider2D characterCollider;
        [SerializeField]
        protected Animator characterAnimator;

        [SerializeField]
        protected SpriteRenderer spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get { return spriteRenderer; }
        }
        [SerializeField]
        protected Transform particlePoint;
        public Transform ParticlePoint
        {
            get { return particlePoint; }
        }
        [SerializeField]
        protected Transform throwPoint;
        public Transform ThrowPoint
        {
            get { return throwPoint; }
        }




        [Title("Collision")]
        [SerializeField]
        protected float offsetRaycastX = 0.0001f;
        [SerializeField]
        protected float offsetRaycastY = 0.0001f;
        [HorizontalGroup("RaycastNumber")]
        [SerializeField]
        protected int numberRaycastVertical = 2;
        [HorizontalGroup("RaycastNumber")]
        [SerializeField]
        protected int numberRaycastHorizontal = 2;


        [Title("Parameter")]
        [SerializeField]
        BattleFeedbackManagerData battleFeedback;

        /*[SerializeField]
        protected CharacterData characterData;
        public CharacterData CharacterData
        {
            get { return characterData; }
        }*/

        [SerializeField]
        [HideLabel]
        protected CharacterStatController characterStat;
        public CharacterStatController CharacterStat
        {
            get { return characterStat; }
        }

        //[HideInInspector]
        //[SerializeField]
        protected Character target;
        public Character Target
        {
            get { return target; }
        }

        [SerializeField]
        protected ICharacterInfoDrawer healthBar;
        public ICharacterInfoDrawer HealthBar
        {
            get { return healthBar; }
        }

        [SerializeField]
        protected ICharacterController[] characterControllers;
        public ICharacterController[] CharacterControllers
        {
            get { return characterControllers; }
        }



        [SerializeField]
        protected CharacterState state = CharacterState.Idle;
        public CharacterState State
        {
            get { return state; }
        }




        protected float speedX = 0;
        protected float speedY = 0;
        protected float speedZ = 0;

        protected float actualSpeedX = 0;
        protected float actualSpeedY = 0;

        protected float characterMotionSpeed = 1;

        protected bool invulnerableState = false;
        protected bool noKnockback = false;

        protected float knockbackTime = 0;
        protected int knockbackAnimation = 0;

        protected float knockdownValue = 0;


        protected AttackController previousAttack; // Pour les combos
        protected AttackController currentAttack;
        protected AttackController currentAttackController;

        protected float throwerScale = 1;
        protected int throwDirection = 0;
        protected Transform throwTransform;

        protected bool endAction = false;
        protected bool canEndAction = false;
        protected bool canMoveCancel = false;
        protected bool canTargetCombo = false;


        [FoldoutGroup("Advanced")]
        [SerializeField]
        protected bool active = true;

        [FoldoutGroup("Advanced")]
        [SerializeField]
        protected int direction = 1;

        [FoldoutGroup("Advanced")]
        [SerializeField]
        protected bool inAir = false;

        [FoldoutGroup("Advanced")]
        [SerializeField]
        protected bool autoCombo = false;



        [Title("Event")]
        [FoldoutGroup("Events")]
        [SerializeField]
        UnityEventCharacterBattle OnDead;
        [FoldoutGroup("Events")]
        [SerializeField]
        UnityEventAttackBehavior OnHit;
        [FoldoutGroup("Events")]
        [SerializeField]
        UnityEventCharacterBattle OnGuard;
        [FoldoutGroup("Events")]
        [SerializeField]
        UnityEvent OnActionHit;
        [FoldoutGroup("Events")]
        [SerializeField]
        UnityEvent OnActionEnd;
        [FoldoutGroup("Events")]
        [SerializeField]
        UnityEventCharacterBattle OnGuardBreak;
        [FoldoutGroup("Events")]
        [SerializeField]
        UnityEventCharacterBattle OnWallBounce;



        int layerMask;
        Vector2 bottomLeft;
        Vector2 upperLeft;
        Vector2 bottomRight;
        Vector2 upperRight;

        Transform collisionInfo;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
        public int GetDirection()
        {
            return direction;
        }

        public void SetTarget(Character value)
        {
            target = value;
            if (healthBar != null)
                healthBar.DrawTarget(value.CharacterStat.GetHP(), value.CharacterStat.GetHPMax(), value.CharacterStat.CharacterData.CharacterName);
        }

        public void ShowCharacter(bool b)
        {
            if(b == false)
                this.transform.localScale = Vector3.zero;
            else
                this.transform.localScale = Vector3.one;
            SetActive(b);
        }

        public void SetActive(bool b)
        {
            active = b;
            if(b == false)
            {
                CancelAction();
                SetSpeed(0, 0);
            }
            else
            {
                ResetToIdle();
            }
        }

        public void SetHealthBar(ICharacterInfoDrawer drawer)
        {
            healthBar = drawer;
            healthBar.DrawCharacter(characterStat.CharacterData);
        }

        public void SetAutoCombo(bool b)
        {
            autoCombo = b;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void OnDestroy()
        {
            battleFeedback.RemoveCharacter(this);
            OnWallBounce.RemoveAllListeners();
            OnGuardBreak.RemoveAllListeners();
            OnGuard.RemoveAllListeners();
            OnHit.RemoveAllListeners();
            OnDead.RemoveAllListeners();
        }

        public virtual void SetCharacter(CharacterData data, CharacterStatController stat)
        {
            //characterData = data;
            characterStat = stat;
        }

        protected virtual void Start()
        {
            battleFeedback.AddCharacter(this);
            characterStat.CreateStatController();
            //SetCharacter(characterData, new CharacterStatController(characterData));
        }

        protected virtual void Update()
        {
            // On trigger enter / On collider enter
            if (canEndAction == false)
                canEndAction = true;

            UpdateController();

            if (state != CharacterState.Throw)
                ApplyGravity();
            else
                UpdateThrow();
            if (knockbackTime > 0)
                UpdateKnockback();
            UpdateCollision();
            SetAnimation();

            LateUpdateController();
                         
            EndActionState();
            // Les animations events sont joué après l'Update
        }

        protected virtual void UpdateController()
        {
            if (active == false)
                return;
            if (characterControllers == null)
                return;
            for (int i = 0; i < characterControllers.Length; i++)
                characterControllers[i].UpdateController(this);
        }

        protected virtual void LateUpdateController()
        {
            if (active == false)
                return;
            if (characterControllers == null)
                return;
            for (int i = 0; i < characterControllers.Length; i++)
                characterControllers[i].LateUpdateController(this);
        }

        protected void UpdateCollision()
        {
            actualSpeedX = speedX;
            actualSpeedY = speedY;
            actualSpeedX *= characterMotionSpeed;
            actualSpeedY *= characterMotionSpeed;

            layerMask = 1 << 8;

            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            UpdatePositionX();

            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            UpdatePositionY();

            transform.position += new Vector3(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime, actualSpeedY * Time.deltaTime);

        }

        private void UpdatePositionX()
        {

            RaycastHit2D raycastX;
            Vector2 originRaycast;

            if (actualSpeedX < 0)
            {
                //RaycastCollision(bottomLeft, upperLeft, offsetRaycastX, numberRaycastHorizontal);
                // ======================================================================================================
                originRaycast = bottomLeft - new Vector2(offsetRaycastX, 0);
                for (int i = 0; i < numberRaycastHorizontal; i++)
                {
                    raycastX = Physics2D.Raycast(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Mathf.Abs(actualSpeedX * Time.deltaTime) + offsetRaycastX, layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                    if (raycastX.collider != null)
                    {
                        collisionInfo = raycastX.collider.transform;
                        float distance = raycastX.point.x - bottomLeft.x;
                        distance += offsetRaycastX;
                        actualSpeedX = distance / Time.deltaTime;
                        OnWallCollision();
                        return;

                    }
                    originRaycast += new Vector2(0, Mathf.Abs(upperLeft.y - bottomLeft.y) / (numberRaycastHorizontal - 1));
                }
                // ======================================================================================================

            }
            else if (actualSpeedX > 0)
            {
                //RaycastCollision(bottomRight, upperRight, offsetRaycastX, numberRaycastHorizontal);
                // ======================================================================================================
                originRaycast = bottomRight + new Vector2(offsetRaycastX, 0);
                for (int i = 0; i < numberRaycastHorizontal; i++)
                {
                    raycastX = Physics2D.Raycast(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Mathf.Abs(actualSpeedX * Time.deltaTime) + offsetRaycastX, layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                    if (raycastX.collider != null)
                    {
                        collisionInfo = raycastX.collider.transform;
                        float distance = raycastX.point.x - bottomRight.x;
                        distance -= offsetRaycastX;
                        actualSpeedX = distance / Time.deltaTime;
                        OnWallCollision();
                        return;
                    }
                    originRaycast += new Vector2(0, Mathf.Abs(upperRight.y - bottomRight.y) / (numberRaycastHorizontal - 1));
                }
                // ======================================================================================================

            }
        }


        private void UpdatePositionY()
        {

            RaycastHit2D raycastY;
            Vector2 originRaycast;

            if (actualSpeedY < 0)
            {
                // ======================================================================================================
                originRaycast = bottomLeft - new Vector2(0, offsetRaycastY);
                for (int i = 0; i < numberRaycastVertical; i++)
                {
                    raycastY = Physics2D.Raycast(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Mathf.Abs(actualSpeedY * Time.deltaTime) + offsetRaycastY, layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Color.yellow);
                    if (raycastY.collider != null)
                    {
                        collisionInfo = raycastY.collider.transform;
                        float distance = raycastY.point.y - bottomLeft.y;
                        distance += offsetRaycastY;
                        actualSpeedY = distance / Time.deltaTime;
                        return;

                    }
                    originRaycast += new Vector2(Mathf.Abs(bottomRight.x - bottomLeft.x) / (numberRaycastVertical - 1), 0);
                }
                // ======================================================================================================

            }
            else if (actualSpeedY > 0)
            {
                // ======================================================================================================
                originRaycast = upperLeft + new Vector2(0, offsetRaycastY);
                for (int i = 0; i < numberRaycastVertical; i++)
                {
                    raycastY = Physics2D.Raycast(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Mathf.Abs(actualSpeedY * Time.deltaTime) + offsetRaycastY, layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Color.yellow);
                    if (raycastY.collider != null)
                    {
                        collisionInfo = raycastY.collider.transform;
                        float distance = raycastY.point.y - upperLeft.y;
                        distance -= offsetRaycastY;
                        actualSpeedY = distance / Time.deltaTime;
                        return;
                    }
                    originRaycast += new Vector2(Mathf.Abs(upperRight.x - upperLeft.x) / (numberRaycastVertical - 1), 0);
                }
                // ======================================================================================================

            }
        }


        protected virtual void OnWallCollision()
        {
            WallBounce();
        }

        private void WallBounce()
        {
            if (state == CharacterState.Hit && inAir == true)
            {
                direction *= -1;
                speedX = -speedX * 0.5f;
                /*if(speedZ > 0)
                    speedZ *= 2;*/
                SetCharacterMotionSpeed(0, 0.25f);
                knockbackTime = characterStat.GetKnockbackTime();
                knockbackAnimation += 1;
                if (knockbackAnimation == 2)
                    knockbackAnimation = 0;
                characterAnimator.SetTrigger("Hit");
                characterAnimator.SetInteger("HitAnimation", knockbackAnimation);
                OnWallBounce.Invoke(this);
            }
        }





        protected void JumpDefault()
        {
            speedZ = characterStat.GetJumpImpulsion();
            inAir = true;
        }

        public void Jump(float impulsion)
        {
            speedZ = impulsion;
            inAir = true;
        }

        protected void ApplyGravity()
        {
            if(inAir == true)
            {
                speedZ -= ((characterStat.GetGravity() * Time.deltaTime) * characterMotionSpeed);
                speedZ = Mathf.Max(speedZ, characterStat.GetGravityMax());
                spriteRenderer.transform.localPosition += new Vector3(0, (speedZ * Time.deltaTime) * characterMotionSpeed, 0);
                if (spriteRenderer.transform.localPosition.y <= 0 && characterMotionSpeed != 0)
                {
                    if(state == CharacterState.Hit || state == CharacterState.Dead)
                    {
                        CharacterDown();
                    }
                    inAir = false;
                    speedZ = 0;
                    spriteRenderer.transform.localPosition = new Vector3(spriteRenderer.transform.localPosition.x, 0, spriteRenderer.transform.localPosition.z);
                    OnGroundCollision();
                }
            }
        }

        protected virtual void OnGroundCollision()
        {

        }

        protected void SetAnimation()
        {
            if (direction == 1)
                spriteRenderer.flipX = false;
            else if (direction == -1)
                spriteRenderer.flipX = true;
            characterAnimator.SetBool("AerialUp", inAir);
            if (inAir == true && speedZ <= 0)
                characterAnimator.SetBool("AerialDown", true);
            else
                characterAnimator.SetBool("AerialDown", false);

            if (state == CharacterState.Idle || state == CharacterState.Moving)
                characterAnimator.SetBool("Moving", (speedX != 0 || speedY != 0));

            characterAnimator.SetBool("Guard", (state == CharacterState.Guard));
        }



        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "EnemyAttack" || col.tag == "PlayerAttack")
            {
                AttackController attackIncoming = col.GetComponent<AttackController>();
                // Hit
                if (CheckIfHit(attackIncoming) == true)
                {
                    // Guard
                    if (state == CharacterState.Guard && attackIncoming.AttackBehavior.GuardBreak)
                    {
                        GuardBreak();
                    }
                    else if (state == CharacterState.Guard)
                    {
                        OnGuard.Invoke(this);
                        return;
                    }
                    characterStat.TakeDamage(attackIncoming.AttackBehavior);
                    attackIncoming.HasHit(this);
                    if (characterStat.GetHP() <= 0 && state != CharacterState.Dead)
                    {
                        Knockback(attackIncoming);
                        KnockbackDeath(attackIncoming);
                    }
                    else
                    {
                        Knockback(attackIncoming);
                    }
                }
            }
        }

        private bool CheckIfHit(AttackController attack)
        {
            if (invulnerableState == true)
                return false;
            if (attack.CheckCollisionY(this.transform.position.y) == false)
                return false;
            if (state == CharacterState.Dead && inAir == false)
                return false;
            if (state == CharacterState.Throw && attack.AttackBehavior.AttackThrow == false)
                return false;
            if (inAir == true && attack.AttackBehavior.ThrowState == true)
                return false;
            if (state == CharacterState.Down && attack.AttackBehavior.WakeUpAttack == false)
                return false;
            if (this.tag + "Attack" != attack.tag)
                return true;
            return false;
        }

        public void KnockbackDeath(AttackController attack)
        {
            state = CharacterState.Dead;
            speedZ += 1;
            inAir = true;
            characterAnimator.SetTrigger("Dead");
            OnDead.Invoke(this);
        }

        public void Knockback(AttackController attack)
        {
            if(noKnockback == true && state != CharacterState.Dead)
            {
                if (healthBar != null)
                    healthBar.DrawHealth(characterStat.GetHP(), characterStat.GetHPMax());
                OnHit.Invoke(attack.AttackBehavior);
                return;
            }
            CancelAct();
            if (attack.AttackBehavior.ThrowState == true)
            {
                state = CharacterState.Throw;
                return;
            }
            if(state != CharacterState.Dead)
                state = CharacterState.Hit;

            if(inAir == true)
            {
                if (attack.Direction == 0)
                    speedX = attack.AttackBehavior.KnockbackAerialPowerX * characterStat.GetMass() * -direction;
                else
                    speedX = attack.AttackBehavior.KnockbackAerialPowerX * characterStat.GetMass() * attack.Direction;
                speedY = 0;
                if (attack.AttackBehavior.ResetGravity == true)
                    speedZ = attack.AttackBehavior.KnockbackAerialPowerZ * characterStat.GetMass();
                else
                    speedZ += attack.AttackBehavior.KnockbackAerialPowerZ * characterStat.GetMass();
            }
            else
            {
                if (attack.Direction == 0)
                    speedX = attack.AttackBehavior.KnockbackPowerX * characterStat.GetMass() * -direction;
                else
                    speedX = attack.AttackBehavior.KnockbackPowerX * characterStat.GetMass() * attack.Direction;
                speedY = 0;
                speedZ = attack.AttackBehavior.KnockbackPowerZ * characterStat.GetMass();
                if (attack.AttackBehavior.KnockbackPowerZ > 0)
                {
                    inAir = true;
                }
            }

            knockdownValue += attack.AttackBehavior.KnockdownValue;
            if (knockdownValue >= characterStat.GetKnockdownResistance())
            {
                CharacterDown();
            }
            else
            {
                knockbackTime = characterStat.GetKnockbackTime();
                knockbackAnimation += 1;
                if (knockbackAnimation == 2)
                    knockbackAnimation = 0;
                characterAnimator.SetTrigger("Hit");
                characterAnimator.SetInteger("HitAnimation", knockbackAnimation);
            }
            if(healthBar != null)
                healthBar.DrawHealth(characterStat.GetHP(), characterStat.GetHPMax());
            OnHit.Invoke(attack.AttackBehavior);
        }


        protected void UpdateKnockback()
        {
            knockbackTime -= Time.deltaTime * characterMotionSpeed;
            if (inAir == false)
            {
                speedX = Mathf.Lerp(speedX, 0, 0.2f * characterMotionSpeed);
            }
            else
            {
                knockbackTime = 0.1f;
            }
            if (knockbackTime <= 0)
            {
                if (state == CharacterState.Hit || state == CharacterState.Down)
                {
                    state = CharacterState.Idle;
                    characterAnimator.SetTrigger("Idle");
                }
                speedX = 0;
                speedY = 0;
                knockdownValue = 0;
            }
        }

        public void CharacterDown()
        {
            if (state == CharacterState.Dead)
            {
                characterAnimator.SetTrigger("Dead");
                battleFeedback.RemoveCharacter(this);
                StartCoroutine(DisappearCoroutine());
                return;
            }
            state = CharacterState.Down;
            characterAnimator.SetTrigger("Down");
            knockbackTime = characterStat.GetWakeUpRecovery();
            speedX = 0;
            speedY = 0;

        }


        private IEnumerator DisappearCoroutine()
        {
            yield return new WaitForSeconds(2f);
            characterCollider.enabled = false;
            Vector3 startPosition = new Vector3(1, 1, 1);
            Vector3 finalPosition = new Vector3(1, 0, 1);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                this.transform.localScale = Vector3.Lerp(startPosition, finalPosition, t);
                yield return null;
            }
            Destroy(this.gameObject);
        }







        public void Throw(Transform throwAttach, int direction, float scale)
        {
            throwerScale = scale;
            throwDirection = direction;
            state = CharacterState.Throw;
            throwTransform = throwAttach;
            speedX = 0;
            speedY = 0;
            knockbackAnimation += 1;
            if (knockbackAnimation == 2)
                knockbackAnimation = 0;
            characterAnimator.SetTrigger("Hit");
            characterAnimator.SetInteger("HitAnimation", knockbackAnimation);
            //this.transform.SetParent(throwTransform);
        }

        public void UpdateThrow()
        {
            float x = throwTransform.position.x - (throwTransform.localPosition.x * throwerScale) + (throwTransform.localPosition.x * throwerScale * throwDirection);
            float y = throwTransform.position.y - (throwTransform.localPosition.y * throwerScale);
            MoveToPointInstant(new Vector3(x, y, throwTransform.position.z));
            spriteRenderer.transform.localPosition = new Vector3(spriteRenderer.transform.localPosition.x, throwTransform.localPosition.y * throwerScale, throwTransform.localPosition.z);
        }





        /// <summary>
        /// Cancel Action mais sans retour à l'idle
        /// </summary>
        public void CancelAct()
        {
            if (currentAttackController != null)
                currentAttackController.CancelAction();
            currentAttack = null;
            previousAttack = null;
            currentAttackController = null;

            canMoveCancel = false;
            canEndAction = false;
            endAction = false;
            canTargetCombo = false;

            noKnockback = false;
            invulnerableState = false;

            state = CharacterState.Idle;
        }

        public void CancelAction()
        {
            CancelAct();

            characterAnimator.SetTrigger("Idle");
        }






        public bool CanAct()
        {
            if (state == CharacterState.Idle || state == CharacterState.Moving)
                return true;
            else if (canMoveCancel == true && currentAttack.AttackBehavior.ComboTo != null)
                return true;
            else if (canMoveCancel == true && canTargetCombo == true && currentAttack.AttackBehavior.TargetCombo != null)
                return true;
            return false;
        }

        private AttackController CheckCombo(AttackController action)
        {
            if (canMoveCancel == true && currentAttack != null)
            {
                if (previousAttack == action)
                {
                    if (canTargetCombo == true && currentAttack.AttackBehavior.TargetCombo != null)
                    {
                        return currentAttack.AttackBehavior.TargetCombo;
                    }
                    else if (currentAttack.AttackBehavior.ComboTo != null)
                    {
                        return currentAttack.AttackBehavior.ComboTo;
                    }
                }
            }
            return action;
        }

        public void Action(AttackController action)
        {
            if (action == null)
                return;
            state = CharacterState.Acting;         
            currentAttack = CheckCombo(action);
            previousAttack = action;
            characterAnimator.ResetTrigger("Idle");
            //characterAnimator.Play(currentAttack.AttackBehavior.AttackAnimation2.name, 0, 0f);
            characterAnimator.Play(currentAttack.AttackBehavior.AttackAnimation, 0, 0f);
            endAction = false;
            canEndAction = false;
            canMoveCancel = false;
            canTargetCombo = false;
            noKnockback = action.AttackBehavior.IsArmor;
            invulnerableState = action.AttackBehavior.IsInvulnerable;
            if (currentAttack.AttackBehavior.KeepMomentum == false) 
            {
                speedX = 0;
                speedY = 0;
            }

        }

        // Appelé par les anims
        public void ActionActive()
        {
            if (currentAttack != null)
            {
                currentAttackController = (Instantiate(currentAttack, this.transform.position, Quaternion.identity));
                currentAttackController.CreateAttack(this, target);
            }
        }

        public void ActionReactive()
        {
            if (currentAttack != null)
            {
                currentAttackController.ActionActive();
            }
        }

        // Appelé par les anims
        public void ActionActiveInstant(AttackController attack)
        {
            currentAttackController = (Instantiate(attack, this.transform.position, Quaternion.identity));
            currentAttackController.CreateAttack(this, target);
        }

        // Appelé par les anims
        public void MoveCancelable()
        {
            if (canTargetCombo == false && currentAttack.AttackBehavior.CancelOnlyOnHit == true)
                return;
            canMoveCancel = true;
            if(autoCombo == true && canTargetCombo == true)
            {
                Action(currentAttack.AttackBehavior.ComboTo);
            }
        }

        // Appelé par les anims
        public void EndAction()
        {
            if (state == CharacterState.Acting && canEndAction == true)
            {
                endAction = true;
            }
        }

        // Appelé par l'attack controller
        /*public void EndActionAttackController()
        {
            //currentAttackController = null;
            if (state == CharacterState.Acting && canEndAction == true)
            {
                endAction = true;
            }
        }*/

        protected void EndActionState()
        {
            if (endAction == true)
            {
                CancelAction();
                OnActionEnd.Invoke();
            }
        }


        public void HitConfirm()
        {
            canTargetCombo = true;
        }





        public void Guard()
        {
            state = CharacterState.Guard;
            characterAnimator.SetBool("Guard", true);
            //OnGuardBreak.Invoke(this);
        }

        public void GuardBreak()
        {
            OnGuardBreak.Invoke(this);
        }











        public void ResetToIdle()
        {
            state = CharacterState.Idle;
            characterAnimator.SetTrigger("Idle");
        }

        public void MoveTo(Transform t)
        {
            this.transform.position = t.position;
        }

        public void MoveToTarget()
        {
            Vector2 direction = target.transform.position - this.transform.position;
            direction.Normalize();
            SetSpeed(direction.x * characterStat.GetSpeed(), direction.y * characterStat.GetSpeed());
        }

        public void MoveToTargetHorizontal(float multiplier)
        {
            Vector2 direction = target.transform.position - this.transform.position;
            direction.Normalize();
            SetSpeed((1 * GetDirection() * characterStat.GetSpeed() * multiplier), direction.y * characterStat.GetSpeed() * multiplier);
        }


        public void MoveToPointInstant(Vector3 point)
        {
            Vector2 direction = point - this.transform.position;
            SetSpeed(direction.x / Time.deltaTime, direction.y / Time.deltaTime);
        }


        public bool MoveToPoint(Vector3 point, float time, float totalTime)
        {
            Vector2 direction = point - this.transform.position;
            if (Mathf.Abs(direction.magnitude) < 0.1f)
            {
                SetSpeed(0, 0);
                return true;
            }
            else
            {
                direction.Normalize();
                SetSpeed(direction.x * characterStat.GetSpeed(), direction.y * characterStat.GetSpeed());
                return false;
            }
        }
        public void MoveForward(float multiplier)
        {
            SetSpeed(characterStat.GetSpeed() * multiplier * direction, 0);
        }

        public void SetSpeed(float newSpeedX, float newSpeedY)
        {
            speedX = newSpeedX;
            speedY = newSpeedY;
        }

        public void LookAt(Transform targetPos)
        {
            LookAtPosition(targetPos.transform.position);
        }

        public void LookAtPosition(Vector3 targetPos)
        {
            if (targetPos.x < this.transform.position.x)
                direction = -1;
            if (targetPos.x > this.transform.position.x)
                direction = 1;
        }

        public void TurnBack()
        {
            direction = -direction;
            if (direction == 1)
                spriteRenderer.flipX = false;
            else if (direction == -1)
                spriteRenderer.flipX = true;
        }


        public void SetCharacterMotionSpeed(float newSpeed, float time = 0)
        {
            characterMotionSpeed = newSpeed;
            characterAnimator.speed = characterMotionSpeed;
            if (currentAttackController != null)
                currentAttackController.AttackMotionSpeed(newSpeed);
            if(time > 0)
            {
                StartCoroutine(MotionSpeedCoroutine(time));
            }
        }


        private IEnumerator MotionSpeedCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            characterMotionSpeed = characterStat.GetMotionSpeed();
            characterAnimator.speed = characterMotionSpeed;
            if (currentAttackController != null)
                currentAttackController.AttackMotionSpeed(characterMotionSpeed);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace