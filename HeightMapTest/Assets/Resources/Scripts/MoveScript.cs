using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [Header("Object Properties")]
    public GameObject MovableObject;

    [Range(0.1f, 20.0f)]
    public float force = 10.0f;

    private void Start()
    {
        if (MovableObject == null)
        {
            Debug.LogError("No object attached to this script. Disable it.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MovableObject.gameObject.transform.Translate(Vector3.left * force * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MovableObject.gameObject.transform.Translate(Vector3.right * force * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MovableObject.gameObject.transform.Translate(Vector3.forward * force * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MovableObject.gameObject.transform.Translate(Vector3.back * force * Time.deltaTime, Space.World);
        }
    }
}