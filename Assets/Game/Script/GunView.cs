using UnityEngine;

public class GunView : MonoBehaviour
{
    Vector3 baseScale = new Vector3(120f, 120f, 120f);
    Vector3 UpTo = new Vector3(130f, 130f, 130f);
    bool isFirst = true;
    float zRot = 0, yRot = 0;
    Vector3 startPos;
    private void OnEnable()
    {
        transform.localScale = baseScale;
        isFirst = true;
    }
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            if (isFirst)
            {
                isFirst = false;
                startPos = Input.mousePosition;
                return;
            }
            else
            {
                zRot += (Input.mousePosition.y - startPos.y) * 0.1f;
                zRot = Mathf.Clamp(zRot, -55, 55);
                yRot += (Input.mousePosition.x - startPos.x) * 0.3f;
                //yRot = Mathf.Clamp(yRot, -55, 55);
                startPos = Input.mousePosition;
            }
            ScaleUp();
        }
        else
        {
            isFirst = true;
            zRot = Mathf.Lerp(zRot, 0, 0.2f);
            yRot = Mathf.Lerp(yRot, 0, 0.2f);
            ScaleDown();
        }
        transform.eulerAngles = new Vector3(0, 90 + yRot, zRot);
    }
    void ScaleUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, UpTo, 0.075f);
    }
    void ScaleDown()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, baseScale, 0.075f);
    }
}
