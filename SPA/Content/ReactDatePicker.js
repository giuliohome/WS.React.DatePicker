import { DatePicker } from "https://unpkg.com/react-datepicker@0.53.0/dist/react-datepicker.js"
(function()
{
 "use strict";
 var Global,WebSharper,Obj,DatePicker$1,ReactDatePicker,IntelliFactory,Runtime;
 Global=self;
 WebSharper=Global.WebSharper;
 Obj=WebSharper&&WebSharper.Obj;
 DatePicker$1=Global.DatePicker=Global.DatePicker||{};
 ReactDatePicker=DatePicker$1.ReactDatePicker=DatePicker$1.ReactDatePicker||{};
 IntelliFactory=Global.IntelliFactory;
 Runtime=IntelliFactory&&IntelliFactory.Runtime;
 ReactDatePicker=DatePicker$1.ReactDatePicker=Runtime.Class({
  get_DatePicker:function()
  {
   return this.DatePickerImport;
  }
 },Obj,ReactDatePicker);
 ReactDatePicker.New=Runtime.Ctor(function()
 {
  Obj.New.call(this);
  this.DatePickerImport=DatePicker;
 },ReactDatePicker);
}());
