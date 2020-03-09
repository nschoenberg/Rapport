using System;

namespace Rapport.Data.Models
{
    public class WorklogModel
    {

        internal WorklogModel()
        {
            StartedUtc = DateTime.UtcNow;
        }

        public DateTime StartedUtc { get; }

        public bool IsFinished => FinishedUtc.HasValue;

        public DateTime? FinishedUtc { get; private set; }

        public TimeSpan Finish()
        {
            if (FinishedUtc.HasValue == false)
            {
                FinishedUtc = DateTime.UtcNow;
            }

            return StartedUtc - FinishedUtc.Value;
        }

    }
}
