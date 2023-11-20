# Paint Application Assignment

This repository contains a Full Stack developer assignment, implementing a feature-rich paint application with additional functionalities beyond the basic requirements. The application allows users to spray paint images, change color and density, erase spray, save changes, and reopen edited images without saving the spray onto the original image.

## Folder Structure

The project follows a straightforward folder structure:

```plaintext
|-- PaintApplicationAssignment
    |-- MainWindow.xaml
    |-- MainWindow.xaml.cs
    |-- PaintApplicationAssignment.sln
    |-- ...
```

- **`PaintApplicationAssignment`**: Main project folder.
  - **`MainWindow.xaml`**: XAML file defining the application's main window.
  - **`MainWindow.xaml.cs`**: C# code-behind file implementing the application's logic.
  - **`PaintApplicationAssignment.sln`**: Visual Studio solution file.

## Tech Stack

The project is developed using the following technologies:

- **C#**: Primary programming language.
- **Windows Presentation Foundation (WPF)**: UI framework for creating desktop applications.
- **XAML**: Markup language for defining UI elements in WPF.
- **.NET Framework**: Software framework for building Windows applications.

## How to Run the Project (Windows)

1. Ensure you have the .NET Framework installed on your machine.
2. Clone this repository to your local machine.
3. Open the solution file (`PaintApplicationAssignment.sln`) in Visual Studio.
4. Build and run the project from Visual Studio.

## Functionality and File Explanations

### Drawing and Spray Painting

- The application offers a drawing feature with and without images, allowing users to create digital art.
- Spray painting is the primary feature, enabling users to spray paint images using the mouse.

### Line Effect, Brush Effect, and Reset

- Additional features include a line effect, brush effect, and a reset effect that clears the drawing.

### RGB Color Code

- Instead of standard colors, the application provides an RGB color coder for more precise color selection.

### File Descriptions

- **`MainWindow.xaml`**: Defines the structure of the main window, including UI elements and layout.
- **`MainWindow.xaml.cs`**: Contains the C# code implementing the application's logic, including spray painting, color selection, and additional features.
- **`SprayEffect.cs`**: A separate class handling the spray painting effect, with methods to start and stop spraying.

## Saving Changes

- Users can save changes to a new image.
- (Still Working) When reopening the application, users can edit and update the spray paint on the same image without saving the spray onto the original image.

## Extra Features

- **Drawing Feature**: Allows users to create digital art with or without images.
- **Line Effect**: Introduces a line effect for more precise drawing.
- **Brush Effect**: Enhances drawing capabilities with a brush effect.
- **Reset Effect**: Provides a reset feature to clear the drawing.
- **RGB Color Coder**: Improves color selection accuracy with an RGB color coder.