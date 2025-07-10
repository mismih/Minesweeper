using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public int mineCount = 10;

    public Tile[,] tiles;

    void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        tiles = new Tile[width, height];

        // Pravimo prazne tileove
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile();
            }
        }

        // Random postavljamo mine
        PlaceMines();

        // Izracunamo brojeve
        CalculateNumbers();
    }

    void PlaceMines()
    {
        int placed = 0;
        while (placed < mineCount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if (!tiles[x, y].isMine)
            {
                tiles[x, y].isMine = true;
                placed++;
            }
        }
    }

    void CalculateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y].isMine) continue;

                int count = 0;
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = x + dx;
                        int ny = y + dy;
                        if (IsValid(nx, ny) && tiles[nx, ny].isMine)
                        {
                            count++;
                        }
                    }
                }
                tiles[x, y].neighborMines = count;
            }
        }
    }

    bool IsValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public void RevealTile(int x, int y)
    {
        // Proverava granice
        if (!IsValid(x, y)) return;

        Tile tile = tiles[x, y];

        // preskace ako je vec flaggovana ili otkrivena
        if (tile.isRevealed || tile.isFlagged) return;

        // otkriva
        tile.isRevealed = true;

        // game over ako je mina
        if (tile.isMine)
        {
            Debug.Log("Game Over!");
            // Kasnije cemo dodati Game over logiku
            return;
        }

        // ako je prazno otkrij i susedna polja
        if (tile.neighborMines == 0)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue; 
                    RevealTile(x + dx, y + dy);
                }
            }
        }
    }
}
