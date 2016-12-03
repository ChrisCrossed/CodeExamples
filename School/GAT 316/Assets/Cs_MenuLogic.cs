using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.UI;

public class Cs_MenuLogic : MonoBehaviour
{
    bool b_RunGame;
    float f_Alpha;

    // Controller Input
    GamePadState state;
    GamePadState prevState;
    public PlayerIndex playerOne = PlayerIndex.One;

    // Use this for initialization
    void Start ()
    {
        state = GamePad.GetState(playerOne);
	}
	
	// Update is called once per frame
	void Update ()
    {
        prevState = state;
        state = GamePad.GetState(playerOne);

        if (state.Buttons.Start == ButtonState.Pressed) b_RunGame = true;

        // Cap alpha
	    if(!b_RunGame)
        {
            f_Alpha += Time.deltaTime;
            if ( f_Alpha > 1.0f ) f_Alpha = 1.0f;
        }
        else
        {
            f_Alpha -= Time.deltaTime;
            if (f_Alpha < 0.0f) f_Alpha = 0.0f;
        }

        // Run through text and set alpha

	}
}
