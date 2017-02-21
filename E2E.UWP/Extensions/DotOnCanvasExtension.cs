using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace E2E.UWP.Extensions.DotOnCanvasExtension
{
    public static class DotOnCanvasExtension
    {
        public static void ClearDots(this Canvas canvas)
        {
            foreach (var item in canvas.Children.AsEnumerable())
            {
                if (item.GetType() == typeof(Ellipse))
                {
                    canvas.Children.Remove(item);
                }
            }
        }

        public static void DrawDotByPercent(this Canvas canvas, double xPercent, double yPercent)
        {
            var width = canvas.ActualWidth;
            var height = canvas.ActualHeight;

            double amplifiedXPercent = 0;
            if (xPercent > 0.6)
                amplifiedXPercent = xPercent * 1.25;
            else if (xPercent < 0.4)
                amplifiedXPercent = xPercent / 1.25;

            double amplifiedYPercent = 0;
            if (yPercent > 0.6)
                amplifiedYPercent = xPercent * 1.2;
            else if (yPercent < 0.4)
                amplifiedYPercent = xPercent / 1.2;

            double xOutput = width * amplifiedXPercent;
            double yOutput = height * amplifiedYPercent;

            DrawDotByPosition(canvas, xOutput, yOutput);
        }

        public static void DrawDotByPosition(this Canvas canvas, double x, double y)
        {
            var dot = new Ellipse();
            dot.Fill = new SolidColorBrush(Colors.Green);
            dot.Height = 10;
            dot.Width = 10;
            dot.Margin = new Thickness(x, y, 0, 0);
            canvas.Children.Add(dot);
        }
    }
}
