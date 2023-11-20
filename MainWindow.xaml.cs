using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Threading;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Media.Imaging;



namespace PaintApplicationAssignment
{
    public partial class MainWindow : Window
    {

        private readonly DrawingAttributes PenAttributes = new()
        {
            Color = Colors.Black,
            Height = 2,
            Width = 2
        };

        private readonly DrawingAttributes HighlighterAttributes = new()
        {
            Color = Colors.Black,
            Height = 3,
            Width = 3,
            IgnorePressure = true,
            IsHighlighter = true,
            StylusTip = StylusTip.Rectangle
        };
        public MainWindow()
        {
            InitializeComponent();
            Canvas.DefaultDrawingAttributes = PenAttributes;
            SprayBtn_Click(null, null);
        }

        private SolidColorBrush sprayColor = Brushes.Blue; // Default spray color
        private double sprayDensity = 2; // Default spray density

        private void SprayBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set the editing mode to Spray
            SetEditingMode(EditingMode.Spray);

            // Disable the InkCanvas editing mode
            Canvas.EditingMode = InkCanvasEditingMode.None;

            double sprayRadius = sprayDensity; // Adjust this value to change the spray radius
            int particlesPerSecond = 10000; // Adjust this value to change the spray density

            // Start the spray functionality
            SprayEffect sprayEffect = new SprayEffect(Canvas, sprayRadius, particlesPerSecond, PenAttributes);

            sprayEffect.StartSpraying();
        }


        // Add the SprayEffect class to your MainWindow class or as a separate class

        private void PenColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            // Update the spray color when the color picker selection changes
            if (IsLoaded)
                PenAttributes.Color = PenColorPicker.SelectedColor ?? Colors.Black;
        }
        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the spray density when the thickness slider value changes
            if (IsLoaded)
            {
                PenAttributes.Width = ThicknessSlider.Value;
                PenAttributes.Height = ThicknessSlider.Value;
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the ink canvas
            if (ShowConfirmationDialog("Are you sure you want to clear the image?"))
            {
                Canvas.Children.Clear();
                Canvas.Strokes.Clear();
            }
        }

        private bool ShowConfirmationDialog(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Load the image onto the canvas
                    Uri imageUri = new Uri(openFileDialog.FileName);
                    BitmapImage bitmapImage = new BitmapImage(imageUri);

                    // Calculate the aspect ratio
                    double aspectRatio = bitmapImage.PixelWidth / (double)bitmapImage.PixelHeight;

                    // Create an Image control
                    Image image = new Image();

                    // Set the width of the image to fit the canvas
                    image.Width = Canvas.ActualWidth;

                    // Adjust the height to maintain the aspect ratio
                    image.Height = image.Width / aspectRatio;

                    // Set the source of the image
                    image.Source = bitmapImage;

                    // Adjust the size of the InkCanvas
                    Canvas.Width = image.Width;
                    Canvas.Height = image.Height;

                    // Clear existing children and add the image to the canvas
                    Canvas.Children.Clear();
                    Canvas.Children.Add(image);
                }
                catch (Exception ex)
                {
                    ShowError($"Error loading the image: {ex.Message}");
                }
            }
        }


        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg;*.jpeg)|*.jpg;*.jpeg";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Find the bounds of the drawn content
                    Rect bounds = VisualTreeHelper.GetDescendantBounds(Canvas);
                    // Create a cropped RenderTargetBitmap
                    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                        (int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Default);

                    DrawingVisual dv = new DrawingVisual();
                    using (DrawingContext ctx = dv.RenderOpen())
                    {
                        // Draw the content onto the DrawingVisual
                        VisualBrush vb = new VisualBrush(Canvas);
                        ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
                    }

                    // Render the DrawingVisual onto the RenderTargetBitmap
                    renderTargetBitmap.Render(dv);

                    // Save the cropped image
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Change this to JpegBitmapEncoder if needed
                    encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                    using (var stream = saveFileDialog.OpenFile())
                    {
                        encoder.Save(stream);
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error saving the image: {ex.Message}");
                }
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EraserBtn_Click(object sender, RoutedEventArgs e)
        {
            // Eraser button click event
            SetEditingMode(EditingMode.Eraser);
        }

        private void PenBtn_Click(object sender, RoutedEventArgs e)
        {
            // Pen button click event
            SetEditingMode(EditingMode.Pen);
        }

        private void HighlighterBtn_Click(object sender, RoutedEventArgs e)
        {
            // Highlighter button click event
            SetEditingMode(EditingMode.Highlighter);
        }
      private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            // Select button click event
            SetEditingMode(EditingMode.Select);
        }

        private void SetEditingMode(EditingMode mode)
        {
            SelectBtn.IsChecked = false;
            PenBtn.IsChecked = false;
            HighlighterBtn.IsChecked = false;
            EraserBtn.IsChecked = false;
            SprayBtn.IsChecked = false;

            string modeName = string.Empty;

            switch (mode)
            {
                case EditingMode.Select:
                    SelectBtn.IsChecked = true;
                    Canvas.EditingMode = InkCanvasEditingMode.Select;
                    modeName = "Select";
                    break;
                case EditingMode.Pen:
                    PenBtn.IsChecked = true;
                    Canvas.EditingMode = InkCanvasEditingMode.Ink;
                    Canvas.DefaultDrawingAttributes = PenAttributes;
                    modeName = "Pen";
                    break;
                case EditingMode.Highlighter:
                    HighlighterBtn.IsChecked = true;
                    Canvas.EditingMode = InkCanvasEditingMode.Ink;
                    Canvas.DefaultDrawingAttributes = HighlighterAttributes;
                    modeName = "Highlighter";
                    break;
                case EditingMode.Eraser:
                    EraserBtn.IsChecked = true;
                    Canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    modeName = "Eraser";
                    break;
                case EditingMode.Spray:
                    SprayBtn.IsChecked = true;
                    Canvas.EditingMode = InkCanvasEditingMode.None;
                    modeName = "Spray";
                    break;
            }

            UpdateSelectedModeLabel(modeName);
        }

        private void UpdateSelectedModeLabel(string mode)
        {
            SelectedModeLabel.Content = $"Selected Mode: {mode}";
        }

        public enum EditingMode
        {
            Select, Pen, Highlighter, Eraser, Spray
        }
    }
}
