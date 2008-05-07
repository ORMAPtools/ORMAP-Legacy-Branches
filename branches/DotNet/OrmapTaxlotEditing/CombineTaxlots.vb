#Region "Copyright 2008 ORMAP Tech Group"

' File:  CombineTaxlots.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  January 8, 2008
'
' Copyright Holder:  ORMAP Tech Group  
' Contact Info:  ORMAP Tech Group (a.k.a. opet developers) may be reached at 
' opet-developers@lists.sourceforge.net
'
' This file is part of the ORMAP Taxlot Editing Toolbar.
'
' ORMAP Taxlot Editing Toolbar is free software; you can redistribute it and/or
' modify it under the terms of the GNU General Public License as published by 
' the Free Software Foundation; either version 3 of the License, or (at your 
' option) any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT 
' ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
' FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License located
' in the COPYING.txt file for more details.
'
' You should have received a copy of the GNU General Public License along
' with the ORMAP Taxlot Editing Toolbar; if not, write to the Free Software 
' Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

#End Region

#Region "Subversion Keyword Expansion"
'Tag for this file: $Name$
'SCC revision number: $Revision$
'Date of Last Change: $Date$
#End Region

#Region "Imported Namespaces"
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ADF.BaseClasses
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports OrmapTaxlotEditing.DataMonitor
Imports OrmapTaxlotEditing.SpatialUtilities
Imports OrmapTaxlotEditing.StringUtilities
Imports OrmapTaxlotEditing.Utilities
#End Region

<ComVisible(True)> _
<ComClass(CombineTaxlots.ClassId, CombineTaxlots.InterfaceId, CombineTaxlots.EventsId), _
ProgId("ORMAPTaxlotEditing.CombineTaxlots")> _
Public NotInheritable Class CombineTaxlots
    Inherits BaseCommand
    Implements IDisposable

#Region "Class-Level Constants And Enumerations (none)"
#End Region

#Region "Built-In Class Members (Constructors, Etc.)"

#Region "Constructors"

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()

        ' Define protected instance field values for the public properties
        MyBase.m_category = "OrmapToolbar"  'localizable text 
        MyBase.m_caption = "CombineTaxlots"   'localizable text 
        MyBase.m_message = "Combine Selected Taxlots"   'localizable text 
        MyBase.m_toolTip = "Combine Selected Taxlots" 'localizable text 
        MyBase.m_name = MyBase.m_category & "_CombineTaxlots"  'unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

        Try
            ' Set the bitmap based on the name of the class.
            _bitmapResourceName = Me.GetType().Name + ".bmp"
            MyBase.m_bitmap = New Bitmap(Me.GetType(), _bitmapResourceName)
        Catch ex As ArgumentException
            Trace.WriteLine(ex.Message, "Invalid Bitmap")
        End Try

    End Sub

#End Region

#End Region

#Region "Custom Class Members"

#Region "Fields"

    Private _application As IApplication
    Private _bitmapResourceName As String

#End Region

