using System.Windows;
using Library_Management.Data;
using Library_Management.Models;
using Library_Management.Views;
using System.Linq;

namespace Library_Management.Views;

public partial class GenreManagementWindow : Window
{
    private readonly LibraryDbContext _context;
    public GenreManagementWindow(LibraryDbContext context)
    {
        InitializeComponent();
        _context = context;
        LoadGenres();
    }

    public void LoadGenres()
    {
        var genres = _context.Genres.ToList();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new GenreWindow();
        if (window.ShowDialog() == true)
        {
            _context.Genres.Add(window.Genre);
            _context.SaveChanges();
            LoadGenres();
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (GenresDataGrid.SelectedItem is Genre selecedGenre)
        {
            var window = new GenreWindow(selecedGenre);
            if (window.ShowDialog() == true)
            {
                _context.Genres.Update(window.Genre);
                _context.SaveChanges();
                LoadGenres();
            }
        }
        else
        {
            MessageBox.Show("Please select a Genre");
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (GenresDataGrid.SelectedItem is Genre selecedGenre)
        {
            if (MessageBox.Show("Are you sure you want to delete this genre?", "Confirm Delete",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Genres.Remove(selecedGenre);
                _context.SaveChanges();
                LoadGenres();
            }
        }
        else
        {
            MessageBox.Show("Please select a Genre");
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}