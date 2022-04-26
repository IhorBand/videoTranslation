using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VideoTranslate.Service.Services
{
    public abstract class BaseService
    {
        protected T TraceAction<T>(ActivitySource activitySource, string className, Func<T> method, [CallerMemberName] string? callerMemberName = null)
        {
            var activity = activitySource.StartActivity($"{className}.{callerMemberName}", ActivityKind.Internal);

            try
            {
                return method();
            }
            finally
            {
                activity?.Stop();
            }
        }

        protected void TraceAction(ActivitySource activitySource, string className, Action method, [CallerMemberName] string? callerMemberName = null)
        {
            var activity = activitySource.StartActivity($"{className}.{callerMemberName}", ActivityKind.Internal);

            try
            {
                method();
            }
            finally
            {
                activity?.Stop();
            }
        }
    }
}
