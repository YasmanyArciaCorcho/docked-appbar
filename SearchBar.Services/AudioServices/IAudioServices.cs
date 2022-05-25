using System;

namespace Services.AudioServices
{
    public interface IAudioServices: IDisposable
    {
        void ChangeAudioLevel(double audioLevel);

        double GetAudioLevel();
    }
}