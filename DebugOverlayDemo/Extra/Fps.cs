using System.Diagnostics;

namespace DebugOverlayDemo
{
	public sealed class Fps
	{
		public uint FramesPerSecond { get { return mFramesPerSecond; } }

		private uint mFrameCount = 0;
		private uint mFramesPerSecond = 0;
		private Stopwatch mStopwatch = new Stopwatch();

		/// <summary>
		/// Starts the FPS timer.
		/// </summary>
		public Fps()
		{
			mStopwatch.Start();
		}

		/// <summary>
		/// Updates the FPS.
		/// </summary>
		public void Tick()
		{
			mFrameCount++;

			if (mStopwatch.ElapsedMilliseconds > 1000)
			{
				mFramesPerSecond = mFrameCount;
				mFrameCount = 0;

				mStopwatch.Reset();
				mStopwatch.Start();
			}
		}
	}
}
