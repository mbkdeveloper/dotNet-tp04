namespace GestionArticles.Models.Help
{
    // Un article du panier : un produit et sa quantité
    public class Item
    {
        public int quantite { get; set; }

        public Product Prod { get; set; }

        public string Description
        {
            get { return Prod.Name; }
        }

        public decimal UnitPrice
        {
            get { return Prod.Price; }
        }

        public int categoryId
        {
            get { return Prod.CategoryId; }
        }

        public Category? category
        {
            get { return Prod.Category; }
        }

        public decimal TotalPrice
        {
            get { return Prod.Price * quantite; }
        }

        public Item(Product p)
        {
            this.Prod = p;
        }

        public bool Equals(Item item)
        {
            return item.Prod.ProductId == this.Prod.ProductId;
        }
    }
}
