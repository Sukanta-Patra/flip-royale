using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public int rows;
    public int columns;
    public int turns;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetRows(int _rows) => rows = _rows;
    public int GetRows() => rows;
    public void SetColumns(int _columns) => columns = _columns;
    public int GetColumns() => columns;
    public void SetTurns(int _turns) => turns = _turns;
    public int GetTurns() => turns;
}
