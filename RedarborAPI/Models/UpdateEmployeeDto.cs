using System.ComponentModel.DataAnnotations;
public class UpdateEmployeeDto
{
    public int? companyid { get; set; }
    public string? email { get; set; }
    public string? fax { get; set; }
    public string? name { get; set; }
    public string? password { get; set; }
    public int? portalId { get; set; }
    public int? roleId { get; set; }
    public int? statusId { get; set; }
    public string? telephone { get; set; }
    public string? username { get; set; }
}
