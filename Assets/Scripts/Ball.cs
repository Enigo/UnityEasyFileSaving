using UnityEngine;

public class Ball : MonoBehaviour
{
    public BallType Type;

    
    public enum BallType
    {
        Right,
        Left,
        Top,
        Bottom
    }
}