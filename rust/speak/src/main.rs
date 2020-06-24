winrt::import!(
    dependencies
        os
    types
        windows::media::core::*
        windows::media::playback::*
        windows::media::speech_synthesis::*
);

use windows::media::core::*;
use windows::media::playback::*;
use windows::media::speech_synthesis::*;

fn set_voice(synthesizer: &SpeechSynthesizer, v: &str) -> winrt::Result<()> {
    for voice in SpeechSynthesizer::all_voices()? {
        if voice.display_name()? == v {
            synthesizer.set_voice(voice)?;
            break;
        }
    }
    Ok(())
}

fn main() -> winrt::Result<()> {
    let voice = "Microsoft Ichiro";
    let text  = "こんにちは、世界";
    let synthesizer = SpeechSynthesizer::new()?;
    set_voice(&synthesizer, &voice)?;
    let stream = synthesizer.synthesize_text_to_stream_async(text)?.get()?;
    let player = MediaPlayer::new()?;
    let content_type = stream.content_type()?;
    player.set_source(MediaSource::create_from_stream(stream, content_type)?)?;
    player.play()?;
    loop {
        std::thread::sleep(std::time::Duration::from_millis(200));
        if player.playback_session()?.playback_state()? != MediaPlaybackState::Playing {
            break;
        }
    };
    Ok(())
}
