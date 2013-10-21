using System;
using System.Diagnostics;
using EightBitCamera.Data.Commands;
using EightBitCamera.Data.Queries;

namespace EightBitCamera.Helpers
{
    /// <summary>
    /// This helper class controls the behaviour of the FeedbackOverlay control
    /// When the app has been launched 5 times the initial prompt is shown
    /// If the user reviews no more prompts are shown
    /// When the app has bee launched 10 times and not been reviewed, the prompt is shown
    /// </summary>
    public class FeedbackHelper
    {
        private const int FIRST_COUNT = 5;
        private const int SECOND_COUNT = 10;

        private int _launchCount = 0;
        private bool _reviewed = false;

        public static readonly FeedbackHelper Default = new FeedbackHelper();

        private FeedbackState _state = FeedbackState.Inactive;

        public FeedbackState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// This should only be called when the app is Launching
        /// </summary>
        public void Launching()
        {
            _launchCount = new LaunchCountQuery().Get();
            _reviewed = new ReviewedQuery().Get();

            if (!_reviewed)
            {
                _launchCount++;

                if (_launchCount == FIRST_COUNT)
                    _state = FeedbackState.FirstReview;
                else if (_launchCount == SECOND_COUNT)
                    _state = FeedbackState.SecondReview;

                StoreState();
            }
        }

        /// <summary>
        /// Stores current state
        /// </summary>
        private void StoreState()
        {
            new LaunchCountCommand().Set(_launchCount);
            new ReviewedCommand().Set(_reviewed);
        }

        /// <summary>
        /// Call when user has reviewed
        /// </summary>
        public void Reviewed()
        {
            _reviewed = true;

            StoreState();
        }
    }
}
