

namespace Library_Management.Models;

public class Author
{
    public int Id { get; set; } 
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public DateTime DateBirth { get; set; }
    
    public string Country { get; set; } = string.Empty;
    
    public string FullName => $"{FirstName} {LastName}";
    
    public List<Book> Books { get; set; } = new List<Book>();
}