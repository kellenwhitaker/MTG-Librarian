using System.Collections.Generic;
using System.Linq;

namespace MTG_Librarian
{
    public static class TaskCollectionExtensions
    {
        public static IEnumerable<BackgroundTask> FindCompleted(this IEnumerable<BackgroundTask> tasks)
        {
            return tasks.Where(x => x.RunState == RunState.Completed);
        }

        public static IEnumerable<BackgroundTask> FindCompletedOrFailed(this IEnumerable<BackgroundTask> tasks)
        {
            return tasks.Where(x => x.RunState == RunState.Completed || x.RunState == RunState.Failed);
        }

        public static IEnumerable<BackgroundTask> FindInitialized(this IEnumerable<BackgroundTask> tasks)
        {
            return tasks.Where(x => x.RunState == RunState.Initialized);
        }
    }
}
