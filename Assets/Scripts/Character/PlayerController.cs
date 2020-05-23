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
    public class PlayerController : Character
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Moveset")]
        [SerializeField]
        AttackController crouchController;

        [SerializeField]
        AttackController groundDefaultAttack;

        [SerializeField]
        AttackController jumpDefaultAttack;
        [SerializeField]
        AttackController jumpForwardAttack;

        [SerializeField]
        AttackController throwAttack;
        [SerializeField]
        AttackController specialAttack;
        [SerializeField]
        AttackController specialForwardAttack;

        [SerializeField]
        AttackController dashBack;
        [SerializeField]
        AttackController dashForward;

        [Title("Parameter")]
        [SerializeField]
        float crouchJumpTime = 0.1f;

        [Title("Buffer Parameter")]
        [SerializeField]
        float bufferTime = 0.25f;


        float crouchTime = 0;
        bool doubleJump = false;

        IEnumerator bufferCoroutine = null;
        bool bufferNormalActive = false;
        bool bufferSpecialActive = false;
        bool bufferJumpActive = false;
        bool bufferDashActive = false;

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

        /*protected override void Update()
        {
            if (canEndAction == false)
                canEndAction = true;

            if (canInput == true)
            {
                InputThrow();
                InputMovement();
                InputDash();
                InputJump();
                InputAction();
                InputSpecial();
            }
            ApplyGravity();
            if (knockbackTime > 0)
                UpdateKnockback();
            UpdateCollision();
            SetAnimation();
            EndActionState();
        }*/

        protected override void UpdateController()
        {
            if (active == true)
            {
                InputThrow();
                InputMovement();
                InputDash();
                InputJump();
                InputAction();
                InputSpecial();
            }
        }


        private void InputAction()
        {
            if (state != CharacterState.Idle && state != CharacterState.Moving && state != CharacterState.Acting)
            {
                return;
            }

            if (bufferNormalActive == true)
            {
                if (CanAct() == true)
                {
                    NormalAttack();
                    return;
                }
            }
            if (Input.GetButtonDown("ControllerX"))
            {
                if (CanAct() == true)
                {
                    NormalAttack();
                }
                else
                {
                    bufferNormalActive = true;
                    StartBuffer();
                }
            }
        }

        private void NormalAttack()
        {
            if(inAir == true && speedX != 0)
            {
                Action(jumpForwardAttack);
            }
            else if (inAir == true)
            {
                Action(jumpDefaultAttack);
            }
            else
            {
                Action(groundDefaultAttack);
            }
            StopBuffer();
        }

        // =========================================================================================
        private void InputSpecial()
        {
            if (state != CharacterState.Idle && state != CharacterState.Moving && state != CharacterState.Acting)
            {
                return;
            }

            // Buffer
            if (bufferSpecialActive == true && state == CharacterState.Acting && canMoveCancel == true)
            {
                SpecialAttack();
            }

            // Input
            if (Input.GetButtonDown("ControllerY") && state == CharacterState.Acting)
            {
                if (canMoveCancel == true)
                {
                    SpecialAttack();
                }
                else
                {
                    bufferSpecialActive = true;
                    StartBuffer();
                }
            }
            else if (Input.GetButtonDown("ControllerY"))
            {
                SpecialAttack();
            }
        }


        private void SpecialAttack()
        {
            if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
                Action(specialForwardAttack);
            else
                Action(specialAttack);
            StopBuffer();
        }




        // =========================================================================================
        private void InputThrow()
        {
            if (state != CharacterState.Idle && state != CharacterState.Moving || inAir != false)
            {
                return;
            }
            if (Input.GetButton("ControllerR1"))
            {
                Action(throwAttack);
            }
            StopBuffer();
        }



        // =========================================================================================
        private void InputMovement()
        {
            if (state != CharacterState.Idle && state != CharacterState.Moving || inAir != false)
            {
                return;
            }
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f)
            {
                speedY = Input.GetAxis("Vertical");
            }
            else
            {
                speedY = 0;
            }

            if (Input.GetAxis("Horizontal") > 0.2f)
            {
                speedX = Input.GetAxis("Horizontal");
                direction = 1;
            }
            else if (Input.GetAxis("Horizontal") < -0.2f)
            {
                speedX = Input.GetAxis("Horizontal");
                direction = -1;
            }
            else
            {
                speedX = 0;
            }
            Vector2 move = new Vector2(speedX, speedY);
            move.Normalize();
            speedX = move.x * defaultSpeed;
            speedY = move.y * defaultSpeed;

        }

        // =========================================================================================
        private void InputDash()
        {
            if ((Input.GetButtonDown("ControllerB") || bufferDashActive == true) && state == CharacterState.Acting && canMoveCancel == true && currentAttack != null)
            {
                if (currentAttack.AttackBehavior.DashCancel == true)
                    CancelAction();
            }
            else if (Input.GetButtonDown("ControllerB") && state == CharacterState.Acting)
            {
                bufferDashActive = true;
                StartBuffer();
            }

            if (state != CharacterState.Idle && state != CharacterState.Moving || inAir != false)
            {
                return;
            }

            if(bufferDashActive == true)
            {
                DashAction();
            }
            else if (Input.GetButtonDown("ControllerB"))
            {
                DashAction();
            }
        }

        private void DashAction()
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                Action(dashForward);
            }
            else
            {
                Action(dashBack);
            }
            StopBuffer();
        }


        // =========================================================================================
        private void InputJump()
        {

            if (crouchTime > 0)
            {
                crouchTime -= Time.deltaTime * characterMotionSpeed;
                if(crouchTime <= 0)
                {
                    speedX = defaultSpeed * direction;
                    JumpDefault();
                    endAction = true;
                    EndActionState();
                }
            }

            if((Input.GetButtonDown("ControllerA") || bufferJumpActive == true) && state == CharacterState.Acting && canMoveCancel == true && currentAttack != null)
            {
                if(currentAttack.AttackBehavior.JumpCancel == true)
                    CancelAction();
                else
                {
                    bufferJumpActive = true;
                    StartBuffer();
                }
            }
            else if (Input.GetButtonDown("ControllerA") && state == CharacterState.Acting)
            {
                bufferJumpActive = true;
                StartBuffer();
            }
            if (state != CharacterState.Idle && state != CharacterState.Moving)
                return;

            if(bufferJumpActive == true)
            {
                bufferJumpActive = false;
                JumpAction();
            }
            if (Input.GetButtonDown("ControllerA"))
            {
                JumpAction();
            }
        }

        private void JumpAction()
        {
            if (inAir == false)
            {
                doubleJump = false;
                if (Input.GetAxis("Horizontal") > 0.2f)
                {
                    direction = 1;
                    crouchTime = crouchJumpTime;
                    Action(crouchController);
                }
                else if (Input.GetAxis("Horizontal") < -0.2f)
                {
                    direction = -1;
                    crouchTime = crouchJumpTime;
                    Action(crouchController);
                }
                else
                {
                    speedX = 0;
                    speedY = 0;
                    JumpDefault();
                }
            }
            else if (inAir == true && speedZ > 0 && doubleJump == false)
            {
                doubleJump = true;
                Jump(jumpImpulsion * 0.75f);
                if (Input.GetAxis("Horizontal") > 0.2f)
                {
                    speedX = defaultSpeed;
                    direction = 1;
                }
                else if (Input.GetAxis("Horizontal") < -0.2f)
                {
                    speedX = -defaultSpeed;
                    direction = -1;
                }
                else
                {
                    speedX = 0;
                    speedY = 0;
                }
            }
            StopBuffer();
        }


        // =========================================================================================
        public void StopBuffer()
        {
            if (bufferCoroutine != null)
                StopCoroutine(bufferCoroutine);
            bufferNormalActive = false;
            bufferSpecialActive = false;
            bufferJumpActive = false;
            bufferDashActive = false;
        }


        public void StartBuffer()
        {
            if (bufferCoroutine != null)
                StopCoroutine(bufferCoroutine);
            bufferCoroutine = FrameBufferCoroutine();
            StartCoroutine(bufferCoroutine);
        }

        private IEnumerator FrameBufferCoroutine()
        {
            yield return new WaitForSeconds(bufferTime);
            bufferNormalActive = false;
            bufferSpecialActive = false;
            bufferJumpActive = false;
            bufferDashActive = false;
        }



        // Placeholder
        public void PlayGetCardAnimation()
        {
            state = CharacterState.Acting;
            characterAnimator.Play("Lia_GetCard");
        }
        // Placeholder
        public void PlayIdleAnimation()
        {
            state = CharacterState.Idle;
            characterAnimator.Play("Lia_Idle");
        }

        #endregion

    } 

} // #PROJECTNAME# namespace