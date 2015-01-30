using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DebugOverlayDemo
{
	public class Application : Microsoft.Xna.Framework.Game
	{
		FlyCamera mCamera;
		Fps mFps;

		public Application()
		{
			new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			mCamera = new FlyCamera(GraphicsDevice);
			mCamera.Position = new Vector3(0, 10, 30);
			mCamera.AspectRation = GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
			mCamera.FieldOfView = MathHelper.PiOver4;
			mCamera.NearPlaneDistance = 0.1f;
			mCamera.FarPlaneDistance = 10000.0f;
			mCamera.Speed = 10;
			mCamera.MouseDampening = .002f;

			mFps = new Fps();

			// Create the DebugOverlay.
			new DebugOverlay(GraphicsDevice, Content);

			base.Initialize();
		}

		protected override void Update(GameTime gameTime)
		{
			#region Input.
			if (!IsActive)
				return;

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				this.Exit();
			}
			#endregion

			/////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////
			// Let's draw some stuff...
			// Draw code can be called from anywhere in the application -
			//		even in separate threads! Use it for AI, Physics,
			//		Gameplay, Graphics, etc.

			// Lines.
			for (int i = 0; i < 25; ++i)
			{
				DebugOverlay.Line(new Vector3(.5f * i, 5, 5), new Vector3(.5f * i, 5, -5), Color.Tomato);
				DebugOverlay.Line(new Vector3(.5f * i, 3, 5), new Vector3(.5f * i, 3, -5), Color.Violet);
				DebugOverlay.Line(new Vector3(.5f * i, 1, 5), new Vector3(.5f * i, 1, -5), Color.SeaGreen);
			}

			// Points.
			float radius = 4;
			for (int i = 0; i < 360; i += 2)
			{
				float rads = i * MathHelper.Pi / 180;
				DebugOverlay.Point(new Vector3(
					(float)Math.Cos(rads) * radius,
					-5, 
					(float)Math.Sin(rads) * radius),
					Color.Black);
			}

			// Spheres.
			DebugOverlay.Sphere(new Vector3(-10, 0, 5), 3, Color.Brown);
			DebugOverlay.Sphere(new Vector3(-5, 0, 5), 2, Color.DarkGreen);
			DebugOverlay.Sphere(new Vector3(-2, 0, 5), 1, Color.Crimson);
			DebugOverlay.Sphere(new Vector3(-.5f, 0, 5), .5f, Color.DarkSalmon);

			// Bounding box.
			DebugOverlay.BoundingBox(new BoundingBox(new Vector3(-10, 0, -10), new Vector3(-5, 5, -5)), Color.RoyalBlue);

			// Screen text.
			DebugOverlay.ScreenText("FPS: " + mFps.FramesPerSecond, new Vector2(5, 7), Color.DimGray);
			DebugOverlay.ScreenText("FPS: " + mFps.FramesPerSecond, new Vector2(7, 5), Color.White);

			// Arrows.
			DebugOverlay.Arrow(Vector3.Zero, Vector3.UnitX * 10, 1, Color.Red);
			DebugOverlay.Arrow(Vector3.Zero, Vector3.UnitY * 10, 1, Color.Green);
			DebugOverlay.Arrow(Vector3.Zero, Vector3.UnitZ * 10, 1, Color.Blue);

			/////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////

			#region Update.
			mCamera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

			mFps.Tick();

			base.Update(gameTime);
			#endregion
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Draw the DebugOverlay.
			DebugOverlay.Singleton.Draw(mCamera.ProjectionMatrix, mCamera.ViewMatrix);

			base.Draw(gameTime);
		}
	}
}