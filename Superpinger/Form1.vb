Imports System.Drawing

Public Class Form1
    Dim n
    Dim pen As New Pen(Color.Blue, 1)
    Dim sdf As New Drawing.Drawing2D.GraphicsPath
    Dim a As New PointF
    Dim b As New PointF
    Dim t As Integer
    Dim mustStop As Integer
    Dim PingThread As New Threading.Thread(AddressOf Pinger)


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Button2.Enabled = True
        Button1.Enabled = False
        PingThread.Start()
    End Sub
    Sub CG(ByVal sender As Object, ByVal e As PaintEventArgs) Handles TableLayoutPanel1.Paint
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.DrawPath(pen, sdf)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        a.X = 0
        a.Y = 352
        b.X = a.X
        b.Y = a.Y
    End Sub
    Sub Pinger()
        'ShapeContainer1.Invalidate()
        TextBox1.Enabled = False
        Dim vorrt = 1 * 60 * 60
        Dim ddd As Date = FormatDateTime(Now, DateFormat.ShortTime)
        Dim tt = Math.Round(vorrt / 16, 0)
        Label01.Text = FormatDateTime(ddd.AddSeconds(tt), DateFormat.ShortTime)
        Label02.Text = FormatDateTime(ddd.AddSeconds(tt * 2), DateFormat.ShortTime)
        Label03.Text = FormatDateTime(ddd.AddSeconds(tt * 4), DateFormat.ShortTime)
        Label04.Text = FormatDateTime(ddd.AddSeconds(tt * 8), DateFormat.ShortTime)
        Label05.Text = FormatDateTime(ddd.AddSeconds(tt * 12), DateFormat.ShortTime)
        Label06.Text = FormatDateTime(ddd.AddSeconds(tt * 16), DateFormat.ShortTime)
        Dim k = 0
        'n = msec
        't = Zeit verstrichen
        t = 1
        Try
            My.Computer.Network.Ping(TextBox1.Text, 5000)
        Catch ex As Exception
            If Err.Number = 5 Then
                MsgBox("Unable to resolve DNS-Name")
            Else
                MsgBox(ex.Message)
            End If
            Button2.Enabled = False
            Button1.Enabled = True
            Exit Sub
        End Try
        For ol = 0 To 477
            If mustStop Then
                Exit Sub
            End If
            Schreibe(FormatDateTime(Now(), DateFormat.LongTime) & " ", Color.Blue)
            Schreibe(TextBox1.Text & " wird angepingt..." & vbCrLf, Color.Black)
            Dim aktuell = Now
            Try
                If My.Computer.Network.Ping(TextBox1.Text, 5000) = True Then
                    n = (Now() - aktuell).TotalMilliseconds
                    Schreibe(FormatDateTime(Now(), DateFormat.LongTime) & " ", Color.Blue)
                    Schreibe("Antwort von " & TextBox1.Text & " (" & (Now() - aktuell).TotalMilliseconds & " ms)" & vbCrLf, Color.Green)
                Else
                    n = (Now() - aktuell).TotalMilliseconds
                    Schreibe(FormatDateTime(Now(), DateFormat.LongTime) & " ", Color.Blue)
                    Schreibe("Zeitüberschreitung" & vbCrLf, Color.Red)
                End If
            Catch ex As Exception
                Schreibe(FormatDateTime(Now(), DateFormat.LongTime) & " ", Color.Blue)
                Schreibe("Host nicht erreichbar." & vbCrLf, Color.Red)
            End Try

            Dim diff = (Now() - aktuell).TotalMilliseconds
            a.X = b.X
            a.Y = b.Y
            b.X = b.X + 1
            b.Y = 352 - n * 8 / 20
            Schreibe("Warte " & Math.Round((1000 - diff) / 1000, 2) & " Sekunden" & vbCrLf, Color.Black)
            Try
                Threading.Thread.Sleep(1000 - diff)
            Catch ex As Exception
            End Try
            sdf.AddLine(a, b)
            TableLayoutPanel1.Invalidate()
        Next
    End Sub
    Sub Schreibe(ByVal Text As String, ByVal Farbe As Color)
        Dim start = RichTextBox1.TextLength
        RichTextBox1.AppendText(Text)
        Dim Ende = RichTextBox1.TextLength
        RichTextBox1.Select(start, Ende)
        RichTextBox1.SelectionColor = Farbe
        RichTextBox1.SelectionLength = 0
        RichTextBox1.ScrollToCaret()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        mustStop = True
        TextBox1.Enabled = True
        Button1.Enabled = True
        Button2.Enabled = False

    End Sub
End Class
