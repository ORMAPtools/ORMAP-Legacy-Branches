﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.832
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")>  _
Partial Friend NotInheritable Class DefaultValuesSettings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As DefaultValuesSettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New DefaultValuesSettings),DefaultValuesSettings)
    
    Public Shared ReadOnly Property [Default]() As DefaultValuesSettings
        Get
            Return defaultInstance
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("3")>  _
    Public Property County() As String
        Get
            Return CType(Me("County"),String)
        End Get
        Set
            Me("County") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property TownshipPart() As String
        Get
            Return CType(Me("TownshipPart"),String)
        End Get
        Set
            Me("TownshipPart") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("S")>  _
    Public Property TownshipDirection() As String
        Get
            Return CType(Me("TownshipDirection"),String)
        End Get
        Set
            Me("TownshipDirection") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property RangePart() As String
        Get
            Return CType(Me("RangePart"),String)
        End Get
        Set
            Me("RangePart") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("E")>  _
    Public Property RangeDirection() As String
        Get
            Return CType(Me("RangeDirection"),String)
        End Get
        Set
            Me("RangeDirection") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property MapSuffixType() As String
        Get
            Return CType(Me("MapSuffixType"),String)
        End Get
        Set
            Me("MapSuffixType") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("000")>  _
    Public Property MapSuffixNumber() As String
        Get
            Return CType(Me("MapSuffixNumber"),String)
        End Get
        Set
            Me("MapSuffixNumber") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property QuarterSection() As String
        Get
            Return CType(Me("QuarterSection"),String)
        End Get
        Set
            Me("QuarterSection") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property QuarterQuarterSection() As String
        Get
            Return CType(Me("QuarterQuarterSection"),String)
        End Get
        Set
            Me("QuarterQuarterSection") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("--")>  _
    Public Property Anomaly() As String
        Get
            Return CType(Me("Anomaly"),String)
        End Get
        Set
            Me("Anomaly") = value
        End Set
    End Property
End Class
