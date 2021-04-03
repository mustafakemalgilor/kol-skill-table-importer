/**
 * ______________________________________________________
 * This file is part of ko-skill-table-importer project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2016)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace KOSkillImporter.Forms
{
    public enum ProgressBarDisplayText
    {
        Percentage,
        CustomText
    }

    public partial class CustomProgressBar : ProgressBar
    {
        //Property to set to decide whether to print a % or Text
        public ProgressBarDisplayText DisplayStyle { get; set; }

        //Property to hold the custom text
        public String CustomText { get; set; }

        public CustomProgressBar()
        {
            // Modify the ControlStyles flags
            //http://msdn.microsoft.com/en-us/library/system.windows.forms.controlstyles.aspx
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            Graphics g = e.Graphics;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);
            rect.Inflate(-3, -3);
            if (Value > 0)
            {
                // As we doing this ourselves we need to draw the chunks on the progress bar
                var clip = new Rectangle(rect.X, rect.Y, (int) Math.Round(((float) Value/Maximum)*rect.Width),
                                               rect.Height);
                ProgressBarRenderer.DrawHorizontalChunks(g, clip);
            }

            // Set the Display text (Either a % amount or our custom text
            string text = DisplayStyle == ProgressBarDisplayText.Percentage ? Value.ToString(CultureInfo.InvariantCulture) + '%' : CustomText;


            using (Font f = new Font("Tahoma", 10))
            {

                SizeF len = g.MeasureString(text, f);
                // Calculate the location of the text (the middle of progress bar)
                Point location = new Point(Convert.ToInt32(value: (rect.Width/2) - (len.Width/2)),
                                           Convert.ToInt32(value: (rect.Height/2) - (len.Height/2))+4);
                // Draw the custom text
                g.DrawString(text, f, Brushes.DarkSlateBlue, location);
            }
        }
    }
}
