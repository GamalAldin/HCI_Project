using Svg;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public static class SvgIconRenderer
{
    /// <summary>
    /// Loads an SVG file and displays it in a PictureBox.
    /// </summary>
    /// <param name="svgPath">Path to the SVG file</param>
    /// <param name="targetPictureBox">The PictureBox to render into</param>
    public static void Render(string svgPath, PictureBox targetPictureBox)
    {
        try
        {
            if (!File.Exists(svgPath))
            {
                MessageBox.Show($"File not found: {svgPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var svgDoc = SvgDocument.Open(svgPath);
            Bitmap bitmap = svgDoc.Draw();

            targetPictureBox.Image = bitmap;
            targetPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to load SVG:\n" + ex.Message, "Rendering Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
