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
    module React =
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
            selected: Moment.Moment;
            onChange: Moment -> unit;
        }
    
    [<JavaScript>]
    module HelloWorld =
        open System
        open WebSharper.React.Bindings
        open WebSharper.React.Html


        let Example () =
            
            let count, setCount = React.UseState 0
            
            let myDate, setMyDate = React.UseState (Moment(DateTime.Today.JS))
            let importDatePicker = JS.Eval("DatePicker.default") :?> React.Class 
            let datePicker =
                React.CreateElement ( importDatePicker , 
                            {
                                selected = myDate 
                                onChange = setMyDate 
                            }, [||])
            
            React.setCount <- setCount
            div [] [
                datePicker
                p [] [Html.textf "You selected %s date" (myDate.ToDate().ToDateString())]
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

    let People =
        ListModel.FromSeq [
            "John"
            "Paul"
        ]

    let count = UIBase.MyVars.count

    [<SPAEntryPoint>]
    let Main () =
        let newName = Var.Create ""

        IndexTemplate.Main()
            .ListContainer(
                People.View.DocSeqCached(fun (name: string) ->
                    IndexTemplate.ListItem().Name(name).Doc()
                )
            )
            .Name(newName)
            .Add(fun _ ->
                People.Add(newName.Value)
                newName.Value <- ""
            )
            .Doc()
        |> Doc.RunById "main"
        

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
                       ReactHook.React.setCount next 
                       )
               ] [
                   text "W# UI Click Me!"
               ]
               div [
                   on.afterRender ( fun el ->
                           ReactHook.React.FunctionComponent ReactHook.HelloWorld.Example ()
                           |> WebSharper.React.React.Mount el
                       )
                   ] []
           ]
        |> Doc.RunById "mainUI"

