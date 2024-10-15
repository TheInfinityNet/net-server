using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("google_providers")]
    public class GoogleProvider : AccountProvider
    {

        [Column("google_id")]
        public Guid GoogleId { get; set; }

        [ForeignKey("Id")]
        public virtual AccountProvider AccountProvider { get; set; }

    }

}
