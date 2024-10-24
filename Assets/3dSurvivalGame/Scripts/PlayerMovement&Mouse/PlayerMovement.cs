using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUR
{

    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;

        public float speed = 12f;
        public float gravity = -9.81f * 2;
        public float jumpHeight = 3f;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        Vector3 velocity;

        bool isGrounded;

        // 플레이어가 움직였는지 확인
        private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
        private bool isMoving;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();     
            lastPosition = transform.position;
        }
        // Update is called once per frame
        void Update()
        {
            if (DialogueSystem.Instance.dialogueUIActive == false)
            {
                Move();
            }
        }



        void Move()
        {
            //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //right is the red Axis, foward is the blue axis
            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            //check if the player is on the ground so he can jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                //the equation for jumping
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            // 점프했을 때는 소리가 안나오게
            if(lastPosition != gameObject.transform.position && isGrounded == true)
            {
                isMoving = true;
                SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
            }
            else
            {
                isMoving= false;
                SoundManager.Instance.grassWalkSound.Stop();    //플레이어가 움직이지 않으면 소리 멈춤
            }
            lastPosition = gameObject.transform.position;       // 멈추면 그 위치를 새로운 lastPosition 으로 지정
        }
    }
}