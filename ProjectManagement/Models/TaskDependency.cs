//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TaskDependency
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int DependentTaskId { get; set; }
    
        public virtual ProjectMilestone ProjectMilestone { get; set; }
        public virtual ProjectMilestone ProjectMilestone1 { get; set; }
    }
}