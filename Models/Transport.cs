using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using VolgaIT2023.ValidationAttributes;

namespace VolgaIT2023.Models
{
    ///
    /// Касательно поля CanBeRented:
    /// В моем исполнении его менять может только владелец транспорта.
    /// Т.е. если транспорт арендуется, то CanBeRented = True, но
    /// отображаться в списке доступного транспорта он уже не будет
    /// и арендовать его нельзя.
    ///
    public interface ITransport
    {
        /// <summary>
        /// Id транспортного средства
        /// </summary>
        [Key]
        public long Id { get; }
        /// <summary>
        /// id аккаунта владельца
        /// </summary>
        public long OwnerId { get; }
        /// <summary>
        /// Можно ли арендовать транспорт?
        /// </summary>
        public bool CanBeRented { get; set; }
        /// <summary>
        /// Тип транспорта [Car, Bike, Scooter]
        /// </summary>
        public string TransportType { get; set; }
        /// <summary>
        /// Модель транспорта
        /// </summary>
        [Required]
        public string Model { get; set; }
        /// <summary>
        /// Цвет транспорта
        /// </summary>
        [Required]
        public string Color { get; set; }
        /// <summary>
        /// Номерной знак
        /// </summary>
        [Required]
        public string Identifier { get; set; }
        /// <summary>
        /// Описание (может быть null)
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Географическая широта местонахождения транспорта
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Географическая долгота местонахождения транспорта
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Цена аренды за минуту (может быть null)
        /// </summary>
        public double? MinutePrice { get; set; }
        /// <summary>
        /// Цена аренды за сутки (может быть null)
        /// </summary>
        public double? DayPrice { get; set; }
    }
    public class Transport : ITransport
    {
        public Transport() { }
        public Transport(TransportCreateRequest request)
        {
            this.Update(request);
        }
        public void Update(TransportCreateRequest request)
        {
            CanBeRented = request.CanBeRented;
            TransportType = request.TransportType;
            Model = request.Model;
            Color = request.Color;
            Identifier = request.Identifier;
            Description = request.Description;
            Latitude = request.Latitude;
            Longitude = request.Longitude;
            MinutePrice = request.MinutePrice;
            DayPrice = request.DayPrice;
        }
        public Transport(TransportAdminCreateRequest request)
        {
            this.Update(request);
        }
        public void Update(TransportAdminCreateRequest request)
        {
            OwnerId = request.OwnerId;
            CanBeRented = request.CanBeRented;
            TransportType = request.TransportType;
            Model = request.Model;
            Color = request.Color;
            Identifier = request.Identifier;
            Description = request.Description;
            Latitude = request.Latitude;
            Longitude = request.Longitude;
            MinutePrice = request.MinutePrice;
            DayPrice = request.DayPrice;
        }

        public bool IsRentable()
        {
            if (!CanBeRented) return false;
            if (Rents.Where(i => i.TimeEnd == null).FirstOrDefault() != null) return false;
            return true;
        }
        public static bool IsRentableQuery(Transport transport)
        {
            return transport.IsRentable();
        }
        /// <summary>
        /// Id транспортного средства
        /// </summary>
        [Key]
        public long Id { get; private set; }
        /// <summary>
        /// id аккаунта владельца
        /// </summary>
        public long OwnerId { get; set; }
        /// <summary>
        /// Можно ли арендовать транспорт?
        /// </summary>
        [Required]
        public bool CanBeRented { get; set; }
        /// <summary>
        /// Тип транспорта [Car, Bike, Scooter]
        /// </summary>
        [RegularExpression("Car|Bike|Scooter", ErrorMessage = "Type must be Car, Bike or Scooter.")]
        public string TransportType { get; set; } = "";
        /// <summary>
        /// Модель транспорта
        /// </summary>
        [Required]
        public string Model { get; set; } = "";
        /// <summary>
        /// Цвет транспорта
        /// </summary>
        [Required]
        public string Color {get; set;} = "";
        /// <summary>
        /// Номерной знак
        /// </summary>
        [Required]
        public string Identifier { get; set; } = "";
        /// <summary>
        /// Описание (может быть null)
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Географическая широта местонахождения транспорта
        /// </summary>
        [Required]
        public double Latitude { get; set; }
        /// <summary>
        /// Географическая долгота местонахождения транспорта
        /// </summary>
        [Required]
        public double Longitude { get; set; }
        /// <summary>
        /// Цена аренды за минуту (может быть null)
        /// </summary>
        public double? MinutePrice { get; set; }
        /// <summary>
        /// Цена аренды за сутки (может быть null)
        /// </summary>
        public double? DayPrice { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(OwnerId))]
        public virtual Account Owner { get; }
        [JsonIgnore]
        public virtual ICollection<Rent> Rents { get; }
    }
    [ModelMetadataType(typeof(Transport))]
    public class TransportCreateRequest : ITransport
    {
        public long Id { get; private set; }
        public long OwnerId { get; private set; }
        public bool CanBeRented { get; set; }
        public string TransportType { get; set; } = "";
        public string Model { get; set; } = "";
        public string Color { get; set; } = "";
        public string Identifier { get; set; } = "";
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? MinutePrice { get; set; }
        public double? DayPrice { get; set; }
    }
    public class TransportListRequest
    {
        public double Lat { get; set; }
        public double Long { get; set; }
        public double Radius { get; set; }
        [RegularExpression("Car|Bike|Scooter|All", ErrorMessage = "Type must be Car, Bike, Scooter or All.")]
        public string Type { get; set; } = "";
    }
    [ModelMetadataType(typeof(Transport))]
    public class TransportAdminCreateRequest : ITransport
    {
        public long Id { get; private set; }
        [ExistingAttribute<Account>]
        public long OwnerId { get; set; }
        public bool CanBeRented { get; set; }
        public string TransportType { get; set; } = "";
        public string Model { get; set; } = "";
        public string Color { get; set; } = "";
        public string Identifier { get; set; } = "";
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? MinutePrice { get; set; }
        public double? DayPrice { get; set; }
    }

}
