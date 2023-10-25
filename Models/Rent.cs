using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;
using VolgaIT2023.ValidationAttributes;

namespace VolgaIT2023.Models
{
    public class Rent
    {
        public Rent() { }
        public Rent(RentAdminCreateRequest createRequest)
        {
            this.Update(createRequest);
        }
        public void End(double Lat, double Long)
        {
            RentedTransport.Latitude = Lat;
            RentedTransport.CanBeRented = true;
            RentedTransport.Longitude = Long;//TODO CHECK IF THEY ARE CHANGED
            int time;
            TimeSpan Difference = (DateTime.Now - DateTime.Parse(this.TimeStart));
            if (this.PriceType == "Days")
            {
                time = (int)(Difference.TotalDays);
            }
            else
            {
                time = (int)(Difference.TotalMinutes);
            }
            TimeEnd = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
            FinalPrice = (PriceOfUnit * time);
            User.Balance-=(double)FinalPrice;
        }
        public void Update(Rent rent)
        {
            this.TransportId= rent.TransportId;
            this.UserId = rent.UserId;
            this.TimeStart = rent.TimeStart;
            this.TimeEnd = rent.TimeEnd;
            this.PriceOfUnit = rent.PriceOfUnit;
            this.PriceType = rent.PriceType;
            this.FinalPrice = rent.FinalPrice;
        }
        public void Update(RentAdminCreateRequest rent)
        {
            this.TransportId = rent.TransportId;
            this.UserId = rent.UserId;
            this.TimeStart = rent.TimeStart;
            this.TimeEnd = rent.TimeEnd;
            this.PriceOfUnit = rent.PriceOfUnit;
            this.PriceType = rent.PriceType;
            this.FinalPrice = rent.FinalPrice;
        }
        /// <summary>
        /// Id аренды
        /// </summary>
        [Key]
        public long Id { get; private set; }
        /// <summary>
        /// Id транспортного средства, взятого в аренду
        /// </summary>
        [ExistingAttribute<Transport>]
        public long TransportId { get; set; }
        /// <summary>
        /// Id аккаунта который будет владеть транспортом на время аренды
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Дата и время начала аренды в ISO 8601
        /// </summary>
        [Required]
        public string TimeStart { get; set; } = "";
        /// <summary>
        /// Дата и время окончания аренды в ISO 8601 (может быть null)
        /// </summary>
        public string? TimeEnd { get; set; }
        /// <summary>
        /// Цена единицы вермени аренды (цена за минуту или за день)
        /// </summary>
        public double PriceOfUnit { get; set; }
        /// <summary>
        /// Тип оплаты [Minutes, Days]
        /// </summary>
        [Required]
        [RegularExpression("Days|Minutes", ErrorMessage = "PriceType must be Days or Minutes.")]
        public string PriceType { get; set; } = "Days";
        /// <summary>
        /// Финальная стоимость аренды (может быть null)
        /// </summary>
        public double? FinalPrice { get; set; } = null;
        [JsonIgnore]
        [ForeignKey(nameof(TransportId))]
        public virtual Transport RentedTransport { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public virtual Account User { get; set; }
    }
    [ModelMetadataType(typeof(Rent))]
    public class RentAdminCreateRequest
    {
        public long Id { get; private set; }
        public long TransportId { get; set; }
        public long UserId { get; set; }
        public string TimeStart { get; set; } = "";
        public string? TimeEnd { get; set; }
        public double PriceOfUnit { get; set; }
        public string PriceType { get; set; } = "Days";
        public double? FinalPrice { get; set; } = null;
    }
}
