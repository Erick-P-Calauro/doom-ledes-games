using Unity.Mathematics;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float sensibility = 200f;
    public Transform player;
    float xrotation = 0f;
    private CharacterController controller;
    public float speed = 10.0f;
    private float gravity = -9.81f;
    public Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         controller = GetComponent<CharacterController>();
        //Aplica uma força ao objeto
        //rb.GetComponent<Rigidbody>().AddForce(new Vector3(3,0,0));
    }

    // Update is called once per frame
    void Update()
    {
        float inputx = Input.GetAxisRaw("Horizontal");
        float inputz = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(inputx, -9.81f, inputz);
        controller.Move(movement * Time.deltaTime * speed); 
    }
}
