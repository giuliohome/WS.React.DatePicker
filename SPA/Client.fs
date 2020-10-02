namespace UIBase

    open WebSharper
    open WebSharper.JavaScript
    open WebSharper.UI
    open WebSharper.UI.Html
    open WebSharper.UI.Client
    open WebSharper.UI.Notation
    
    [<JavaScript>]
    module MyVars =
        let count = Var.Create 0
        
        
        
namespace ReactHook

    open WebSharper
    open WebSharper.JavaScript
    open WebSharper.React
    open WebSharper.React.Html
    open System
    open WebSharper.Moment
    
    [<JavaScript>]
    module WrapReact =
        open System
        open WebSharper.React.Bindings
        
        let FunctionComponent (f: 'props -> React.Element) (props: 'props) : React.Element =
            React.CreateElement(As<Func<obj, React.Element>> f, box props)
            
        [<Inline "React.useState($0)">]
        let UseState (init: 'T) = X<'T * ('T -> unit)>
        
        let mutable setCount  = fun (i:int) ->  ()
    
    type DatePickerProps =
        {
            //className: string;
            selected: Date;
            onChange: Date -> unit;
            showTimeSelect: bool
        }
    
    [<JavaScript>]
    module HelloWorld =
        open System
        open WebSharper.React.Bindings
        open WebSharper.React.Html
        open WebSharper.JavaScript


        let Example () =
            
            let count, setCount = WrapReact.UseState 0
            
            let myDate, setMyDate = WrapReact.UseState (DateTime.Today.JS)
            let importDatePicker = JS.Eval("window.MyDatePicker") :?> React.Class 
            let propDP = 
                            {
                                selected = myDate 
                                onChange = setMyDate
                                showTimeSelect = true
                            }
            let datePicker =
                React.CreateElement( importDatePicker, propDP)
            WrapReact.setCount <- setCount
            div [] [
                
                datePicker
                p [] [Html.textf "You selected %s date %s time" (myDate.ToDateString()) (myDate.ToTimeString())]
                p [] [Html.textf "You clicked %i times" count]
                button [on.click (fun _ -> 
                            setCount (count + 1)
                            UIBase.MyVars.count.Set (count + 1) 
                        )] [
                    text "W# React Click me"
                ]
            ]


namespace SPA

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.UI.Html

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>
    
    let count = UIBase.MyVars.count

    [<SPAEntryPoint>]
    let Main () =

        

        let welcome = p [] [
               count.View
               |> View.Map (sprintf "You clicked %i times")
               |> textView
               ]
        div [] [
               welcome
               button [
                   on.click (fun _ _ ->
                       let next = count.Value + 1 
                       count.Set(next)
                       ReactHook.WrapReact.setCount next 
                       )
               ] [
                   text "W# UI Click Me!"
               ]
               div [
                   on.afterRender ( fun el ->
                           ReactHook.WrapReact.FunctionComponent ReactHook.HelloWorld.Example ()
                           |> WebSharper.React.React.Mount el
                       )
                   ] []
           ]
        |> Doc.RunById "mainUI"

