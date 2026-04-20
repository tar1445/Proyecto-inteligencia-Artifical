using Unity.Mathematics;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private float Sensibilidad = 100;
    private float rotacionHorizontal = 0; 
    private float rotacionVertical = 0;

    [SerializeField] Transform player; 
    void Start()
    {
        
        Cursor.visible = false;
    }


    void Update()
    {
        float valorX = Input.GetAxis("Mouse X") * Sensibilidad * Time.deltaTime;
        float valorY = Input.GetAxis("Mouse Y") * Sensibilidad * Time.deltaTime;

        rotacionHorizontal += valorX;
        rotacionVertical -= valorY;


        rotacionVertical = math.clamp(rotacionVertical, -90, 90);

        transform.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);

        player.Rotate(Vector3.up * valorX);

      

    }
}
