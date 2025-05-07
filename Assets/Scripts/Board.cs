using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject titleObject;

    public float cameraSizeOffset;
    public float cameraVerticalOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupBoard();
        PositionCamera();
        
    }
    //The grid was created
    private void SetupBoard()
    {
        for(int x=0; x<width; x++)
        {
            for(int y=0; y<height; y++)
            {
                var o = Instantiate(titleObject, new Vector3(x, y, -5), Quaternion.identity);
                o.transform.parent = transform;

            }
        }
    }
    private void PositionCamera()
    {
        float newPosX = (float)width / 2f;
        float newPosY = (float)height / 2f;

        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosY - 0.5f + cameraVerticalOffset, -10f);

        float horizontal = width + 1;
        float vertical = (height / 2) + 1;

        Camera.main.orthographicSize = horizontal > vertical ? horizontal + cameraSizeOffset : vertical + cameraSizeOffset;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
