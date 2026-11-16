using Assets.Scripts.Utils;

namespace Assets.Scripts.Movement {
	public class HorizontalMovement {
		public float Offset { get; private set; }

		private const float SPEED = 10.0f;

		public void Update(GameInputActions gameInputActions) {
			float inputAxis = gameInputActions.Movement.Horizontal.ReadValue<float>();
			if (inputAxis == 0)
				return;

			Offset += inputAxis * SPEED * GameTime.Current;
		}
	}
}
