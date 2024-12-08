using InfinityNetServer.Services.Identity.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("external_providers")]
    public class ExternalProvider : AccountProvider
    {

        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [Column("external_name")]
        public ExternalProviderName ExternalName { get; set; }

        [ForeignKey("Id")]
        public virtual AccountProvider AccountProvider { get; set; }

    }

}
