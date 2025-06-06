using UnityEngine;

public class Tile : MonoBehaviour // Sistema de coordenadas 
{
    public int x;
    public int y;
    public Board board;

    public void Setup(int x_, int y_, Board board_)
    {
        x = x_;
        y = y_;
        board = board_;

    }
    //Deteccion den input del Mouse
    public void OnMouseDown()
    {
        board.TileDown(this);
    }
    public void OnMouseEnter()
    {
        board.TileOver(this);
    }
    public void OnMouseUp()
    {
        board.TileUp(this);
    }
}
