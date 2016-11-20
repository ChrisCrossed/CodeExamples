using UnityEngine;
using System.Collections;

public class Cs_CameraController : MonoBehaviour
{
	public void Init_CameraPosition( float f_BoardWidth_, float f_BoardHeight_, float f_BlockWidth_ )
    {
        print(f_BoardWidth_ + ", " + f_BoardHeight_ + ", " + f_BlockWidth_);

        Vector3 v3_CamPos = new Vector3();

        float f_Ratio = f_BoardHeight_ / f_BoardWidth_;
        if (f_BoardHeight_ < f_BoardWidth_) f_Ratio = f_BoardWidth_ / f_BoardHeight_ / 2;

        float xPos = (f_BoardWidth_ * f_BlockWidth_ / 2) - (f_BlockWidth_ / 2);
        float yPos = xPos * f_Ratio;
        float zPos = (yPos * -2) - f_BlockWidth_;

        print("New: " + xPos + ", " + yPos + ", " + zPos);

        v3_CamPos.x = xPos;
        v3_CamPos.y = yPos;
        v3_CamPos.z = zPos;

        gameObject.transform.position = v3_CamPos;
    }
}
