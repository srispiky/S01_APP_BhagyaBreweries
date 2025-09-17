using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] ContactFormModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var mail = new MailMessage();
            mail.From = new MailAddress("srispiky@gmail.com");
            mail.To.Add("bhagyabeverages@gmail.com");
            mail.Subject = $"Contact Form: {model.Name}";
            mail.Body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}";

            // Use configuration or secrets for credentials
            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("srispiky@gmail.com", smtpPassword),
                EnableSsl = true
            };
            smtp.Send(mail);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the exception (use ILogger in production)
            return StatusCode(500, $"Error sending email: {ex.Message}");
        }
    }
}

public class ContactFormModel
{
    [Required]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Message { get; set; }
}