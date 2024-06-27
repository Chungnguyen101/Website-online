namespace DATN2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTotalAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.tb_Order", "TotalAmoun");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "TotalAmoun", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.tb_Order", "TotalAmount");
        }
    }
}
