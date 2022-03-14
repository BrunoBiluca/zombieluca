using UnityEngine;

namespace Assets.GameAssets.ZombiePong
{
    public class SimplePongAI : IPaddleInputs
    {
        private readonly Transform body;
        private Transform ball;
        private bool isEnabled = false;

        public SimplePongAI(Transform body, Transform ball)
        {
            this.body = body;
            this.ball = ball;
        }

        public Vector2 Move => EvaluateMove();

        private Vector2 EvaluateMove()
        {
            if(!isEnabled) return Vector2.zero;

            if(ball == null) return Vector2.zero;

            return EvaluateMoveFromBallAndBody();
        }

        private Vector2 EvaluateMoveFromBallAndBody()
        {
            try
            {
                if(ball.transform.position.z > body.position.z)
                    return new Vector2(0f, 1f);

                if(ball.transform.position.z < body.position.z)
                    return new Vector2(0f, -1f);
            }
            catch(MissingReferenceException)
            {
                ball = null;
            }
            return Vector2.zero;
        }

        public void Enable()
        {
            isEnabled = true;
        }

        public void SetBall(Transform ball)
        {
            this.ball = ball;
        }
    }
}