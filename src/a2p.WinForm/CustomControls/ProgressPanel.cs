namespace a2p.WinForm.CustomControls

{
    public class ProgressPanel : Panel
    {
        //public Color BorderColor { get; set; } = uwOrangeDeep;
        //public int BorderThickness { get; set; } = 2;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw custom border
            //using (var pen = new Pen(BorderColor, BorderThickness))
            //{
            // var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            // e.Graphics.DrawRectangle(pen, rect);
            //}

            // Draw the border



            base.OnPaint(e);

            // Custom border color
            Color borderColor = Color.FromArgb(239, 112, 32);


            int borderWidth = 3;
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                  borderColor, borderWidth, ButtonBorderStyle.Inset,
                  borderColor, borderWidth, ButtonBorderStyle.Inset,
                  borderColor, borderWidth, ButtonBorderStyle.Outset,
                  borderColor, borderWidth, ButtonBorderStyle.Outset);
        }
    }
}
