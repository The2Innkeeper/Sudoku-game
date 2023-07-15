using System.Configuration;
using System.Data;
using System.Windows;

namespace SudokuWPF_test1;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        // Display error message
        MessageBox.Show("An unexpected error occurred: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        // Prevent the application from crashing
        e.Handled = true;
    }
}
