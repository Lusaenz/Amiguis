using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject titleObject;
    public GameObject[] availablePieces;

    public float cameraSizeOffset;
    public float cameraVerticalOffset;
    Tile[,] Tiles;
    Piece[,] Pieces;

    Tile startTile;
    Tile endTile;
    void Start()
    {
        Tiles = new Tile[width, height];
        Pieces = new Piece[width, height];
        SetupBoard();
        PositionCamera();
        SetupPieces();
    }

    private void SetupPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)];
                var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity);
                o.transform.parent = transform;
                Pieces[x, y] = o.GetComponent<Piece>();
                Pieces[x, y]?.Setup(x, y, this);
            }
        }
    }
    private void PositionCamera()
    {
        float newPosX = (float)width / 2f;
        float newPosY = (float)height / 2f;

        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosY - 0.5f + cameraVerticalOffset, -10f);

        float horizontal = width + 1;
        float vertical = (height / 2) + 1;

        Camera.main.orthographicSize = horizontal > vertical ? horizontal + cameraSizeOffset : vertical + cameraSizeOffset;
    }
    //The grid was created
    private void SetupBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var o = Instantiate(titleObject, new Vector3(x, y, -5), Quaternion.identity);
                o.transform.parent = transform;
                Tiles[x, y] = o.GetComponent<Tile>();
                Tiles[x, y]?.Setup(x, y, this);
            }
        }
    }

    public void TileDown(Tile tile_) // funcion de cuando selecciono una casilla
    {
        startTile = tile_;

    }
    public void TileOver(Tile tile_) // funcion de cuando arrastro el mouse
    {
        endTile = tile_;

    }
    public void TileUp(Tile tile_) // funcion de cuando suelto el mouse
    {
        if (startTile != null && endTile != null && IsCloseTo(startTile, endTile))
        {
            SwapTitles();
        }
        startTile = null; // se reinicia la pieza inicial y final, osea las que se mueven,
        endTile = null;

    }

    //funcion que se encarga de actualizar la informacion del sistema de cooordenadas (los arrays de dos dimensiones) y de llamar la funcion de Move para cada pieza.
    private void SwapTitles()
    {
        //Referencias a las piezas que se estan moviemdo.
        var StarPiece = Pieces[startTile.x, startTile.y];
        var EndPiece = Pieces[endTile.x, endTile.y];

        StarPiece.Move(endTile.x, endTile.y);
        EndPiece.Move(startTile.x, startTile.y);

        Pieces[startTile.x, startTile.y] = EndPiece;
        Pieces[endTile.x, endTile.y] = StarPiece;
    }
    // Funcion tiene como proposito verificar sis dos tile estan uno al lado del otro, si son adyacentes en linea recta no en diagonal./ Funcion de limitacion de movimiento de piezas,
    public bool IsCloseTo(Tile start, Tile end)
    {
        //Primera condición: start.x y end.x difieren en 1 y y es igual → significa que están en la misma fila, pero en columnas vecinas (izquierda o derecha).
        if(Math.Abs((start.x-end.x)) == 1 && start.y == end.y) //Math.Abs(...) == 1: esto verifica que haya una diferencia de solo una unidad entre las posiciones (es decir, que estén justo al lado).
        {
            return true;
        }
        //Segunda condición: start.y y end.y difieren en 1 y x es igual → están en la misma columna, pero en filas vecinas (arriba o abajo).
        if(Math.Abs((start.y-end.y)) == 1 && start.x == end.x)
        {
            return true;
        }
        //Si no se cumple ninguna de estas dos, devuelve false → no están uno al lado del otro.
        return false;
    }
}
