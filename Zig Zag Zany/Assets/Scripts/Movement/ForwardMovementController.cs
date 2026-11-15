using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace Assets.Scripts.Movement {
	public class ForwardMovementController : MonoBehaviour {
		public SplineContainer spline;

		private GameInputActions _gameInputActions;

		private float _speed = 0;
		private float _t = 0;

		public bool _forwardPressed = false;
		public bool _backwardPressed = false;

		void Awake() {
			_gameInputActions = new GameInputActions();
		}

		void OnEnable() {
			_gameInputActions.Movement.Forward.performed += OnForwardPerformed;
			_gameInputActions.Movement.Forward.canceled += OnForwardCanceled;
			_gameInputActions.Movement.Backward.performed += OnBackwardPerformed;
			_gameInputActions.Movement.Backward.canceled += OnBackwardCanceled;
		}

		void OnDisable() {
			_gameInputActions.Movement.Forward.performed -= OnForwardPerformed;
			_gameInputActions.Movement.Forward.canceled -= OnForwardCanceled;
			_gameInputActions.Movement.Backward.performed -= OnBackwardPerformed;
			_gameInputActions.Movement.Backward.canceled -= OnBackwardCanceled;
		}

		private void Update() {
			if (_forwardPressed)
				_speed = Mathf.Min(_speed + 0.1f, 20);

			if (_backwardPressed)
				_speed = Mathf.Max(_speed - 0.1f, -20);

			if (_speed != 0) {
				_t = Mathf.Clamp(_t + _speed * Time.deltaTime, 0f, 1f);

				Vector3 position = spline.EvaluatePosition(_t);
				Vector3 tangent = spline.EvaluateTangent(_t);

				transform.position = position;
				transform.rotation = Quaternion.LookRotation(tangent);
			}
		}

		private void OnForwardPerformed(InputAction.CallbackContext context) {
			Debug.Log("OnForwardPerformed");
			if (_backwardPressed)
				return;

			_forwardPressed = true;
		}

		private void OnForwardCanceled(InputAction.CallbackContext context) {
			_forwardPressed = false;
		}

		private void OnBackwardPerformed(InputAction.CallbackContext context) {
			if (_forwardPressed)
				return;

			_backwardPressed = true;
		}

		private void OnBackwardCanceled(InputAction.CallbackContext context) {
			_backwardPressed = false;
		}
	}
}
