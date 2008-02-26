#Region "Copyright 2008 ORMAP Tech Group"

' File:  SpatialUtilities.vb
'
' Original Author:  OPET.NET Migration Team (Shad Campbell, James Moore, 
'                   Nick Seigal)
'
' Date Created:  20080221
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

#Region "Subversion Keyword expansion"
'Tag for this file: $Name$
'SCC revision number: $Revision:$
'Date of Last Change: $Date:$
#End Region

#Region "Imported namespace statements"
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
#End Region

#Region "Class Declaration"
Public NotInheritable Class SpatialUtilities
#Region "Custom Class Members"
#Region "Public Members"

    ''' <summary>
    ''' Calculates Taxlot values from ORMAPMapnum
    ''' </summary>
    ''' <param name="feature">A feature from the Taxlot feature class</param>
    ''' <param name="mapIndexLayer">The Map Index feature layer</param>
    ''' <remarks>Update the ORMAP fields in a_pFeat to the reflect the current ORMAP Number and Map Number elements in the overlaying Map Index polygon.</remarks>
    Public Shared Sub CalculateTaxlotValues(ByRef feature As IFeature, ByRef mapIndexLayer As IFeatureLayer)
        'TODO: JWM flesh out
        Try

        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Converts a domain descriptive value to the stored code
    ''' </summary>
    ''' <param name="fields">An object that supports the IFields interface</param>
    ''' <param name="fieldName">A field that exists in fields</param>
    ''' <param name="codedValue">A coded name to convert to a coded value</param>
    ''' <returns>A string that represents the domain coded value that corresponds with the coded name, codedValue, or a empty string.</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertCodeValueDomainToCode(ByVal fields As IFields, ByVal fieldName As String, ByVal codedValue As String) As String
        Try
            Dim fieldIndex As Integer
            fieldIndex = fields.FindField(fieldName)
            If fieldIndex > -1 Then

                Dim field As IField
                field = fields.Field(fieldIndex)

                Dim domain As ICodedValueDomain
                domain = CType(field.Domain, ICodedValueDomain)
                If domain Is Nothing Then
                    Return String.Empty
                Else
                    If TypeOf domain Is ICodedValueDomain Then
                        Dim codedValueDomain As ICodedValueDomain
                        codedValueDomain = domain

                        For domainIndex As Integer = 0 To codedValueDomain.CodeCount - 1
                            If codedValueDomain.Name(domainIndex) = codedValue Then
                                Return CStr(codedValueDomain.Value(domainIndex))
                            End If
                        Next domainIndex
                    Else
                        Return codedValue 'if range domain return the value
                    End If
                End If 'if domain is nothing
                ConvertCodeValueDomainToCode = String.Empty
            End If
            ConvertCodeValueDomainToCode = String.Empty
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            ConvertCodeValueDomainToCode = String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Converts a code index to the  domain descriptive value
    ''' </summary>
    ''' <param name="fields">An object that supports the IFields interface</param>
    ''' <param name="fieldName">A field that exists in fields</param>
    ''' <param name="codedValue">A coded value to covert to a coded name</param>
    ''' <returns>A string that represents the domain coded name that corresponds with the coded value, codedValue, or a zero-length string.</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertCodeValueDomainToDescription(ByVal fields As IFields, ByVal fieldName As String, ByVal codedValue As String) As Object
        Try
            Dim fieldIndex As Integer
            fieldIndex = fields.FindField(fieldName)
            If fieldIndex > -1 Then
                Dim field As IField
                field = fields.Field(fieldIndex)

                Dim domain As ICodedValueDomain
                domain = CType(field.Domain, ICodedValueDomain)
                If domain Is Nothing Then
                    Return String.Empty
                Else
                    If TypeOf domain Is ICodedValueDomain Then
                        Dim codedValueDomain As ICodedValueDomain
                        codedValueDomain = domain
                        For domainIndex As Integer = 0 To codedValueDomain.CodeCount - 1
                            If codedValueDomain.Name(domainIndex) = codedValue Then
                                Return codedValueDomain.Name(domainIndex)
                            End If
                        Next domainIndex
                    Else
                        Return codedValue 'if range domain return the value
                    End If
                End If 'if domain is nothing
                ConvertCodeValueDomainToDescription = String.Empty
            End If
            ConvertCodeValueDomainToDescription = String.Empty
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            ConvertCodeValueDomainToDescription = String.Empty
        End Try
    End Function

    Public Shared Function FindFeatureLayerByDSName(ByVal dataSetName As String) As IFeatureLayer
        Try

        Catch ex As Exception

        End Try
    End Function

    ''' <summary>
    ''' Retrieve a feature-linked annotation feature
    ''' </summary>
    ''' <param name="thisObject">An initialized geodatabase object</param>
    ''' <returns>An object that supports the IFeature interface.</returns>
    ''' <remarks>Finds all related objects to the feature through the first found relationship class, and returns the first related object as the return value.
    ''' This is optimized for annotation because there is a single relationship class.</remarks>
    Public Shared Function GetRelatedObjects(ByVal thisObject As IObject) As IFeature
        Try
            Dim relationshipClassEnum As IEnumRelationshipClass
            Dim parentSet As ISet

            relationshipClassEnum = thisObject.Class.RelationshipClasses(esriRelRole.esriRelRoleAny)
            If Not relationshipClassEnum Is Nothing Then
                Dim thisRelationshipClass As IRelationshipClass
                thisRelationshipClass = relationshipClassEnum.Next
                If Not thisRelationshipClass Is Nothing Then
                    parentSet = thisRelationshipClass.GetObjectsRelatedToObject(thisObject)
                End If
            Else
                Return Nothing
            End If
            ' Returns the first related feature
            If Not parentSet Is Nothing Then
                Dim parentFeature As IFeature
                parentFeature = CType(parentSet.Next, IFeature)
                If Not parentFeature Is Nothing Then
                    Return parentFeature
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Return a cursor for the selected features
    ''' </summary>
    ''' <param name="layer">The feature layer to return the selection from</param>
    ''' <returns>An object that supports the IFeatureCursor interface</returns>
    ''' <remarks>References the currently selected features in layer, and return a cursor with the feature in it.</remarks>
    Public Shared Function GetSelectedFeatures(ByVal layer As IFeatureLayer) As IFeatureCursor
        Try
            If Not TypeOf layer Is IFeatureLayer Then
                Return Nothing
            End If
            Dim thisSelection As IFeatureSelection
            thisSelection = CType(layer, IFeatureSelection)
            thisSelection.SelectionSet.Search(Nothing, False, CType(GetSelectedFeatures, ICursor))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Determines if the feature layer has a selection
    ''' </summary>
    ''' <param name="layer">An object that supports the IFeatureLayer2</param>
    ''' <returns>True or False</returns>
    ''' <remarks>Checking the selection set of layer, determine if one, many, or no features are selected.</remarks>
    Public Shared Function HasSelectedFeatures(ByVal layer As IFeatureLayer2) As Boolean
        Try
            If layer Is Nothing Or Not TypeOf layer Is IFeatureLayer Then
                Return False
            End If
            'how many are selected
            Dim featuresSelected As IFeatureSelection
            featuresSelected = CType(layer, IFeatureSelection)
            Dim thisFeatureCursor As IFeatureCursor
            featuresSelected.SelectionSet.Search(Nothing, False, CType(thisFeatureCursor, ICursor))
            If Not thisFeatureCursor Is Nothing Then
                Dim thisFeature As IFeature
                thisFeature = thisFeatureCursor.NextFeature
                If Not thisFeature Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Determine if the feature belongs to the Taxlot feature class
    ''' </summary>
    ''' <param name="thisObject">A valid initialized geodatabase object</param>
    ''' <returns>True or False</returns>
    ''' <remarks>Determine if thisObject belongs to the Taxlot feature class by checking the name of the dataset of thisObject feature class
    ''' againts the Taxlot Feature Class constant.</remarks>
    Public Shared Function IsTaxlot(ByVal thisObject As IObject) As Boolean
        Try
            Dim thisObjectClass As IObjectClass
            Dim thisDataset As IDataset
            thisObjectClass = thisObject.Class
            thisDataset = CType(thisObjectClass, IDataset)
            If String.Compare(thisDataset.Name, EditorExtension.TableNamesSettings.TaxLotFC, True) = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Determine if the feature belongs to the MapIndex feature class
    ''' </summary>
    ''' <param name="thisObject">A valid initialized geodatabase object</param>
    ''' <returns>True or False</returns>
    ''' <remarks>Compares the name of the dataset of thisObject feature class to the Map Index layer name in order to determine if thisObject
    ''' belongs to the MapIndex feature class.</remarks>
    Public Shared Function IsMapIndex(ByVal thisObject As IObject) As Boolean
        Try
            Dim thisObjectClass As IObjectClass
            Dim thisDataset As IDataset
            thisObjectClass = thisObject.Class
            thisDataset = CType(thisObjectClass, IDataset)
            If String.Compare(thisDataset.Name, EditorExtension.TableNamesSettings.MapIndexFC, True) = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Determine if a feature is annotation
    ''' </summary>
    ''' <param name="thisObject">A valid initialized geodatabase object</param>
    ''' <returns>True or False</returns>
    ''' <remarks>Compares the feature type of thisObjec with that of annotation
    ''' and return the truth value of the comparison</remarks>
    Public Shared Function IsAnno(ByVal thisObject As IObject) As Boolean
        Try
            Dim thisObjectClass As IObjectClass
            thisObjectClass = thisObject.Class

            If TypeOf thisObject Is IFeature Then
                Dim thisFeatureClass As IFeatureClass
                thisFeatureClass = CType(thisObjectClass, IFeatureClass)
                If thisFeatureClass.FeatureType = esriFeatureType.esriFTAnnotation Then
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Reads a value from a row, given a field name.
    ''' </summary>
    ''' <param name="row">An object that implements the IRow interface</param>
    ''' <param name="fieldName">A field that exists in row</param>
    ''' <param name="dataType">A string value indicating data type of the field</param>
    ''' <returns></returns>
    ''' <remarks>Reads the value of a field with a domain and translates the value from the coded value to the coded name.</remarks>
    Public Shared Function ReadValue(ByRef row As IRow, ByVal fieldName As String, Optional ByVal dataType As String = "") As String
        Try
            Dim fieldIndex As Integer
            Dim returnValue As String

            fieldIndex = row.Fields.FindField(fieldName)
            If fieldIndex > -1 Then
                If String.Compare(dataType, "date", True) = 0 Then
                    If IsDBNull(row.Value(fieldIndex)) Then
                        returnValue = CStr(System.DateTime.Today)
                    Else
                        returnValue = CStr(row.Value(fieldIndex))
                    End If
                Else
                    If IsDBNull(row.Value(fieldIndex)) Then
                        returnValue = String.Empty
                    Else
                        returnValue = CStr(row.Value(fieldIndex))
                    End If
                End If
                'Determine if a Domain Field
                Dim field As IField
                field = row.Fields.Field(fieldIndex)
                Dim domain As IDomain
                domain = field.Domain
                If domain Is Nothing Then
                    Return returnValue
                Else
                    If domain.Type = esriDomainType.esriDTCodedValue Then
                        'If TypeOf domain Is ICodedValueDomain Then
                        Dim thisCodedValueDomain As ICodedValueDomain
                        thisCodedValueDomain = CType(domain, ICodedValueDomain)
                        Dim domainValue As Object
                        domainValue = row.Value(fieldIndex)
                        'search domain for the code
                        For domainIndex As Integer = 0 To thisCodedValueDomain.CodeCount - 1
                            If thisCodedValueDomain.Value(domainIndex) = domainValue Then 'TODO: JWM Need to resolve this comparison
                                Return thisCodedValueDomain.Name(domainIndex)
                            End If
                        Next domainIndex
                    Else
                        Return returnValue
                    End If
                End If
                Return returnValue
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Update Auto fields in a feature class.
    ''' </summary>
    ''' <param name="feature"> An object that implements the Ifeature interface</param>
    ''' <remarks>Update the AutoWho and the AutoDate fields with the current username and date/time, respectively.</remarks>
    Public Shared Sub UpdateAutoFields(ByRef feature As IFeature)
        Try
            If feature Is Nothing Then
                Exit Try
            End If
            Dim indexAutoDateField As Integer

            indexAutoDateField = feature.Fields.FindField(EditorExtension.AllTablesSettings.AutoDateField)

            If indexAutoDateField > -1 Then
                feature.Value(indexAutoDateField) = System.DateTime.Now
            End If
            Dim indexAutoWhoField As Integer
            indexAutoWhoField = feature.Fields.FindField(EditorExtension.AllTablesSettings.AutoWhoField)
            If indexAutoWhoField > -1 Then
                feature.Value(indexAutoWhoField) = Utilities.GetUserName
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Determine the validity of a taxlot number
    ''' </summary>
    ''' <param name="taxlotNumber">The taxlot value to validate</param>
    ''' <param name="thisGeometry">The geometry of the feature to check</param>
    ''' <returns>True or False</returns>
    ''' <remarks>Determine if the feature represented by thisGeometry with taxlot taxlotNumber is a unique and therefore valid.</remarks>
    Public Shared Function ValidateTaxlotNumber(ByVal taxlotNumber As String, ByRef thisGeometry As IGeometry) As Boolean
        Try
            'check for existence of Taxlot layer
            Dim thisTaxlotFeatureLayer As IFeatureLayer
            thisTaxlotFeatureLayer = FindFeatureLayerByDSName(EditorExtension.TableNamesSettings.MapIndexFC)
            If thisTaxlotFeatureLayer Is Nothing Then 'TODO: JWM Place strings in resource file
                MessageBox.Show("Unable to locate Taxlot layer in Table of Contents. This process requires a feature class called " & EditorExtension.TableNamesSettings.TaxLotFC)
                Exit Try
            End If

            'check for existence of Map index layer
            Dim thisTaxlotFeatureClass As IFeatureClass
            thisTaxlotFeatureClass = thisTaxlotFeatureLayer.FeatureClass

            Dim mapIndexFeatureLayer As IFeatureLayer
            mapIndexFeatureLayer = FindFeatureLayerByDSName(EditorExtension.TableNamesSettings.MapIndexFC)
            If mapIndexFeatureLayer Is Nothing Then
                MessageBox.Show("Unable to locate MapIndex layer in Table of Contents. This process requires a feature class called " & EditorExtension.TableNamesSettings.MapIndexFC)
                Exit Try
            End If
            Dim mapIndexFeatureClass As IFeatureClass
            mapIndexFeatureClass = mapIndexFeatureLayer.FeatureClass

            ' Checks for the existence of a current ORMAP Number and Taxlot number
            Dim mapIndexORMAPValue As String
            'mapIndexORMAPValue= getvalueViaOverlay()'TODO: JWM Flesh this function out
            If mapIndexORMAPValue.Length = 0 Then
                Return True
                Exit Try
            End If

            'Make sure this number is unique within taxlots with this OM number
            Dim whereClause As String = String.Concat(EditorExtension.TaxLotSettings.MapNumberField, "='", mapIndexORMAPValue, "' AND ", EditorExtension.TaxLotSettings.TaxlotField, " = '", taxlotNumber, "'")
            'TODO: JWM check these EditorExtension values
            Dim cursor As ICursor
            cursor = AttributeQuery(CType(thisTaxlotFeatureClass, ITable), whereClause)
            If Not cursor Is Nothing Then
                Dim row As IRow
                row = cursor.NextRow
                If Not row Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Function


