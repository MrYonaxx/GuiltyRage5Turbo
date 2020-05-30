using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{

    public class CameraBattleController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        Camera mainCamera;
        [SerializeField]
        Transform focusTarget;
        [SerializeField]
        Transform focusTargetLock;

        [SerializeField]
        float smoothCamera = 2;
        [SerializeField]
        Vector3 cameraOffset = Vector3.zero;

        [Header("Clamp")]

        [SerializeField]
        float clampLeft = -6;
        [SerializeField]
        float clampRight = 6;
        [SerializeField]
        float clampDown = 6;
        [SerializeField]
        float clampUp = 6;

        float orthographicDefaultSize = 0;

        Vector3 velocity = Vector3.zero;

        Vector3 actualFocusPosition;

        private void Start()
        {
            orthographicDefaultSize = mainCamera.orthographicSize;
            FocusDefault();
        }

        private void Update()
        {
            FocusOnTarget(focusTarget.position);
        }

        public void FocusOnTarget(Vector3 targetPos)
        {
            /*actualViewX = focusTarget.transform.position.x;
            actualViewY = focusTarget.transform.position.y;
            //this.transform.position = new Vector3(focusTarget.position.x, focusTarget.position.y, this.transform.position.z);
            transform.position -= new Vector3(((transform.position.x - actualViewX) * smoothCamera * 3) * Time.deltaTime, 
                                              ((transform.position.y - actualViewY) * smoothCamera * 3) * Time.deltaTime,
                                               0);*/
            if (focusTargetLock != null)
                targetPos = targetPos + ((focusTargetLock.position - focusTarget.position) / 2);

            float realClampLeft = clampLeft * ((orthographicDefaultSize / mainCamera.orthographicSize));
            float realClampRight = clampRight * ((orthographicDefaultSize / mainCamera.orthographicSize));
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, realClampLeft, realClampRight), Mathf.Clamp(targetPos.y, clampDown, clampUp), 0);

            transform.position = Vector3.SmoothDamp(transform.position, targetPos + new Vector3(0, 0, this.transform.position.z) + cameraOffset, ref velocity, smoothCamera);
            //OffsetCamera();
            ClampCamera();
        }

        /*private void OffsetCamera()
        {
            transform.position = transform.position + cameraOffset;
        }*/

        private void ClampCamera()
        {
            float realClampLeft = clampLeft * (clampRight - clampLeft * (orthographicDefaultSize / mainCamera.orthographicSize));
            float realClampRight = clampRight * (clampRight - clampLeft * (orthographicDefaultSize / mainCamera.orthographicSize));
            //float realClampDown = 0;
            //float clampUp = 0;
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, realClampLeft, realClampRight),
                                                Mathf.Clamp(this.transform.position.y, clampDown, clampUp),
                                                this.transform.position.z);
        }





        public void FocusDefault()
        {
            actualFocusPosition = focusTarget.position;
        }

        public void SetTarget(Transform lockTransform)
        {
            focusTarget = lockTransform;
        }
        public void SetLocked(Transform lockTransform)
        {
            focusTargetLock = lockTransform;
        }

        public void SetNewClampX(Vector2 newClamp)
        {
            clampLeft = newClamp.x;
            clampRight = newClamp.y;
        }


    }
}
