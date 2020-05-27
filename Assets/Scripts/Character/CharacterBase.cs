using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharacterBase : MonoBehaviour
    {
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


        protected CharacterState state = CharacterState.Idle;
        protected bool inAir = false;
        protected int direction = 1;

        protected float speedX = 0;
        protected float speedY = 0;
        protected float speedZ = 0;

        protected float actualSpeedX = 0;
        protected float actualSpeedY = 0;

        protected float characterMotionSpeed = 1;

        int layerMask;
        Vector2 bottomLeft;
        Vector2 upperLeft;
        Vector2 bottomRight;
        Vector2 upperRight;

        Transform collisionInfo;




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





        /*protected void ApplyGravity()
        {
            if (inAir == true)
            {
                speedZ -= ((gravity * Time.deltaTime) * characterMotionSpeed);
                speedZ = Mathf.Max(speedZ, gravityMax);
                spriteRenderer.transform.localPosition += new Vector3(0, (speedZ * Time.deltaTime) * characterMotionSpeed, 0);
                if (spriteRenderer.transform.localPosition.y <= 0 && characterMotionSpeed != 0)
                {
                    inAir = false;
                    speedZ = 0;
                    spriteRenderer.transform.localPosition = new Vector3(spriteRenderer.transform.localPosition.x, 0, spriteRenderer.transform.localPosition.z);
                    OnGroundCollision();
                }
            }
        }*/




        protected virtual void OnWallCollision()
        {

        }

        protected virtual void OnGroundCollision()
        {

        }






        public void MoveToPointInstant(Vector3 point)
        {
            Vector2 direction = point - this.transform.position;
            SetSpeed(direction.x / Time.deltaTime, direction.y / Time.deltaTime);
        }


       /* public bool MoveToPoint(Vector3 point, float time, float totalTime)
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
                SetSpeed(direction.x * defaultSpeed, direction.y * defaultSpeed);
                return false;
            }
        }
        public void MoveForward(float multiplier)
        {
            SetSpeed(defaultSpeed * multiplier * direction, 0);
        }*/

        public void SetSpeed(float newSpeedX, float newSpeedY)
        {
            speedX = newSpeedX;
            speedY = newSpeedY;
        }
        public void TurnBack()
        {
            direction = -direction;
            if (direction == 1)
                spriteRenderer.flipX = false;
            else if (direction == -1)
                spriteRenderer.flipX = true;
        }


        /*public void SetCharacterMotionSpeed(float newSpeed, float time = 0)
        {
            characterMotionSpeed = newSpeed;
            characterAnimator.speed = characterMotionSpeed;
            if (currentAttackController != null)
                currentAttackController.AttackMotionSpeed(newSpeed);
            if (time > 0)
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
            characterMotionSpeed = defaultMotionSpeed;
            characterAnimator.speed = characterMotionSpeed;
            if (currentAttackController != null)
                currentAttackController.AttackMotionSpeed(characterMotionSpeed);
        }*/


    }
}
