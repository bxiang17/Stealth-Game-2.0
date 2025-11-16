using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public event System.Action OnPlayerReachedFinish;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform finishPoint;
    [SerializeField] private float finishDistance = 1f;


    private bool isWalking;
    private bool disabled;

    private void Start()
    {
        Guard.OnGuardHasSpottedPlayer += Disable;
    }

    private void Update()
    {
        Vector3 inputVector = Vector3.zero;
        if (!disabled) 
        {
            inputVector = gameInput.GetMovementVectorNormalized();
        }

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards moveDir

            // Try only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Try only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }
        
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed); 
        
        CheckFinish();
    }

    /*
    void OnTriggerEnter(Collider hitCollider) 
    {
        if (hitCollider.tag == "Finish")
        {
            Disable();
            if (OnPlayerReachedFinish != null)
            {
                OnPlayerReachedFinish();
            }
        }
    }
    */

    private void CheckFinish()
    {
        if (finishPoint == null) return;

        Vector3 playerPos = transform.position;
        Vector3 finishPos = finishPoint.position;

        // Ignore height
        playerPos.y = 0f;
        finishPos.y = 0f;

        float distance = Vector3.Distance(playerPos, finishPos);

        if (distance <= finishDistance)
        {
            Disable();
            OnPlayerReachedFinish?.Invoke();
        }
    }

    void Disable()
    {
        disabled = true;
    }
    
    public bool IsWalking()
    {
        return isWalking;
    }

    void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }
}
