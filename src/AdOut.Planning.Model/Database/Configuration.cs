using AdOut.Extensions.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("Configurations")]
    public class Configuration : PersistentEntity
    {
        public Configuration()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
