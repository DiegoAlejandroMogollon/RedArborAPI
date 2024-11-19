using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Key]
    public int id { get; set; }

    [Required]
    public int companyid { get; set; }

    [Required]
    public DateTime createdon { get; set; }

    public DateTime? deletedon { get; set; }

    [Required, EmailAddress]
    public string email { get; set; }

    public string? fax { get; set; }

    public string? name { get; set; }

    public DateTime? lastlogin { get; set; }

    [Required]
    public string password { get; set; }

    [Required]
    public int portalid { get; set; }

    [Required]
    public int roleid { get; set; }

    [Required]
    public int statusid { get; set; }

    public string? telephone { get; set; }

    public DateTime? updatedon { get; set; }

    [Required]
    public string username { get; set; }
}
