using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Profile.Domain.Entities
{
    [Table("page_profiles")]
    public class PageProfile : Profile
    {

        [Column("name")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [ForeignKey("Id")]
        public virtual Profile Profile { get; set; }

    }

}
