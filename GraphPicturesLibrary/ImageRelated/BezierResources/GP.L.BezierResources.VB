Imports GraphPictures.Library.CompilerExtentions

Namespace Library

	Public Module BezierResources

		REM <!DOCTYPE> Declerations in SVGFiles mean that [the .NET XmlDocument Object] gets stuck in an infinite loop whilst loading the File, so this RegExp is used for the removal thereof:
		Public Const DOCTYPERemovalRegExp$ = "\<\!DOCTYPE.*\r?\n?.*\>"

		''' <summary>
		''' Creates Quadratic Beziers representing the lines and curves in the SVG File
		''' </summary>
		''' <param name="_SVGFilePath"></param>
		''' <returns></returns>
		''' <remarks>Supporting ONLY Potrace-outputted SVGs for now. (Only the path Elements are delt with, and only the M, L, and C commands are handled.)</remarks>
		Public Function GetBeziersFromPotraceSVG(ByVal _SVGFilePath$) As IDesmosExpression()

			Try

				'Check if the Specified File actually exists...
				If Not System.IO.File.Exists(_SVGFilePath) Then Throw (New System.Exception("An attempt was made to Resolve the Beziers from an SVG File, however the File does not exist. The path was: " & _SVGFilePath))

				'This is a List(Of IDesmosExpression), because this allows us to add to the list, anything which implamants the IDesmosExpression Interface. For instance, QuadraticBezier, CubicBezier, and NOrderBezier all implement this interface.
				Dim _BeziersToReturn As New List(Of IDesmosExpression)()
				Dim _SVGFile As New System.Xml.XmlDocument()
				Dim _SVGFileText$ = System.Text.RegularExpressions.Regex.Replace(My.Computer.FileSystem.ReadAllText(_SVGFilePath), BezierResources.DOCTYPERemovalRegExp$, String.Empty)

				'This MsgDebug() Sub is defined in GP.L.Resources.VB. Turn on Debugging mode, via the #Const Switch at the top of that file.
				MsgDebug("Loading this text into _SVGFile: " & vbCrLf & vbCrLf & _SVGFileText$)

				'Now that the _SVGFileText$ String is clensed of the <!DOCTYPE>, it can be loaded into the .NET XMLDOCUMENT Object.
				_SVGFile.LoadXml(_SVGFileText$)

				'Sometimes, Potrace.exe will actually decide (in it's infinite wisdom), that there is no contents to a RasterGraphic, and produce an Empty, but still syntactically-valid, SVG. We need to check for this.
				If _SVGFile.GetElementsByTagName("path").Count = 0 Then
					MsgDebug("The SVGFile """ & _SVGFilePath & """ contained no Beziers")
					'Just return no Expressions for that frame, because it was probably just a blank frame in the video, or a blank RasterGraphic
					Return (New List(Of IDesmosExpression)()).ToArray()
				End If

				REM For Each <*> in [the first <g> in <svg>]
				For Each _Path As Xml.XmlNode In _SVGFile.GetElementsByTagName("g")(0)

					REM Check if the current <*> is actually a <path>
					If _Path.LocalName = "path" Then

						REM Keep Track of the last point used, and update it whenever a new point is used.
						Dim _LastUsedPoint As New Drawing.Point(0I, 0I)

						REM Keep track of the Point at the start of a <path>, and update it at the start of a new SubPath
						Dim _PathStartPoint As Drawing.Point

						REM Keep track of the point at which an Instruction's Line ends, updating this for each Path Drawn
						Dim _LastInstructionEndPoint As Drawing.Point

						'#WHERE {this} means the Points of the current _Instruction (i.e. _Instruction.Points)
						For Each _Instruction As SVGPathInstruction In SVGPathInstruction.FromDAttributeInstructions(_Path.Attributes!d.InnerText)

							REM Apart from Z, it should be impossible for an _Instruction to have no .Points; the PathInstruction String wouldn't make it through the RegExp

							MsgDebug("Processing _Instruction: " & vbCrLf & _Instruction.ToString())
							REM Example _Instruction: M2000 3467

							REM Check if the Instruction was a [z]. This instruction has no Points associated with it, so is delt with seperatly
							If (_Instruction.CommandLetter = "Z"c) OrElse (_Instruction.CommandLetter = "z"c) Then

								'A Z|z means [join up the <path> back to the starting point]

								REM W3.Org says:
								REM If a "closepath" is followed immediately by any other command (i.e. not M|m), then the next subpath starts at the same initial point as the current subpath.
								'...Hopefully that's not a problem because Potrace will never do this.

								_BeziersToReturn.Add(QuadraticBezier.StraightLineFromPoints(_LastUsedPoint, _PathStartPoint))
								_LastUsedPoint = (_PathStartPoint) : Continue For

							End If

							If Char.IsUpper(_Instruction.CommandLetter) Then REM The Instruction is one with ABSOLUTE Coordinates

								Select Case _Instruction.CommandLetter
									Case "M"c 'Move to {this}

										REM Set the _LastUsedPoint to {this}0
										_LastUsedPoint = _Instruction.Points(0)

										REM Since there should ALWAYS be an M|m at the start of a new SubPath in Potrace SVG's, this sets the _PathStartPoint
										_PathStartPoint = _LastUsedPoint

										REM This should also affect the _LIEP
										_LastInstructionEndPoint = _PathStartPoint

									Case "L"c 'Line to {this}

										REM Create a StraightLine-Style Bezier, with a [StartPoint] and [EndPoint]
										REM Set the _LastUsedPoint to {this}

										'  0	 1	   2		Length = 3
										'(5 4) (6 7) (9 8)

										REM This DOES iterate when there is only one Point in the _Instruction

										For _PointNumber% = 0I To (_Instruction.Points.MaxIndex()) Step +1
											_BeziersToReturn.Add(QuadraticBezier.StraightLineFromPoints(_LastUsedPoint, _Instruction.Points(_PointNumber)))
											_LastUsedPoint = _Instruction.Points(_PointNumber)
										Next

										REM Update the _LIEP
										_LastInstructionEndPoint = _LastUsedPoint

										REM What about if there was only 1 Point in the L Instruction - The ForLoop will have iterated JUST ONCE, i.e. It iterates once,
										REM ...And then checks it's Boolean Expression, and dosen't go round again because _PointNumber = [(_Instruction.Points.Length - 1) which is 0, for 1 Point]
										REM Therefore, A Line would have been drawn from the LUP (Could still be (0, 0) if not changed since Initialisation) to the only Point we had been given.

									Case "C"c 'Curve to {this}

										REM Create a CubicBezier with the correct [StartPoint], [ControlPointOne], [ControlPointTwo], and [EndPoint]
										REM Set the _LastUsedPoint to {this}LAST

										REM There must be at least 3 Points in {this} in order to create a CubicBezier with 4 Points
										If Not (_Instruction.Points.Length >= 3) Then Throw (New GraphPicturesConversionException("There were fewer than 3 Points in a C Instruction. The _Instruction was: " & _Instruction.ToString()))

										'Go forwards through the {this} Points, creating Beziers, jumping 3 points at a time

										REM This DOES NOT iterate when there is only one Point in the _Instruction, which there wouldn't be, because that would have thrown the above exception
										For _PointNumber% = 0I To (_Instruction.Points.Length - 2) Step +3

											_BeziersToReturn.Add(New CubicBezier(_LastInstructionEndPoint, _Instruction.Points(_PointNumber), _Instruction.Points(_PointNumber + 1), _Instruction.Points(_PointNumber + 2)))
											_LastUsedPoint = _Instruction.Points(_PointNumber + 2)

										Next

										REM The LUP is now the Last Point in {this}, that the ForLoop managed to cover.

										'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
										'If there were a multiple of 3 Points in {this}, then we have covered all of them already, so don't need to make sure to cover them all with the following...
										If (_Instruction.Points.Length Mod 3) = 0 Then Continue For

										REM Because that ForLoop would only cover all the points if [the Number of Points was GreaterThan 2 AND A multiple of 3], we need to go from the
										REM ...End of the _Instruction.Points, to make sure the final Cubic Bezier is drawn.

										REM (Length - 1)	= LAST ITEM
										REM (Length - 2)	= PENULTIMATE ITEM
										REM (Length - 3)	= One before Penultimate Item
										REM (Length - 4)	= Two before Penultimate Item

										'We know that there are AT LEAST 4 Points in {this}, because
										'[if there were fewer than 3 then the exception would have been thrown], and
										'[if there were a multiple of 3, including 3 itself, then we've exited select already].'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5

										REM There are [3 or More] Points in {this}

										Dim _LastBezier As CubicBezier = (New CubicBezier(
										   _StartPoint:=(_Instruction.Points(_Instruction.Points.Length - 4)),
										   _ControlPointOne:=_Instruction.Points(_Instruction.Points.Length - 3),
										   _controlpointtwo:=_Instruction.Points(_Instruction.Points.Length - 2),
										   _EndPoint:=_Instruction.Points(_Instruction.Points.Length - 1)
										  ))

										If Not (_LastBezier = _BeziersToReturn.Last()) Then _BeziersToReturn.Add(_LastBezier)

										REM Unconditionally set the _LUP to the Last Point in {this}
										_LastUsedPoint = _Instruction.Points(_Instruction.Points.Length - 1)

										REM Update the _LIEP
										_LastInstructionEndPoint = _LastUsedPoint

									Case Else
										Throw (New Exception("An UPPERCASE <path> InstructionLetter was detected, but was not able to be delt with. The dInstruction was: " & _Instruction.ToString()))
								End Select

							ElseIf [Char].IsLower(_Instruction.CommandLetter) Then	REM The Instruction is one with RELATIVE Coordinates

								Select Case _Instruction.CommandLetter
									Case "m"c 'Move (the virtual pen) by {this}

										_LastUsedPoint = _LastUsedPoint.MovedBy(_Instruction.Points(0))

										REM Since there should ALWAYS be an M|m at the start of a new SubPath in Potrace SVG's, this sets the _PathStartPoint
										_PathStartPoint = _LastUsedPoint

										REM This should also affect the _LIEP
										_LastInstructionEndPoint = _PathStartPoint

									Case "l"c 'Line to [_LastUsedPoint + {this}], then update LUP

										REM This DOES iterate when there is only one Point in the _Instruction

										For _PointNumber% = 0I To (_Instruction.Points.MaxIndex()) Step +1
											_BeziersToReturn.Add(QuadraticBezier.StraightLineFromPoints(_LastUsedPoint, _LastUsedPoint.MovedBy(_Instruction.Points(_PointNumber))))
											_LastUsedPoint = _LastUsedPoint.MovedBy(_Instruction.Points(_PointNumber))
										Next

										REM Update the _LIEP
										_LastInstructionEndPoint = _LastUsedPoint

										'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
										'Should we be doing this?
										'_PathStartPoint = _LastUsedPoint

										REM That ForLoop will have iterated at least once, and if there was only 1 Point in {this}, then a line would have been drawn from the LUP to [the only point we had]

									Case "c"c 'Curve to [_LastUsedPoint + {this}], then update LUP
										MsgDebug("LUP at start of c: " & _LastUsedPoint.ToString())
										REM Create a CubicBezier with the correct [StartPoint], [ControlPointOne], [ControlPointTwo], and [EndPoint]
										REM Set the _LastUsedPoint to {this}LAST

										REM Take a Snapshot of the LUP, which will be used later IF there are not a multiple of 3 Points in {this}
										Dim _LUPSnapshotFromStartOfInstruction As Drawing.Point = _LastUsedPoint

										REM There must be at least 3 Points in {this} in order to create a CubicBezier with 4 Points
										If Not (_Instruction.Points.Length >= 3) Then Throw (New GraphPicturesConversionException("There were fewer than 3 Points in a c Instruction. The _Instruction was: " & _Instruction.ToString()))

										'Go forwards through the {this} Points, creating Beziers, jumping 3 points at a time

										REM This DOES NOT iterate when there is only one Point in the _Instruction (which is never, because of the above Exception-Throwing Check)
										For _PointNumber% = 0I To (_Instruction.Points.Length - 2) Step +3

											MsgDebug("In For Loop. _PointNumber: " & _PointNumber.ToString())

											_BeziersToReturn.Add(New CubicBezier(
											 _LastInstructionEndPoint,
											 _LastInstructionEndPoint.MovedBy(_Instruction.Points(_PointNumber)),
											 _LastInstructionEndPoint.MovedBy(_Instruction.Points(_PointNumber + 1)),
											 _LastInstructionEndPoint.MovedBy(_Instruction.Points(_PointNumber + 2))
											))

											'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
											_LastUsedPoint = _LastInstructionEndPoint.MovedBy(_Instruction.Points(_PointNumber + 2))

											REM The last Bezier drawn is essentially the last instruction, so:
											_LastInstructionEndPoint = _LastUsedPoint

										Next

										'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
										'If there were a multiple of 3 Points in {this}, then we have covered all of them already, so don't need to make sure to cover them all with the following...
										'MsgDebug("(_Instruction.Points.Length Mod 3) = 0 : " & ((_Instruction.Points.Length Mod 3) = 0).ToString())
										'_LastUsedPoint = _Instruction.Points.LUPAsItWouldHaveBeenAfterIndex(_Instruction.Points.Length - 1, _LUPSnapshotFromStartOfInstruction)
										If (_Instruction.Points.Length Mod 3) = 0 Then Continue For

										REM Because that ForLoop would only cover all the points if [the Number of Points was GreaterThan 2 AND A multiple of 3], we need to go from the
										REM ...End of the _Instruction.Points, to make sure the final Bezier is drawn.

										REM (Length - 1)	= LAST ITEM
										REM (Length - 2)	= PENULTIMATE ITEM
										REM (Length - 3)	= One before Penultimate Item
										REM (Length - 4)	= Two before Penultimate Item

										'We know that there are AT LEAST 4 Points in {this}, because
										'[if there were fewer than 3 then the exception would have been thrown], and
										'[if there were a multiple of 3, including 3 itself, then we've exited Select already].

										'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
										'Should the line below be commented out?
										'_LastUsedPoint = _Instruction.Points.LUPAsItWouldHaveBeenAfterIndex(_Instruction.Points.Length - 4, _LUPSnapshotFromStartOfInstruction)

										Dim _LastBezier As CubicBezier = (New CubicBezier(
										  _StartPoint:=(_LastUsedPoint.MovedBy(_Instruction.Points(_Instruction.Points.Length - 4))),
										  _ControlPointOne:=_LastUsedPoint.MovedBy(_Instruction.Points(_Instruction.Points.Length - 3)),
										  _ControlPointTwo:=_LastUsedPoint.MovedBy(_Instruction.Points(_Instruction.Points.Length - 2)),
										  _EndPoint:=_LastUsedPoint.MovedBy(_Instruction.Points(_Instruction.Points.Length - 1))
										 ))

										If Not (_LastBezier = _BeziersToReturn.Last()) Then _BeziersToReturn.Add(_LastBezier)

										REM Unconditionally set the _LUP to the Last Point in {this} (moved by all previous points)
										MsgDebug("c Instruction: Setting _LUP to: " & _Instruction.Points.LUPAsItWouldHaveBeenAfterIndex(_Instruction.Points.Length - 1, _LUPSnapshotFromStartOfInstruction).ToString())
										'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
										'Should the LUP be set to this?
										_LastUsedPoint = _PathStartPoint.MovedBy(_Instruction.Points(_Instruction.Points.Length - 1))

										REM Update the _LIEP
										_LastInstructionEndPoint = _LastUsedPoint.MovedBy(_Instruction.Points(_Instruction.Points.Length - 1))

										'Try:
										'_LastUsedPoint = ((_LastUsedPoint.MovedBy(_Instruction.Points(_Instruction.Points.Length - 3))).MovedBy(_Instruction.Points(_Instruction.Points.Length - 2))).MovedBy(_Instruction.Points(_Instruction.Points.Length - 1))

										'%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
									Case Else
										Throw (New Exception("An lowercase <path> InstructionLetter was detected, but was not able to be delt with. The dInstruction was: " & _Instruction.ToString()))
								End Select

							Else
								REM The FirstChar of the Instruction was not UpperCase or LowerCase
								Throw (New GraphPicturesConversionException("The first Character of an Instruction in the SVG <path> was not recognised. The Instruction was: " & _Instruction.ToString()))
							End If
						Next

					Else
						REM The current <*> is not a <path>
						Throw (New GraphPicturesConversionException("An unexpected XMLNode was found within a <g> tag in the SVG File. It's contents was: " & _Path.OuterXml))
					End If

				Next

				MsgDebug("Finished converting the SVGFile into Beziers. (Without any Exceptions...)" & vbCrLf & vbCrLf & "Good luck, Master...")
				Return _BeziersToReturn.ToArray()

			Catch _Ex As System.Exception When True
				Throw (New GraphPicturesConversionException("The following Exception was thrown when attempting to resolve the Beziers from an SVG File: " & _Ex.Message, _Ex))
			End Try

		End Function

	End Module

End Namespace