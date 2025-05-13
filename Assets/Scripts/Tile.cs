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
        board =  board_;

    }
}
