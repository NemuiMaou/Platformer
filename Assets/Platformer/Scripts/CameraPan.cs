using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public float speed = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxis("CameraPan");
        transform.position += new Vector3(direction, 0f, 0f) * (speed * Time.deltaTime);
    }
}
