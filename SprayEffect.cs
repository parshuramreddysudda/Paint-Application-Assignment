using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows;

public class SprayEffect
{
    private readonly InkCanvas inkCanvas;
    private readonly double sprayRadius;
    private readonly int particlesPerSecond;
    private readonly DispatcherTimer sprayTimer;
    private readonly Random random;
    private readonly SolidColorBrush sprayColor;
    private readonly DrawingAttributes sprayAttributes;

    private bool IsMousePressed = false;
    private Point sprayCenter;

    public SprayEffect(InkCanvas inkCanvas, double sprayRadius, int particlesPerSecond, DrawingAttributes sprayAttributes)
    {
        this.inkCanvas = inkCanvas;
        this.sprayRadius = sprayRadius;
        this.particlesPerSecond = particlesPerSecond;
        this.random = new Random();
        this.sprayAttributes = sprayAttributes; // Use DrawingAttributes

        this.sprayTimer = new DispatcherTimer();
        this.sprayTimer.Tick += SprayTimer_Tick;

        // Subscribe to mouse events
        inkCanvas.MouseDown += InkCanvas_MouseDown;
        inkCanvas.MouseMove += InkCanvas_MouseMove;
        inkCanvas.MouseUp += InkCanvas_MouseUp;
    }

    public void StartSpraying()
    {
        sprayTimer.Interval = TimeSpan.FromSeconds(1.0 / particlesPerSecond);
        sprayTimer.Start();
    }

    private void SprayTimer_Tick(object sender, EventArgs e)
    {
        // Check if the mouse button is pressed
        if (IsMousePressed)
        {
            // Calculate a random point within the spray radius
            double angle = random.NextDouble() * 2 * Math.PI;
            double distance = Math.Sqrt(random.NextDouble()) * sprayRadius;

            double particleX = sprayCenter.X + distance * Math.Cos(angle);
            double particleY = sprayCenter.Y + distance * Math.Sin(angle);

            // Create a small ellipse at the calculated point
            Ellipse ellipse = new Ellipse
            {
                Width = sprayAttributes.Width,
                Height = sprayAttributes.Height,
                Fill = new SolidColorBrush(sprayAttributes.Color) // Use the color from DrawingAttributes
            };

            // Position the ellipse at the calculated point
            InkCanvas.SetLeft(ellipse, particleX);
            InkCanvas.SetTop(ellipse, particleY);

            // Add the ellipse to the ink canvas
            inkCanvas.Children.Add(ellipse);
        }
    }


    private void InkCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        IsMousePressed = true;
        sprayCenter = e.GetPosition(inkCanvas);
    }

    private void InkCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (IsMousePressed)
        {
            sprayCenter = e.GetPosition(inkCanvas);
        }
    }

    private void InkCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        IsMousePressed = false;
    }
}