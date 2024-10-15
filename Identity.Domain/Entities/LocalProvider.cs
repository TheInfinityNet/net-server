using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("local_providers")]
    public class LocalProvider : AccountProvider
    {

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(255)]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [ForeignKey("Id")]
        public virtual AccountProvider AccountProvider { get; set; }

    }

}
