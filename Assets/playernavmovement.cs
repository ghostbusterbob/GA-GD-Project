using UnityEngine;

public class playernavmovement : MonoBehaviour
{
    public Transform Player;
    void Start()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, 10 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(0, 0, 10 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -10 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(0, 0, -10 * Time.deltaTime);
        }
    }
    void Update()

    {

    }
    void Movemement()
    {

    }
}
