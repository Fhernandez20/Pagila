using System;
using System.Collections.Generic;

namespace PagilaDemo.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int StoreId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public int AddressId { get; set; }

    public bool Activebool { get; set; }

    public DateOnly CreateDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public int? Active { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<PaymentP202201> PaymentP202201s { get; set; } = new List<PaymentP202201>();

    public virtual ICollection<PaymentP202202> PaymentP202202s { get; set; } = new List<PaymentP202202>();

    public virtual ICollection<PaymentP202203> PaymentP202203s { get; set; } = new List<PaymentP202203>();

    public virtual ICollection<PaymentP202204> PaymentP202204s { get; set; } = new List<PaymentP202204>();

    public virtual ICollection<PaymentP202205> PaymentP202205s { get; set; } = new List<PaymentP202205>();

    public virtual ICollection<PaymentP202206> PaymentP202206s { get; set; } = new List<PaymentP202206>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public virtual Store Store { get; set; } = null!;
}
