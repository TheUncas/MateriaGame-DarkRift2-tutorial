using System.Diagnostics;
using System.Threading;
using System;

namespace Utilities
{
    public class Clock
    {
        #region Properties

        /// <summary>
        /// Set to true if you want to stop the thread
        /// </summary>
        public bool isAbortRequested;

        /// <summary>
        /// seconds between each tick
        /// </summary>
        public float secondsBetweenTicks;

        /// <summary>
        /// milliseconds between each tick
        /// </summary>
        public int milliSecondsBetweenTicks;

        /// <summary>
        /// Number of the frame
        /// </summary>
        public readonly int currentTick;

        #endregion

        #region Event declaration
        public delegate void TickHandler();
        public event TickHandler Tick;
        #endregion


        private AutoResetEvent timerEvent = new AutoResetEvent(true);

        #region Constructor
        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="pTicksPerSecond"></param>
        /// <param name="pStartAt"></param>
        public Clock(float pTicksPerSecond, int pStartAt = 0)
        {
            //////////////////////////
            // Calculate variables
            secondsBetweenTicks = 1 / pTicksPerSecond;
            milliSecondsBetweenTicks = (int)(secondsBetweenTicks * 1000);

            //////////////////////////
            // Start the tread
            Thread gameBackgroundThread = new Thread(TickOnBackground);
            gameBackgroundThread.Priority = ThreadPriority.AboveNormal;
            gameBackgroundThread.IsBackground = true;
            gameBackgroundThread.Start();
        }

        #endregion
        public void TickOnBackground()
        {
            ////////////////////////////////////
            // Variable initilisation
            double nextTickInMilliSeconds = milliSecondsBetweenTicks;
            double ellapsedMilliSeconds = 0;
            double totalFrameTimeInMilliSeconds = 0;
            double maxFrameTime = 0;

            //Start the timer
            Stopwatch timer = Stopwatch.StartNew();
            timer.Start();

            //While the abort request is not received
            while (!isAbortRequested)
            {
                ellapsedMilliSeconds = timer.Elapsed.TotalMilliseconds;

                //Invoke all suscribers
                Tick?.Invoke();

                totalFrameTimeInMilliSeconds += timer.Elapsed.TotalMilliseconds - ellapsedMilliSeconds;
                maxFrameTime = Math.Max(maxFrameTime, timer.Elapsed.TotalMilliseconds - ellapsedMilliSeconds);

                if (timer.Elapsed.TotalMilliseconds < nextTickInMilliSeconds)
                {
                    timerEvent.WaitOne((int)(nextTickInMilliSeconds - timer.Elapsed.TotalMilliseconds));
                }

                nextTickInMilliSeconds += milliSecondsBetweenTicks;
            }
        }
    }
}