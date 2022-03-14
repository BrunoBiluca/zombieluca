using UnityEngine;

namespace Assets.GameAssets.ZombiePong
{
    public class BallHandler : MonoBehaviour
    {
        public float BallInitialSpeed = 1f;

        private Rigidbody rb;
        private Vector3 lookDirection;
        private Transform mesh;
        private float angle;

        public void Start()
        {
            rb = GetComponent<Rigidbody>();

            mesh = transform.Find("mesh");

            var dirX = Random.Range(-1f, 1f);
            var dirZ = Random.Range(-1f, 1f);
            lookDirection = new Vector3(dirX, 0f, dirZ);
            rb.AddForce(lookDirection * BallInitialSpeed, ForceMode.Impulse);
        }

        public void Deactivate()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            var rotY = Vector3.Angle(transform.forward, rb.velocity);

            angle += rb.velocity.magnitude;

            var rot = new Vector3(
                angle,
                rotY,
                mesh.localRotation.z
            );
            mesh.localRotation = Quaternion.Euler(rot);
        }

        public void FixedUpdate()
        {
            rb.AddForce(rb.velocity * BallInitialSpeed * Time.deltaTime, ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision collision)
        {
            lookDirection = Vector3.Cross(lookDirection, collision.GetContact(0).normal);
        }
    }
}