using UnityEngine;
using System.Collections;

enum Enum_ElevatorStatus
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
	
	// Update is called once per frame
	void Update ()
    {
        print(elevatorStatus);

        float f_SnapDistance = 0.025f;

        if(elevatorStatus == Enum_ElevatorStatus.GoTo_Bottom)
        {
            // Lerp from the bottom position toward the top position
            v3_newPos = Vector3.SmoothDamp(gameObject.transform.position, go_BottomPosition.transform.position, ref v3_RefVelocity, f_Speed);

            // Apply the new position
            gameObject.transform.position = v3_newPos;

            // Move on to next position within the State Machine
            if (gameObject.transform.position.y <= (go_BottomPosition.transform.position.y + f_SnapDistance))
            {
                gameObject.transform.position = go_BottomPosition.transform.position;

                elevatorStatus = Enum_ElevatorStatus.Bottom_Stall;
            }
        }

        if (elevatorStatus == Enum_ElevatorStatus.Bottom_Stall)
        {
            f_Timer += Time.deltaTime;

            if(f_Timer >= f_Delay)
            {
                f_Timer = 0.0f;

                elevatorStatus = Enum_ElevatorStatus.GoTo_Top;
            }
        }

        if (elevatorStatus == Enum_ElevatorStatus.GoTo_Top)
        {
            // Lerp from the bottom position toward the top position
            v3_newPos = Vector3.SmoothDamp(gameObject.transform.position, go_TopPosition.transform.position, ref v3_RefVelocity, f_Speed);
            
            // Apply the new position
            gameObject.transform.position = v3_newPos;

            // Move on to next position within the State Machine
            if (gameObject.transform.position.y >= (go_TopPosition.transform.position.y - f_SnapDistance))
            {
                gameObject.transform.position = go_TopPosition.transform.position;

                elevatorStatus = Enum_ElevatorStatus.Top_Stall;
            }
        }

        if (elevatorStatus == Enum_ElevatorStatus.Top_Stall)
        {
            f_Timer += Time.deltaTime;

            if(f_Timer >= f_Delay)
            {
                f_Timer = 0.0f;

                elevatorStatus = Enum_ElevatorStatus.GoTo_Bottom;
            }
        }
    }
}
