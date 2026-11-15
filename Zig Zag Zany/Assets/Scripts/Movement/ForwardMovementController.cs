using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace Assets.Scripts.Movement {
	public class ForwardMovementController : MonoBehaviour {
		public SplineContainer spline;

		private GameInputActions _gameInputActions;

		private float _speed = 0;
		private float _t = 0;

		private bool _forwardPressed = false;
		private bool _backwardPressed = false;

		private const float SPEED_INCREMENT = 0.25f;
		private const float HIGH_SPEED = 10f;

		void Awake() {
			_gameInputActions = new GameInputActions();
		}

		void OnEnable() {
			_gameInputActions.Movement.Forward.performed += OnForwardPerformed;
			_gameInputActions.Movement.Forward.canceled += OnForwardCanceled;
			_gameInputActions.Movement.Backward.performed += OnBackwardPerformed;
			_gameInputActions.Movement.Backward.canceled += OnBackwardCanceled;

			_gameInputActions.Movement.Enable();
		}

		void OnDisable() {
			_gameInputActions.Movement.Forward.performed -= OnForwardPerformed;
			_gameInputActions.Movement.Forward.canceled -= OnForwardCanceled;
			_gameInputActions.Movement.Backward.performed -= OnBackwardPerformed;
			_gameInputActions.Movement.Backward.canceled -= OnBackwardCanceled;

			_gameInputActions.Movement.Disable();
		}

		private void Update() {
			if (_forwardPressed) {
				_speed = Mathf.Min(_speed + SPEED_INCREMENT * GameTime.Current, HIGH_SPEED);
			} else if (_backwardPressed) {
				_speed = Mathf.Max(_speed - SPEED_INCREMENT * GameTime.Current, -HIGH_SPEED);
			} else {
				_speed = Mathf.MoveTowards(_speed, 0f, SPEED_INCREMENT * GameTime.Current);
			}

			if (_speed != 0) {
				_t = Mathf.Clamp(_t + _speed * GameTime.Current, 0f, 1f);

				Vector3 position = spline.EvaluatePosition(_t);
				Vector3 tangent = spline.EvaluateTangent(_t);

				transform.position = position;
				transform.rotation = Quaternion.LookRotation(tangent);
			}
		}

		private void OnForwardPerformed(InputAction.CallbackContext context) {
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