#End Region

#Region "Private Members"

    ''' <summary>
    ''' Return a cursor that represents the results of an attribute query
    ''' </summary>
    ''' <param name="table">An object that supports the ITable interface</param>
    ''' <param name="whereClause">An Sql Where clause</param>
    ''' <returns>Return a cursor that represents the results of an attribute query</returns>
    ''' <remarks>Creates a cursor from table that contains all feature records that meet the criteria in whereClause</remarks>
    Private Shared Function AttributeQuery(ByRef table As ITable, Optional ByRef whereClause As String = "") As ICursor
        Try
            Dim thisQueryFilter As IQueryFilter
            thisQueryFilter = New QueryFilter
            thisQueryFilter.WhereClause = whereClause
            Dim thisCursor As ICursor
            thisCursor = table.Search(thisQueryFilter, False)
            If table.RowCount(thisQueryFilter) = 0 Then
                Return Nothing
            Else
                Return thisCursor
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Return a feature cursor based on the results of a spatial query
    ''' </summary>
    ''' <param name="inFeatureClass">Feature class to search</param>
    ''' <param name="searchGeometry">Geometry to search in relation to spatialRelation</param>
    ''' <param name="spatialRelation">Geometry relationship to searchGeometry</param>
    ''' <param name="whereClause">SQL Where clause</param>
    ''' <param name="isUpdateable">Read/Write state of the return cursor</param>
    ''' <returns>Returns a feature cursor that represents the results of the spatial query</returns>
    ''' <remarks>Given a feature class, pFeatureClassIn, a search geometry, pSearchGeometry, a spatial relationship, lSpatialRelation,
    ''' an Sql search statement, sWhereClause, and whether or not
    ''' the returned cursor should be updateable, bUpdateable.
    ''' Perform a spatial query on pFeatureClassIn where feature
    ''' which meet criteria sWhereClause have a relationship of lSpatialRelation to pSearchGeometry. The returned cursor is updatable if bUpdateable is True.</remarks>
    Private Shared Function DoSpatialQuery(ByRef inFeatureClass As IFeatureClass, ByRef searchGeometry As IGeometry, ByRef spatialRelation As ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum, Optional ByRef whereClause As String = "", Optional ByVal isUpdateable As Boolean = False) As IFeatureCursor
        Try
            Dim thisSpatialFilter As ISpatialFilter
            thisSpatialFilter = New SpatialFilter
            thisSpatialFilter.Geometry = searchGeometry

            Dim shapeFieldName As String
            shapeFieldName = inFeatureClass.ShapeFieldName
            thisSpatialFilter.GeometryField = shapeFieldName

            thisSpatialFilter.SpatialRel = spatialRelation
            thisSpatialFilter.WhereClause = whereClause

            Dim thisQueryFilter As IQueryFilter
            thisQueryFilter = thisSpatialFilter
            Dim thisFeatureCursor As IFeatureCursor
            If isUpdateable Then
                thisFeatureCursor = inFeatureClass.Update(thisQueryFilter, False)
            Else
                thisFeatureCursor = inFeatureClass.Search(thisQueryFilter, False)
            End If
            DoSpatialQuery = thisFeatureCursor

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            DoSpatialQuery = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Copy envelope points to polygon
    ''' </summary>
    ''' <param name="envelope"></param>
    ''' <returns>A Polygon</returns>
    ''' <remarks></remarks>
    Private Function EnvelopeToPolygon(ByRef envelope As IEnvelope) As IPolygon
        Dim thisPolygon As IPolygon
        thisPolygon = New Polygon
        Dim pointCollection As IPointCollection
        pointCollection = CType(thisPolygon, IPointCollection)
        With pointCollection
            .AddPoint(envelope.UpperRight)
            .AddPoint(envelope.LowerRight)
            .AddPoint(envelope.LowerLeft)
            .AddPoint(envelope.UpperLeft)
        End With
        Return thisPolygon
    End Function

    Private Shared Function GetAnnoSizeByScale(ByVal thisFeatureClassName As String, ByVal scale As Integer) As Double
        Try
            Dim size As String

            With EditorExtension.TaxlotNumberAnnoSettings
                if string.Compare(thisFeatureClassName,EditorExtension.TaxlotNumberAnnoSettings.
                    Select Case scale
                        Case 120
                            size = .TextSize00120Scale
                        Case 240
                            size = .TextSize00240Scale
                        Case 360
                            size = .TextSize00360Scale
                        Case 480
                            size = .TextSize00480Scale
                        Case 600
                            size = .TextSize00600Scale
                        Case 1200
                            size = .TextSize01200Scale
                        Case 2400
                            size = .TextSize02400Scale
                        Case 4800
                            size = .TextSize04800Scale
                        Case 9600
                            size = .TextSize09600Scale
                        Case 24000
                            size = .TextSize24000Scale
                        Case Else
                            ' Default size
                            size = "5"
                    End Select
            End With
        Catch ex As Exception

        End Try
    End Function
    ''' <summary>
    ''' Determine the x and y coordinates of the center of envelope, and return them as a Point object
    ''' </summary>
    ''' <param name="envelope">An envelope object of type IEnvelope</param>
    ''' <returns>A Point object that represents the center of the envelope</returns>
    ''' <remarks></remarks>
    Private Shared Function GetCenterOfEnvelope(ByRef envelope As IEnvelope) As IPoint
        Try
            Dim center As IPoint
            center = New Point
            center.X = envelope.XMin + (envelope.XMax - envelope.XMin) / 2
            center.Y = envelope.YMin + (envelope.YMax - envelope.YMin) / 2
            GetCenterOfEnvelope = center
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message)
            GetCenterOfEnvelope = Nothing
        End Try
    End Function

#End Region
#End Region
End Class
#End Region