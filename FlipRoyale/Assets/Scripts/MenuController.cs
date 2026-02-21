using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private const string GAME_SCENE = "Game";

    [SerializeField] private InputField rowsInput;
    [SerializeField] private InputField columnsInput;
    [SerializeField] private Text totalPairsText;

    [SerializeField] private Button startGameButton;

    private int pairsLimit = 30;
    private int columns;
    private int rows;
    private int turns;

    private void Start()
    {
        startGameButton.interactable = false;
    }

    public void OnRowColumnInputChanged()
    {
        if (string.IsNullOrEmpty(rowsInput.text.Trim()) || string.IsNullOrEmpty(columnsInput.text.Trim()))
        {
            totalPairsText.text = "Enter desired rows and columns to start the game.";
            return;
        }

        rows = int.Parse(rowsInput.text);
        columns = int.Parse(columnsInput.text);

        int pairs = (rows * columns) / 2;
        //setting a limit of max cards to safe guard UI limitation of cards spawning OOB
        if (pairs < pairsLimit)
        {
            if ((rows * columns) % 2 != 0)
            {
                totalPairsText.text = $"Total Pairs : <color=red>{pairs}</color>(Total cards <color=red>({rows * columns})</color> cannot be ODD.)";
                startGameButton.interactable = false;
            }
            else
            {
                turns = (int) Mathf.Floor(rows * columns * 1.75f);
                if (turns % 2 != 0) turns++; //Making sure total turns are always even for card pairs
                totalPairsText.text = $"Total Pairs : <color=green>{pairs}</color>. Total calculated turns allowed : {turns}";
                startGameButton.interactable = true;
            }
        }
        else
        {
            totalPairsText.text = $"Total Pairs : <color=red>{pairs}</color>(Total pairs should not exceed {pairsLimit})";
            startGameButton.interactable = false;
        }
    }

    public void OnStartGame()
    {
        //Save grid settings
        GameSettings.Instance.SetRows(rows);
        GameSettings.Instance.SetColumns(columns);
        GameSettings.Instance.SetTurns(turns);

        //Load Game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(GAME_SCENE);
    }

}
