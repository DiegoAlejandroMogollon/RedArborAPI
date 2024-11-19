using System.ComponentModel.DataAnnotations;

public class EmployeeDto
{
    public int? id { get; set; }


    public int companyid { get; set; }


    public string email { get; set; }

    public string? fax { get; set; }

    public string? name { get; set; }


    public string password { get; set; }


    public int portalid { get; set; }


    public int roleid { get; set; }


    public int statusid { get; set; }

    public string? telephone { get; set; }


    public string username { get; set; }

    public DateTime? updatedon { get; set; }

    public DateTime? deletedon { get; set; }

     public DateTime? createdon { get; set; }

     public DateTime? lastlogin { get; set; }
}
