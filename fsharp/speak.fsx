#r "System.Runtime.WindowsRuntime"

open System
open System.Threading.Tasks

let SpeechSynthesizer = Type.GetType @"
    Windows.Media.SpeechSynthesis.SpeechSynthesizer,
    Windows.Foundation.UniversalApiContract,
    ContentType=WindowsRuntime"
let SpeechSynthesisStream = Type.GetType @"
    Windows.Media.SpeechSynthesis.SpeechSynthesisStream,
    Windows.Foundation.UniversalApiContract,
    ContentType=WindowsRuntime"
let MediaPlayer = Type.GetType @"
    Windows.Media.Playback.MediaPlayer,
    Windows.Foundation.UniversalApiContract,
    ContentType=WindowsRuntime"
let TypedEventHandler = Type.GetType(@"
    Windows.Foundation.TypedEventHandler`2,
    Windows.Foundation.FoundationContract,
    ContentType=WindowsRuntime").MakeGenericType(MediaPlayer, typeof<obj>)

let AsTaskGeneric = query {
    for m in typeof<WindowsRuntimeSystemExtensions>.GetMethods() do
    where (m.Name = "AsTask")
    let ps = m.GetParameters()
    where (ps.Length = 1 && ps.[0].ParameterType.Name = "IAsyncOperation`1")
    select m
    exactlyOne }
let await resultType winRtTask =
    let AsTask = AsTaskGeneric.MakeGenericMethod [|resultType|]
    let task = AsTask.Invoke(null, [|winRtTask|])
    task.GetType().GetProperty("Result").GetValue(task)

let setVoice synthesizer (v: string) =
    let AllVoices = SpeechSynthesizer.GetProperty("AllVoices")
    match [ for voice in AllVoices.GetValue(null) :?> seq<obj> do
            let DisplayName = voice.GetType().GetProperty("DisplayName")
            if (DisplayName.GetValue(voice) :?> String) = v then yield voice ] with
    | [voice] -> synthesizer.GetType().GetProperty("Voice").SetValue(synthesizer, voice)
    | _ -> ()

let voice = "Microsoft Ichiro"
let text  = "こんにちは、世界"
let synthesizer = Activator.CreateInstance(SpeechSynthesizer)
setVoice synthesizer voice
let stream =
    SpeechSynthesizer.GetMethod("SynthesizeTextToStreamAsync").Invoke(synthesizer, [|text|])
    |> await SpeechSynthesisStream
let player = Activator.CreateInstance(MediaPlayer)
MediaPlayer.GetMethod("SetStreamSource").Invoke(player, [|stream|])
let tcs = TaskCompletionSource<unit>()
type C = static member f(_: obj, _: obj) = tcs.SetResult()
let d = Delegate.CreateDelegate(TypedEventHandler, typeof<C>.GetMethod("f"))
MediaPlayer.GetEvent("MediaEnded").AddMethod.Invoke(player, [|d|])
MediaPlayer.GetMethod("Play").Invoke(player, [||])
tcs.Task.Result
