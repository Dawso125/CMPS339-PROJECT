using System.ComponentModel.DataAnnotations.Schema;

namespace CMPS339_PROJECT.Models
{
    public class Attraction
    {
        public int Id { get; set; }

        [ForeignKey("AmusementPark")]
        public int ParkId { get; set; }
        public Parks? Park { get; set; }
    }

    public class AttractionDto
    {
        public int Id { get; set; }

        [ForeignKey("AmusementPark")]
        public int ParkId { get; set;}
    }
}