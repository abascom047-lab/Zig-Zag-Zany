using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Splines;

namespace Assets.Scripts.Movement {
	public class MainMovementController : MonoBehaviour {
		public CharacterController CharControl;
		public SplineContainer Spline;

		private ForwardMovement _forwardMovement;
		private HorizontalMovement _horizontalMovement;
		private GameInputActions _gameInputActions;

		void Awake() {
			_forwardMovement = new ForwardMovement(Spline);
			_horizontalMovement = new HorizontalMovement();
			_gameInputActions = new GameInputActions();
		}

		void OnEnable() {
			_forwardMovement.Enable(_gameInputActions);
			_gameInputActions.Movement.Enable();
		}

		void OnDisable() {
			_forwardMovement.Disable(_gameInputActions);
			_gameInputActions.Movement.Disable();
		}

		void Update() {
			_forwardMovement.Update();
			_horizontalMovement.Update(_gameInputActions);

			Vector3 targetPosition = _forwardMovement.Center + _forwardMovement.Right * _horizontalMovement.Movement;

			if (CharControl.transform.position != targetPosition) {
				Vector3 direction = targetPosition - CharControl.transform.position;
				CharControl.Move(direction);
			}
		}
	}
}
