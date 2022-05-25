using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Services.AudioServices
{
    public class ComputerAudioServices: IAudioServices
    {
        private readonly IAudioController _audioCtl = new CoreAudioController();

        public void ChangeAudioLevel(double audioLevel)
        {
            var audioDevice = _audioCtl.DefaultPlaybackDevice;
            audioDevice.Volume = audioLevel;
        }

        public double GetAudioLevel()
        {
            var audioDevice = _audioCtl.DefaultPlaybackDevice;
            return audioDevice.Volume;
        }
        
        public void Dispose()
        {
            _audioCtl.Dispose();
        }
        
        // ComputerSettingsServices.OpenSoundDevices();
    }
}