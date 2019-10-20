using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BallController : MonoBehaviour
{
    public List<Ball> ObjectsPrefabs;
    public Text LivesLeftCounter;
    public Text StarsCounter;

    private DataHandler _handler;
    private readonly Random _random = new Random();
    private Ball _activeBall;
    private int _livesLeft = 3, _stars, _incorrectSwipes;

    private void Start()
    {
        _handler = FindObjectOfType<DataHandler>();
        LivesLeftCounter.text = _livesLeft.ToString();
        StarsCounter.text = _handler.LoadStarsData().starsCollected.ToString();

        SpawnNew();
    }

    private void SpawnNew()
    {
        var randomElement = GetRandomPiece();
        _activeBall = Instantiate(randomElement, Vector3.zero, Quaternion.identity, gameObject.transform);
    }

    private Ball GetRandomPiece()
    {
        return ObjectsPrefabs.ElementAt(_random.Next(ObjectsPrefabs.Count));
    }

    public void CheckSwipe(Ball.BallType type)
    {
        if (_activeBall.Type == type)
        {
            HandleStar();
            StartCoroutine(DestroyBall());
        }
        else
        {
            LivesLeftCounter.text = (--_livesLeft).ToString();
            _incorrectSwipes++;
            if (_livesLeft <= 0)
            {
                LevelFailed();
            }
        }
    }

    private void LevelFailed()
    {
        _handler.SaveData(_incorrectSwipes, _stars);
        _livesLeft = 3;
        _stars = 0;

        LivesLeftCounter.text = _livesLeft.ToString();
    }


    private IEnumerator DestroyBall()
    {
        Vector2 positionToMove;
        switch (_activeBall.Type)
        {
            case Ball.BallType.Bottom:
                positionToMove = new Vector2(0, -100);
                break;
            case Ball.BallType.Top:
                positionToMove = new Vector2(0, 100);
                break;
            case Ball.BallType.Left:
                positionToMove = new Vector2(-100, 0);
                break;
            case Ball.BallType.Right:
                positionToMove = new Vector2(100, 0);
                break;
            default:
                positionToMove = new Vector2(0, -100);
                break;
        }

        for (var i = 0; i < 10; i++)
        {
            _activeBall.transform.position =
                Vector2.MoveTowards(_activeBall.transform.position, positionToMove, Time.deltaTime * 25);
            yield return new WaitForSeconds(0.01f);
        }

        Destroy(_activeBall.gameObject);
        SpawnNew();
        yield return null;
    }

    private void HandleStar()
    {
        if (_random.Next(100) > 50)
        {
            _stars++;
            var stars = int.Parse(StarsCounter.text);
            StarsCounter.text = (++stars).ToString();
        }
    }
}