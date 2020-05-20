/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    /// <summary>
    /// Definition of the InputController class
    /// </summary>
    public class InputController : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        float joystickDeadZone = 0.25f;

        [SerializeField]
        bool hold = false;

        [SerializeField]
        UnityEvent eventBoutonY;
        [SerializeField]
        UnityEvent eventBoutonX;
        [SerializeField]
        UnityEvent eventBoutonB;
        [SerializeField]
        UnityEvent eventBoutonA;

        [Header("Buttons")]
        [SerializeField]
        UnityEvent eventButtonR1;
        [SerializeField]
        UnityEvent eventButtonL1;
        [SerializeField]
        UnityEvent eventButtonR2;
        [SerializeField]
        UnityEvent eventButtonL2;

        [Header("Joystick Left")]
        [SerializeField]
        UnityEvent eventLeftLeft;
        [SerializeField]
        UnityEvent eventLeftUp;
        [SerializeField]
        UnityEvent eventLeftRight;
        [SerializeField]
        UnityEvent eventLeftDown;
        [SerializeField]
        UnityEvent eventLeftStop;

        [Header("Joystick Right")]
        [SerializeField]
        UnityEvent eventRightLeft;
        [SerializeField]
        UnityEvent eventRightUp;
        [SerializeField]
        UnityEvent eventRightRight;
        [SerializeField]
        UnityEvent eventRightDown;

        [Header("Joystick Options")]
        [SerializeField]
        UnityEvent eventBoutonStart;

        [SerializeField]
        bool canRepeatJoystickLeft = false;

        bool inputLeftStickEnter = false;
        bool inputRightStickEnter = false;
        bool inputRightTrigger = false;
        bool inputLeftTrigger = false;

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


        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        protected void Update()
        {
            if (hold == false)
            {
                CheckInput();
                CheckTrigger();
            }
            else
            {
                CheckInputHold();
                CheckTrigger();
            }
            CheckLeftDirection();
            CheckRightDirection();
        }

        private void CheckInput()
        {
            if(Input.GetButtonDown("ControllerY"))
            {
                eventBoutonY.Invoke();
            }
            if (Input.GetButtonDown("ControllerX"))
            {
                eventBoutonX.Invoke();
            }
            if (Input.GetButtonDown("ControllerB"))
            {
                eventBoutonB.Invoke();
            }
            if (Input.GetButtonDown("ControllerA"))
            {
                eventBoutonA.Invoke();
            }
            if (Input.GetButtonDown("ControllerStart"))
            {
                eventBoutonStart.Invoke();
            }
        }

        private void CheckTrigger()
        {
            if (Input.GetButtonDown("ControllerR1"))
            {
                eventButtonR1.Invoke();
            }
            if (Input.GetButtonDown("ControllerL1"))
            {
                eventButtonL1.Invoke();
            }

            if (Input.GetAxis("ControllerTriggers") < -0.8f && inputRightTrigger == false)
            {
                inputRightTrigger = true;
                eventButtonR2.Invoke();
            }
            else if (Input.GetAxis("ControllerTriggers") >= -0.8f)
            {
                inputRightTrigger = false;
            }

            if (Input.GetAxis("ControllerTriggers") > 0.8f && inputLeftTrigger == false)
            {
                inputLeftTrigger = true;
                eventButtonL2.Invoke();
            }
            else if (Input.GetAxis("ControllerTriggers") <= 0.8f)
            {
                inputLeftTrigger = false;
            }
        }



        private void CheckInputHold()
        {
            if (Input.GetButton("ControllerY"))
            {
                eventBoutonY.Invoke();
            }
            if (Input.GetButton("ControllerX"))
            {
                eventBoutonX.Invoke();
            }
            if (Input.GetButton("ControllerB"))
            {
                eventBoutonB.Invoke();
            }
            if (Input.GetButton("ControllerA"))
            {
                eventBoutonA.Invoke();
            }
            if (Input.GetButton("ControllerStart"))
            {
                eventBoutonStart.Invoke();
            }
        }


        private void CheckLeftDirection()
        {
            if (inputLeftStickEnter == true)
            {
                if (canRepeatJoystickLeft == true)
                    inputLeftStickEnter = false;

                if (Mathf.Abs(Input.GetAxis("ControllerLeftHorizontal")) < joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerLeftVertical")) < joystickDeadZone)
                {
                    eventLeftStop.Invoke();
                    inputLeftStickEnter = false;
                }
                else
                    return;

            }

            if (Input.GetAxis("ControllerLeftHorizontal") > joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerLeftVertical")) < joystickDeadZone)
            {
                eventLeftRight.Invoke();
                inputLeftStickEnter = true;
            }
            else if (Input.GetAxis("ControllerLeftHorizontal") < -joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerLeftVertical")) < joystickDeadZone)
            {
                eventLeftLeft.Invoke();
                inputLeftStickEnter = true;
            }
            else if (Input.GetAxis("ControllerLeftVertical") < -joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerLeftHorizontal")) < joystickDeadZone)
            {
                eventLeftUp.Invoke();
                inputLeftStickEnter = true;
            }
            else if (Input.GetAxis("ControllerLeftVertical") > joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerLeftHorizontal")) < joystickDeadZone)
            {
                eventLeftDown.Invoke();
                inputLeftStickEnter = true;
            }
            else if (canRepeatJoystickLeft == true)
            {
                eventLeftStop.Invoke();
            }
        }


        private void CheckRightDirection()
        {
            if (inputRightStickEnter == true)
            {
                if (Mathf.Abs(Input.GetAxis("ControllerRightHorizontal")) < joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerRightVertical")) < joystickDeadZone)
                    inputRightStickEnter = false;
                else
                    return;

            }

            if (Input.GetAxis("ControllerRightHorizontal") > joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerRightVertical")) < joystickDeadZone)
            {
                eventRightRight.Invoke();
                inputRightStickEnter = true;
            }
            else if (Input.GetAxis("ControllerRightHorizontal") < -joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerRightVertical")) < joystickDeadZone)
            {
                eventRightLeft.Invoke();
                inputRightStickEnter = true;
            }
            else if (Input.GetAxis("ControllerRightVertical") < -joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerRightHorizontal")) < joystickDeadZone)
            {
                eventRightUp.Invoke();
                inputRightStickEnter = true;
            }
            else if (Input.GetAxis("ControllerRightVertical") > joystickDeadZone && Mathf.Abs(Input.GetAxis("ControllerRightHorizontal")) < joystickDeadZone)
            {
                eventRightDown.Invoke();
                inputRightStickEnter = true;
            }
        }


        public void EventButtonA()
        {
            eventBoutonA.Invoke();
        }
        public void EventButtonB()
        {
            eventBoutonB.Invoke();
        }
        public void EventButtonY()
        {
            eventBoutonY.Invoke();
        }
        public void EventStart()
        {
            eventBoutonStart.Invoke();
        }


        #endregion

    } // InputController class

} // #PROJECTNAME# namespace