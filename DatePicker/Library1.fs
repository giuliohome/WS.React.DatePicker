namespace DatePicker

open WebSharper
open WebSharper.JavaScript
open WebSharper.React
open WebSharper.React.Html
open System


module DatePickerLibrary =
    
    [<JavaScriptExport>]
    type ReactDatePicker() = 
        [<Inline>]
        member this.DatePicker : System.Func<obj,React.Element> = 
            JS.Import("DatePicker", "https://unpkg.com/react-datepicker@0.53.0/dist/react-datepicker.js")