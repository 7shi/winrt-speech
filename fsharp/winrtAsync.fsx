#r "System.Runtime.WindowsRuntime"

open System

let AsTaskGeneric = query {
    for m in typeof<WindowsRuntimeSystemExtensions>.GetMethods() do
    where (m.Name = "AsTask")
    let ps = m.GetParameters()
    where (ps.Length = 1 && ps.[0].ParameterType.Name = "IAsyncOperation`1")
    select m
    exactlyOne }

let await resultType winRtTask =
    let asTask = AsTaskGeneric.MakeGenericMethod [|resultType|]
    let task = asTask.Invoke(null, [|winRtTask|])
    task.GetType().GetProperty("Result").GetValue(task)
