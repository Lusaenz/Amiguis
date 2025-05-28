using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

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

    bool swappingPieces = false;
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
        int maxIterations = 50;
        int currentIterations = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                currentIterations = 0;
                var newPiece = CreatePieceAt(x, y);
                while (HasPreviousMatches(x, y))
                {
                    CleartePieceAt(x, y);
                    newPiece = CreatePieceAt(x, y);
                    currentIterations++;
                    if (currentIterations > maxIterations)
                    {
                        break;
                    }

                }

            }
        }
    }

    private void CleartePieceAt(int x, int y)
    {
        var pieceToClear = Pieces[x, y];
        Destroy(pieceToClear.gameObject);
        Pieces[x, y] = null;
    }
    private Piece CreatePieceAt(int x, int y)
    {
        var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)];
        var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity);
        o.transform.parent = transform;
        Pieces[x, y] = o.GetComponent<Piece>();
        Pieces[x, y]?.Setup(x, y, this);
        return Pieces[x, y];
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
            StartCoroutine(SwapTitles());
        }
    }

    //funcion que se encarga de actualizar la informacion del sistema de cooordenadas (los arrays de dos dimensiones) y de llamar la funcion de Move para cada pieza.
    //Esta funcion mueve la piezas, espere que las piezas termine de moversen busca los matches y nos devuelve los resultados.
    IEnumerator SwapTitles()
    {
        // Referencias a las piezas que se están moviendo.
        var StarPiece = Pieces[startTile.x, startTile.y];
        var EndPiece = Pieces[endTile.x, endTile.y];

        StarPiece.Move(endTile.x, endTile.y);
        EndPiece.Move(startTile.x, startTile.y);

        Pieces[startTile.x, startTile.y] = EndPiece;
        Pieces[endTile.x, endTile.y] = StarPiece;

        yield return new WaitForSeconds(0.6f);

        var startMatches = GetMatchByPiece(startTile.x, startTile.y, 3);
        var endMatches = GetMatchByPiece(endTile.x, endTile.y, 3);

        var allMatches = startMatches.Union(endMatches).ToList();

        if (allMatches.Count == 0)
        {
            // Si no se encontraron matches, deshacer el movimiento.
            StarPiece.Move(startTile.x, startTile.y);
            EndPiece.Move(endTile.x, endTile.y);
            Pieces[startTile.x, startTile.y] = StarPiece;
            Pieces[endTile.x, endTile.y] = EndPiece;
        }
        else
        {
            CleartePieces(allMatches);
        }
        startTile = null;
        endTile = null;
        swappingPieces = false;

        yield return null;
    }

    private void CleartePieces(List<Piece> piecesToClear)
    {
        piecesToClear.ForEach(piece =>
        {
            CleartePieceAt(piece.x, piece.y);
        });
        List<int> columns = GetColumns(piecesToClear);
        List<Piece> collapsdPieces = collapseColumns(columns, 0.3f);
    }
    private List<int> GetColumns(List<Piece> piecesToClear)
    {
        var result = new List<int>();
        piecesToClear.ForEach(piece =>
        {
            if (!result.Contains(piece.x))
            {
                result.Add(piece.x);
            }

        });

        return result;
    }
    private List<Piece> collapseColumns(List<int> columns, float timeToCollapse)
    {
        List<Piece> movingPieces = new List<Piece>();
        for (int i = 0; i < columns.Count; i++)
        {
            var column = columns[i];
            for (int y = 0; y < height; y++)
            {
                if (Pieces[column, y] == null)
                {
                    for (int yplus = y + 1; yplus < height; yplus++)
                    {
                        if (Pieces[column, yplus] != null)
                        {
                            Pieces[column, yplus].Move(column, y);
                            Pieces[column, y] = Pieces[column, yplus];
                            if (!movingPieces.Contains(Pieces[column, y]))
                            {
                                movingPieces.Add(Pieces[column, y]);
                            }
                            Pieces[column, yplus] = null;
                            break;
                        }

                    }

                }
            }
        }
        return movingPieces;

    }

    // Funcion tiene como proposito verificar sis dos tile estan uno al lado del otro, si son adyacentes en linea recta no en diagonal./ Funcion de limitacion de movimiento de piezas,
    public bool IsCloseTo(Tile start, Tile end)
    {
        //Primera condición: start.x y end.x difieren en 1 y y es igual → significa que están en la misma fila, pero en columnas vecinas (izquierda o derecha).
        if (Math.Abs((start.x - end.x)) == 1 && start.y == end.y) //Math.Abs(...) == 1: esto verifica que haya una diferencia de solo una unidad entre las posiciones (es decir, que estén justo al lado).
        {
            return true;
        }
        //Segunda condición: start.y y end.y difieren en 1 y x es igual → están en la misma columna, pero en filas vecinas (arriba o abajo).
        if (Math.Abs((start.y - end.y)) == 1 && start.x == end.x)
        {
            return true;
        }
        //Si no se cumple ninguna de estas dos, devuelve false → no están uno al lado del otro.
        return false;
    }
    // Verifica si hay matches hacia abajo o a la izquierda de la pieza actual
    bool HasPreviousMatches(int posx, int posy)
    {
        var downMatches = GetMatchByDirection(posx, posy, new Vector2(0, -1), 2);
        var leftMatches = GetMatchByDirection(posx, posy, new Vector2(-1, 0), 2);

        return (downMatches?.Count > 0 || leftMatches?.Count > 0);
    }

    // Busca matches en una dirección (usado para matches horizontales o verticales)
    public List<Piece> GetMatchByDirection(int xpos, int ypos, Vector2 direction, int minPiece = 3)
    {
        List<Piece> matches = new List<Piece>();
        Piece startPiece = Pieces[xpos, ypos];
        matches.Add(startPiece);

        int maxVal = Mathf.Max(width, height);

        for (int i = 1; i < maxVal; i++)
        {
            int nextX = xpos + (int)direction.x * i;
            int nextY = ypos + (int)direction.y * i;

            if (nextX >= 0 && nextX < width && nextY >= 0 && nextY < height)
            {
                var nextPiece = Pieces[nextX, nextY];
                if (nextPiece != null && nextPiece.pieceType == startPiece.pieceType)
                {
                    matches.Add(nextPiece);
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (matches.Count >= minPiece)
        {
            return matches;
        }

        return null;
    }

    // Busca matches completos de una pieza, combinando direcciones opuestas
    public List<Piece> GetMatchByPiece(int xpos, int ypos, int minPiece = 3)
    {
        var upMatches = GetMatchByDirection(xpos, ypos, new Vector2(0, 1), 2);
        var downMatches = GetMatchByDirection(xpos, ypos, new Vector2(0, -1), 2);
        var rightMatches = GetMatchByDirection(xpos, ypos, new Vector2(1, 0), 2);
        var leftMatches = GetMatchByDirection(xpos, ypos, new Vector2(-1, 0), 2);

        if (upMatches == null) upMatches = new List<Piece>();
        if (downMatches == null) downMatches = new List<Piece>();
        if (rightMatches == null) rightMatches = new List<Piece>();
        if (leftMatches == null) leftMatches = new List<Piece>();

        var verticalMatches = upMatches.Union(downMatches).ToList();
        var horizontalMatches = leftMatches.Union(rightMatches).ToList();

        var foundMatches = new List<Piece>();

        if (verticalMatches.Count >= minPiece)
        {
            foundMatches = foundMatches.Union(verticalMatches).ToList();
        }
        if (horizontalMatches.Count >= minPiece)
        {
            foundMatches = foundMatches.Union(horizontalMatches).ToList();
        }

        return foundMatches;
    }

}
