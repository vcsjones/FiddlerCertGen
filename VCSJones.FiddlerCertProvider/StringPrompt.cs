using System;
using System.Drawing;
using System.Windows.Forms;

namespace VCSJones.FiddlerCertProvider
{
    public static class StringPrompt
    {
        public static string PromptForString(string title, string description, IWin32Window owner = null)
        {
            using (var form = new PromptForm(title, description))
            {
                var result = form.ShowDialog(owner);
                if (result == DialogResult.OK)
                {
                    return form._textInput?.Text;
                }
            }
            return null;
        }

        private sealed class PromptForm : Form
        {
            private readonly string _description;
            private SizeF _boxSize;
            internal readonly TextBox _textInput;


            public PromptForm(string title, string description)
            {
                _description = description;
                _textInput = new TextBox();
                Text = title;
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                Width = 500;
                using (var g = CreateGraphics())
                {
                    _boxSize = g.MeasureString(_description, Font, new SizeF(490, float.PositiveInfinity), StringFormat.GenericDefault);
                }
                var screenRectangle = RectangleToScreen(ClientRectangle);
                var titleHeight = screenRectangle.Top - Top;
                Button okButton = new Button { Text = "OK" }, cancelButton = new Button { Text = "Cancel" };
                okButton.Click += delegate { DialogResult = DialogResult.OK; };
                _textInput.Left = 5;
                _textInput.Top = (int) _boxSize.Height + 10;
                _textInput.Width = ClientRectangle.Width - 10;
                okButton.Left = ClientRectangle.Width - okButton.Width - 5;
                cancelButton.Left = okButton.Left - cancelButton.Width - 5;
                okButton.Top = _textInput.Bottom + 5;
                cancelButton.Top = _textInput.Bottom + 5;
                Controls.Add(okButton);
                Controls.Add(cancelButton);
                Controls.Add(_textInput);
                AcceptButton = okButton;
                CancelButton = cancelButton;
                Height = okButton.Bottom + titleHeight + 5 + SystemInformation.BorderSize.Height*2;
            }

            protected override void OnShown(EventArgs e)
            {
                base.OnShown(e);
                _textInput.Focus();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.DrawString(_description, Font, Brushes.Black, new RectangleF(new PointF(5, 5), _boxSize));
            }
        }
    }
}
