using UnityEngine;
using System.Collections;

public class Cs_WallLogic : MonoBehaviour
{
    // WallLogic does two things: Ensures it is set as a 'Wall' layer, and contains functions to turn itself semi-transparent when told to (by camera colliders)

    bool b_GoToTransparent;

    float f_TransparencyTimer = 1.0f;
    [SerializeField] float f_TimeToTransparent = .5f;
    [SerializeField] float f_LowestTransparencyPoint = 0.25f;

    // Use this for initialization
    void Start()
    {
        // Set the wall to be considered a 'wall'
        int i_LayerMask = LayerMask.NameToLayer("Wall");
        gameObject.layer = i_LayerMask;
    }

    // Update is called once per frame
    void Update()
    {
        // If we're either going transparent, or returning from transparent
        if ( (b_GoToTransparent && f_TransparencyTimer > f_LowestTransparencyPoint) ||
             (!b_GoToTransparent && f_TransparencyTimer < 1 ) )
        {

            // Lerp from current transparency point to required point
            if(b_GoToTransparent)
            {
                // Reduce the transparency by the time modifier
                f_TransparencyTimer -= (Time.deltaTime / f_TimeToTransparent);

                if (f_TransparencyTimer < f_LowestTransparencyPoint) f_TransparencyTimer = f_LowestTransparencyPoint;
            }
            else
            {
                // Increase the transparency point by the time modifier
                f_TransparencyTimer += (Time.deltaTime / f_TimeToTransparent);

                if (f_TransparencyTimer > 1) f_TransparencyTimer = 1;
            }

            SetMaterialsVisibility(f_TransparencyTimer);
        }
	}

    void SetMaterialsVisibility( float f_Transparency_ )
    {
        Material[] mat_CurrColor = gameObject.GetComponent<MeshRenderer>().materials;

        for(int i_ = 0; i_ < mat_CurrColor.Length; ++i_)
        {
            // if(mat_CurrColor[i_].re)

            Color currColor = mat_CurrColor[i_].color;
            currColor.a = f_Transparency_;
            mat_CurrColor[i_].color = currColor;
        }
        /*Color clr_CurrColor = gameObject.GetComponent<MeshRenderer>().material.color;
        clr_CurrColor.a = f_TransparencyTimer;
        gameObject.GetComponent<MeshRenderer>().material.color = clr_CurrColor;
        print(clr_CurrColor.ToString());
        */
    }

    public void SetVisibilityState( bool b_GoToTransparent_ )
    {
        b_GoToTransparent = b_GoToTransparent_;
    }
}
