﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IDM_API.Entities;

[Table("tbl_menu")]
public partial class tbl_menu
{
    [Key]
    public int MenuID { get; set; }

    [StringLength(50)]
    public string Code { get; set; }

    [Required]
    [StringLength(255)]
    public string MenuName { get; set; }

    [StringLength(255)]
    public string MenuPath { get; set; }

    [Column(TypeName = "text")]
    public string MenuUrl { get; set; }

    [StringLength(255)]
    public string MenuIcon { get; set; }

    public int? MenuParentID { get; set; }

    public int? Priority { get; set; }

    [InverseProperty("MenuParent")]
    public virtual ICollection<tbl_menu> InverseMenuParent { get; set; } = new List<tbl_menu>();

    [ForeignKey("MenuParentID")]
    [InverseProperty("InverseMenuParent")]
    public virtual tbl_menu MenuParent { get; set; }

    [InverseProperty("Menu")]
    public virtual ICollection<tbl_menu_role> tbl_menu_roles { get; set; } = new List<tbl_menu_role>();
}