﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.1
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")>  _
Partial Friend NotInheritable Class AllTablesSettings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As AllTablesSettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New AllTablesSettings()),AllTablesSettings)
    
    Public Shared ReadOnly Property [Default]() As AllTablesSettings
        Get
            Return defaultInstance
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("AutoDate")>  _
    Public Property AutoDateField() As String
        Get
            Return CType(Me("AutoDateField"),String)
        End Get
        Set
            Me("AutoDateField") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("AutoMethod")>  _
    Public Property AutoMethodField() As String
        Get
            Return CType(Me("AutoMethodField"),String)
        End Get
        Set
            Me("AutoMethodField") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("AutoWho")>  _
    Public Property AutoWhoField() As String
        Get
            Return CType(Me("AutoWhoField"),String)
        End Get
        Set
            Me("AutoWhoField") = value
        End Set
    End Property
End Class
