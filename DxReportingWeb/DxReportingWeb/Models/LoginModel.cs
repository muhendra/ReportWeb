namespace DxReportingWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
    public class Response
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? USER_CODE { get; set; }
        public string? USER_NAME { get; set; }

    }
}
