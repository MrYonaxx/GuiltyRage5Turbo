/*****************************************************************
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
    public class PlayerController : MonoBehaviour, ICharacterController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("PlayerController")]
        [SerializeField]
        int playerID = 1;
        [SerializeField]
        ControllerConfigurationData controllerConfig = null;

        [Title("Moveset")]
        [SerializeField]
        AttackController crouchController;

        [Title("Coups Normaux")]
        [Header("Sol")]
        [SerializeField]
        AttackController groundDefaultAttack;
        [SerializeField]
        AttackController groundRunAttack;

        [Header("Aerien")]
        [SerializeField]
        AttackController jumpDefaultAttack;
        [SerializeField]
        AttackController jumpForwardAttack;
        [SerializeField]
        AttackController jumpUpAttack;
        [SerializeField]
        AttackController jumpDownAttack;

        [Title("Speciaux")]
        [SerializeField]
        AttackController specialAttack;
        [SerializeField]
        AttackController specialForwardAttack;
        [SerializeField]
        AttackController specialRunAttack;

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

        int runDirection = 0;
        int runInput = 0;
        bool isRunning = false;

        protected Character character;
        protected CharacterState state;

        float stickX = 0;
        float stickY = 0;


        IEnumerator bufferCoroutine = null;
        bool bufferNormalActive = false;
        bool bufferSpecialActive = false;
        bool bufferJumpActive = false;
        bool bufferDashActive = false;




        protected string controllerA = "ControllerA";
        protected string controllerB = "ControllerB";
        protected string controllerX = "ControllerX";
        protected string controllerY = "ControllerY";
        protected string controllerR1 = "ControllerR1";
        protected string controllerL1 = "ControllerL1";
        protected string controllerLeftHorizontal = "ControllerLeftHorizontal";
        protected string controllerLeftVertical = "ControllerLeftVertical";



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

        protected virtual void Start()
        {
            if (controllerConfig != null)
            {
                controllerA = controllerConfig.controllerA + "_" + playerID;
                controllerB = controllerConfig.controllerB + "_" + playerID;
                controllerX = controllerConfig.controllerX + "_" + playerID;
                controllerY = controllerConfig.controllerY + "_" + playerID;
            }
            else
            {
                controllerA += "_" + playerID;
                controllerB += "_" + playerID;
                controllerX += "_" + playerID;
                controllerY += "_" + playerID;
            }
            controllerR1 += "_" + playerID;
            controllerL1 += "_" + playerID;
            controllerLeftHorizontal += "_" + playerID;
            controllerLeftVertical += "_" + playerID;
        }

        public virtual void UpdateController(Character c)
        {
            character = c;
            state = c.State;
            //InputThrow();
            InputMovement(c, state);
            //InputDash();
            InputJump(c, state);
            InputAction(c, state);
            InputSpecial();
        }

        public void LateUpdateController(Character c)
        {
            character = c;
        }




        // =========================================================================================
        protected virtual void InputMovement(Character character, CharacterState state)
        {
            if (state != CharacterState.Idle && state != CharacterState.Moving || character.InAir != false)
            {
                return;
            }
            if (Mathf.Abs(Input.GetAxis(controllerLeftVertical)) > 0.2f)
            {
                stickY = Input.GetAxis(controllerLeftVertical);
            }
            else
            {
                stickY = 0;
            }

            if (Input.GetAxis(controllerLeftHorizontal) > 0.2f)
            {
                stickX = Input.GetAxis(controllerLeftHorizontal);
                character.Direction = 1;
            }
            else if (Input.GetAxis(controllerLeftHorizontal) < -0.2f)
            {
                stickX = Input.GetAxis(controllerLeftHorizontal);
                character.Direction = -1;
            }
            else
            {
                stickX = 0;
            }
            Vector2 move = new Vector2(stickX, stickY);
            move.Normalize();
            character.SetSpeed(move.x * character.CharacterStat.GetSpeed(), move.y * character.CharacterStat.GetSpeed());

        }





        // =========================================================================================
        protected virtual void InputAction(Character character, CharacterState state)
        {
            // Buffer
            if (bufferNormalActive == true)
            {
                if (character.CanAct() == true)
                {
                    NormalAttack(character);
                    return;
                }
            }

            // Input
            if (Input.GetButtonDown(controllerX))
            {
                bufferNormalActive = true;
                StartBuffer();
                if (character.CanAct() == true)
                {
                    NormalAttack(character);
                }
            }
        }

        protected virtual void NormalAttack(Character character)
        {
            // Aerien
            if (character.InAir == true && Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f)
            {
                character.Action(jumpForwardAttack);
            }
            else if (character.InAir == true && Input.GetAxis(controllerLeftVertical) > 0.2f)
            {
                character.Action(jumpUpAttack);
            }
            else if (character.InAir == true && Input.GetAxis(controllerLeftVertical) < -0.2f)
            {
                character.Action(jumpDownAttack);
            }
            else if (character.InAir == true)
            {
                character.Action(jumpDefaultAttack);
            }

            // Ground
            else if (isRunning == true)
            {
                runInput = 0;
                isRunning = false;
                character.Action(groundRunAttack);
            }
            else if(Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f)
            {
                character.Action(groundDefaultAttack);
            }
            else
            {
                character.Action(groundDefaultAttack);
            }
            StopBuffer();
        }






        // =========================================================================================
        protected virtual void InputSpecial()
        {
             // Buffer
            if (bufferSpecialActive == true)
            {
                if (state == CharacterState.Acting && character.CanMoveCancel == true)
                {
                    SpecialAttack();
                    return;
                }
                else if (state == CharacterState.Idle || state == CharacterState.Moving)
                {
                    SpecialAttack();
                    return;
                }
            }

             // Input
            if (Input.GetButtonDown(controllerY))
            {
                bufferSpecialActive = true;
                StartBuffer();
                if (state == CharacterState.Acting && character.CanMoveCancel == true)
                    SpecialAttack();
                else if (state == CharacterState.Idle || state == CharacterState.Moving)
                    SpecialAttack();
            }
        }


        protected virtual void SpecialAttack()
        {
            if(Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f)
                character.Action(specialForwardAttack);
            else
                character.Action(specialAttack);
            StopBuffer();
        }




         // =========================================================================================
         /*private void InputDash()
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
         }*/


        // =========================================================================================
        private void InputJump(Character character, CharacterState state)
        {
            if (crouchTime > 0 && character.State != CharacterState.Acting)
            {
                crouchTime = 0;
            }
            if (crouchTime > 0)
            {
                crouchTime -= Time.deltaTime;// * characterMotionSpeed;
                if (crouchTime <= 0)
                {
                    character.SetSpeed(character.CharacterStat.GetSpeed() * character.Direction, 0);
                    character.CancelAct();
                    character.JumpDefault();
                    return;
                }
            }

            if(bufferJumpActive == true)
            {
                if(state == CharacterState.Idle || state == CharacterState.Moving)
                {
                    JumpAction();
                }
                else if (state == CharacterState.Acting && character.CanMoveCancel == true && character.CurrentAttack != null)
                {
                    if (character.CurrentAttack.AttackBehavior.JumpCancel == true)
                    {
                        character.CancelAction();
                        JumpAction();
                    }
                }
            }

            if (Input.GetButtonDown(controllerA))
            {
                bufferJumpActive = true;
                StartBuffer();
                if (state == CharacterState.Idle || state == CharacterState.Moving)
                {
                    JumpAction();
                }
                else if (state == CharacterState.Acting && character.CanMoveCancel == true && character.CurrentAttack != null)
                {
                    if (character.CurrentAttack.AttackBehavior.JumpCancel == true)
                    {
                        character.CancelAction();
                        JumpAction();
                    }
                }
            }
        }

        private void JumpAction()
        {
            if (character.InAir == false)
            {
                doubleJump = false;
                if (Input.GetAxis(controllerLeftHorizontal) > 0.2f)
                {
                    character.Direction = 1;
                    crouchTime = crouchJumpTime;
                    character.Action(crouchController);
                }
                else if (Input.GetAxis(controllerLeftHorizontal) < -0.2f)
                {
                    character.Direction = -1;
                    crouchTime = crouchJumpTime;
                    character.Action(crouchController);
                }
                else
                {
                    character.SetSpeed(0, 0);
                    character.JumpDefault();
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


        #endregion

    } 

} // #PROJECTNAME# namespace