using System;
using System.Collections.Generic;

namespace PagilaDemo.Models;

public partial class Rental
{
    public int RentalId { get; set; }

    public DateTime RentalDate { get; set; }

    public int InventoryId { get; set; }

    public int CustomerId { get; set; }

    public DateTime? ReturnDate { get; set; }

    public int StaffId { get; set; }

    public DateTime LastUpdate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Inventory Inventory { get; set; } = null!;

    public virtual ICollection<PaymentP202201> PaymentP202201s { get; set; } = new List<PaymentP202201>();

    public virtual ICollection<PaymentP202202> PaymentP202202s { get; set; } = new List<PaymentP202202>();

    public virtual ICollection<PaymentP202203> PaymentP202203s { get; set; } = new List<PaymentP202203>();

    public virtual ICollection<PaymentP202204> PaymentP202204s { get; set; } = new List<PaymentP202204>();

    public virtual ICollection<PaymentP202205> PaymentP202205s { get; set; } = new List<PaymentP202205>();

    public virtual ICollection<PaymentP202206> PaymentP202206s { get; set; } = new List<PaymentP202206>();

    public virtual Staff Staff { get; set; } = null!;
}
