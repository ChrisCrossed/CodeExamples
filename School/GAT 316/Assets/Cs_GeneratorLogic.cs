using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Cs_GeneratorLogic : MonoBehaviour
{
    bool b_PlayerInCollider;

    bool b_ObjectiveActive = true;

    [SerializeField] GameObject go_Objective;

    // Controller Input
    GamePadState state;
    GamePadState prevState;
    public PlayerIndex playerOne = PlayerIndex.One;
	
	// Update is called once per frame
	void Update ()
    {
	    if(b_PlayerInCollider)
        {
            prevState = state;
            state = GamePad.GetState(playerOne);

            if (state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released)
            {
                Set_ObjectiveState(true);

                if(go_Objective != null)
                {
                    if(go_Objective.GetComponent<Cs_GateScript>())
                    {
                        go_Objective.GetComponent<Cs_GateScript>().Set_ObjectiveActive(true);

                        go_Objective.GetComponent<Cs_GateScript>().Set_DoorOpen(true);
                    }
                }
            }
        }
	}

    public void Set_ObjectiveState( bool b_ObjectiveActive_)
    {
        b_ObjectiveActive = b_ObjectiveActive_;
    }

    void OnTriggerEnter( Collider collider_ )
    {
        if(collider_.transform.root.gameObject.name == "Player")
        {
            b_PlayerInCollider = true;
        }
    }

    void OnTriggerExit(Collider collider_)
    {
        if (collider_.transform.root.gameObject.name == "Player")
        {
            b_PlayerInCollider = false;
        }
    }
}
