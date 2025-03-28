using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library_Management.Data;
using Library_Management.Models;
using Library_Management.Views;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Library_Management;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly LibraryDbContext _context;
    
    public MainWindow()
    {
        InitializeComponent();
        _context = new LibraryDbContext();
        
        _context.Database.EnsureCreated();
        
        LoadBooks();
        LoadFilters();
    }

    private void LoadFilters()
    {
        var authors = _context.Authors.ToList();
        AuthorFilrerBox.ItemsSource = authors;
        AuthorFilrerBox.DisplayMemberPath = "FullName";
        AuthorFilrerBox.SelectedValuePath = "Id";
        AuthorFilrerBox.SelectedIndex = -1;
        
        var genres = _context.Genres.ToList();
        GenreFilrerBox.ItemsSource = genres;
        GenreFilrerBox.DisplayMemberPath = "Name";
        GenreFilrerBox.SelectedValuePath = "Id";
        GenreFilrerBox.SelectedIndex = -1;
    }

    private void LoadBooks()
    {
        var query = _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            query = query.Where(b => b.Title.Contains(SearchTextBox.Text));
        }

        if (AuthorFilrerBox.SelectedValue is int authorId)
        {
         query = query.Where(b => b.AuthorId == authorId);
        }

        if (GenreFilrerBox.SelectedValue is int genreId)
        {
            query = query.Where(b => b.GenreId == genreId);
        }

        var books = query.ToList().Select(b => new
        {
            b.Id,
            b.Title,
            AuthorName = b.Author.FullName,
            b.PublishYear,
            b.ISBN,
            GenreName = b.Genre.Name,
            b.QuantitiyInStock,
        }).ToList();
        
        BooksDataGrid.ItemsSource = books;
        
    }
    
    private void FilterButton_Click(object sender, RoutedEventArgs e)
    {
        LoadBooks(); 
    }
    

    private void AddBookButton_Click(object sender, RoutedEventArgs e)
    {
        var bookWindow = new BookWindow(_context);
        if (bookWindow.ShowDialog() == true)
        {
            LoadBooks();
        }
    }

    private void EditBookButton_Click(object sender, RoutedEventArgs e)
    {
        dynamic selected = BooksDataGrid.SelectedItem;
        if (selected == null)
        {
            MessageBox.Show("Please select a book to edit.");
            return;
        }
        
        int bookId = selected.Id;
        var book =  _context.Books.FirstOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            var bookWindow = new BookWindow(_context, book);
            if (bookWindow.ShowDialog() ==  true )
            {
                LoadBooks();
            }
        }
    }

    private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
    {
        dynamic selected = BooksDataGrid.SelectedItem;
        if (selected == null)
        {
            MessageBox.Show("Please select a book to delete.");
            return;
        }
        
        int bookId = selected.Id;
        var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            if (MessageBox.Show("Are you sure you want to delete this book?", "Confirm Delete",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                LoadBooks();
            }
        }
    }

    private void ManageAuthorsButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new AuthorManagementWindow(_context);
        window.ShowDialog();
        LoadFilters();
        LoadBooks();
    }

    private void ManageGenresButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new GenreManagementWindow(_context);
        window.ShowDialog();
        LoadFilters();
        LoadBooks();
    }
}