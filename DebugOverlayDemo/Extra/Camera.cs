using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DebugOverlayDemo
{
	public class FlyCamera
	{
		public Vector3 Position { get; set; }
		public float AspectRation { get; set; }
		public float FieldOfView { get; set; }
		public float NearPlaneDistance { get; set; }
		public float FarPlaneDistance { get; set; }
		public float Speed { get; set; }
		public float MouseDampening { get; set; }

		public Viewport Viewport { get { return mGraphicsDevice.Viewport; } }
		public int ViewportHeight { get { return Viewport.Height; } }
		public int ViewportWidth { get { return Viewport.Width; } }

		private readonly float mPitchRange = MathHelper.ToRadians(89);

		private Quaternion mHorizontalRotation;
		private Quaternion mVerticalRotation;
		private Matrix mTotalRotation;

		private float mYaw = 0;
		private float mPitch = 0;

		private Matrix mProjectionMatrix = Matrix.Identity;
		private Matrix mViewMatrix = Matrix.Identity;

		private GraphicsDevice mGraphicsDevice;

		public Matrix ProjectionMatrix
		{
			get
			{
				mProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
					FieldOfView,
					AspectRation,
					NearPlaneDistance,
					FarPlaneDistance);

				return mProjectionMatrix;
			}
		}

		public Matrix ViewMatrix
		{
			get
			{
				mTotalRotation = Matrix.CreateFromQuaternion(mVerticalRotation) * Matrix.CreateFromQuaternion(mHorizontalRotation);
				mViewMatrix = Matrix.CreateLookAt(Position, Position + Vector3.Transform(Vector3.Forward, mTotalRotation), Vector3.Up);

				return mViewMatrix;
			}
		}

		public FlyCamera(GraphicsDevice graphicsDevice)
		{
			mGraphicsDevice = graphicsDevice;

			// Initialize mouse to center of screen.
			int windowCenterHorizontal = ViewportWidth / 2;
			int windowCenterVertical = ViewportHeight / 2;
			Mouse.SetPosition(windowCenterHorizontal, windowCenterVertical);
		}

		public Vector3 Forward { get { return mTotalRotation.Forward; } }
		public Vector3 Back { get { return mTotalRotation.Backward; } }
		public Vector3 Left { get { return Matrix.CreateFromQuaternion(mHorizontalRotation).Left; } }
		public Vector3 Right { get { return Matrix.CreateFromQuaternion(mHorizontalRotation).Right; } }

		public void AdjustAltitude(float distance) { Position += Vector3.UnitY * distance; }
		public void MoveForward(float distance) { Position += Forward * distance; }
		public void MoveBackward(float distance) { Position += Back * distance; }
		public void MoveLeft(float distance) { Position += Left * distance; }
		public void MoveRight(float distance) { Position += Right * distance; }

		public void AdjustRotation(float deltaYaw, float deltaPitch)
		{
			mYaw += deltaYaw;
			mPitch += deltaPitch;
			mPitch = MathHelper.Clamp(mPitch, -mPitchRange, mPitchRange);

			mHorizontalRotation = Quaternion.CreateFromAxisAngle(Vector3.Up, mYaw);
			mVerticalRotation = Quaternion.CreateFromAxisAngle(Vector3.Right, mPitch);
		}

		public void Update(float secondsSinceLastTick)
		{
			#region Mouse-look.
			MouseState mouseState = Mouse.GetState();

			int windowCenterHorizontal = ViewportWidth / 2;
			int windowCenterVertical = ViewportHeight / 2;

			int offsetX = mouseState.X - windowCenterHorizontal;
			int offsetY = mouseState.Y - windowCenterVertical;

			AdjustRotation(-offsetX * MouseDampening, -offsetY * MouseDampening);

			Mouse.SetPosition(windowCenterHorizontal, windowCenterVertical);
			#endregion

			#region Keyboard movement.
			KeyboardState keyboardState = Keyboard.GetState();

			float distance = Speed * secondsSinceLastTick;

			if (keyboardState.IsKeyDown(Keys.W))
				MoveForward(distance);
			if (keyboardState.IsKeyDown(Keys.A))
				MoveLeft(distance);
			if (keyboardState.IsKeyDown(Keys.S))
				MoveBackward(distance);
			if (keyboardState.IsKeyDown(Keys.D))
				MoveRight(distance);
			if (keyboardState.IsKeyDown(Keys.LeftShift))
				AdjustAltitude(distance);
			if (keyboardState.IsKeyDown(Keys.LeftControl))
				AdjustAltitude(-distance);
			#endregion
		}
	}
}