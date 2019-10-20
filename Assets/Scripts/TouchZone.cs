using UnityEngine;

public class TouchZone : MonoBehaviour
{
    private Vector2 _touchDownPosition = Vector2.negativeInfinity;
    private BallController _controller;

    private void Start()
    {
        _controller = FindObjectOfType<BallController>();
    }

    private void Update()
    {
        DetectMouseTouch();

        if (Input.touches.Length > 0)
        {
            var touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _touchDownPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    CheckSwipe(touch.position);
                    break;
            }
        }
    }

    private void DetectMouseTouch()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer) return;

        if (Input.GetMouseButtonDown(0))
        {
            _touchDownPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            CheckSwipe(Input.mousePosition);
        }
    }

    private void CheckSwipe(Vector2 endPosition)
    {
        var swipe = endPosition - _touchDownPosition;
        var swipeDirection = swipe.normalized;
        DetectSwipeDirection(swipeDirection);
    }

    private void DetectSwipeDirection(Vector3 dragVector)
    {
        var positiveX = Mathf.Abs(dragVector.x);
        var positiveY = Mathf.Abs(dragVector.y);
        if (positiveX > positiveY)
        {
            _controller.CheckSwipe(dragVector.x > 0 ? Ball.BallType.Right : Ball.BallType.Left);
        }
        else
        {
            _controller.CheckSwipe(dragVector.y > 0 ? Ball.BallType.Top : Ball.BallType.Bottom);
        }
    }
}