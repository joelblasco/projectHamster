using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Controlador para el jugador, será desactivado cuando este sea el enemigo.
 * El hamster tendrá la opción de correr a izquierda y derecha de la habitación e interactuar con algunos objetos. 
 * Adicionalmente podrá escalar la cama y objetos parecidos (?)
 * De momento solo se movera en un eje.
 */
public class playerController : MonoBehaviour
{
    public float speed, jumpForce, height;
    Animator myAnim; //to do
    Rigidbody rb;
    public bool grounded, isEnemy;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = false;
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * height, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, height))
        {
            if (hit.transform.tag == "suelo")
            {
                grounded = true;
            }
        }

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
        float motion = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(motion, 0, 0);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "climber")
        {
            if(!grounded && Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce/2 * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
    }
}
