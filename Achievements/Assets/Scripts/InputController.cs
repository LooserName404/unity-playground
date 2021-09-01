using System;
using UnityEngine;

namespace TestLab
{
    public class InputController : MonoBehaviour
    {
        public Action OnJump;
        public Action OnMoveLeft;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnJump?.Invoke();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                OnMoveLeft?.Invoke();
            }
        }
    }
}
