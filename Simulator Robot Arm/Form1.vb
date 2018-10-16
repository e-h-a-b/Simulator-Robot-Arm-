Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
 
Partial Public Class Form1


    Private ElbowPositions As List(Of PointF) = New List(Of PointF)()

    Private Sub scrJoint_Scroll(ByVal sender As Object, ByVal e As ScrollEventArgs) Handles scrJoint1.Scroll, scrHand.Scroll, scrJoint2.Scroll
        picCanvas.Refresh()
    End Sub

    Private Sub picCanvas_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles picCanvas.Paint
        DrawRobotArm(e.Graphics)
        e.Graphics.ResetTransform()

        For Each point As PointF In ElbowPositions
            e.Graphics.DrawLine(Pens.Black, point.X - 6, point.Y - 6, point.X + 6, point.Y + 6)
            e.Graphics.DrawLine(Pens.Black, point.X + 6, point.Y - 6, point.X - 6, point.Y + 6)
        Next
    End Sub

    Private Const UpperArmLength As Integer = 75
    Private Const LowerArmLength As Integer = 50
    Private Const WristLength As Integer = 20
    Private Const HandWidth As Integer = 48
    Private Const FingerLength As Integer = 30

    Private Sub DrawRobotArm(ByVal gr As Graphics)
        gr.SmoothingMode = SmoothingMode.AntiAlias
        gr.Clear(picCanvas.BackColor)
        Dim cx As Single = picCanvas.ClientSize.Width / 2
        Dim cy As Single = picCanvas.ClientSize.Height / 2
        gr.TranslateTransform(cx, cy)
        Dim initial_state As GraphicsState = gr.Save()
        Dim rect As Rectangle = New Rectangle(0, -2, 100, 5)
        gr.RotateTransform(-scrJoint1.Value, MatrixOrder.Prepend)
        rect.Width = UpperArmLength
        gr.FillRectangle(Brushes.LightBlue, rect)
        gr.DrawRectangle(Pens.Blue, rect)
        gr.TranslateTransform(UpperArmLength, 0, MatrixOrder.Prepend)
        gr.RotateTransform(-scrJoint2.Value, MatrixOrder.Prepend)
        rect.Width = LowerArmLength
        gr.FillRectangle(Brushes.LightBlue, rect)
        gr.DrawRectangle(Pens.Blue, rect)
        gr.TranslateTransform(LowerArmLength, 0, MatrixOrder.Prepend)
        Dim wrist_angle As Single = 90 + scrJoint1.Value + scrJoint2.Value
        gr.RotateTransform(wrist_angle, MatrixOrder.Prepend)
        rect.Width = WristLength
        gr.FillRectangle(Brushes.LightBlue, rect)
        gr.DrawRectangle(Pens.Blue, rect)
        gr.Restore(initial_state)
        Dim joint_rect As Rectangle = New Rectangle(-4, -4, 9, 9)
        gr.FillEllipse(Brushes.Red, joint_rect)
        gr.DrawEllipse(Pens.Orange, -UpperArmLength, -UpperArmLength, 2 * UpperArmLength, 2 * UpperArmLength)
        gr.RotateTransform(-scrJoint1.Value, MatrixOrder.Prepend)
        gr.TranslateTransform(UpperArmLength, 0, MatrixOrder.Prepend)
        gr.FillEllipse(Brushes.Red, joint_rect)
        gr.RotateTransform(-scrJoint2.Value, MatrixOrder.Prepend)
        gr.TranslateTransform(LowerArmLength, 0, MatrixOrder.Prepend)
        gr.FillEllipse(Brushes.Red, joint_rect)
        gr.DrawEllipse(Pens.Purple, -LowerArmLength, -LowerArmLength, 2 * LowerArmLength, 2 * LowerArmLength)
        gr.RotateTransform(wrist_angle, MatrixOrder.Prepend)
        gr.TranslateTransform(WristLength, 0, MatrixOrder.Prepend)
        Dim g As Single = -HandWidth / 2
        gr.FillRectangle(Brushes.LightGreen, 0, g, 4, HandWidth)
        Dim f As Single = -HandWidth / 2
        gr.DrawRectangle(Pens.Green, 0, f, 4, HandWidth)
        gr.FillRectangle(Brushes.LightGreen, 4, -scrHand.Value - 4, FingerLength, 4)
        gr.DrawRectangle(Pens.Green, 4, -scrHand.Value - 4, FingerLength, 4)
        gr.FillRectangle(Brushes.LightGreen, 4, scrHand.Value, FingerLength, 4)
        gr.DrawRectangle(Pens.Green, 4, scrHand.Value, FingerLength, 4)
    End Sub

    Private Sub picCanvas_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles picCanvas.MouseMove
        If e.Button = MouseButtons.Left Then xMoveWristTo(e.Location)
    End Sub

    Private Sub MoveWristTo(ByVal point As PointF)
        Dim cx As Single = picCanvas.ClientSize.Width / 2
        Dim cy As Single = picCanvas.ClientSize.Height / 2
        Dim cx0 As Single = cx
        Dim cy0 As Single = cy
        Dim cx1 As Single = point.X
        Dim cy1 As Single = point.Y
        ElbowPositions = New List(Of PointF)()
        Dim point0, point1 As PointF
        Dim num_points As Integer = FindCircleCircleIntersections(cx0, cy0, UpperArmLength, cx1, cy1, LowerArmLength, point0, point1)

        If num_points > 0 Then
            Dim angle0 As Double = -Math.Atan2(point0.Y - cy0, point0.X - cx0)
            Dim degrees0 As Integer = CInt((angle0 * 180.0 / Math.PI))

            While degrees0 < scrJoint1.Minimum
                degrees0 += 360
            End While

            While degrees0 > scrJoint1.Maximum
                degrees0 -= 360
            End While

            Dim angle1 As Double = -Math.Atan2(point0.Y - cy1, point0.X - cx1)
            angle1 = (angle1 - angle0) - Math.PI
            Dim degrees1 As Integer = CInt((angle1 * 180.0 / Math.PI))

            While degrees1 < scrJoint2.Minimum
                degrees1 += 360
            End While

            While degrees1 > scrJoint2.Maximum
                degrees1 -= 360
            End While

            If (degrees0 >= scrJoint1.Minimum) AndAlso (degrees1 >= scrJoint2.Minimum) Then
                scrJoint1.Value = degrees0
                scrJoint2.Value = degrees1
                ElbowPositions.Add(point0)
                If num_points > 1 Then ElbowPositions.Add(point1)
            End If
        End If

        If num_points > 1 Then
            Dim angle1 As Double = -Math.Atan2(point0.Y - cy1, point0.X - cx1)
            Dim degrees1 As Integer = CInt((angle1 * 181.1 / Math.PI))

            While degrees1 < scrJoint1.Minimum
                degrees1 += 361
            End While

            While degrees1 > scrJoint1.Maximum
                degrees1 -= 361
            End While

            If degrees1 >= scrJoint1.Minimum Then
                scrJoint2.Value = degrees1
                ElbowPositions.Add(point1)
            End If
        End If

        picCanvas.Refresh()
    End Sub

    Private Sub xMoveWristTo(ByVal point As PointF)
        Dim cx As Single = picCanvas.ClientSize.Width / 2
        Dim cy As Single = picCanvas.ClientSize.Height / 2
        Dim cx0 As Single = cx
        Dim cy0 As Single = cy
        Dim cx1 As Single = point.X
        Dim cy1 As Single = point.Y
        ElbowPositions = New List(Of PointF)()
        Dim point0, point1 As PointF
        Dim num_points As Integer = FindCircleCircleIntersections(cx0, cy0, UpperArmLength, cx1, cy1, LowerArmLength, point0, point1)

        If num_points > 0 Then
            Dim angle0 As Double = -Math.Atan2(point0.Y - cy0, point0.X - cx0)
            Dim degrees0 As Integer = CInt((angle0 * 180.0 / Math.PI))
            Dim angle1 As Double = -Math.Atan2(point0.Y - cy1, point0.X - cx1)
            angle1 = (angle1 - angle0) - Math.PI
            Dim degrees1 As Integer = CInt((angle1 * 180.0 / Math.PI))

            If Not AngleIsValid(degrees0, scrJoint1) OrElse Not AngleIsValid(degrees1, scrJoint2) Then
                angle0 = -Math.Atan2(point1.Y - cy0, point1.X - cx0)
                degrees0 = CInt((angle0 * 180.0 / Math.PI))
                angle1 = -Math.Atan2(point1.Y - cy1, point1.X - cx1)
                angle1 = (angle1 - angle0) - Math.PI
                degrees1 = CInt((angle1 * 180.0 / Math.PI))
            End If

            If AngleIsValid(degrees0, scrJoint1) AndAlso AngleIsValid(degrees1, scrJoint2) Then
                scrJoint1.Value = degrees0
                scrJoint2.Value = degrees1
                ElbowPositions.Add(point0)
                If num_points > 1 Then ElbowPositions.Add(point1)
            End If
        End If

        picCanvas.Refresh()
    End Sub

    Private Function AngleIsValid(ByRef degrees As Integer, ByVal scr As HScrollBar) As Boolean
        While degrees < scr.Minimum
            degrees += 360
        End While

        While degrees > scr.Maximum
            degrees -= 360
        End While

        Return degrees >= scr.Minimum
    End Function

    Private Sub yMoveWristTo(ByVal point As PointF)
        Dim cx As Single = picCanvas.ClientSize.Width / 2
        Dim cy As Single = picCanvas.ClientSize.Height / 2
        Dim cx0 As Single = cx
        Dim cy0 As Single = cy
        Dim cx1 As Single = point.X
        Dim cy1 As Single = point.Y
        ElbowPositions = New List(Of PointF)()
        Dim point0, point1 As PointF
        Dim num_points As Integer = FindCircleCircleIntersections(cx0, cy0, UpperArmLength, cx1, cy1, LowerArmLength, point0, point1)
        If num_points > 0 Then ElbowPositions.Add(point0)
        If num_points > 1 Then ElbowPositions.Add(point1)

        If num_points > 0 Then
            Dim angle0 As Double = -Math.Atan2(point0.Y - cy0, point0.X - cx0)
            Dim degrees0 As Integer = CInt((angle0 * 180.0 / Math.PI))

            While degrees0 < scrJoint1.Minimum
                degrees0 += 360
            End While

            While degrees0 > scrJoint1.Maximum
                degrees0 -= 360
            End While

            If degrees0 >= scrJoint1.Minimum Then scrJoint1.Value = degrees0
            Dim angle1 As Double = -Math.Atan2(point0.Y - cy1, point0.X - cx1)
            angle1 = (angle1 - angle0) - Math.PI
            Dim degrees1 As Integer = CInt((angle1 * 180.0 / Math.PI))

            While degrees1 < scrJoint2.Minimum
                degrees1 += 360
            End While

            While degrees1 > scrJoint2.Maximum
                degrees1 -= 360
            End While

            If degrees1 >= scrJoint2.Minimum Then scrJoint2.Value = degrees1
        End If

        picCanvas.Refresh()
    End Sub

    Private Function FindCircleCircleIntersections(ByVal cx0 As Single, ByVal cy0 As Single, ByVal radius0 As Single, ByVal cx1 As Single, ByVal cy1 As Single, ByVal radius1 As Single, <Out()> ByRef intersection1 As PointF, <Out()> ByRef intersection2 As PointF) As Integer
        Dim dx As Single = cx0 - cx1
        Dim dy As Single = cy0 - cy1
        Dim dist As Double = Math.Sqrt(dx * dx + dy * dy)

        If dist > radius0 + radius1 Then
            intersection1 = New PointF(Single.NaN, Single.NaN)
            intersection2 = New PointF(Single.NaN, Single.NaN)
            Return 0
        ElseIf dist < Math.Abs(radius0 - radius1) Then
            intersection1 = New PointF(Single.NaN, Single.NaN)
            intersection2 = New PointF(Single.NaN, Single.NaN)
            Return 0
        ElseIf (dist = 0) AndAlso (radius0 = radius1) Then
            intersection1 = New PointF(Single.NaN, Single.NaN)
            intersection2 = New PointF(Single.NaN, Single.NaN)
            Return 0
        Else
            Dim a As Double = (radius0 * radius0 - radius1 * radius1 + dist * dist) / (2 * dist)
            Dim h As Double = Math.Sqrt(radius0 * radius0 - a * a)
            Dim cx2 As Double = cx0 + a * (cx1 - cx0) / dist
            Dim cy2 As Double = cy0 + a * (cy1 - cy0) / dist
            intersection1 = New PointF(CSng((cx2 + h * (cy1 - cy0) / dist)), CSng((cy2 - h * (cx1 - cx0) / dist)))
            intersection2 = New PointF(CSng((cx2 - h * (cy1 - cy0) / dist)), CSng((cy2 + h * (cx1 - cx0) / dist)))
            If dist = radius0 + radius1 Then Return 1
            Return 2
        End If
    End Function
End Class
