using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Movement {
	public class HorizontalMovement2 {
		private const float SPEED = 5.0f;

		public Vector3 GetMovement(GameInputActions gameInputActions, RailRig rig) {
			float inputAxis = gameInputActions.Movement.Horizontal.ReadValue<float>();
			if (inputAxis == 0)
				return Vector3.zero;

			return rig.Right * SPEED * inputAxis * GameTime.Current;
		}
	}
}