#Region "Properties"

    Private WithEvents _partnerCombineTaxlotsForm As CombineTaxlotsForm

    Friend ReadOnly Property PartnerCombineTaxlotsForm() As CombineTaxlotsForm
        Get
            If _partnerCombineTaxlotsForm Is Nothing OrElse _partnerCombineTaxlotsForm.IsDisposed Then
                setPartnerCombineTaxlotsForm(New CombineTaxlotsForm())
            End If
            Return _partnerCombineTaxlotsForm
        End Get
    End Property

    Private Sub setPartnerCombineTaxlotsForm(ByVal value As CombineTaxlotsForm)
        If value IsNot Nothing Then
            _partnerCombineTaxlotsForm = value
            ' Subscribe to partner form events.
            AddHandler _partnerCombineTaxlotsForm.Load, AddressOf PartnerTaxlotAssignmentForm_Load
            AddHandler _partnerCombineTaxlotsForm.uxCombine.Click, AddressOf uxCombine_Click
            AddHandler _partnerCombineTaxlotsForm.uxHelp.Click, AddressOf uxHelp_Click
        Else
            ' Unsubscribe to partner form events.
            RemoveHandler _partnerCombineTaxlotsForm.Load, AddressOf PartnerTaxlotAssignmentForm_Load
            RemoveHandler _partnerCombineTaxlotsForm.uxCombine.Click, AddressOf uxCombine_Click
            RemoveHandler _partnerCombineTaxlotsForm.uxHelp.Click, AddressOf uxHelp_Click
        End If
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub PartnerTaxlotAssignmentForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles PartnerTaxlotAssignmentForm.Load

        If HasSelectedFeatures(TaxlotFeatureLayer) Then

            With PartnerCombineTaxlotsForm

                If .uxTaxlotNumber.Items.Count = 0 Then
                    ' Get a list of distinct taxlot numbers for the set of taxlots to be combined.
                    Dim theCurrentTaxlotSelection As IFeatureSelection = DirectCast(TaxlotFeatureLayer, IFeatureSelection)

                    ' TODO: [NIS] Escape with error message in the case where selected features span tax maps...

                    Dim theQueryFilter As IQueryFilter = New QueryFilter
                    theQueryFilter.SubFields = EditorExtension.TaxLotSettings.TaxlotField
                    ' TODO: [ALL] Fix WhereClause issues (see http://edndoc.esri.com/arcobjects/9.2/ComponentHelp/esriGeoDatabase//IQueryFilter_WhereClause.htm).
                    theQueryFilter.WhereClause = "DISTINCT(" & EditorExtension.TaxLotSettings.TaxlotField & ")"

                    Dim theCursor As ICursor = Nothing
                    theCurrentTaxlotSelection.SelectionSet.Search(theQueryFilter, True, theCursor)
                    Dim theQueryField As Integer = theCursor.FindField(EditorExtension.TaxLotSettings.TaxlotField)
                    Dim theRow As IRow = theCursor.NextRow
                    Do Until theRow Is Nothing
                        .uxTaxlotNumber.Items.Add(theRow.Value(theQueryField))
                        theRow = theCursor.NextRow
                    Loop
                End If

            End With

        End If

    End Sub

    Private Sub uxCombine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerTaxlotAssignmentForm.uxCombine.Click

        ' Get the user-entered or selected taxlot.
        Dim theTaxlotNumber As String = Nothing
        Dim uxTaxlotNumber As ComboBox = PartnerCombineTaxlotsForm.uxTaxlotNumber
        If uxTaxlotNumber.FindStringExact(uxTaxlotNumber.Text) = -1 Then
            MessageBox.Show("Invalid Taxlot number. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        Else
            theTaxlotNumber = uxTaxlotNumber.Text.Trim
        End If

        ' TODO: [JWM] See http://sourceforge.net/tracker/index.php?func=detail&aid=1951096&group_id=151824&atid=782251
        ' TODO: [NIS] The user will be warned if the taxlot number selected is not unique in
        ' the current map, but the combine will still be allowed.

        ' Verify that within this map index, this taxlot number is unique.
        ' If not unique, give user option to quit.

        ' Get the selected taxlot from the set of taxlots to be combined.
        Dim theFeature As IFeature = getSelectedTaxlotFromSet(theTaxlotNumber)
        Dim theArea As IArea
        theArea = DirectCast(theFeature.Shape, IArea)

        If Not IsTaxlotNumberLocallyUnique(theTaxlotNumber, theArea.Centroid) Then
            If MessageBox.Show("The current Taxlot value (" & theTaxlotNumber & ") is not unique within this MapIndex. " & _
                    "Continue the combine process anyway?", _
                    "Combine Taxlots", MessageBoxButtons.OKCancel, _
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        combine(theTaxlotNumber)
        ' Note: Only the removed taxlots will send their numbers to
        ' the CancelledNumbers table and only if they are unique 
        ' in the map at the time of deletion.

        PartnerCombineTaxlotsForm.Close()

    End Sub

    Private Sub uxHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles PartnerTaxlotAssignmentForm.uxHelp.Click
        ' TODO [NIS] Evaluate help systems and implement.
        MessageBox.Show("Sorry. Help not implemented at this time.")
    End Sub

#End Region

#Region "Methods"

    Friend Sub DoButtonOperation()

        Try
            PartnerCombineTaxlotsForm.uxTaxlotNumber.Enabled = False
            PartnerCombineTaxlotsForm.uxCombine.Enabled = False

            ' Check for valid data.

            CheckValidTaxlotDataProperties()
            If Not HasValidTaxlotData Then
                MessageBox.Show("Missing data: Valid ORMAP Taxlot layer not found in the map." & vbNewLine & _
                                "Please load this dataset into your map.", _
                                "Combine Taxlots", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            Else
                If HasSelectedFeatures(TaxlotFeatureLayer) Then
                    ' Allow the user to select the new combined taxlot number from the list.
                    PartnerCombineTaxlotsForm.uxTaxlotNumber.Enabled = True
                    PartnerCombineTaxlotsForm.uxCombine.Enabled = True
                Else
                    PartnerCombineTaxlotsForm.uxTaxlotNumber.Enabled = False
                    PartnerCombineTaxlotsForm.uxCombine.Enabled = False
                    Exit Sub
                End If
            End If

            PartnerCombineTaxlotsForm.ShowDialog()

        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Shared Function getSelectedTaxlotFromSet(ByVal theTaxlotNumber As String) As IFeature

        ' Get the selected taxlot from the set of taxlots to be combined.
        Dim theCurrentTaxlotSelection As IFeatureSelection = DirectCast(TaxlotFeatureLayer, IFeatureSelection)

        Dim theQueryFilter As IQueryFilter = New QueryFilter
        theQueryFilter.SubFields = EditorExtension.TaxLotSettings.TaxlotField
        ' TODO: [ALL] Fix WhereClause issues (see http://edndoc.esri.com/arcobjects/9.2/ComponentHelp/esriGeoDatabase//IQueryFilter_WhereClause.htm).
        theQueryFilter.WhereClause = EditorExtension.TaxLotSettings.TaxlotField & " = " & theTaxlotNumber

        Dim theCursor As ICursor = Nothing
        theCurrentTaxlotSelection.SelectionSet.Search(theQueryFilter, True, theCursor)
        Dim theQueryField As Integer = theCursor.FindField(EditorExtension.TaxLotSettings.TaxlotField)
        Dim theRow As IRow = theCursor.NextRow
        Do Until theRow Is Nothing
            theRow = theCursor.NextRow
        Loop
        Dim theFeature As IFeature = DirectCast(theRow, IFeature)

        Return theFeature

    End Function

    Private Shared Sub addByDataType(ByVal theNewFeature As IFeature, ByVal thisSelectedFeature As IFeature, ByVal thisFieldIndex As Integer)

        Select Case theNewFeature.Fields.Field(thisFieldIndex).Type

            Case esriFieldType.esriFieldTypeDouble
                theNewFeature.Value(thisFieldIndex) = CDbl(theNewFeature.Value(thisFieldIndex)) + CDbl(thisSelectedFeature.Value(thisFieldIndex))

            Case esriFieldType.esriFieldTypeInteger
                theNewFeature.Value(thisFieldIndex) = CInt(theNewFeature.Value(thisFieldIndex)) + CInt(thisSelectedFeature.Value(thisFieldIndex))

            Case esriFieldType.esriFieldTypeSmallInteger
                theNewFeature.Value(thisFieldIndex) = CShort(theNewFeature.Value(thisFieldIndex)) + CShort(thisSelectedFeature.Value(thisFieldIndex))

            Case esriFieldType.esriFieldTypeSingle
                theNewFeature.Value(thisFieldIndex) = CSng(theNewFeature.Value(thisFieldIndex)) + CSng(thisSelectedFeature.Value(thisFieldIndex))

        End Select

    End Sub

    Private Shared Sub areaWeightedAddByDataType(ByVal theNewFeature As IFeature, ByVal thisSelectedFeature As IFeature, ByVal thisArea As IArea, ByVal thisCurve As ICurve, ByVal theAreaSum As Double, ByVal theLengthSum As Double, ByVal thisFieldIndex As Integer)

        Dim theWeight As Double
        Select Case thisSelectedFeature.Shape.GeometryType

            Case esriGeometryType.esriGeometryPolygon
                theWeight = thisArea.Area / theAreaSum

            Case esriGeometryType.esriGeometryPolyline, esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryBezier3Curve, esriGeometryType.esriGeometryCircularArc, esriGeometryType.esriGeometryEllipticArc, esriGeometryType.esriGeometryPath
                theWeight = thisCurve.Length / theLengthSum

        End Select

        Select Case theNewFeature.Fields.Field(thisFieldIndex).Type

            Case esriFieldType.esriFieldTypeDouble
                theNewFeature.Value(thisFieldIndex) = CDbl(theNewFeature.Value(thisFieldIndex)) + CDbl(thisSelectedFeature.Value(thisFieldIndex)) * theWeight

            Case esriFieldType.esriFieldTypeInteger
                theNewFeature.Value(thisFieldIndex) = CInt(theNewFeature.Value(thisFieldIndex)) + CInt(thisSelectedFeature.Value(thisFieldIndex)) * theWeight

            Case esriFieldType.esriFieldTypeSmallInteger
                theNewFeature.Value(thisFieldIndex) = CShort(theNewFeature.Value(thisFieldIndex)) + CShort(thisSelectedFeature.Value(thisFieldIndex)) * theWeight

            Case esriFieldType.esriFieldTypeSingle
                theNewFeature.Value(thisFieldIndex) = CSng(theNewFeature.Value(thisFieldIndex)) + CSng(thisSelectedFeature.Value(thisFieldIndex)) * theWeight

        End Select

    End Sub

    Private Sub combine(ByVal theTaxlotNumber As String)

        Try

            ' NOTE: Taxlots are already selected and the new combined taxlot number is known at this point.

            Dim theTaxlotFClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
            Dim theTaxlotFieldIndex As Integer
            Dim theTaxlotDataset As ESRI.ArcGIS.Geodatabase.IDataset

            theTaxlotFClass = TaxlotFeatureLayer.FeatureClass
            theTaxlotFieldIndex = LocateFields(TaxlotFeatureLayer.FeatureClass, EditorExtension.TaxLotSettings.TaxlotField)
            theTaxlotDataset = DirectCast(theTaxlotFClass, IDataset)

            ' Find the Reference Lines feature class to insert any deleted lines:

            Dim theReferenceLinesFeatureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
            Dim theReferenceLinesFClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
            theReferenceLinesFeatureWorkspace = DirectCast(theTaxlotDataset.Workspace, IFeatureWorkspace)
            theReferenceLinesFClass = theReferenceLinesFeatureWorkspace.OpenFeatureClass(EditorExtension.TableNamesSettings.ReferenceLinesFC)
            If theReferenceLinesFClass Is Nothing Then
                'If feature class not present, don't move lines
                MessageBox.Show("Unable to locate Reference Lines feature class.", "Combine Taxlots", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Try
            End If

            ' Combine taxlots:

            Dim theWorkspaceEdit As ESRI.ArcGIS.Geodatabase.IWorkspaceEdit
            theWorkspaceEdit = DirectCast(theTaxlotDataset.Workspace, IWorkspaceEdit)

            If theWorkspaceEdit.IsBeingEdited Then

                Dim theSelectedFeaturesCursor As IFeatureCursor
                theSelectedFeaturesCursor = GetSelectedFeatures(TaxlotFeatureLayer) 'Make sure more than one selected

                If Not theSelectedFeaturesCursor Is Nothing Then

                    '----------------------------------------
                    ' Merge the features, evaluate the merge rules 
                    ' and assign values to fields appropriately.
                    '----------------------------------------
                    Dim theKeepFeature As IFeature = getSelectedTaxlotFromSet(theTaxlotNumber)

                    ' Start edit operation
                    EditorExtension.Editor.StartOperation()

                    ' Create a new feature to be the merge feature
                    Dim theNewFeature As IFeature
                    'NOT USED ANY MORE - theNewFeature = theTaxlotFClass.CreateFeature
                    theNewFeature = theKeepFeature

                    ' Extract the default subtype from the feature's class.
                    ' Initialize the default values for the new feature.
                    Dim theSubtypes As ISubtypes
                    Dim theDefaultSubtypeCode As Integer
                    theSubtypes = DirectCast(theNewFeature.Class, ISubtypes)
                    theDefaultSubtypeCode = theSubtypes.DefaultSubtypeCode

                    ' Merge policy
                    Dim theRowSubtypes As IRowSubtypes
                    theRowSubtypes = CType(theNewFeature, IRowSubtypes)
                    theRowSubtypes.InitDefaultValues()
                    Dim theSubtypeCode As Integer = theRowSubtypes.SubtypeCode

                    ' Get the first feature
                    Dim theFields As IFields
                    Dim thisSelectedFeature As IFeature
                    thisSelectedFeature = theSelectedFeaturesCursor.NextFeature
                    theFields = theTaxlotFClass.Fields

                    Dim theFeatureCount As Integer
                    Dim thisPolygon As IPolygon
                    Dim thisArea As IArea = Nothing
                    Dim thisCurve As ICurve = Nothing
                    Dim theAreaSum As Double = 0
                    Dim theLengthSum As Double = 0

                    ' TODO: [NIS] Create these procedures and call instead of first Do...Loop below.
                    'If NeedsAreaForDomain(thisSelectedFeature) Then
                    '    theAreaSum = GetAreaSum(theSelectedFeaturesCursor)
                    'End If
                    'If NeedsLengthForDomain(thisSelectedFeature) Then
                    '    theLengthSum = GetLengthSum(theSelectedFeaturesCursor)
                    'End If

                    '- - - - - - - - - - - - - - - - - - - -
                    ' Add up areas or lengths for the selected features,
                    ' depending on feature geometry type:
                    '- - - - - - - - - - - - - - - - - - - -
                    theFeatureCount = 1
                    Do
                        Select Case thisSelectedFeature.Shape.GeometryType
                            Case esriGeometryType.esriGeometryPolygon
                                thisPolygon = DirectCast(thisSelectedFeature.Shape, IPolygon)
                                thisArea = DirectCast(thisPolygon, IArea)
                                theAreaSum += thisArea.Area

                            Case esriGeometryType.esriGeometryPolyline, esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryBezier3Curve, esriGeometryType.esriGeometryCircularArc, esriGeometryType.esriGeometryEllipticArc, esriGeometryType.esriGeometryPath
                                thisCurve = DirectCast(thisSelectedFeature.Shape, ICurve)
                                theLengthSum += thisCurve.Length

                            Case Else
                                Throw New Exception("Invalid geometry type: " & thisSelectedFeature.Shape.GeometryType.ToString)

                        End Select

                        thisSelectedFeature = theSelectedFeaturesCursor.NextFeature
                        theFeatureCount += 1
                    Loop Until thisSelectedFeature Is Nothing

                    '- - - - - - - - - - - - - - - - - - - -
                    ' Build the combined feature and set its attributes
                    ' (based on merge rules, if present):
                    '- - - - - - - - - - - - - - - - - - - -
                    Dim theOutputGeometry As IGeometry = Nothing

                    theFeatureCount = 1
                    Do
                        ' Get the selected feature's geometry
                        Dim theGeometry As ESRI.ArcGIS.Geometry.IGeometry
                        theGeometry = thisSelectedFeature.ShapeCopy
                        If theFeatureCount = 1 Then
                            '[The first feature...]
                            theOutputGeometry = theGeometry
                        Else
                            '[Not the first feature...]
                            ' Merge the geometry of the features
                            Dim theTopoOperator As ESRI.ArcGIS.Geometry.ITopologicalOperator
                            theTopoOperator = DirectCast(theOutputGeometry, ESRI.ArcGIS.Geometry.ITopologicalOperator)
                            theOutputGeometry = theTopoOperator.Union(theGeometry)
                        End If

                        Select Case thisSelectedFeature.Shape.GeometryType
                            Case esriGeometryType.esriGeometryPolygon
                                thisPolygon = DirectCast(thisSelectedFeature.Shape, IPolygon)
                                thisArea = DirectCast(thisPolygon, IArea)

                            Case esriGeometryType.esriGeometryPolyline, esriGeometryType.esriGeometryLine, esriGeometryType.esriGeometryBezier3Curve, esriGeometryType.esriGeometryCircularArc, esriGeometryType.esriGeometryEllipticArc, esriGeometryType.esriGeometryPath
                                thisCurve = DirectCast(thisSelectedFeature.Shape, ICurve)

                            Case Else
                                ' Will have been caught already above

                        End Select

                        ' Go through each field. 
                        ' If it has a domain associated with it, then evaluate the merge policy:

                        For thisFieldIndex As Integer = 0 To (theFields.FieldCount - 1)

                            Dim theField As IField
                            Dim theDomain As IDomain
                            theField = theFields.Field(thisFieldIndex)
                            theDomain = theSubtypes.Domain(theSubtypeCode, theField.Name)

                            If Not theDomain Is Nothing Then
                                Select Case theDomain.MergePolicy

                                    Case esriMergePolicyType.esriMPTSumValues 'Sum values
                                        If theFeatureCount = 1 Then
                                            theNewFeature.Value(thisFieldIndex) = 0 'initialize the first one
                                        End If
                                        addByDataType(theNewFeature, thisSelectedFeature, thisFieldIndex)

                                    Case esriMergePolicyType.esriMPTAreaWeighted 'Area/length weighted average
                                        If theFeatureCount = 1 Then
                                            theNewFeature.Value(thisFieldIndex) = 0 'initialize the first one
                                        End If
                                        areaWeightedAddByDataType(theNewFeature, thisSelectedFeature, thisArea, thisCurve, theAreaSum, theLengthSum, thisFieldIndex)

                                    Case Else 'If no merge policy, just take one of the existing values
                                        theNewFeature.Value(thisFieldIndex) = thisSelectedFeature.Value(thisFieldIndex)

                                End Select 'do not need a case for default value as it is set above
                            Else 'If not a domain, copy the existing value
                                If theNewFeature.Fields.Field(thisFieldIndex).Editable Then 'Don't attempt to copy objectid or other non-editable field
                                    theNewFeature.Value(thisFieldIndex) = thisSelectedFeature.Value(thisFieldIndex)
                                End If
                            End If
                        Next thisFieldIndex

                        If CStr(thisSelectedFeature.Value(theTaxlotFieldIndex)) = theTaxlotNumber Then
                            'Cannot delete the feature that is being kept so skip it
                        Else
                            thisSelectedFeature.Delete()
                        End If

                        thisSelectedFeature = theSelectedFeaturesCursor.NextFeature
                        theFeatureCount += 1

                    Loop Until thisSelectedFeature Is Nothing

                    ' Set the new feature geametry to the combined geometry.
                    theNewFeature.Shape = theOutputGeometry

                    ' Set the new combined taxlot number.
                    theNewFeature.Value(theTaxlotFieldIndex) = theTaxlotNumber

                    ' Store the feature edits.
                    theNewFeature.Store()

                    ' Select the new feature:

                    Dim theMxDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument
                    Dim theMap As ESRI.ArcGIS.Carto.IMap
                    theMxDoc = DirectCast(EditorExtension.Application.Document, IMxDocument)
                    theMap = theMxDoc.FocusMap
                    theMap.ClearSelection()
                    theMap.SelectFeature(TaxlotFeatureLayer, theNewFeature)

                    ' Refresh the display area of the new feature:

                    Dim theInvalidArea As IInvalidArea
                    theInvalidArea = New InvalidArea
                    theInvalidArea.Display = EditorExtension.Editor.Display
                    theInvalidArea.Add(theNewFeature)
                    theInvalidArea.Invalidate(CShort(esriScreenCache.esriAllScreenCaches))

                    '- - - - - - - - - - - - - - - - - - - -
                    ' Move deleted taxlot lines to Reference Lines,
                    ' line type 33 (historical):
                    '- - - - - - - - - - - - - - - - - - - -
                    Dim theTaxlotLinesLayer As ESRI.ArcGIS.Carto.IFeatureLayer
                    theTaxlotLinesLayer = FindFeatureLayerByDSName(EditorExtension.TableNamesSettings.TaxLotLinesFC)
                    If Not theTaxlotLinesLayer Is Nothing Then
                        Dim theTaxlotLinesFClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
                        theTaxlotLinesFClass = theTaxlotLinesLayer.FeatureClass

                        Dim theLineTypeFieldIndex As Integer

                        theLineTypeFieldIndex = LocateFields(theReferenceLinesFClass, EditorExtension.TaxLotLinesSettings.LineTypeField)

                        Dim theCombinedGeom As ESRI.ArcGIS.Geometry.IGeometry
                        theCombinedGeom = theNewFeature.Shape

                        Dim theTaxlotLinesFCursor As IFeatureCursor
                        theTaxlotLinesFCursor = DoSpatialQuery(theTaxlotLinesFClass, theCombinedGeom, esriSpatialRelEnum.esriSpatialRelContains, "", True)
                        If Not theTaxlotLinesFCursor Is Nothing Then
                            Dim thisLineFeature As IFeature
                            thisLineFeature = theTaxlotLinesFCursor.NextFeature
                            Do While Not thisLineFeature Is Nothing
                                Dim thisNewLineFeature As ESRI.ArcGIS.Geodatabase.IFeature
                                thisNewLineFeature = theReferenceLinesFClass.CreateFeature
                                thisNewLineFeature.Shape = thisLineFeature.ShapeCopy
                                thisNewLineFeature.Value(theLineTypeFieldIndex) = 33
                                thisNewLineFeature.Store()
                                theTaxlotLinesFCursor.DeleteFeature()
                                thisLineFeature = theTaxlotLinesFCursor.NextFeature
                            Loop
                        End If
                    End If

                    ' Finish edit operation
                    EditorExtension.Editor.StopOperation(("Taxlots Combined"))
                End If
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)

        End Try

    End Sub

#End Region

#End Region

#Region "Inherited Class Members"

#Region "Properties"

    ''' <summary>
    ''' Called by ArcMap once per second to check if the command is enabled.
    ''' </summary>
    ''' <remarks>WARNING: Do not put computation-intensive code here.</remarks>
    Public Overrides ReadOnly Property Enabled() As Boolean
        Get
            Dim canEnable As Boolean
            canEnable = EditorExtension.CanEnableExtendedEditing
            Return canEnable
        End Get
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Called by ArcMap when this command is created.
    ''' </summary>
    ''' <param name="hook">A generic <c>Object</c> hook to an instance of the application.</param>
    ''' <remarks>The application hook may not point to an <c>IMxApplication</c> object.</remarks>
    Public Overrides Sub OnCreate(ByVal hook As Object)
        If Not hook Is Nothing Then

            'Disable tool if parent application is not ArcMap
            If TypeOf hook Is IMxApplication Then
                _application = DirectCast(hook, IApplication)
                setPartnerCombineTaxlotsForm(New CombineTaxlotsForm())
                MyBase.m_enabled = True
            Else
                MyBase.m_enabled = False
            End If
        End If

        ' NOTE: Add other initialization code here...

    End Sub

    Public Overrides Sub OnClick()

        DoButtonOperation()

    End Sub

#End Region

#End Region

#Region "Implemented Interface Members"

#Region "IDisposable Interface Implementation"

    Private _isDuringDispose As Boolean ' Used to track whether Dispose() has been called and is in progress.

    ''' <summary>
    ''' Dispose of managed and unmanaged resources.
    ''' </summary>
    ''' <param name="disposing">True or False.</param>
    ''' <remarks>
    ''' <para>Member of System::IDisposable.</para>
    ''' <para>Dispose executes in two distinct scenarios. 
    ''' If disposing equals true, the method has been called directly
    ''' or indirectly by a user's code. Managed and unmanaged resources
    ''' can be disposed.</para>
    ''' <para>If disposing equals false, the method has been called by the 
    ''' runtime from inside the finalizer and you should not reference 
    ''' other objects. Only unmanaged resources can be disposed.</para>
    ''' </remarks>
    Friend Sub Dispose(ByVal disposing As Boolean)
        ' Check to see if Dispose has already been called.
        If Not Me._isDuringDispose Then

            ' Flag that disposing is in progress.
            Me._isDuringDispose = True

            If disposing Then
                ' Free managed resources when explicitly called.

                ' Dispose managed resources here.
                '   e.g. component.Dispose()

            End If

            ' Free "native" (shared unmanaged) resources, whether 
            ' explicitly called or called by the runtime.

            ' Call the appropriate methods to clean up 
            ' unmanaged resources here.
            _bitmapResourceName = Nothing
            MyBase.m_bitmap = Nothing

            ' Flag that disposing has been finished.
            _isDuringDispose = False

        End If

    End Sub

#Region " IDisposable Support "

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#End Region

#End Region

#Region "Other Members"

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "cd9a4052-836a-42a1-bd09-b3bee8dee1a9"
    Public Const InterfaceId As String = "3429b024-92e6-4e59-b2fb-720efce1a98c"
    Public Const EventsId As String = "c612e825-3371-407d-8fb9-f478981a22dc"
#End Region

#Region "COM Registration Function(s)"
    <ComRegisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub RegisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryRegistration(registerType)

        'Add any COM registration code after the ArcGISCategoryRegistration() call

    End Sub

    <ComUnregisterFunction(), ComVisibleAttribute(False)> _
    Public Shared Sub UnregisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryUnregistration(registerType)

        'Add any COM unregistration code after the ArcGISCategoryUnregistration() call

    End Sub

#Region "ArcGIS Component Category Registrar generated code"
    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Register(regKey)

    End Sub

    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        MxCommands.Unregister(regKey)

    End Sub

#End Region
#End Region

#End Region

End Class



