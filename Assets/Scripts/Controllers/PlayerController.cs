using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public float MOVEMENT_SPEED;

        private InputAction MoveAction;
        private Rigidbody rigidbodyComponent;


        private void Start()
        {
            
        }

        public void Initialize()
        {
            rigidbodyComponent = GetComponent<Rigidbody>();
            MoveAction = InputSystem.actions.FindAction("Move");
        }

        private void Update()
        {
            var move = MoveAction.ReadValue<Vector2>();
            Vector3 translate = new(move.x, 0, move.y);
            rigidbodyComponent.MovePosition(transform.position + transform.rotation * translate * Time.deltaTime * MOVEMENT_SPEED);
        }
    }
}