using System.Windows;
using Library_Management.Data;
using Library_Management.Models;
using Library_Management.Views;
using System.Linq;

namespace Library_Management.Views;

public partial class AuthorWindow : Window
{
    public Author? Author { get; private set; }
    
    public AuthorWindow(Author? author=null)
    {
        InitializeComponent();
        Author = author;
        if (Author != null)
        {
            FirstNameTextBox.Text = Author.FirstName;
            LastNameTextBox.Text = Author.LastName;
            BirthDatePicker.SelectedDate = Author.DateBirth;
            CountryTextBox.Text = Author.Country;
        }
        else
        {
            BirthDatePicker.SelectedDate = DateTime.Now;
        }
    }

    private void SaveButton_Clic(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
            string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
            BirthDatePicker.SelectedDate == null ||
            string.IsNullOrWhiteSpace(CountryTextBox.Text))
        {
            MessageBox.Show("Please enter all fields.");
            return;
        }

        if (Author == null)
        {
            Author = new Author();
        }

        Author.FirstName = FirstNameTextBox.Text;
        Author.LastName = LastNameTextBox.Text;
        Author.DateBirth = BirthDatePicker.SelectedDate.Value;
        Author.Country = CountryTextBox.Text;
        
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}