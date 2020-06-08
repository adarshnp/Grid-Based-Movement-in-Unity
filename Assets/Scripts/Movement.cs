using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //1 : up , -1 : down , 0 : null
    private int verticalDirection;
    //1 : right , -1 : left , 0 : null
    private int HorizontalDirection;
    //to start access of grid only after grid is initialised
    private bool isInitialised = false;
    //to stop turn while travelling unless its on a node
    private bool canTurn;

    public Transform player;
    //position contained in next node in waiting
    private Vector3 targetPosition;
    //unit vector to define rotation
    private Vector3 moveDirection;
    //indexes
    private int x;
    private int y;
    //max number of rows and columns
    private int rows;
    private int cols;

    public Swipe swipeControls;
    public GridManager gridHandler;
    //speed controller
    public float speed;
    void Start()
    {
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        canTurn = false;
        verticalDirection = 0;
        HorizontalDirection = -1;
        moveDirection = Vector3.left;
        yield return new WaitForEndOfFrame();

        gridHandler.GetParameters(ref rows, ref cols);
        gridHandler.GetOrigin(ref x, ref y);
        transform.position = gridHandler.GetGridPosition(x,y);
        x += verticalDirection;
        y += HorizontalDirection;
        targetPosition = gridHandler.GetGridPosition(x,y);
        isInitialised = true;
    }
    void Update()
    {
        if (!isInitialised) { return; }
        Move();
        UpdateDirection();
        if (canTurn)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.forward);
            canTurn = false;
        }
    }
    //update direction 
    void UpdateDirection()
    {
        if (swipeControls.SwipeLeft)
        {
            moveDirection = Vector3.left;
            HorizontalDirection = -1;
            verticalDirection = 0;
        }
        if (swipeControls.SwipRight)
        {
            moveDirection = Vector3.right;
            HorizontalDirection = 1;
            verticalDirection = 0;
        }
        if (swipeControls.SwipeUp)
        {
            moveDirection = Vector3.up;
            verticalDirection = 1;
            HorizontalDirection = 0;
        }
        if (swipeControls.SwipeDown)
        {
            moveDirection = Vector3.down;
            verticalDirection = -1;
            HorizontalDirection = 0;
        }
    }
    //keep moving the player
    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (transform.position == targetPosition)
        {
            x += verticalDirection;
            y += HorizontalDirection;
            canTurn = true;
        }
        Jump();
        targetPosition = gridHandler.GetGridPosition(x, y);
    }
    //to enter from other side of grid once the player reaches the grid edge
    void Jump()
    {
        //checking right side
        if(x >= rows)
        {
            transform.position = gridHandler.GetGridPosition(0, y);
            x = verticalDirection;
            targetPosition = gridHandler.GetGridPosition(x, y);
        }
        //checking left side
        if(x < 0)
        {
            transform.position = gridHandler.GetGridPosition(rows-1, y);
            x = rows - 1 + verticalDirection;
            targetPosition = gridHandler.GetGridPosition(x, y);
        }
        //checking top
        if(y >= cols)
        {
            transform.position = gridHandler.GetGridPosition(x, 0);
            y = HorizontalDirection;
            targetPosition = gridHandler.GetGridPosition(x, y);
        }
        //checking bottom
        if(y < 0)
        {
            transform.position = gridHandler.GetGridPosition(x, cols-1);
            y = cols - 1 + HorizontalDirection;
            targetPosition = gridHandler.GetGridPosition(x, y);
        }
    }
}
