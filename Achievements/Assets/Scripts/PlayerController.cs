using System;
using UnityEngine;

namespace TestLab
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private bool _isGrounded;
        private Rigidbody2D _rigidbody;

        public Action OnJumpSuccesfully;
        public Action OnMoveLeftSuccessfully;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            var input = FindObjectOfType<InputController>();
            input.OnJump += Jump;
            input.OnMoveLeft += MoveLeft;
        }

        private void MoveLeft()
        {
            _rigidbody.AddForce(Vector2.left * 0.1f, ForceMode2D.Force);
            OnMoveLeftSuccessfully?.Invoke();
        }

        private void OnCollisionEnter2D()
        {
            _isGrounded = true;
        }

        private void OnCollisionExit2D()
        {
            _isGrounded = false;
        }

        private void Jump()
        {
            if (CanJump())
            {
                _rigidbody.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                OnJumpSuccesfully?.Invoke();
            }
        }

        private bool CanJump()
        {
            return _isGrounded;
        }
    }
}
