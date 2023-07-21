using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{

    public Animator myAnim;
    public Rigidbody2D myRB;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;
    private enum MovementState {Walk,LeftWalk,DownWalk,Attack,LeftAttack,DownAttack,Idle,LeftIdle,DownIdle }

    // Start is called before the first frame update
    void FixedUpdate()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       

        UpdateAnimationState();
        ProcessInputs();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && moveDirection.x != 0 || moveDirection.y != 0)
        {
            lastMoveDirection = moveDirection;
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

  

    private void UpdateAnimationState()
    {
       // myRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * 0.1f * Time.deltaTime;
        myAnim.SetFloat("MoveX", moveDirection.x);
        myAnim.SetFloat("Magnitude", moveDirection.magnitude);
        myAnim.SetFloat("MoveY", moveDirection.y);
        myAnim.SetFloat("LastMoveX", lastMoveDirection.x);
        myAnim.SetFloat("LastMoveY", lastMoveDirection.y);
    }
}
