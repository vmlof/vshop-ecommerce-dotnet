using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VShop.ProductApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql(
                "INSERT INTO \"Products\" (\"Name\",\"Price\",\"Description\",\"Stock\",\"ImageUrl\",\"CategoryId\")" +
                "Values" +
                "('Caderno',7.55,'Caderno',10,'https://images.unsplash.com/photo-1581431886211-6b932f8367f2?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',1)," +
                "('Lápis',3.45,'Lápis Preto',20,'https://images.unsplash.com/photo-1667687435942-4fdff73a3ed6?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',1)," +
                "('Clips',5.33,'Clips para papel',50,'https://images.unsplash.com/photo-1668435075104-0e993cd60ba7?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',2)," +
                "('Caderno Espiral pequeno', 9.55, 'Caderno Espiral pequeno 100 folhas universitário 1 UN', 60, 'https://images.unsplash.com/photo-1598620617137-2ab990aadd37?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', 1)," +
                "('Lápis Coloridos', 3.45, 'Lápis Coloridos Faber Castel 12 unidades', 20, 'https://images.unsplash.com/photo-1627873828998-50b7aeec7ffe?q=80&w=1129&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', 1)," +
                "('Caneta Esferográfica', 12.80, 'Caneta Esferográfica BIC Cristal Fashion, 10 Cores Vibrantes', 100, 'https://images.unsplash.com/photo-1585336261022-680e295ce3fe?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', 1)," +
                "('Borracha Branca', 4.50, 'Borracha Branca Lavável pequena oval', 25, 'https://images.unsplash.com/photo-1627186131254-702e182adb80?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', 1)," +
                "('Caderno de Anotações 100 fl', 18.99, 'Caderno de Anotações 13x21 cm sem pauta preto', 35, 'https://images.unsplash.com/photo-1511871893393-82e9c16b81e3?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', 1)," +
                "('Cola Branca 1 Kg', 23.80, 'Cola Branca Cascorez 1 Kg', 25, 'https://schoolsupplyboxes.com/cdn/shop/products/Elmers_4_oz_glue.png?v=1556734776&width=640', 1)," +
                "('Papel Sulfite A4 75 g', 11.20, 'Papel Sulfite Chamequinho A4 75 g com 4 cores', 25, 'https://images.unsplash.com/photo-1573978828027-e830975e272c?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', 1);");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM \"Products\"");
        }
    }
}
