using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * El enemigo tendrá las mismas opciones de movimiento que el jugador, solo que será controlado por el juego
 * El funcionamiento de esta "IA" es muy simple, tendrá 5 estados: correr derecha, correr izquierda, saltar, subir y quedarse quieto.
 * RunLeft - RunRight - Jump - Climb - Stand
 * Cada x segundos decidirá si continua con la acción que estaba realizando o si bien cambia de acción. Cada acción tendrá un porcentaje y cada vez que la realice
 * este porcentaje disminuirá.
 */
public class enemyController : MonoBehaviour
{
    //AI
    [SerializeField] private enum aiState { RunLeft, RunRight, Jump, Climb, Stand}
    [SerializeField] private aiState currentState;
    [SerializeField] private float aiRunLeft, aiRunRight, aiJump, aiClimb, aiStand;
    [SerializeField] private float aiTicTime, removeAiValue;
    //Movement
    [SerializeField] private float vHorizontal, vVertical,speed;
    //Physics
    [SerializeField] private float jumpForce, height, gravityModifier, timeOnAir;
    private Rigidbody rb;
    
    void Start()
    {
        stand();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(tic());
    }

    void Update()
    {
        switch (currentState)
        {
            case aiState.RunLeft:
                if (reachedWall()) goRight();
                break;
            case aiState.RunRight:
                if (reachedWall()) goLeft();
                break;
            case aiState.Stand:
                break;
            case aiState.Climb:
                break;
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(vHorizontal * speed * Time.deltaTime, vVertical *speed*Time.deltaTime, 0);
        if (!grounded()) //para compensar la falta de gravedad y darle una apariencia de más pesadez, simulo más gravedad.
        {
            rb.AddForce(Vector3.down * gravityModifier * timeOnAir * Time.deltaTime);
            timeOnAir += 1f * jumpForce * 75;
            Mathf.Clamp(timeOnAir, 0, 1500);
        }
    }
    private bool grounded()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down * height, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, height))
        {
            if (hit.transform.tag == "suelo")
            {
                timeOnAir = 0;
                return true;
            }
        }
        return false;
    }
    IEnumerator tic(float randomGivenValue =-1) //elegir cambiar a una accion o quedarse igual si no se da ninguna nueva accion
    {
        float r;
        if (randomGivenValue == -1) r = Random.Range(0f, 100f);
        else r = randomGivenValue;
        if(r < aiRunLeft) // corre izquierda
        {
            goLeft();
        }else if(r < aiRunLeft + aiRunRight) //corre derecha
        {
            goRight();
        }else if(r < aiRunRight + aiRunLeft + aiJump) // salta
        {
            jump();
        }else if(r < aiRunLeft + aiRunRight + aiJump + aiClimb) // escalar
        {
            climb();
        }else if(r < aiRunLeft + aiRunRight + aiJump + aiClimb + aiStand) // quieto
        {
            stand();
        }
        //print(r+ " : switched to "+currentState.ToString());
        yield return new WaitForSeconds(aiTicTime);
        StartCoroutine(tic());
    }
    #region ACTIONS
    void goLeft()
    {
        vHorizontal = -1; //hacia la izquierda
        vVertical = 0; //no volamos
        currentState = aiState.RunLeft;
        aiRunLeft -= removeAiValue;
    }
    void goRight()
    {
        vHorizontal = 1; //hacia la derecha
        vVertical = 0;
        currentState = aiState.RunRight;
        aiRunRight -= removeAiValue;
    }
    void jump()
    {
        vVertical = 0; //no modificaremos el movimiento horizontal mientras saltamos
        currentState = aiState.Jump;
        aiJump -= removeAiValue;
        if (grounded()) //salto
        {
            rb.velocity = Vector3.up * jumpForce * Time.deltaTime * 1000;
        }
    }
    void climb()
    {
        currentState = aiState.Climb;
        aiClimb -= removeAiValue;
    }
    void stand()
    {
        vHorizontal = 0; 
        vVertical = 0;
        currentState = aiState.Stand;
        aiStand -= removeAiValue;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "climber" && currentState == aiState.Climb)
        {
            vVertical = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "climber")
        {
            
        }
    }
    bool reachedWall()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.right * height * vHorizontal, Color.red);
        if (Physics.Raycast(transform.position, Vector3.right*vHorizontal, out hit, height*vHorizontal))
        {
            if (hit.transform.tag == "suelo")
            {
                print("FOUND WALL");
                return true;
            }
        }
        return false;
    }
    #endregion ACTIONS
}
