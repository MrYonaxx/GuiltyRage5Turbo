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
    public class HamtaroController : Character
    {
        [Space]
        [Space]
        [Space]
        [Title("PlayerController")]
        [SerializeField]
        int playerID = 1;
        [SerializeField]
        ControllerConfigurationData controllerConfig = null;

        [Title("Moveset")]
        [SerializeField]
        AttackController crouchController;
        [SerializeField]
        AttackController jumpLand;
        [SerializeField]
        AttackController runEnd;

        [Title("Normal")]
        [SerializeField]
        AttackController groundDefaultAttack;
        [SerializeField]
        AttackController groundRunAttack;

        [Title("Jump Normal")]
        [SerializeField]
        AttackController jumpDefaultAttack;
        [SerializeField]
        AttackController jumpForwardAttack;
        [SerializeField]
        AttackController jumpUpAttack;
        [SerializeField]
        AttackController jumpDownAttack;

        [Title("Throw")]
        [SerializeField]
        AttackController throwAttack;

        [Title("Special")]
        [SerializeField]
        AttackController specialAttack;
        [SerializeField]
        AttackController specialForwardAttack;
        [SerializeField]
        AttackController specialAerialAttack;
        [SerializeField]
        AttackController specialAerialForwardAttack;
        [SerializeField]
        AttackController specialRunAttack;

        [SerializeField]
        AttackController dashBack;
        [SerializeField]
        AttackController dashForward;

        [Title("Charged Attack")]
        [SerializeField]
        AttackController chargedAttack;
        [SerializeField]
        float chargeTotalTime = 0.5f;



        [Title("Parameter")]
        [SerializeField]
        bool canRunY = false;
        [SerializeField]
        float crouchJumpTime = 0.1f;
        [SerializeField]
        float runSpeedBonus = 1.5f;
        [SerializeField]
        float doubleJumpRatio = 0.75f;

        [Title("Buffer Parameter")]
        [SerializeField]
        float bufferTime = 0.25f;
        [SerializeField]
        float runBufferTime = 0.25f;


        float chargeTime = 0;
        float crouchTime = 0;
        bool doubleJump = false;

        IEnumerator bufferCoroutine = null;
        IEnumerator runningCoroutine = null;
        bool bufferNormalActive = false;
        bool bufferSpecialActive = false;
        bool bufferJumpActive = false;
        bool bufferDashActive = false;

        int runDirection = 0;
        int runInput = 0;
        bool isRunning = false;

        bool willJumpLand = false;



        string controllerA = "ControllerA";
        string controllerB = "ControllerB";
        string controllerX = "ControllerX";
        string controllerY = "ControllerY";
        string controllerR1 = "ControllerR1";
        string controllerL1 = "ControllerL1";
        string controllerLeftHorizontal = "ControllerLeftHorizontal";
        string controllerLeftVertical = "ControllerLeftVertical";

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        protected override void Start()
        {
            base.Start();
            string[] controllers;
            controllers = Input.GetJoystickNames();
            for(int i = 0; i < controllers.Length; i++)
            {
                Debug.Log(controllers[i]);
            }
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


        protected override void UpdateController()
        {
            JumpLand();
            if (active == true)
            {
                //InputThrow();
                InputMovement();
                InputRun();
                //InputDash();
                InputJump();
                InputCharged();
                InputAction();
                InputSpecial();
            }
        }

        protected override void OnGroundCollision()
        {
            base.OnGroundCollision();
            if(currentAttack != null && state == CharacterState.Acting)
            {
                if(currentAttack.AttackBehavior.CancelOnGround == true)
                {
                    if(currentAttack.AttackBehavior.OnGroundCombo != null)
                        Action(currentAttack.AttackBehavior.OnGroundCombo);
                    else 
                        CancelAction();
                }
            }
        }

        protected override void OnWallCollision()
        {
            base.OnWallCollision();
            if (currentAttack != null && state == CharacterState.Acting)
            {
                if (currentAttack.AttackBehavior.CancelOnWall == true)
                {               
                    if (currentAttack.AttackBehavior.OnWallCombo != null)
                        Action(currentAttack.AttackBehavior.OnWallCombo);
                    else
                        CancelAction();
                }
            }
            if(isRunning == true && (state == CharacterState.Idle || state == CharacterState.Moving))
            {
                characterAnimator.SetBool("Climb", true);
                inAir = true;
                speedZ = 3;
            }
            else
            {
                characterAnimator.SetBool("Climb", false);
            }
        }


        private void JumpLand()
        {
            if(jumpLand != null)
            {
                if (state != CharacterState.Idle && state != CharacterState.Moving)
                {
                    return;
                }
                if (inAir == true && willJumpLand == false)
                {
                    willJumpLand = true;
                }
                if (inAir == false && willJumpLand == true)
                {
                    willJumpLand = false;
                    Action(jumpLand);
                }
            }
        }




        // =========================================================================================
        private void InputMovement()
        {
            if (state != CharacterState.Idle && state != CharacterState.Moving || inAir != false)
            {
                return;
            }
            if (Mathf.Abs(Input.GetAxis(controllerLeftVertical)) > 0.2f && (isRunning == false || canRunY == true))
            {
                speedY = Input.GetAxis(controllerLeftVertical);
            }
            else
            {
                speedY = 0;
            }

            if (Input.GetAxis(controllerLeftHorizontal) > 0.2f)
            {
                speedX = Input.GetAxis(controllerLeftHorizontal);
                direction = 1;
            }
            else if (Input.GetAxis(controllerLeftHorizontal) < -0.2f)
            {
                speedX = Input.GetAxis(controllerLeftHorizontal);
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
        private void InputRun()
        {
            if(direction != runDirection )//&& state != CharacterState.Acting)
            {
                if (CanAct() && inAir == false && isRunning == true && runEnd != null)
                    Action(runEnd);
                runInput = 0;
                isRunning = false;
                if (runningCoroutine != null)
                    StopCoroutine(runningCoroutine);
                runDirection = direction;

            }

            if (isRunning == true && state == CharacterState.Acting && canMoveCancel == true && currentAttack != null)
            {
                if (currentAttack.AttackBehavior.RunCancel == true)
                {
                    CancelAction();
                    //direction = runDirection;
                }
            }

            if (Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) < 0.2f && runInput == -1)
            {
                runInput = 0;
            }
            else if (Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f && runInput == 0)
            {
                runDirection = (int)Mathf.Sign(Input.GetAxis(controllerLeftHorizontal));
                runInput += 1;
                RunBuffer();
            }
            else if (Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) < 0.2f && runInput == 1)
            {
                runInput += 1;
                RunBuffer();
            }
            else if (Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f && runInput == 2)
            {
                runInput += 1;
                isRunning = true;
                if (runningCoroutine != null)
                    StopCoroutine(runningCoroutine);
            }
            else if (Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) < 0.2f && runInput == 3)
            {
                runInput = 0;
                isRunning = false;
                if(CanAct() && inAir == false && runEnd != null)
                    Action(runEnd);
            }

            if(isRunning == true && (state == CharacterState.Moving || state == CharacterState.Idle) && inAir == false)
            {
                speedX = (defaultSpeed + runSpeedBonus) * direction;
                //speedY = 0;
                characterAnimator.SetBool("Run", true);
            }
            else if (isRunning == false)
            {
                characterAnimator.SetBool("Run", false);
            }
        }

        private void RunBuffer()
        {
            if (runningCoroutine != null)
                StopCoroutine(runningCoroutine);
            runningCoroutine = RunBufferCoroutine();
            StartCoroutine(runningCoroutine);
        }

        private IEnumerator RunBufferCoroutine()
        {
            yield return new WaitForSeconds(runBufferTime);
            runInput = -1;
            isRunning = false;
        }




        // =========================================================================================
        private void InputCharged()
        {
            if (chargedAttack == null)
                return;
            if(Input.GetButton(controllerX))
            {
                if(state != CharacterState.Hit)
                    chargeTime += Time.deltaTime;
            }
            else if(Input.GetButtonUp(controllerX))
            {
                if(chargeTime >= chargeTotalTime)
                {
                    if (state == CharacterState.Idle || state == CharacterState.Moving)
                    {
                        Action(chargedAttack);
                    }
                }
                chargeTime = 0;
            }
        }

        /*private void InputChargedHamtaro()
        {
            if (chargedAttack != null)
                return;
            if (Input.GetButton(controllerX))
            {
                if (state != CharacterState.Hit)
                    chargeTime += Time.deltaTime;
            }
            else
            {
                if (chargeTime == chargeTotalTime)
                {
                    if (state == CharacterState.Idle && state == CharacterState.Moving)
                    {
                        Action(chargedAttack);
                    }
                }
                chargeTime = 0;
            }
        }*/



        // =========================================================================================
        private void InputAction()
        {
            if (state != CharacterState.Idle && state != CharacterState.Moving && state != CharacterState.Acting)
            {
                return;
            }

            // Buffer
            if (bufferNormalActive == true)
            {
                if (CanAct() == true)
                {
                    NormalAttack();
                    return;
                }
            }

            // Input
            if (Input.GetButtonDown(controllerX))
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
            if (inAir == true && Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f)
            {
                Action(jumpForwardAttack);
            }
            else if (inAir == true && Input.GetAxis(controllerLeftVertical) > 0.2f)
            {
                Action(jumpUpAttack);
            }
            else if (inAir == true && Input.GetAxis(controllerLeftVertical) < -0.2f)
            {
                Action(jumpDownAttack);
            }
            else if (inAir == true)
            {
                Action(jumpDefaultAttack);
            }
            else if (isRunning == true)
            {
                runInput = 0;
                isRunning = false;
                Action(groundRunAttack);
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
            /*if (bufferSpecialActive == true && state == CharacterState.Acting && canMoveCancel == true)
            {
                SpecialAttack();
            }*/


            if (bufferSpecialActive == true && state == CharacterState.Acting && canMoveCancel == true && currentAttack != null)
            {
                if (currentAttack.AttackBehavior.SpecialCancel == true)
                    CancelAction();
            }

            if (bufferSpecialActive == true && (state == CharacterState.Idle || state == CharacterState.Moving))
            {
                StopBuffer();
                SpecialAttack();
            }



            // Input
            if (Input.GetButtonDown(controllerY) && state == CharacterState.Acting)
            {
                if (canMoveCancel == true && currentAttack != null)
                {
                    if (currentAttack.AttackBehavior.SpecialCancel == true)
                        SpecialAttack();
                    else
                    {
                        bufferSpecialActive = true;
                        StartBuffer();
                    }
                }
                else
                {
                    bufferSpecialActive = true;
                    StartBuffer();
                }
            }
            else if (Input.GetButtonDown(controllerY))
            {
                SpecialAttack();
            }
        }

        private void SpecialAttack()
        {
            if (inAir == true && Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f)
            {
                direction = (int)Mathf.Sign(Input.GetAxis(controllerLeftHorizontal));
                Action(specialAerialForwardAttack);
            }
            else if (inAir == true)
                Action(specialAerialAttack);
            else if (isRunning == true)
                Action(specialRunAttack);
            else if (Mathf.Abs(Input.GetAxis(controllerLeftHorizontal)) > 0.2f)
            {
                direction = (int)Mathf.Sign(Input.GetAxis(controllerLeftHorizontal));
                Action(specialForwardAttack);
            }
            else
                Action(specialAttack);        
            StopBuffer();
        }




        // =========================================================================================
        /*private void InputThrow()
        {
            if (state != CharacterState.Idle && state != CharacterState.Moving || inAir != false)
            {
                return;
            }
            if (Input.GetButton("ControllerR1"))
            {
                Action(throwAttack);
            }
        }*/




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

            if (bufferDashActive == true)
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
        }*/


        // =========================================================================================
        private void InputJump()
        {

            if (crouchTime > 0)
            {
                crouchTime -= Time.deltaTime * characterMotionSpeed;
                if (crouchTime <= 0)
                {
                    speedX = defaultSpeed * direction;
                    if (isRunning == true)
                        speedX += runSpeedBonus * direction;
                    //endAction = true;
                    CancelAct();
                    JumpDefault();
                }
            }

            if ((Input.GetButtonDown(controllerA) || bufferJumpActive == true) && state == CharacterState.Acting && canMoveCancel == true && currentAttack != null)
            {
                if (currentAttack.AttackBehavior.JumpCancel == true)
                    CancelAction();
                else
                {
                    bufferJumpActive = true;
                    StartBuffer();
                }
            }
            else if (Input.GetButtonDown(controllerA) && state == CharacterState.Acting)
            {
                bufferJumpActive = true;
                StartBuffer();
            }
            if (state != CharacterState.Idle && state != CharacterState.Moving)
                return;

            if (bufferJumpActive == true)
            {
                bufferJumpActive = false;
                JumpAction();
            }
            if (Input.GetButtonDown(controllerA))
            {
                JumpAction();
            }
        }

        private void JumpAction()
        {
            if (inAir == false)
            {
                doubleJump = false;
                if (Input.GetAxis(controllerLeftHorizontal) > 0.2f)
                {
                    direction = 1;
                    crouchTime = crouchJumpTime;
                    Action(crouchController);
                }
                else if (Input.GetAxis(controllerLeftHorizontal) < -0.2f)
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
                characterAnimator.SetTrigger("DoubleJump");
                doubleJump = true;
                Jump(jumpImpulsion * doubleJumpRatio);
                if (Input.GetAxis(controllerLeftHorizontal) > 0.2f)
                {
                    speedX = defaultSpeed;
                    direction = 1;
                }
                else if (Input.GetAxis(controllerLeftHorizontal) < -0.2f)
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
            if (isRunning == true)
                speedX += runSpeedBonus * direction;
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
}
