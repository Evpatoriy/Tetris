using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    private GameObject previewTetramino;
    private GameObject nextTetramino;
    private bool gameStarted = false;
    private Vector2 previewTetraminoPosition = new Vector2(-4f, 15);
    
    void Start()
    {
        SpawnNextTetramino();
    }

    public bool CheckIsAboveGrid (Tetramino tetramino) {

        for (int x = 0; x < gridWidth; ++x) {
            foreach (Transform mine in tetramino.transform) {
                Vector2 pos = Round(mine.position);
                if (pos.y > gridHeight - 1) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsFullRowAt (int y) {

        for (int x = 0; x < gridWidth; ++x) {
            if (grid[x, y] == null) {
                return false;
            }
        }
        return true;
    }

    public void DeleteMinoAt (int y) {
         
         for (int x = 0; x < gridWidth; ++x) {
             Destroy(grid[x, y].gameObject);
             grid[x, y] = null;
         }
    }

    public void MoveRowDown (int y) {

        for (int x = 0; x < gridWidth; ++x) {
            if (grid[x, y] != null) {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            } 
        }
    }

    public void MoveAllRowsDown (int y) {

        for (int i = y; i < gridHeight; ++i) {
            MoveRowDown(i);
        }
    }

    public void DeleteRow () {

        for (int y = 0; y < gridHeight; ++y) {
            if (IsFullRowAt(y)) {
                DeleteMinoAt(y);
                MoveAllRowsDown(y + 1);
                --y;
            }
        }
    }



    public void UpdateGrid (Tetramino tetramino) {

        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                if (grid[x, y] != null) {
                    if (grid[x, y].parent == tetramino.transform) {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform mino in tetramino.transform) {
            Vector2 pos = Round(mino.position);

            if (pos.y < gridHeight) {

                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform GetTransformAtGridPosition (Vector2 pos) {

        if (pos.y > gridHeight-1) {
            return null;
        } else {
            return grid[(int)pos.x, (int)pos.y];        
        }
    }

    public void SpawnNextTetramino() {

        if (!gameStarted) {

            gameStarted = true;
            nextTetramino = (GameObject)Instantiate(Resources.Load(getRandomTetramino(), typeof(GameObject)), new Vector2(5.0f, 19.0f), Quaternion.identity);
            previewTetramino = (GameObject)Instantiate(Resources.Load(getRandomTetramino(), typeof(GameObject)), previewTetraminoPosition, Quaternion.identity);
            previewTetramino.GetComponent<Tetramino>().enabled = false;
        } else {
            previewTetramino.transform.localPosition = new Vector2(5.0f, 19.0f);
            nextTetramino = previewTetramino;
            nextTetramino.GetComponent<Tetramino>().enabled = true;

            previewTetramino = (GameObject)Instantiate(Resources.Load(getRandomTetramino(), typeof(GameObject)), previewTetraminoPosition, Quaternion.identity);
            previewTetramino.GetComponent<Tetramino>().enabled = false;
        }

        

    }

    public bool CheckIsInsideGrid (Vector2 pos) {

        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector2 Round (Vector2 pos) {

        return new Vector2 (Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    string getRandomTetramino() {
        int randomTetramino = Random.Range(1, 8);

        string randomTetraminoName = "Prefabs/Tetramino_T";

        switch (randomTetramino) {

            case 1: 
                randomTetraminoName = "Prefabs/Tetramino_O";
                break;
            case 2:
                randomTetraminoName = "Prefabs/Tetramino_I";
                break;
            case 3:
                randomTetraminoName = "Prefabs/Tetramino_L";
                break;
            case 4:
                randomTetraminoName = "Prefabs/Tetramino_L_reverse";
                break;
            case 5:
                randomTetraminoName = "Prefabs/Tetramino_S";
                break;
            case 6: 
                randomTetraminoName = "Prefabs/Tetramino_Z";
                break;
            case 7: 
                randomTetraminoName = "Prefabs/Tetramino_T"; 
                break;
        }
        return randomTetraminoName;
    }
    public void GameOver() {
        Application.LoadLevel("GameOver");
    }
}
