namespace Identity.Blazor.Models;

public class CreateUserModel
{
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string? Email { get; set; }
}