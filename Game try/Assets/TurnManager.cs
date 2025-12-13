using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public TMP_Text turnText;

    public TurnFighter player1;
    public TurnFighter player2;

    private int currentPlayer = 0;

    private void Awake()
    {
        Instance = this;
    }
  

    private void Start()
    {
        Time.timeScale = 0f;     // ⏸️ PAUSED BY DEFAULT
        StartTurn();
    }

    void StartTurn()
    {
        player1.SetTurn(currentPlayer == 0);
        player2.SetTurn(currentPlayer == 1);

        UpdateTurnText();
    }
    void UpdateTurnText()
    {
        if (currentPlayer == 0)
        {
            turnText.text = "Player 1 Turn";
            turnText.color = Color.green;
        }
        else
        {
            turnText.text = "Player 2 Turn";
            turnText.color = Color.magenta;
        }
    }


    public TurnFighter ActivePlayer()
    {
        return currentPlayer == 0 ? player1 : player2;
    }
    public void ExecuteAction()
    {
        Time.timeScale = 1f;     // ▶️ PLAY ACTION
        Invoke(nameof(EndTurn), 0.5f);
    }

    void EndTurn()
    {
        Time.timeScale = 0f;     // ⏸️ FREEZE AGAIN
        currentPlayer = (currentPlayer + 1) % 2;
        StartTurn();
    }

}
