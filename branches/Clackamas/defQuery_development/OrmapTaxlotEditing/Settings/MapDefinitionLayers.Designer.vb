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
Partial Friend NotInheritable Class MapDefinitionLayers
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As MapDefinitionLayers = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MapDefinitionLayers),MapDefinitionLayers)
    
    Public Shared ReadOnly Property [Default]() As MapDefinitionLayers
        Get
            Return defaultInstance
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property LayerList() As String
        Get
            Return CType(Me("LayerList"),String)
        End Get
        Set
            Me("LayerList") = value
        End Set
    End Property
End Class
