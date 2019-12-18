using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    [SerializeField] private bool playerTurn = false, playing = false;
    [SerializeField] private enemyController enemy;
    [SerializeField] private playerController player;
    private int turns;
    private float elapsedTime, startTime;

    public Text instructions, timer, turnCount, turnSide;
    public GameObject instructionsBackground;
    void Start()
    {
        startNewTurn();
    }

    // Update is called once per frame
    private void startNewTurn()
    {
        playerTurn = !playerTurn;
        instructions.gameObject.SetActive(true);
        instructionsBackground.SetActive(true);
        playing = false;
        enemy.changeStateEnabled = false;
        turnCount.text = $"Turno Nº {turns}";
        instructions.text = $"Turno {turns} - ";
        if (playerTurn)
        {
            player.canMove = false;
            instructions.text += "Tu turno. Tienes 10 segundos para esconderte. \n Pulsa A para empezar!";
            turnSide.text = "Escondete!";
        }
        else
        {
            instructions.text += "Turno de la IA. Busca al hámster! \n Pulsa A para empezar!";
            turnSide.text = "Encuentra!";
        }

        elapsedTime = 0;
        startTime = Time.time;
    }
    private void Update()
    {
        if (playing)
        {
            elapsedTime++;
            timer.text = elapsedTime.ToString("00:00");
            if (elapsedTime > 1000 && playerTurn && !enemy.changeStateEnabled)
            {
                enemy.changeStateEnabled = true;
                print("CAN MOVE" + elapsedTime);
            }

            if (Vector2.Distance(player.transform.position, enemy.transform.position) < 5)
            {
                print("Hi");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                startPlaying();
            }
        }
    }
    private void startPlaying()
    {
        playing = true;
        instructions.gameObject.SetActive(false);
        instructionsBackground.SetActive(false);
        player.canMove = true;
    }
}
