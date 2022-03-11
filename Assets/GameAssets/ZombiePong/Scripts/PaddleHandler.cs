using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;

namespace Assets.GameAssets.ZombiePong
{
    public class PaddleHandler : MonoBehaviour
    {
        public float PaddleSpeed = .6f;
        public bool IsLeft = true;

        private PaddleInputs inputs;

        private List<Transform> childrenTransforms = new List<Transform>();
        private List<Animator> childrenAnimators = new List<Animator>();

        //private ExitGameHandler a;

        private LerpAngle lerpDirection;

        public void Awake()
        {
            foreach(Transform child in transform)
            {
                var zombie = child.Find("jill");
                if(zombie != null)
                {
                    childrenTransforms.Add(zombie);
                    childrenAnimators.Add(zombie.GetComponent<Animator>());
                }
            }

            lerpDirection = new LerpAngle(0f) { RetainState = true };
        }

        public PaddleHandler Setup(PaddleInputs paddleInputs)
        {
            inputs = paddleInputs;
            inputs.Enable();
            return this;
        }

        public void Update()
        {
            var move = inputs.Move;

            UpdateZombiesAnimations(move);

            transform.position += PaddleSpeed * Time.deltaTime * new Vector3(move.x, 0f, move.y);
        }

        private void UpdateZombiesAnimations(Vector2 move)
        {
            if(move.x == 0f && move.y == 0f)
            {
                foreach(var anim in childrenAnimators)
                {
                    anim.SetBool("walking", false);
                }
                lerpDirection.SetEndValue(90f * (IsLeft ? 1 : -1));
            }
            else
            {
                foreach(var anim in childrenAnimators)
                {
                    anim.SetBool("walking", true);
                }

                var directionAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
                lerpDirection.SetEndValue(directionAngle);
            }

            var rot = Quaternion.Euler(0f, lerpDirection.EvalAngle(5f), 0f);
            foreach(var trans in childrenTransforms)
                trans.rotation = rot;
        }
    }
}