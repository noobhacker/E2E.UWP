using E2E.UWP.Helpers;
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
        public static void RemoveDots(this Canvas canvas)
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

            var centerX = LookingDirectionAlgorithm.MiddleX;
            var centerY = LookingDirectionAlgorithm.MiddleY;

            double calibratedXPercent = 0;
            if (centerX > 0.5)
                calibratedXPercent = xPercent + (centerX - 0.5);
            else if (centerX < 0.5)
                calibratedXPercent = xPercent - (0.5 - centerX);
            else
                calibratedXPercent = xPercent;

            double calibratedYPercent = 0;
            if (centerY > 0.5)
                calibratedYPercent = yPercent + (centerY - 0.5);
            else if (centerY < 0.5)
                calibratedYPercent = yPercent - (0.5 - centerY);
            else
                calibratedYPercent = yPercent;

            double xOutput = width * calibratedXPercent;
            double yOutput = height * calibratedYPercent;

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
