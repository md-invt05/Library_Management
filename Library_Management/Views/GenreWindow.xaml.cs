using System.Windows;
using Library_Management.Data;
using Library_Management.Models;
using Library_Management.Views;
using System.Linq;

namespace Library_Management.Views;

public partial class GenreWindow : Window
{
    public Genre? Genre { get; private set; }

    public GenreWindow(Genre? genre = null)
    {
        InitializeComponent();
        Genre = genre;
        if (Genre != null)
        {
            NameTextBox.Text = Genre.Name;
            DescriptionTextBox.Text = Genre.Description;
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Please enter a name.");
            return;
        }

        if (Genre == null)
        {
            Genre = new Genre();
        }

        Genre.Name = NameTextBox.Text;
        Genre.Description = DescriptionTextBox.Text;
        
        DialogResult = true;
    }

    private void CancelButton_Clic(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}