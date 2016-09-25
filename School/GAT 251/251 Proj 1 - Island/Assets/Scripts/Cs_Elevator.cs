using UnityEngine;
using System.Collections;

public enum Enum_ElevatorStatus
{
    GoTo_Bottom,
    Bottom_Stall,
    GoTo_Top,
    Top_Stall
}

public class Cs_Elevator : MonoBehaviour
{
    public GameObject go_BottomPosition;
    public GameObject go_TopPosition;
    public float f_Speed;
    public float f_Delay;
    public float f_Speed_Cap;
    Vector3 v3_RefVelocity;
    float f_Timer;
    Enum_ElevatorStatus elevatorStatus = Enum_ElevatorStatus.Bottom_Stall;
    Vector3 v3_newPos;

	// Use this for initialization
	void Start ()
    {
        v3_newPos = go_BottomPosition.transform.position;

        gameObject.transform.position = v3_newPos;
	}

    public Enum_ElevatorStatus GetState()
    {
        return elevatorStatus;
    }

    public Enum_ElevatorStatus CycleElevator()
    {
        if (elevatorStatus == Enum_ElevatorStatus.Bottom_Stall) elevatorStatus = Enum_ElevatorStatus.GoTo_Top;

        if (elevatorStatus == Enum_ElevatorStatus.Top_Stall) elevatorStatus = Enum_ElevatorStatus.GoTo_Bottom;

        return elevatorStatus;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 0.025f
        float f_SnapDistance = 0.025f;

        if(elevatorStatus == Enum_ElevatorStatus.GoTo_Bottom)
        {
            // v3_newPos = Vector3.SmoothDamp(gameObject.transform.position, go_BottomPosition.transform.position, ref v3_RefVelocity, f_Speed);
            v3_newPos = Vector3.SmoothDamp(gameObject.transform.position, go_BottomPosition.transform.position, ref v3_RefVelocity, 2 / f_Speed);

            if (gameObject.transform.position.y <= (go_BottomPosition.transform.position.y + f_SnapDistance))
            {
                gameObject.GetComponent<Rigidbody>().MovePosition(go_BottomPosition.transform.position);

                elevatorStatus = Enum_ElevatorStatus.Bottom_Stall;
            }
            else gameObject.GetComponent<Rigidbody>().MovePosition(v3_newPos);
        }

        if (elevatorStatus == Enum_ElevatorStatus.Bottom_Stall)
        {
            /*
            f_Timer += Time.deltaTime;

            if(f_Timer >= f_Delay)
            {
                f_Timer = 0.0f;

                elevatorStatus = Enum_ElevatorStatus.GoTo_Top;
            }
            */
        }

        if (elevatorStatus == Enum_ElevatorStatus.GoTo_Top)
        {
            // v3_newPos = Vector3.SmoothDamp(gameObject.transform.position, go_TopPosition.transform.position, ref v3_RefVelocity, f_Speed);
            v3_newPos = Vector3.SmoothDamp(gameObject.transform.position, go_TopPosition.transform.position, ref v3_RefVelocity, 1 / f_Speed);

            if (gameObject.transform.position.y >= (go_TopPosition.transform.position.y - f_SnapDistance))
            {
                gameObject.GetComponent<Rigidbody>().MovePosition(go_TopPosition.transform.position);

                elevatorStatus = Enum_ElevatorStatus.Top_Stall;
            }
            else gameObject.GetComponent<Rigidbody>().MovePosition(v3_newPos);
        }

        if (elevatorStatus == Enum_ElevatorStatus.Top_Stall)
        {
            /*
            f_Timer += Time.deltaTime;

            if(f_Timer >= f_Delay)
            {
                f_Timer = 0.0f;

                elevatorStatus = Enum_ElevatorStatus.GoTo_Bottom;
            }
            */
        }
    }
}
