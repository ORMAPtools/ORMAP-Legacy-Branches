﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3603
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")>  _
Partial Friend NotInheritable Class CartographicLinesSettings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As CartographicLinesSettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New CartographicLinesSettings),CartographicLinesSettings)
    
    Public Shared ReadOnly Property [Default]() As CartographicLinesSettings
        Get
            Return defaultInstance
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("LineType")>  _
    Public Property LineTypeField() As String
        Get
            Return CType(Me("LineTypeField"),String)
        End Get
        Set
            Me("LineTypeField") = value
        End Set
    End Property
End Class
