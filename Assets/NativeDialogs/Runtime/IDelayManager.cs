using System;

namespace NativeDialogs.Runtime
{
    public interface IDelayManager
    {
        void DelayFrame(Action callback);
    }
}