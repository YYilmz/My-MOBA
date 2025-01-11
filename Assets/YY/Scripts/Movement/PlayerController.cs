using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace YY
{
    public class PlayerController : MonoBehaviour
    {
        const string IDLE = "Idle";
        const string WALK = "Walk";

        PlayerMovement input;

        NavMeshAgent agent;
        Animator animator;

        [Header("Movement")]
        [SerializeField] ParticleSystem clickEffect;
        [SerializeField] LayerMask clickableLayers;

        float lookRotationSpeed = 8f;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            input = new PlayerMovement();
            AssignInput();
        }
        void AssignInput()
        {
            input.Main.Move.performed += ctx => ClickToMove();
        }
        void ClickToMove()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers))
            {
                agent.destination = hit.point;
                if (clickEffect != null)
                {
                    Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
                }
            }
        }
        void OnEnable()
        {
            input.Enable();
        }
        void OnDisable()
        {
            input.Disable();
        }

        void Update()
        {
            FaceTarget();
            SetAnimator();
        }

        void FaceTarget()
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
        void SetAnimator()
        {
            if (agent.velocity == Vector3.zero)
            {
                animator.Play(IDLE);
            }
            else
            {
                animator.Play(WALK);
            }
        }
    }
}