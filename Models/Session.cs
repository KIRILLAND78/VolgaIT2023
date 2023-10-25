using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VolgaIT2023.Models
{
    public class Session
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long AccountId { get; set; }
        [Required]
        public string ExpireTime { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(AccountId))]
        public virtual Account Owner { get; }
    }
}
