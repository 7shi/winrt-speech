winrt::import!(
    dependencies
        os
    types
        windows::media::speech_synthesis::*
);

fn main() -> winrt::Result<()> {
    use windows::media::speech_synthesis::*;
    for voice in SpeechSynthesizer::all_voices()? {
        println!("{}: {}", voice.language()?, voice.description()?);
    }
    Ok(())
}
