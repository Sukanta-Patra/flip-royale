using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public int rows;
    public int columns;

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
}
