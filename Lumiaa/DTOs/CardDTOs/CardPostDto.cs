namespace Lumiaa.DTOs.CardDTOs
{
    public class CardPostDto
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
    }
}
