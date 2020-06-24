using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.SpeechSynthesis;
using Windows.Media.Playback;

class Program
{
    static void SetVoice(SpeechSynthesizer synthesizer, string v) {
        foreach (var voice in SpeechSynthesizer.AllVoices) {
            if (voice.DisplayName == v) {
                synthesizer.Voice = voice;
                break;
            }
        }
    }

    static async Task Main()
    {
        var voice = "Microsoft Ichiro";
        var text  = "こんにちは、世界";
        var synthesizer = new SpeechSynthesizer();
        SetVoice(synthesizer, voice);
        var stream = await synthesizer.SynthesizeTextToStreamAsync(text);
        var player = new MediaPlayer();
        player.Source = MediaSource.CreateFromStream(stream, stream.ContentType);
        var tcs = new TaskCompletionSource<int>();
        player.MediaEnded += (sender, o) => tcs.SetResult(0);
        player.Play();
        await tcs.Task;
    }
}
