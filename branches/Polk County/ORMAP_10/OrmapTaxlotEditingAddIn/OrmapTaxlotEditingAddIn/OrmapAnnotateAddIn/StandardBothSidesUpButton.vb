﻿''' <summary>
''' ESRI AddIn button events
''' </summary>
Public Class StandardBothSidesUpButton
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

    ''' <summary>
    ''' ESRI AddIn button OnClick event handler
    ''' </summary>
    Protected Overrides Sub OnClick()
        'Dim _standardBothSidesUp As StandardBothSidesUp = New StandardBothSidesUp
        StandardBothSidesUp.DoButtonOperation()
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class