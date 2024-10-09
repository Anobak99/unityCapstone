using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor_x;
    public float parallaxFactor_y;

    public void Move(Vector3 delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos -= new Vector3(delta.x * parallaxFactor_x, delta.y * parallaxFactor_y);

        transform.localPosition = newPos;
    }
}
