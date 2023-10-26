using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
using VolgaIT2023.ValidationAttributes;

namespace VolgaIT2023.Models
{
    public interface IAccountSignData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class Account : IAccountSignData
    {
        public Account() { }
        public Account(AccountCreateRequest request)
        {
            this.Update(request);
        }
        public Account(AccountAdminCreateRequest request)
        {
            this.Update(request);
        }
        public Account(Account request)
        {
            this.Update(request);
        }
        public void Update(AccountCreateRequest request)
        {
            this.Username = request.Username;
            this.Password = request.Password;
        }
        public void Update(Account request)
        {
            this.Username = request.Username;
            this.Password = request.Password;
            this.Balance = request.Balance;
            this.IsAdmin = request.IsAdmin;
        }
        public void Update(AccountAdminCreateRequest request)
        {
            this.Username = request.Username;
            this.Password = request.Password;
            this.Balance = request.Balance;
            this.IsAdmin = request.IsAdmin;
        }
        /// <summary>
        /// id аккаунт пользователя
        /// </summary>
        [Key]
        public long Id { get; private set; }
        /// <summary>
        /// имя пользователя
        /// </summary>
        [Required]
        [UniqueUsername]
        public string Username { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
        [Required]
        public bool IsAdmin { get; set; } = false;
        [Required]
        public double Balance { get; set; }
        [JsonIgnore]
        public virtual ICollection<Transport> Transports { get; }
        [JsonIgnore]
        public virtual ICollection<Rent> Rents { get; }
        [JsonIgnore]
        public virtual ICollection<Session> Sessions { get; }
    }
    public class AccountSignRequest : IAccountSignData
    {
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
    [ModelMetadataType(typeof(Account))]
    public class AccountCreateRequest : IAccountSignData
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
    [ModelMetadataType(typeof(Account))]
    public class AccountAdminCreateRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsAdmin { get; set; } = false;
        public double Balance { get; set; }
    }
}
