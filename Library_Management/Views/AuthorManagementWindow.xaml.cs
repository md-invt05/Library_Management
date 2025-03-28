using System.Windows;
using Library_Management.Data;
using Library_Management.Models;
using Library_Management.Views;
using System.Linq;

namespace Library_Management.Views;

public partial class AuthorManagementWindow : Window
{
    private readonly LibraryDbContext _context;
    
    public AuthorManagementWindow(LibraryDbContext context)
    {
        InitializeComponent();
        _context = context;
    }

    private void loadAuthors()
    {
        AuthorsDataGrid.ItemsSource = _context.Authors.ToList();
    }
    
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new AuthorWindow();
        if (window.ShowDialog() == true)
        {
            _context.Authors.Add(window.Author);
            _context.SaveChanges();
            loadAuthors();
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (AuthorsDataGrid.SelectedItem is Author selectedAuthor)
        {
            var window = new AuthorWindow(selectedAuthor);
            if (window.ShowDialog() == true)
            {
                _context.Authors.Update(selectedAuthor);
                _context.SaveChanges();
                loadAuthors();
            }
        }
        else
        {
            MessageBox.Show("Please select an Author");
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (AuthorsDataGrid.SelectedItem is Author selectedAuthor)
        {
            if (MessageBox.Show("Are you sure you want to delete this author?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes )
            {
                _context.Authors.Remove(selectedAuthor);
                _context.SaveChanges();
                loadAuthors();
            }
        }
        else
        {
            MessageBox.Show("Please select an Author");
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}