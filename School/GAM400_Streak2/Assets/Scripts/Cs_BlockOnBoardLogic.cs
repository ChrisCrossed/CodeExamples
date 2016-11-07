using UnityEngine;
using System.Collections;

public class Cs_BlockOnBoardLogic : MonoBehaviour
{
    float f_LerpTimer_Horiz;
    float f_LerpTimer_Vert;
    float f_LerpTimer_Max = 0.25f;

    float f_yPos;
    float f_xPos;

    float f_BlockScale;
    int i_BoardWidth;

    bool b_IsDead;
    float f_TransparencyTimer = 0.5f;
    [SerializeField] float f_TimeToTransparent = .5f;
    [SerializeField] float f_LowestTransparencyPoint = 0.05f;

    // Use this for initialization
    void Start ()
    {
        // Init_BlockModel(5, 5, 3, 20);
	}

    public void Init_BlockModel( int i_xPos_, int i_yPos_, float f_BlockScale_, int i_BoardWidth_ )
    {
        f_xPos = i_xPos_;
        f_yPos = i_yPos_;

        f_BlockScale = f_BlockScale_;
        i_BoardWidth = i_BoardWidth_;

        gameObject.transform.position = new Vector3(f_xPos * f_BlockScale, f_yPos * f_BlockScale, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region Update X & Y position
        if (gameObject.transform.position != new Vector3(f_xPos * f_BlockScale, f_yPos * f_BlockScale, 0))
        {
            float f_xPos_Temp;
            float f_yPos_Temp;

            // Lerp to current X
            if(f_LerpTimer_Horiz < f_LerpTimer_Max)
            {
                f_LerpTimer_Horiz += Time.deltaTime;

                if ( f_LerpTimer_Horiz > f_LerpTimer_Max )
                {
                    f_LerpTimer_Horiz = f_LerpTimer_Max;
                }
            }
            
            f_xPos_Temp = Mathf.Lerp(gameObject.transform.position.x, f_xPos * f_BlockScale, f_LerpTimer_Horiz / f_LerpTimer_Max);

            // Lerp to current Y
            if(f_LerpTimer_Vert < f_LerpTimer_Max)
            {
                f_LerpTimer_Vert += Time.deltaTime;

                if(f_LerpTimer_Vert > f_LerpTimer_Max)
                {
                    f_LerpTimer_Vert = f_LerpTimer_Max;
                }
            }

            f_yPos_Temp = Mathf.Lerp(gameObject.transform.position.y, f_yPos * f_BlockScale, f_LerpTimer_Vert / f_LerpTimer_Max);

            gameObject.transform.position = new Vector3(f_xPos_Temp, f_yPos_Temp, 0);
        }
        #endregion

        #region Fade Out (When Destroyed)
        if(b_IsDead)
        {
            f_TransparencyTimer -= (Time.deltaTime / f_TimeToTransparent);

            if (f_TransparencyTimer < f_LowestTransparencyPoint) f_TransparencyTimer = f_LowestTransparencyPoint;

            SetMaterialsVisibility(f_TransparencyTimer);

            if(f_TransparencyTimer ==  f_LowestTransparencyPoint)
            {
                GameObject.Destroy(gameObject);
            }
        }
        #endregion
    }

    public void Set_MoveLeft()
    {
        // Decrement X position by 1
        if(f_xPos - 1 >= 0)
        {
            f_xPos -= 1;
        }

        // Reset LerpTimer_Horiz
        f_LerpTimer_Horiz = 0f;
    }

    public void Set_MoveRight()
    {
        // Increment X position by 1
        if( f_xPos + 1< i_BoardWidth)
        {
            f_xPos += 1;
        }

        // Reset LerpTimer_Horiz
        f_LerpTimer_Horiz = 0.0f;
    }

    public void Set_MoveDown()
    {
        // Decrement Y position by 1
        if(f_yPos - 1 >= 0)
        {
            f_yPos -= 1;
        }

        // Reset LerpTimer_Vert
        f_LerpTimer_Vert = 0.0f;
    }

    public void Set_MoveUp()
    {
        // Increment Y position by 1
        if (f_yPos + 1 >= 0)
        {
            f_yPos += 1;
        }

        // Reset LerpTimer_Vert
        f_LerpTimer_Vert = 0.0f;
    }

    public void Set_MoveDownToPos( int f_yPos_ )
    {
        // Get current Y position
        int i_Temp = (int)(f_yPos - f_yPos_);

        // Loop
        for(int i_ = 0; i_ < i_Temp; ++i_)
        {
            Set_MoveDown();
        }
    }

    public void Set_DeleteBlock()
    {
        // Set state to 'dead'
        b_IsDead = true;
    }

    void SetMaterialsVisibility(float f_Transparency_)
    {
        Material[] mat_CurrColor = gameObject.GetComponent<MeshRenderer>().materials;

        for (int i_ = 0; i_ < mat_CurrColor.Length; ++i_)
        {
            Color currColor = mat_CurrColor[i_].color;

            if (f_Transparency_ == 1.0f)
            {
                // print("Making Opaque: " + f_Transparency_);
                // mat_CurrColor[i_].SetFloat("_Mode", 1.0f);
                Material mat = GetComponentInChildren<MeshRenderer>().materials[i_];

                // Got guide: http://sassybot.com/blog/swapping-rendering-mode-in-unity-5-0/
                mat.SetFloat("_Mode", 0); // Sets the material to Opaque
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = -1;

                // Set transparency for metallic objects only if they are already metallic
                if (mat.GetFloat("_Metallic") > (f_LowestTransparencyPoint - 0.01f))
                {
                    mat.SetFloat("_Metallic", f_Transparency_);
                }
            }
            else
            {
                // print("Making Transparent: " + currColor.a);
                // mat_CurrColor[i_].SetFloat("_Mode", 4.0f);
                Material mat = GetComponentInChildren<MeshRenderer>().materials[i_];

                // Got guide: http://sassybot.com/blog/swapping-rendering-mode-in-unity-5-0/
                mat.SetFloat("_Mode", 3); // Sets the material to Transparent
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                // Set transparency for metallic objects only if they are already metallic
                if (mat.GetFloat("_Metallic") > (f_LowestTransparencyPoint - 0.01f))
                {
                    mat.SetFloat("_Metallic", f_Transparency_);
                }
            }

            // GetComponent<MeshRenderer>().met
            currColor.a = f_Transparency_;
            mat_CurrColor[i_].color = currColor;
        }
    }
}
