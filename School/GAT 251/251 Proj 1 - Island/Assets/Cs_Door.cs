using UnityEngine;
using System.Collections;

enum Enum_MoveDirection
{
    Down,
    Right
}

public class Cs_Door : MonoBehaviour
{
    bool b_IsMoving;
    Vector3 v3_StartingPos;
    float f_MoveDistance;
    float f_MoveSpeed = 2f;

    [SerializeField]
    Enum_MoveDirection moveDirection;

	// Use this for initialization
	void Start ()
    {
        if(moveDirection == Enum_MoveDirection.Right)
        {
            f_MoveDistance  = gameObject.transform.lossyScale.y;
        }
        else
        {
            f_MoveDistance = gameObject.transform.lossyScale.y;
        }

        v3_StartingPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(b_IsMoving)
        {
            if(AllowedToMove())
            {
                Vector3 v3_LerpPosition = gameObject.transform.right * (Time.deltaTime * f_MoveSpeed);
                
                gameObject.transform.position += v3_LerpPosition;
            }
        }
	}

    bool AllowedToMove()
    {
        if(Vector3.Distance(gameObject.transform.position, v3_StartingPos) >= f_MoveDistance)
        {
            b_IsMoving = false;

            return false;
        }

        return true;
    }

    public void MoveDoor()
    {
        b_IsMoving = true;
    }
}
