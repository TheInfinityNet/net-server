using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("facebook_providers")]
    public class FacebookProvider : AccountProvider
    {

        [Column("facebook_id")]
        public Guid FacebookId { get; set; }

        [ForeignKey("Id")]
        public virtual AccountProvider AccountProvider { get; set; }

    }

}
